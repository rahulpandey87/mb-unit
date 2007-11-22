using System;
using System.Data;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using QuickGraph.Predicates;
using QuickGraph.Collections.Filtered;
using TestFu.Data.Graph;

namespace TestFu.Tests.Data.Graph
{
    [TestFixture]
    public class NonNullableDataRelationEdgePredicateTest
    {
        #region Fields and setup
        private NullableAndNonNullableRelationDataSet dataSource;
        private DataGraph graph;
        private DataTableSortAlgorithm.NonNullableDataRelationEdgePredicate edgePredictate;
        private FilteredEdgeListGraph fgraph;

        [SetUp]
        public void SetUp()
        {
            this.dataSource = new NullableAndNonNullableRelationDataSet();
            this.graph = DataGraph.Create(dataSource);
            this.edgePredictate = new DataTableSortAlgorithm.NonNullableDataRelationEdgePredicate();
            this.fgraph = new FilteredEdgeListGraph(this.graph,
                this.edgePredictate
                );

        }
        #endregion

        [Test]
        public void CheckNotNullableRelation()
        {
            foreach (DataRelationEdge edge in this.fgraph.Edges)
            {
                Console.WriteLine("DataRelation: {0}", edge.Relation.RelationName);
                foreach (DataColumn column in edge.Relation.ParentColumns)
                {
                    if (!column.AllowDBNull)
                        return;
                    Console.WriteLine("Column {0} accepts null", column.ColumnName);
                }
                Assert.Fail("All columns are nullable");
            }
        }

        [Test]
        public void CheckFKBAFiltered()
        {
            Assert.IsTrue(this.dataSource.Relations.Contains("FK_B_A"));
            DataRelationEdge edge = this.graph.GetEdge(this.dataSource.Relations["FK_B_A"]);
            Assert.IsNotNull(edge, "Dataraph does not contain relation FK_B_A");
            Assert.IsFalse(fgraph.ContainsEdge(edge));
        }

        [Test]
        public void CheckFKBCNotFiltered()
        {
            Assert.IsTrue(this.dataSource.Relations.Contains("FK_B_C"));
            DataRelationEdge edge = this.graph.GetEdge(this.dataSource.Relations["FK_B_C"]);
            Assert.IsNotNull(edge, "Dataraph does not contain relation FK_B_C");
            Assert.IsTrue(fgraph.ContainsEdge(edge));
        }
    }
}
