using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace MbUnit.Framework.ComponentModel
{
    [DefaultProperty("Name")]
    [ToolboxItem(false)]
    public class TestComponentBase : Component, ITestComponent
    {
        private string name;
        public TestComponentBase(IContainer container)
        {
            container.Add(this);
            this.Name = "Test";
        }

        [Category("Test")]
        [Description("Name of the test case")]
        [DefaultValue("Test")]
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name=value;
            }
        }

        public virtual ITestSuite GetTests()
        {
            throw new NotImplementedException("This method must be overriden");
        }
    }
}
