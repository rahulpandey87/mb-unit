using System;
using System.ComponentModel;

namespace MbUnit.Framework.ComponentModel
{
    [DefaultEvent("Test")]
    [ToolboxItem(true)]
    public class TestCaseComponent : TestComponentBase
    {
        public TestCaseComponent(IContainer container)
            :base(container)
        {}

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
            TestCase tc = new TestCase(name, del, this, EventArgs.Empty);
            return tc;
        }

        public override ITestSuite GetTests()
        {
            TestSuite suite = new TestSuite(this.Name);
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
