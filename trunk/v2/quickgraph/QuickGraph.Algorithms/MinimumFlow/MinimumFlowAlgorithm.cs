using System;
using QuickGraph.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Algorithms.MaximumFlow;
using QuickGraph.Representations;
using QuickGraph.Concepts.Collections;

namespace QuickGraph.Algorithms.MinimumFlow
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <para>
    /// Algorithm extracted from <em>Efficient Algorithms for Constructing Testing Sets, Covering Paths and 
    /// Minimum Flows</em>, Alfred V. Aho, David Lee.
    /// </para>
    /// </remarks>
    public class MinimumFlowAlgorithm : IAlgorithm
    {
        private BidirectionalGraph visitedGraph;
        private EdgeDoubleDictionary capacities;
        private ReversedEdgeAugmentorAlgorithm reverser=null;
        private GraphBalancerAlgorithm balancer=null;
        private MaximumFlowAlgorithm maxFlowF1 = null;

        public MinimumFlowAlgorithm(
            BidirectionalGraph visitedGraph, 
            EdgeDoubleDictionary capacities)
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");
            if (capacities == null)
                throw new ArgumentNullException("capacities");
            this.visitedGraph = visitedGraph;
            this.capacities = capacities;

            this.Initialize();
        }

        public MinimumFlowAlgorithm(BidirectionalGraph visitedGraph)
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");
            if (capacities == null)
                throw new ArgumentNullException("capacities");
            this.visitedGraph = visitedGraph;
            this.capacities = new EdgeDoubleDictionary();
            foreach (IEdge edge in this.visitedGraph.Edges)
                this.capacities.Add(edge, double.MaxValue);

            this.Initialize();
        }

        private void Initialize()
        {
            this.reverser = new ReversedEdgeAugmentorAlgorithm(this.VisitedGraph);
            this.reverser.ReversedEdgeAdded += new EdgeEventHandler(reverser_ReversedEdgeAdded);
            this.maxFlowF1 = new PushRelabelMaximumFlowAlgorithm(
                this.VisitedGraph,
                this.capacities,
                this.reverser.ReversedEdges);
        }

        public BidirectionalGraph VisitedGraph
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

        public void Compute(IVertex source, IVertex sink)
        {
            // step 1 constructor balancing graph
            this.balancer = new GraphBalancerAlgorithm(
                this.VisitedGraph,
                source,
                sink,
                this.capacities
                );

            balancer.Balance();
            this.capacities[balancer.BalancingSourceEdge] = 0;
            this.capacities[balancer.BalancingSinkEdge] = 0;

            // step 2 find max flow
            reverser.AddReversedEdges();
            maxFlowF1.Compute(source, sink);

            // step 3
            this.capacities[balancer.BalancingSourceEdge] = double.MaxValue;
            foreach (IEdge edge in balancer.SurplusEdges)
            {
                IVertex v = edge.Target;
                // find edges
                foreach (IEdge vs in this.VisitedGraph.OutEdges(v))
                {
                    if (vs.Target == balancer.BalancingSource)
                        this.capacities[vs] = 0;
                }
            }

            // step 4:

 //           reverser.RemoveReversedEdges();
   //         balancer.UnBalance();
        }

        void reverser_ReversedEdgeAdded(Object sender, EdgeEventArgs e)
        {
            this.capacities[e.Edge] = 0;
        }
    }
}
