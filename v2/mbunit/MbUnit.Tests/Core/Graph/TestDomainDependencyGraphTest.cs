using System;
using System.Collections;

using MbUnit.Core.Remoting;
using MbUnit.Framework;
using MbUnit.Core.Graph;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Tests.Core.Graph
{
    [TestFixture]
    [TestsOn(typeof(TestDomainDependencyGraph))]
    public class TestDomainDependencyGraphTest
    {
        public string[] AssemblyNames()
        {
            return new String[] { "ParentAssembly", "ChildAssembly", "SickParentAssembly" };
        }

        [CombinatorialTest]
        public void ExecuteAssemblies(
            [UsingFactories("AssemblyNames")] string name,
            [UsingFactories("AssemblyNames")] string secondName,
            [UsingFactories("AssemblyNames")] string thirdName
            )
        {
            int successCount = 0;
            ArrayList names = new ArrayList();
            names.Add(name);
            names.Add(secondName);
            names.Add(thirdName);

            if (names.Contains("ChildAssembly"))
            {
                if (!names.Contains("SickParentAssembly"))
                    successCount++;
            }
            if (names.Contains("ParentAssembly"))
                successCount++;

            string[] files = new string[] { name + ".dll", secondName + ".dll", thirdName + ".dll" };

            using (TestDomainDependencyGraph graph = TestDomainDependencyGraph.BuildGraph(files,null, MbUnit.Core.Filters.FixtureFilters.Any, false))
            {
                ReportResult result = graph.RunTests();
                Assert.AreEqual(successCount, result.Counter.SuccessCount);
            }
        }

        [Test]
        public void ExecuteSickAssembly()
        {
            ExecuteAssemblies("ParentAssembly", "SickParentAssembly", "ChildAssembly");
        }
    }
}
