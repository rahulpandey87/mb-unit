using System;
using System.Collections;

using MbUnit.Core.Remoting;
using MbUnit.Framework;
using MbUnit.Core.Graph;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Framework.Tests.Core.Graph
{
    [TestFixture]
    [TestsOn(typeof(TestDomainDependencyGraph))]
    public class TestDomainDependencyGraphTest
    {
        public string[] AssemblyNames()
        {
            string path = "C:\\mbunit\\v2\\Tests\\MbUnit.Framework\\Core\\Graph\\";
            return new String[] { path + "ParentAssembly", path + "ChildAssembly"};
        }

        [CombinatorialTest]
        public void ExecuteAssemblies(
            [UsingFactories("AssemblyNames")] string name,
            [UsingFactories("AssemblyNames")] string secondName
            )
        {
            int successCount = 0;
            ArrayList names = new ArrayList();
            names.Add(name);
            names.Add(secondName);
       
            if (names.Contains("ChildAssembly"))              
            successCount++;
       
            if (names.Contains("ParentAssembly"))
                successCount++;

            string[] files = new string[] { name + ".dll", secondName + ".dll"};

            using (TestDomainDependencyGraph graph = TestDomainDependencyGraph.BuildGraph(files,null, MbUnit.Core.Filters.FixtureFilters.Any, true))
            {
                ReportResult result = graph.RunTests();
                Assert.AreEqual(successCount, result.Counter.SuccessCount);
            }
        }

        [Test]
        public void ExecuteSickAssembly()
        {
            ExecuteAssemblies("ParentAssembly", "ChildAssembly");
        }
    }
}
