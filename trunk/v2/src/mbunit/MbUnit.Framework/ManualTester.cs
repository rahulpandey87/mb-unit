using System;
using System.Windows.Forms;
namespace MbUnit.Framework {
    /// <summary>
    /// Contains methods that will show a dialog to the tester that diplays steps to take. The dialog also lets the tester adding comments. 
    /// </summary>
    /// <remarks>
    /// When you have given up and have no other choice, you can always count on the prehistorical "manual tests". 
    /// However, manual tests are not the best solution and you should always go for the automated solution.
    /// </remarks>
    /// <example>
    /// <para>By default, MbUnit renames the manual test window with the current test method name.</para>
    /// <code>
    /// using System;
    ///    using MbUnit.Core.Framework;
    ///    using MbUnit.Framework;
    ///
    ///    [TestFixture]
    ///    public class ManualFixture {
    ///        [Test]
    ///        public void DoSomething() {
    ///            ManualTester.DisplayForm(
    ///                "Do this",
    ///                "Do that",
    ///                "You should see this",
    ///                "..."
    ///                );
    ///        }
    ///    }
    /// </code>
    /// </example>
    public sealed class ManualTester {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManualTester"/> class.
        /// </summary>
        private ManualTester() { }

        /// <summary>
        /// Displays a dialog to the tester that diplays the test steps to take
        /// </summary>
        /// <param name="testSteps">The test steps.</param>
        public static void DisplayForm(params string[] testSteps) {
            using (ManualTestForm form = new ManualTestForm()) {
                int i = 0;
                Console.WriteLine("Steps:");
                foreach (string step in testSteps) {
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
