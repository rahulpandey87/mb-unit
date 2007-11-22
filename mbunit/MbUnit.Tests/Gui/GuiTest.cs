using MbUnit.Framework;
using MbUnit.Forms;
using MbUnit.Core;

namespace MbUnit.Tests.Gui
{
    [TestFixture]
    [CurrentFixture]
    public class GuiTest
    {
        [Test]
        public void LaunchAndDoNothing()
        {
            ConsoleTester tester = new ConsoleTester("MbUnit.Gui.exe", "/close");
            tester.ExpectedExitCode = 100;
            tester.Run();
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        public void LaunchRunAndCloseGui()
        {
            ConsoleTester tester = new ConsoleTester(
                "MbUnit.Gui.exe", 
                "/run",
                "/close");
            tester.ExpectedExitCode = 100;
            tester.Run();
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        public void LaunchRunParentAssenblyAndCloseGui()
        {
            ConsoleTester tester = new ConsoleTester(
                "MbUnit.Gui.exe",
                "/run",
                "ParentAssembly.dll",
                "/close");
            tester.ExpectedExitCode = 100;
            tester.Run();
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        public void LaunchRunGenerateReportAndCloseGui()
        {
            ConsoleTester tester = new ConsoleTester(
                "MbUnit.Gui.exe",
                "/run",
                "ParentAssembly.dll",
                "/report-type:html",
                "/close");
            tester.ExpectedExitCode = 100;
            tester.Run();
            System.Threading.Thread.Sleep(1000);
        }

        [Test]
        public void LaunchRunAFewAssembliesAndCloseGui()
        {
            ConsoleTester tester = new ConsoleTester(
                "MbUnit.Gui.exe",
                "/run",
                "ParentAssembly.dll",
                "SickParentAssembly.dll",
                "ChildAssembly.dll",
                "/close");
            tester.ExpectedExitCode = 100;
            tester.Run();
            System.Threading.Thread.Sleep(1000);
        }

     //   [Test]
        public void LaunchRunProjectCloseGui()
        {
            ConsoleTester tester = new ConsoleTester(
                "MbUnit.Gui.exe",
                "/run",
                @"Gui\demo.mbunit",
                "/close");
            tester.ExpectedExitCode = 100;
            tester.Run();
            System.Threading.Thread.Sleep(1000);
        }

    }
}
