using System;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Tests.Samples;


namespace MbUnit.Tests.Tasks.MSBuild
{
    [TestFixture]
    [CurrentFixture]
    public class MSBuildTaskTest
    {
        private string msbuildPath;
        private SampleCompiler compiler;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            string path = Path.GetDirectoryName(typeof(Object).Assembly.Location);
            this.msbuildPath = Path.Combine(path,"msbuild.exe");
            Console.WriteLine("MSBuild path: {0}", this.msbuildPath);
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
                this.msbuildPath,
                @"Tasks\MSBuild\msbuild.sample1.xml"
                );
            tester.Run();
        }

        //[Test]
        //public void MultiAssemblyProject()
        //{
        //    // load project file
        //    ConsoleTester tester = new ConsoleTester(
        //        this.msbuildPath,
        //        @"Tasks\MSBuild\msbuild.sample2.xml"
        //        );
        //    tester.Run();

        //    FileAssert.Exists(@"CustomFolder\CustomNameFormat.xml");
        //    FileAssert.Exists(@"CustomFolder\CustomNameFormat.html");
        //    FileAssert.Exists(@"CustomFolder\CustomNameFormat.txt");
        //    FileAssert.Exists(@"CustomFolder\CustomNameFormat.dox.txt");
        //}
    }
}
