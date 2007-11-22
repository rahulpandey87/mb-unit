using System;
using System.Data;
using System.Collections;
using TestFu.Data.Graph;
using QuickGraph.Concepts;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Tests.Data.Graph
{
    [TypeFixture(typeof(DataSet))]
    [ProviderFactory(typeof(DataSetFactory),typeof(DataSet))]
    public class DataGraphTest
    {
        #region Fields and setup
        private DataGraph graph;

        [SetUp]
        public void SetUp(DataSet dataSource)
        {
            this.graph = DataGraph.Create(dataSource);
        }
        #endregion

        #region Drawing
/*
        [Test]
        public void Draw(DataSet dataSource)
        {
            GraphvizAlgorithm gv = new GraphvizAlgorithm(this.graph);
            gv.FormatVertex+=new FormatVertexEventHandler(gv_FormatVertex);
            gv.FormatEdge+=new FormatEdgeEventHandler(gv_FormatEdge);
            System.Diagnostics.Process.Start(gv.Write(dataSource.DataSetName));
        }
        void gv_FormatVertex(object sender, FormatVertexEventArgs e)
        {
            DataTableVertex v = (DataTableVertex)e.Vertex;
            e.VertexFormatter.Shape = NGraphviz.Helpers.GraphvizVertexShape.Box;
            e.VertexFormatter.Label = v.Table.TableName;
        }

        void gv_FormatEdge(object sender, FormatEdgeEventArgs e)
        {
            DataRelationEdge edge = (DataRelationEdge)e.Edge;
            e.EdgeFormatter.Label.Value = edge.Relation.RelationName;
        }
*/
        #endregion

        #region Counts
        [Test]
        public void TableCountEqualsVerticesCount(DataSet dataSource)
        {
            Console.WriteLine("Table count: {0}, vertices: {1}",
                dataSource.Tables.Count,
                this.graph.VerticesCount
                );
            Assert.AreEqual(
                dataSource.Tables.Count,
                this.graph.VerticesCount
                );
        }
        [Test]
        public void RelationCountEqualsEdgesCount(DataSet dataSource)
        {
            Console.WriteLine("Relations count: {0}, edges: {1}",
                dataSource.Relations.Count,
                this.graph.EdgesCount
                );
            Assert.AreEqual(
                dataSource.Relations.Count,
                this.graph.EdgesCount
                );
        }
        [Test]
        public void DisplayGraph(DataSet dataSource)
        {
            Console.WriteLine("Displaying graph structure:");
            foreach (DataRelationEdge edge in this.graph.Edges)
            {
                Console.WriteLine("{0} -> {1}",
                    edge.Source.Table.TableName,
                    edge.Target.Table.TableName
                    );
            }
        }
        #endregion

        #region DataTable <-> Vertex
        [Test]
        public void EachVertexHasTable(DataSet dataSource)
        {
            foreach (DataTableVertex v in this.graph.Vertices)
            {
                Assert.IsTrue(dataSource.Tables.Contains(v.Table.TableName),
                    "Vertex {0} has a table {1} that was not found in the DataSet",
                    v.ID,
                    v.Table.TableName
                    );
            }
        }

        [Test]
        public void EachTableHasVertex(DataSet dataSource)
        {
            foreach(DataTable table in dataSource.Tables)
            {
                Assert.IsNotNull(graph.GetVertex(table),
                    "The table {0} was not found in the graph", table.TableName);
            }
        }
        #endregion

        #region DataRelation <-> Edge
        [Test]
        public void EachEdgeHasRelation(DataSet dataSource)
        {
            foreach (DataRelationEdge e in this.graph.Edges)
            {
                Assert.IsTrue(dataSource.Relations.Contains(e.Relation.RelationName),
                    "Edge {0} has a relation {1} that was not found in the DataSet",
                    e.ID,
                    e.Relation.RelationName
                    );
            }
        }

        [Test]
        public void EachRelationHasEdge(DataSet dataSource)
        {
            foreach (DataRelation relation in dataSource.Relations)
            {
                Assert.IsNotNull(graph.GetEdge(relation),
                    "The relation {0} was not found in the graph", relation.RelationName);
            }
        }

        [Test]
        public void ParentTableIsTheSourceOfTheEdges(DataSet dataSource)
        {
            foreach (DataRelationEdge e in this.graph.Edges)
            {
                DataTable source = e.Source.Table;

                Assert.AreEqual(e.Relation.ParentTable, source,
                    "Parent table {0} is not equal to the source vertex table {1}",
                    e.Relation.ParentTable.TableName,
                    source.TableName
                    );
            }
        }

        [Test]
        public void ChildTableIsTheTargetOfTheEdges(DataSet dataSource)
        {
            foreach (DataRelationEdge e in this.graph.Edges)
            {
                DataTable target = e.Target.Table;

                Assert.AreEqual(e.Relation.ChildTable, target,
                    "Child table {0} is not equal to the target vertex table {1}",
                    e.Relation.ParentTable.TableName,
                    target.TableName
                    );
            }
        }
        #endregion

    }
}
