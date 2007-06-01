using System;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Collections;
using QuickGraph.Collections;
using QuickGraph.Concepts.MutableTraversals;
using QuickGraph.Concepts.Algorithms;

namespace QuickGraph.Algorithms.MaximumFlow
{
    /// <summary>
    /// A <see cref="IAlgorithm"/> implementation that augments a 
    /// <see cref="IMutableVertexAndEdgeListGraph"/> such that
    /// for all edge (u,v) there exists the edge (v,u) in the graph.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This algorithm can be used to augment a graph with reversed edges to make it usable by
    /// a <see cref="MaximumFlowAlgorithm"/> implementation. It also contains the method
    /// to clean up the graph afterwards.
    /// </para>
    /// </remarks>
    public class ReversedEdgeAugmentorAlgorithm : IAlgorithm
    {
        private IMutableVertexAndEdgeListGraph visitedGraph;
        private EdgeCollection augmentedEgdes = new EdgeCollection();
        private EdgeEdgeDictionary reversedEdges = new EdgeEdgeDictionary();
        private bool augmented = false;

        public ReversedEdgeAugmentorAlgorithm(IMutableVertexAndEdgeListGraph visitedGraph)
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");
            this.visitedGraph = visitedGraph;
        }

        #region IAlgorithm
        public IMutableVertexAndEdgeListGraph VisitedGraph
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
        #endregion

        #region Properties
        /// <summary>
        /// Gets a <see cref="EdgeCollection"/> instance containing the 
        /// augmented edges.
        /// </summary>
        /// <value></value>
        public EdgeCollection AugmentedEdges
        {
            get
            {
                return this.augmentedEgdes;
            }
        }

        /// <summary>
        /// Gets a <see cref="EdgeEdgeDictionary"/> associating
        /// each edge to it's corresponding reversed edge.
        /// </summary>
        /// <value></value>
        public EdgeEdgeDictionary ReversedEdges
        {
            get
            {
                return this.reversedEdges;
            }
        }

        /// <summary>
        /// Gets a value indicating wheter the <see cref="VisitedGraph"/>
        /// has been augmented.
        /// </summary>
        /// <value></value>
        public bool Augmented
        {
            get
            {
                return this.augmented;
            }
        }
        #endregion

        #region Events
        public event EdgeEventHandler ReversedEdgeAdded;
        protected virtual void OnReservedEdgeAdded(EdgeEventArgs e)
        {
            if (this.ReversedEdgeAdded != null)
                this.ReversedEdgeAdded(this, e);
        }
        #endregion

        /// <summary>
        /// Augments the <see cref="VisitedGraph"/> with reversed edges.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The graph has already been augmented.
        /// </exception>
        public void AddReversedEdges()
        {
            if (this.Augmented)
                throw new InvalidOperationException("Graph already augmented");
            // step 1, find edges that need reversing
            EdgeCollection notReversedEdges = new EdgeCollection();
            foreach (IEdge edge in this.VisitedGraph.Edges)
            {
                // if reversed already found, continue
                if (this.reversedEdges.Contains(edge))
                    continue;

                IEdge reversedEdge = this.FindReversedEdge(edge);
                if (reversedEdge != null)
                {
                    // setup edge
                    this.reversedEdges[edge] = reversedEdge;
                    // setup reversed if needed
                    if (!this.reversedEdges.Contains(reversedEdge))
                        this.reversedEdges[reversedEdge] = edge;
                    continue;
                }

                // this edge has no reverse
                notReversedEdges.Add(edge);
            }

            // step 2, go over each not reversed edge, add reverse
            foreach (IEdge edge in notReversedEdges)
            {
                if (this.reversedEdges.Contains(edge))
                    continue;

                // already been added
                IEdge reversedEdge = this.FindReversedEdge(edge);
                if (reversedEdge != null)
                {
                    this.reversedEdges[edge] = reversedEdge;
                    continue;
                }

                // need to create one
                reversedEdge = this.VisitedGraph.AddEdge(edge.Target, edge.Source);
                this.augmentedEgdes.Add(reversedEdge);
                this.reversedEdges[edge] = reversedEdge;
                this.reversedEdges[reversedEdge] = edge;
                this.OnReservedEdgeAdded(new EdgeEventArgs(reversedEdge));
            }

            this.augmented = true;
        }

        /// <summary>
        /// Removes the reversed edges.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The graph is not yet augmented.
        /// </exception>
        public void RemoveReversedEdges()
        {
            if (!this.Augmented)
                throw new InvalidOperationException("Graph is not yet augmented");

            foreach (IEdge edge in this.augmentedEgdes)
                this.VisitedGraph.RemoveEdge(edge);

            this.augmentedEgdes.Clear();
            this.reversedEdges.Clear();

            this.augmented = false;
        }

        protected IEdge FindReversedEdge(IEdge edge)
        {
            foreach (IEdge redge in this.VisitedGraph.OutEdges(edge.Target))
                if (redge.Target == edge.Source)
                    return redge;
            return null;
        }
    }
}
