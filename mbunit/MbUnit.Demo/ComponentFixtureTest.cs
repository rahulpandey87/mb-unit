using System;
using System.IO;
using System.Reflection;
using System.ComponentModel;

using MbUnit.Framework;
using MbUnit.Framework.ComponentModel;

namespace MbUnit.Demo
{
    [CurrentFixture]
    public class ComponentFixtureTest : ComponentFixtureBase
    {
        private System.Diagnostics.PerformanceCounter performanceCounter1;
        private PerformanceCounterChecker performanceCounterChecker1;
        private TestCaseComponent testCaseComponent1;
        private IContainer components;

        public ComponentFixtureTest()
        {
            this.InitializeComponent();
        }

        public ComponentFixtureTest(IContainer container)
            :base(container)
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.performanceCounter1 = new System.Diagnostics.PerformanceCounter();
            this.performanceCounterChecker1 = new MbUnit.Framework.ComponentModel.PerformanceCounterChecker(this.components);
            this.testCaseComponent1 = new MbUnit.Framework.ComponentModel.TestCaseComponent(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterChecker1)).BeginInit();
// 
// performanceCounter1
// 
            this.performanceCounter1.CategoryName = ".NET CLR Data";
            this.performanceCounter1.CounterName = "SqlClient: Current # connection pools";
// 
// performanceCounterChecker1
// 
            this.performanceCounterChecker1.MaxValue = 10F;
            this.performanceCounterChecker1.MinValue = 0F;
            this.performanceCounterChecker1.PerformanceCounter = this.performanceCounter1;
// 
// testCaseComponent1
// 
            this.testCaseComponent1.Test += new System.EventHandler(this.testCaseComponent1_Test_3);
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounterChecker1)).EndInit();

        }

        private void testCaseComponent1_Test_1(object sender, EventArgs e)
        {
            Stream stream = typeof(ComponentFixtureBase).Assembly.GetManifestResourceStream("MbUnit.Framework.ComponentModel.ComponentFixture.png");
            Assert.IsNotNull(stream);
        }

        private void testCaseComponent1_Test(object sender, EventArgs e)
        {
            for (int i = 0; i < 10000; i++)
            {
                int[] array = new int[100];
            }
        }

        private void testCaseComponent1_Test_2(object sender, EventArgs e)
        {
            Console.WriteLine("hello world");
        }

        private void testCaseComponent1_Test_3(object sender, EventArgs e)
        {
            Console.WriteLine("Hello world");
        }
    }
}
