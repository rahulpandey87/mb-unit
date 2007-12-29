using System;
using System.IO;
using System.Configuration;
using System.Reflection;
using MbUnit.Framework.Tests;
using MbUnit.Core.Framework;
using MbUnit.Framework;

using MbUnit.Framework.Tests.Samples;

namespace MbUnit.Framework.Tests.Core.FrameworkBridges
{
    [TestFixture]
    public class NUnitBridgeTest
    {
        private SampleCompiler compiler;

        [SetUp]
        public void SetUp()
        {
            this.compiler = new SampleCompiler("NUnitFixture.cs");

            string nunitFolder = ConfigurationSettings.AppSettings["NUnitFolder"];
            string nunitFrameworkDll = Path.Combine(nunitFolder,@"NUnit.Framework.dll");

            Console.Out.WriteLine("NUnit.Framework.dll: {0}",nunitFrameworkDll);
            this.compiler.Compiler.Parameters.ReferencedAssemblies.Add(nunitFrameworkDll);

            Console.Out.WriteLine("Target assembly: {0}", this.compiler.Compiler.Parameters.OutputAssembly);
        }

        [Test]
        public void Compiles()
        {
            this.compiler.Compiles();
        }

        [Test]
        public void TestCaseCount()
        {
            this.compiler.LoadAndRunFixtures();
            Assert.AreEqual(4, this.compiler.Result.Counter.RunCount);
        }

        [Test]
        public void SuccessCount()
        {
            this.compiler.LoadAndRunFixtures();
            Assert.AreEqual(2, this.compiler.Result.Counter.SuccessCount);
        }

        [Test]
        public void FailureCount()
        {
            this.compiler.LoadAndRunFixtures();
            Assert.AreEqual(1, this.compiler.Result.Counter.FailureCount);
        }
        [Test]
        public void IgnoreCount()
        {
            this.compiler.LoadAndRunFixtures();
            Assert.AreEqual(1, this.compiler.Result.Counter.IgnoreCount);
        }
    }
}
