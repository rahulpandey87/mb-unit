using System;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Algorithms;

namespace TestFu.Data.Graph
{
    public abstract class DataGraphAlgorithm : IAlgorithm
    {
        private DataGraph visitedGraph;

        public DataGraphAlgorithm(DataGraph visitedGraph)
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");
            this.visitedGraph = visitedGraph;
        }

        public DataGraph VisitedGraph
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
    }
}
