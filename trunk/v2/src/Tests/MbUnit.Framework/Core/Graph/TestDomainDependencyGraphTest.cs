using System;
using System.Collections;
using System.IO;

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
            return AssemblyCompiler.AssemblyPaths;
        }

        [CombinatorialTest]
        public void ExecuteAssemblies(
            [UsingFactories("AssemblyNames")] string name,
            [UsingFactories("AssemblyNames")] string secondName
            )
        {
            int successCount = 0;
            ArrayList names = new ArrayList();
            names.Add(Path.GetFileNameWithoutExtension(name));
            names.Add(Path.GetFileNameWithoutExtension(secondName));

            if (names.Contains("SickParentAssembly"))
            {
                if (names.Contains("ParentAssembly"))
                    successCount++;
            }
            else
            {
                if (names.Contains("ChildAssembly"))
                    successCount++;

                if (names.Contains("ParentAssembly"))
                    successCount++;
            }
            string[] files = new string[] { name, secondName };

            using (TestDomainDependencyGraph graph = TestDomainDependencyGraph.BuildGraph(files,null, MbUnit.Core.Filters.FixtureFilters.Any, true))
            {
                ReportResult result = graph.RunTests();
                Assert.AreEqual(successCount, result.Counter.SuccessCount);
            }
        }
    }
}
