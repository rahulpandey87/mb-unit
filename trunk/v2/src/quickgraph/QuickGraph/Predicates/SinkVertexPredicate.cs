using System;

namespace QuickGraph.Predicates
{
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Predicates;
    using QuickGraph.Concepts.Traversals;

    /// <summary>
    /// A predicate to test if a <see cref="IVertex"/> is a root vertex (no 
    /// in-edges).
    /// </summary>
    /// <remarks>
    /// This predicate can be used to iterate over the root vertices of a
    /// graph.
    /// </remarks>
    public class SinkVertexPredicate : IVertexPredicate
    {
        private IImplicitGraph graph;

        /// <summary>
        /// Create the predicate over <paramref name="graph"/>.
        /// </summary>
        /// <param name="graph">graph to visit</param>
        public SinkVertexPredicate(IImplicitGraph graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");
            this.graph = graph;
        }
        #region IVertexPredicate Members

        /// <summary>
        /// Tests if the vertex is a root
        /// </summary>
        /// <param name="v">vertex to test</param>
        /// <returns>true is the vertex has no in-edges, false otherwise</returns>
        public bool Test(IVertex v)
        {
            return this.graph.OutEdgesEmpty(v);
        }

        #endregion
    }
}
