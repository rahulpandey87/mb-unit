using System;
using System.Data;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Predicates;
using QuickGraph.Algorithms;
using QuickGraph.Collections.Filtered;
using QuickGraph.Predicates;

namespace TestFu.Data.Graph
{
    public class DataTableSortAlgorithm : DataGraphAlgorithm
    {
        private bool ignoreAllowDBNull = false;
        private SourceFirstTopologicalSortAlgorithm topo = null;
        private NonNullableDataRelationEdgePredicate nonNullableRelationEdgePredicate =
            new NonNullableDataRelationEdgePredicate();

        public DataTableSortAlgorithm(DataGraph visitedGraph)
            :base(visitedGraph)
        {}

        public bool IgnoreAllowDBNull
        {
            get
            {
                return this.ignoreAllowDBNull;
            }
            set
            {
                this.ignoreAllowDBNull = value;
            }
        }

        public IEdgePredicate NonNullableRelationEdgePredicate
        {
            get
            {
                return this.nonNullableRelationEdgePredicate;
            }
        }

        public void Compute()
        {
            // filter nullabed edges out
            if (this.IgnoreAllowDBNull)
                topo = new SourceFirstTopologicalSortAlgorithm(this.VisitedGraph);
            else
            {
                FilteredVertexAndEdgeListGraph fgraph = new FilteredVertexAndEdgeListGraph(
                    this.VisitedGraph,
                    nonNullableRelationEdgePredicate,
                    QuickGraph.Predicates.Preds.KeepAllVertices()
                    );
                topo = new SourceFirstTopologicalSortAlgorithm(fgraph);
            }
            topo.Compute();
        }

        public DataTable[] GetSortedTables()                
        {
            if (topo == null)
                throw new InvalidOperationException("Sort not computed");

            DataTable[] result = new DataTable[topo.SortedVertices.Count];
            int i = 0;
            foreach (DataTableVertex v in topo.SortedVertices)
            {
                result[i++] = v.Table;
            }
            return result;
        }

        #region NonNullableDataRelationEdgePredicate
        public class NonNullableDataRelationEdgePredicate : IEdgePredicate
        {
            public bool Test(DataRelationEdge e)
            {
                foreach (DataColumn column in e.Relation.ChildColumns)
                {
                    if (!column.AllowDBNull)
                        return true;
                }
                return false;
            }
            #region IEdgePredicate Members

            bool IEdgePredicate.Test(IEdge e)
            {
                return this.Test(e as DataRelationEdge);
            }

            #endregion
        }
        #endregion
    }
}
