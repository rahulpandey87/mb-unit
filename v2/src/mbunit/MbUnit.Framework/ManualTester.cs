using System;
using System.Windows.Forms;
namespace MbUnit.Framework
{
    public sealed class ManualTester
    {
        private ManualTester()
        {}

        public static void DisplayForm(params string[] testSteps)
        {
            using (ManualTestForm form = new ManualTestForm())
            {
                int i = 0;
                Console.WriteLine("Steps:");
                foreach (string step in testSteps)
                {
                    string message = String.Format("{0} {1}", i, step);
                    Console.WriteLine(message);
                    form.TestStepList.Items.Add(message);
                    ++i;
                }
                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(1);
                form.Text = trace.GetFrame(0).GetMethod().DeclaringType.FullName
                    + trace.GetFrame(0).GetMethod().Name;

                DialogResult result = form.ShowDialog();

                // dumping comments
                Console.WriteLine("Manual Test");
                Console.WriteLine(form.Comments);

                Console.WriteLine("Success: {0}", result);
                Assert.AreEqual(result, DialogResult.Yes,
                    "Manual Test failed");
            }
        }
    }
}
