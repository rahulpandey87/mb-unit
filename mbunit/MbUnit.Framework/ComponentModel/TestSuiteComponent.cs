using System;
using System.Collections;
using System.Drawing.Design;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace MbUnit.Framework.ComponentModel
{
    [ToolboxItem(true)]
    public class TestSuiteComponent : TestComponentBase
    {
        private TestCaseComponentCollection testCases;

        public TestSuiteComponent(IContainer container)
            :base(container)
        {
            this.testCases = new TestCaseComponentCollection(this);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TestCaseComponentCollection TestCases
        {
            get
            {
                return this.testCases;
            }
        }

        [Editor(typeof(CollectionEditor),typeof(UITypeEditor))]
        public class TestCaseComponentCollection : CollectionBase
        {
            private WeakReference owner;

            public TestCaseComponentCollection(TestSuiteComponent owner)
            {
                this.owner = new WeakReference(owner);
            }

            public TestSuiteComponent Owner
            {
                get
                {
                    return this.owner.Target as TestSuiteComponent;
                }
            }

            public TestCaseComponent this[int index]
            {
                get
                {
                    return base.List[index] as TestCaseComponent;
                }
                set
                {
                    base.List[index] = value;
                }
            }

            public void Add(Object o)
            {
                this.AddTestCase(o as TestCaseComponent);
            }

            public void AddTestCase(TestCaseComponent testCase)
            {
                this.Owner.Container.Add(testCase);
                this.List.Add(testCase);
            }

            protected override void OnValidate(object value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                TestCaseComponent testCase = value as TestCaseComponent;
                if (testCase == null)
                    throw new ArgumentException("value not assignable to TestCaseComponent");
                base.OnValidate(value);
            }
        }
    }
}
