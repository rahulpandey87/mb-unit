using System;
using System.IO;
using System.Configuration;
using System.Reflection;
using MbUnit.Tests.Samples;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Tests.Core.FrameworkBridges
{
    [TestFixture]
    public class NUnitSerializationBridgeTest
    {
        private SampleCompiler compiler;

        [SetUp]
        public void SetUp()
        {
            //Console.WriteLine("NUnit serialization test");
            this.compiler = new SampleCompiler("NUnitSerializationFixture.cs");

            string nunitFolder = ConfigurationSettings.AppSettings["NUnitFolder"];
            string nunitFrameworkDll = Path.GetFullPath(Path.Combine(nunitFolder, @"NUnit.Framework.dll"));

            Console.Out.WriteLine("NUnit.Framework.dll: {0}", nunitFrameworkDll);
            this.compiler.Compiler.Parameters.ReferencedAssemblies.Add(nunitFrameworkDll);
            this.compiler.Compiler.Parameters.ReferencedAssemblies.Add("System.Xml.dll");

            Console.Out.WriteLine("Target assembly: {0}", this.compiler.Compiler.Parameters.OutputAssembly);
            this.compiler.Compiles();
        }

        [Test]
        public void SuccessCount()
        {
            this.compiler.LoadAndRunFixtures();
            Assert.AreEqual(1, this.compiler.Result.Counter.RunCount);
            Assert.AreEqual(1, this.compiler.Result.Counter.SuccessCount);
        }
    }
}
