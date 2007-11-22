using System;
using System.Data;
using System.Collections;
using TestFu.Data.Graph;
using QuickGraph.Concepts;
using QuickGraph.Algorithms;
using QuickGraph.Collections.Filtered;
using QuickGraph.Predicates;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Tests.Data.Graph
{

    [TypeFixture(typeof(DataSet))]
    [ProviderFactory(typeof(DataSetFactory), typeof(DataSet))]
    public class DataTableSortAlgorithmTest
    {
        #region Fields and setup
        private DataGraph graph;
        private DataTableSortAlgorithm topo;

        [SetUp]
        public void SetUp(DataSet dataSource)
        {
            this.graph = DataGraph.Create(dataSource);
            this.topo = new DataTableSortAlgorithm(this.graph);
        }
        #endregion

        protected void DisplayResults()
        {
            foreach (DataTable table in topo.GetSortedTables())
            {
                Console.WriteLine("\t{0}", table.TableName);
            }
        }

        [Test]
        public void CheckResult(DataSet dataSource)
        {
            Hashtable visited = new Hashtable();
            topo.Compute();
            DisplayResults();

            foreach (DataTable table in topo.GetSortedTables())
            {
                Console.WriteLine("Checking table {0}", table.TableName);
                DataTableVertex v = this.graph.GetVertex(table);
                foreach(DataRelationEdge edge in this.graph.InEdges(v))
                {
                    if(edge.Source==v)
                        continue;
                    if (!topo.IgnoreAllowDBNull && !topo.NonNullableRelationEdgePredicate.Test(edge))
                        continue;

                    Assert.IsTrue(visited.Contains(edge.Source),
                        "Table {0} should be before {1}",
                        edge.Source.Table.TableName,
                        v.Table.TableName
                        );
                }
                visited.Add(v, v);
            }
        }


        [Test]
        public void CheckResultIgnoreAllowDBNull(DataSet dataSource)
        {
            Hashtable visited = new Hashtable();
            topo.IgnoreAllowDBNull = true;
            topo.Compute();
            DisplayResults();

            foreach (DataTable table in topo.GetSortedTables())
            {
                Console.WriteLine("Checking table {0}", table.TableName);
                DataTableVertex v = this.graph.GetVertex(table);
                foreach (DataRelationEdge edge in this.graph.InEdges(v))
                {
                    if (edge.Source == v)
                        continue;
                    if (!topo.IgnoreAllowDBNull && !topo.NonNullableRelationEdgePredicate.Test(edge))
                        continue;

                    Assert.IsTrue(visited.Contains(edge.Source),
                        "Table {0} should be before {1}",
                        edge.Source.Table.TableName,
                        v.Table.TableName
                        );
                }
                visited.Add(v, v);
            }
        }
    }
}
