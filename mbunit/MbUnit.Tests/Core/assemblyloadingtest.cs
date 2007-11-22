using System;
using System.IO;
using MbUnit.Framework;

namespace MbUnit.Tests.Core
{
    [TestFixture]
    public class AssemblyLoadingTest
    {
        private string directory = "FolderWithoutMbUnit";
        [SetUp]
        public void SetUp()
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }

            Directory.CreateDirectory(directory);
            File.Copy("ParentAssembly.dll", Path.Combine(directory, "ParentAssembly.dll"));
        }

        [Test]
        public void RunTestsWithConsoleInternal()
        {
            MbUnit.Core.Cons.MainClass main = new MbUnit.Core.Cons.MainClass();
            main.Main(new string[] { Path.Combine(directory, "ParentAssembly.dll") });
        }

        [Test]
        public void RunTestsWithConsole()
        {
            ConsoleTester runner = new ConsoleTester(
                "MbUnit.Cons.exe",
                Path.Combine(directory, "ParentAssembly.dll")
                );

            runner.Run();
        }

        [Test]
        public void RunTestsWithGui()
        {
            ConsoleTester runner = new ConsoleTester(
                "MbUnit.Gui.exe",
                Path.Combine(directory, "ParentAssembly.dll"),
                "/run",
                "/close"
                );
        }
    }
}

