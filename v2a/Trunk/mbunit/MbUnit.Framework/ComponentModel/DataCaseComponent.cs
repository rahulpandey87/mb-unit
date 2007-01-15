using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace MbUnit.Framework.ComponentModel
{
    [ToolboxItem(true)]
    public class DataCaseComponent : TestComponentBase
    {
        private System.Diagnostics.PerformanceCounter performanceCounter1;
        private string dataMember = null;
        private object dataSource = null;

        public DataCaseComponent(IContainer container)
            :base(container)
        {}

        [Category("Test")]
        [TypeConverter(typeof(DataSourceConverter))]
        public Object DataSource
        {
            get
            {
                return this.dataSource;
            }
            set
            {
                this.dataSource = value;
            }
        }

        [Category("Test")]
        public string DataMember
        {
            get
            {
                return this.dataMember;
            }
            set
            {
                this.dataMember = value;
            }
        }

        public override ITestSuite GetTests()
        {
            throw new NotImplementedException();
        }

        private void InitializeComponent()
        {
            this.performanceCounter1 = new System.Diagnostics.PerformanceCounter();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).BeginInit();
// 
// DataCaseComponent
// 
            this.Name = "Test";
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).EndInit();

        }
    }
}
