using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Diagnostics;
using MbUnit.Framework;

namespace MbUnit.Framework.ComponentModel
{

    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(PerformanceCounterChecker), "PerformanceCounterChecker.png")]
    public class PerformanceCounterChecker :
        Component, 
        ISupportInitialize,
        ITestDecoratorComponent
    {
        private PerformanceCounter performanceCounter = null;
        private float minValue = 0;
        private float maxValue  = float.MaxValue;
        private float currentValue = 0;
        private bool relative = false;

        public PerformanceCounterChecker()
        {
            this.InitializeComponent();
        }

        public PerformanceCounterChecker(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.performanceCounter = new System.Diagnostics.PerformanceCounter();
        }

        [Category("Test")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public PerformanceCounter PerformanceCounter
        {
            get
            {
                return this.performanceCounter;
            }
            set
            {
                bool changed = this.performanceCounter != value;
                this.performanceCounter = value;
                if (changed)
                    this.OnPerformanceCounterChanged(EventArgs.Empty);
            }
        }

        [Category("Data")]
        public event EventHandler PerformanceCounterChanged;
        protected virtual void OnPerformanceCounterChanged(EventArgs args)
        {
            if (this.performanceCounter != null)
            {
                if (this.performanceCounter.CounterType == PerformanceCounterType.RawFraction)
                    this.MaxValue = 100;
            }

            if (this.PerformanceCounterChanged != null)
                this.PerformanceCounterChanged(this, args);
        }

        [Category("Test")]
        [DefaultValue(0)]
        public float MinValue
        {
            get
            {
                return this.minValue;
            }
            set
            {
                this.minValue = value;
            }
        }

        [Category("Test")]
        [DefaultValue(float.MaxValue)]
        public float MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                this.maxValue = value;
            }
        }

        [Category("Test")]
        [DefaultValue(false)]
        public bool Relative
        {
            get
            {
                return this.relative;
            }
            set
            {
                this.relative = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float CurrentValue
        {
            get
            {
                return this.currentValue;
            }
        }

        public virtual void BeginInit()
        {}

        public virtual void EndInit()
        {
            if (this.MinValue > this.MaxValue)
                throw new InvalidOperationException("MinValue must be smaller that MaxValue");
        }

        public virtual void BeginMonitor()
        {
            this.currentValue = this.PerformanceCounter.NextValue();
        }

        public virtual void EndMonitor()
        {
            float finalValue = this.PerformanceCounter.NextValue();

            // corrected value
            float correctedValue = finalValue;
            if (this.Relative)
                correctedValue -= this.CurrentValue;

            Console.WriteLine("Monitoring {0},{1}, for {2},{3} : {4} -> {5} (diff {6})",
                this.PerformanceCounter.CategoryName,
                this.PerformanceCounter.CounterName,
                this.PerformanceCounter.InstanceName,
                this.PerformanceCounter.MachineName,
                this.CurrentValue,
                finalValue,
                correctedValue);

            if (correctedValue > this.MaxValue)
            {
                Assert.Fail("PerformanceCounter {0}, {1}, {2}, {3} value ({4}) was above maxium ({5})",
                    this.PerformanceCounter.CategoryName,
                    this.PerformanceCounter.CounterName,
                    this.PerformanceCounter.InstanceName,
                    this.PerformanceCounter.MachineName,
                    correctedValue,
                    this.MaxValue
                    );
            }

            if (correctedValue < this.MinValue)
            {
                Assert.Fail("PerformanceCounter {0}, {1}, {2}, {3} value ({4}) was under minimum ({5})",
                    this.PerformanceCounter.CategoryName,
                    this.PerformanceCounter.CounterName,
                    this.PerformanceCounter.InstanceName,
                    this.PerformanceCounter.MachineName,
                    correctedValue,
                    this.MinValue
                    );
            }
        }

        public ITestSuite Decorate(ITestSuite suite)
        {
            TestSuite checkedSuite = new TestSuite(suite.Name);
            foreach (ITestCase testCase in suite.TestCases)
            {
                PerformanceCounterCheckerTestCase checkedTestCase = new PerformanceCounterCheckerTestCase(this,testCase);
                checkedSuite.Add(checkedTestCase);
            }
            return checkedSuite;
        }

        private class PerformanceCounterCheckerTestCase : ITestCase
        {
            private PerformanceCounterChecker checker;
            private ITestCase testCase;

            public PerformanceCounterCheckerTestCase(
                PerformanceCounterChecker checker,
                ITestCase testCase)
            {
                this.checker = checker;
                this.testCase = testCase;
            }

            public string Name
            {
                get { return this.testCase.Name; }
            }

            public string Description
            {
                get { return this.testCase.Description; }
            }

            public Object Invoke(Object o, System.Collections.IList args)
            {
                this.checker.BeginMonitor();
                object result = this.testCase.Invoke(o, args);
                this.checker.EndMonitor();

                return result;
            }
        }
    }
}
