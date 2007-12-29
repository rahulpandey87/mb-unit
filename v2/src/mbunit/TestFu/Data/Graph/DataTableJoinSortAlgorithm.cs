using System;
using System.Data;
using System.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Predicates;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using QuickGraph.Collections.Filtered;
using QuickGraph.Predicates;
using QuickGraph.Concepts.Algorithms;

namespace TestFu.Data.Graph
{
    public class DataTableJoinSortAlgorithm : IAlgorithm
    {
        private DataJoinGraph visitedGraph;
        private ArrayList joins = new ArrayList();
        private DataTableJoinVertex startVertex = null;

        public DataTableJoinSortAlgorithm(DataJoinGraph visitedGraph)
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");
            this.visitedGraph = visitedGraph;
        }

        public DataJoinGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }

        Object IAlgorithm.VisitedGraph
        {
            get
            {
                return this.VisitedGraph;
            }
        }

        public ICollection Joins
        {
            get
            {
                return this.joins;
            }
        }

        public DataTableJoinVertex StartVertex
        {
            get
            {
                return this.startVertex;
            }
        }

        public void Compute()
        {
            this.joins.Clear();
            this.startVertex = null;
            UndirectedDepthFirstSearchAlgorithm udfs =
                new UndirectedDepthFirstSearchAlgorithm(this.VisitedGraph);
            udfs.TreeEdge+=new EdgeEventHandler(udfs_TreeEdge);
            udfs.StartVertex+=new VertexEventHandler(udfs_StartVertex);

            udfs.Compute();
        }

        void udfs_TreeEdge(Object sender, EdgeEventArgs e)
        {
            DataRelationJoinEdge edge = (DataRelationJoinEdge)e.Edge;
            this.joins.Add(edge);
        }

        void udfs_StartVertex(Object sender, VertexEventArgs e)
        {
            if (this.startVertex==null)
            {
                this.startVertex = (DataTableJoinVertex)e.Vertex;
                return;
            }
           throw new InvalidOperationException("The Join graph is not connected");
        }
    }
}
