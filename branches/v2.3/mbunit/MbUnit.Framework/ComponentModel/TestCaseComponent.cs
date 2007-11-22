using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing;

namespace MbUnit.Framework.ComponentModel
{
    [DefaultEvent("Test")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(TestCaseComponent), "TestCaseComponent.png")]
    public class TestCaseComponent : TestComponentBase
    {
        private Type exceptionType = typeof(void);

        public TestCaseComponent(IContainer container)
            :base(container)
        {}

        
        [Category("Test")]
       // [DefaultValue(typeof(void))]
        [Editor(typeof(TypeListTypeEditor), typeof(UITypeEditor))]
        [Description("Expected Exception Type; if none, use System.Void")]
        public Type ExceptionType
        {
            get
            {
                return this.exceptionType;
            }
            set
            {
                this.exceptionType = value;
            }
        }
        

        [Category("Test")]
        [Description("Event raised by the TestCase")]
        public event EventHandler Test;

        protected virtual void OnTest(EventArgs e)
        {
            if (this.Test != null)
                this.Test(this, e);
        }

        protected virtual ITestCase CreateTestCase(Delegate del, string name)
        {
            ITestCase tc = new TestCase(name, del, this, EventArgs.Empty);

            if(this.exceptionType!=null && this.exceptionType!=typeof(void))
                tc = new ExpectedExceptionTestCase(tc, this.exceptionType);

            return tc;
        }

        public override ITestSuite GetTests()
        {
            TestSuite suite = new TestSuite(this.Name);
            if (this.Test == null)
                return suite;

            int index = 0;
            foreach(Delegate del in Test.GetInvocationList())
            {
                string name = String.Format("Test{0}", index).TrimEnd('0');
                ITestCase tc = CreateTestCase(del, name);
                suite.Add(tc);
                index++;
            }
            return suite;
        }
    }
}
