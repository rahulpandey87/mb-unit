using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Tests.Samples;
using System.IO;

namespace MbUnit.Tests.Tasks.NAnt
{
    [TestFixture]
    public class NantTaskTest
    {
        private string nantPath;
        private SampleCompiler compiler;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            this.nantPath = 
                System.Configuration.ConfigurationSettings.AppSettings["NAntPath"];
            this.nantPath = System.IO.Path.GetFullPath(this.nantPath);
            Console.WriteLine("Nant path: {0}",this.nantPath);

            foreach(string file in Directory.GetFiles(@"Tasks\NAnt","*.dll"))
                File.Delete(file);
            foreach(string file in Directory.GetFiles(@"Tasks\NAnt","*.exe"))
                File.Delete(file);
            File.Copy(Path.GetFullPath("ParentAssembly.dll"), Path.GetFullPath(@"Tasks\NAnt\ParentAssembly.dll"));
            File.Copy(Path.GetFullPath("SickParentAssembly.dll"), Path.GetFullPath(@"Tasks\NAnt\SickParentAssembly.dll"));
            File.Copy(Path.GetFullPath("ChildAssembly.dll"), Path.GetFullPath(@"Tasks\NAnt\ChildAssembly.dll"));
        }

        [SetUp]
        public void SetUp()
        {
            // create sample fixture
            this.compiler = new SampleCompiler("TestFixtureSample.cs");
            this.compiler.AddMbUnitReferences();
            this.compiler.CreateAssembly();
        }

        [Test]
        public void SimpleProject()
        {
            // load project file
            ConsoleTester tester = new ConsoleTester(
                this.nantPath,
                "-buildfile:Tasks/NAnt/nant.sample.xml",
                "tests"
                );
            tester.Run();
        }

        [Test]
        public void DifferentReports()
        {
            // load project file
            ConsoleTester tester = new ConsoleTester(
                this.nantPath,
                "-buildfile:Tasks/NAnt/nant.sample2.xml",
                "tests"
                );
            tester.Run();
        }

        [Test]
        public void MultipleSetOfAssemblies()
        {
            // load project file
            ConsoleTester tester = new ConsoleTester(
                this.nantPath,
                "-buildfile:Tasks/NAnt/nant.sample3.xml",
                "tests"
                );
            tester.Run();
        }
    }
}
