using System;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.MutableTraversals;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Collections;
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
    public class GraphBalancerAlgorithm : IAlgorithm
    {
        private IMutableBidirectionalVertexAndEdgeListGraph visitedGraph;
        private IVertex source = null;
        private IVertex sink = null;

        private IVertex balancingSource = null;
        private IEdge balancingSourceEdge = null;
        private IVertex balancingSink = null;
        private IEdge balancingSinkEdge = null;
        private EdgeDoubleDictionary capacities = new EdgeDoubleDictionary();
        private EdgeIntDictionary preFlow = new EdgeIntDictionary();
        private VertexCollection surplusVertices = new VertexCollection();
        private EdgeCollection surplusEdges = new EdgeCollection();
        private VertexCollection deficientVertices = new VertexCollection();
        private EdgeCollection deficientEdges = new EdgeCollection();
        private bool balanced = false;

        public GraphBalancerAlgorithm(
            IMutableBidirectionalVertexAndEdgeListGraph visitedGraph, 
            IVertex source, 
            IVertex sink)
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");
            if (source == null)
                throw new ArgumentNullException("source");
            if (!visitedGraph.ContainsVertex(source))
                throw new ArgumentException("source is not part of the graph");
            if (sink == null)
                throw new ArgumentNullException("sink");
            if (!visitedGraph.ContainsVertex(sink))
                throw new ArgumentException("sink is not part of the graph");

            this.visitedGraph = visitedGraph;
            this.source = source;
            this.sink = sink;

            // setting capacities = u(e) = +infty
            foreach(IEdge edge in this.VisitedGraph.Edges)
                this.capacities.Add(edge,double.MaxValue);

            // setting preflow = l(e) = 1
            foreach (IEdge edge in this.VisitedGraph.Edges)
                this.preFlow.Add(edge, 1);
        }
        public GraphBalancerAlgorithm(
            IMutableBidirectionalVertexAndEdgeListGraph visitedGraph,
            IVertex source,
            IVertex sink,
            EdgeDoubleDictionary capacities)
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");
            if (source == null)
                throw new ArgumentNullException("source");
            if (!visitedGraph.ContainsVertex(source))
                throw new ArgumentException("source is not part of the graph");
            if (sink == null)
                throw new ArgumentNullException("sink");
            if (!visitedGraph.ContainsVertex(sink))
                throw new ArgumentException("sink is not part of the graph");
            if (capacities == null)
                throw new ArgumentNullException("capacities");

            this.visitedGraph = visitedGraph;
            this.source = source;
            this.sink = sink;
            this.capacities = capacities;

            // setting preflow = l(e) = 1
            foreach (IEdge edge in this.VisitedGraph.Edges)
                this.preFlow.Add(edge, 1);
        }

        #region Properties
        public IMutableBidirectionalVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }
        object IAlgorithm.VisitedGraph
        {
            get
            {
                return this.VisitedGraph;
            }
        }
        public bool Balanced
        {
            get
            {
                return this.balanced;
            }
        }

        public IVertex Source
        {
            get
            {
                return this.source;
            }
        }
        public IVertex Sink
        {
            get
            {
                return this.sink;
            }
        }
        public IVertex BalancingSource
        {
            get
            {
                return this.balancingSource;
            }
        }
        public IEdge BalancingSourceEdge
        {
            get
            {
                return this.balancingSourceEdge;
            }
        }
        public IVertex BalancingSink
        {
            get
            {
                return this.balancingSink;
            }
        }
        public IEdge BalancingSinkEdge
        {
            get
            {
                return this.balancingSinkEdge;
            }
        }
        public IVertexCollection SurplusVertices
        {
            get
            {
                return this.surplusVertices;
            }
        }
        public IEdgeCollection SurplusEdges
        {
            get
            {
                return this.surplusEdges;
            }
        }
        public IVertexCollection DeficientVertices
        {
            get
            {
                return this.deficientVertices;
            }
        }
        public IEdgeCollection DeficientEdges
        {
            get
            {
                return this.deficientEdges;
            }
        }
        public EdgeDoubleDictionary Capacities
        {
            get
            {
                return this.capacities;
            }
        }
        #endregion

        #region Events
        public event VertexEventHandler BalancingSourceAdded;
        protected virtual void OnBalancingSourceAdded()
        {
            if (this.BalancingSourceAdded != null)
                this.BalancingSourceAdded(this, new VertexEventArgs(this.source));
        }
        public event VertexEventHandler BalancingSinkAdded;
        protected virtual void OnBalancingSinkAdded()
        {
            if (this.BalancingSinkAdded != null)
                this.BalancingSinkAdded(this, new VertexEventArgs(this.sink));
        }
        public event EdgeEventHandler EdgeAdded;
        protected virtual void OnEdgeAdded(IEdge edge)
        {
            if (this.EdgeAdded != null)
                this.EdgeAdded(this, new EdgeEventArgs(edge));
        }
        public event VertexEventHandler SurplusVertexAdded;
        protected virtual void OnSurplusVertexAdded(IVertex vertex)
        {
            if (this.SurplusVertexAdded != null)
                this.SurplusVertexAdded(this, new VertexEventArgs(vertex));
        }
        public event VertexEventHandler DeficientVertexAdded;
        protected virtual void OnDeficientVertexAdded(IVertex vertex)
        {
            if (this.DeficientVertexAdded != null)
                this.DeficientVertexAdded(this, new VertexEventArgs(vertex));
        }
        #endregion

        #region Balancing
        public int GetBalancingIndex(IVertex v)
        {
            int bi =  0;
            foreach(IEdge edge in this.VisitedGraph.OutEdges(v))
            {
                int pf = this.preFlow[edge];
                bi+=pf;
            }
            foreach(IEdge edge in this.VisitedGraph.InEdges(v))
            {
                int pf = this.preFlow[edge];
                bi-=pf;
            }
            return bi;
        }

        public void Balance()
        {
            if (this.Balanced)
                throw new InvalidOperationException("Graph already balanced");

            // step 0
            // create new source, new sink
            this.balancingSource = this.visitedGraph.AddVertex();
            this.OnBalancingSourceAdded();
            this.balancingSink = this.visitedGraph.AddVertex();
            this.OnBalancingSinkAdded();

            // step 1
            this.balancingSourceEdge = this.VisitedGraph.AddEdge(this.BalancingSource, this.Source);
            this.capacities.Add(this.balancingSourceEdge, double.MaxValue);
            this.preFlow.Add(this.balancingSourceEdge, 0);
            OnEdgeAdded(balancingSourceEdge);

            this.balancingSinkEdge = this.VisitedGraph.AddEdge(this.Sink, this.BalancingSink);
            this.capacities.Add(this.balancingSinkEdge, double.MaxValue);
            this.preFlow.Add(this.balancingSinkEdge, 0);
            OnEdgeAdded(balancingSinkEdge);

            // step 2
            // for each surplus vertex v, add (source -> v)
            foreach (IVertex v in this.VisitedGraph.Vertices)
            {
                if (v == this.balancingSource)
                    continue;
                if (v == this.balancingSink)
                    continue;
                if (v == this.source)
                    continue;
                if (v == this.sink)
                    continue;

                int balacingIndex = this.GetBalancingIndex(v);
                if (balacingIndex==0)
                    continue;

                if (balacingIndex < 0)
                {
                    // surplus vertex
                    IEdge edge = this.VisitedGraph.AddEdge(this.BalancingSource, v);
                    this.surplusEdges.Add(edge);
                    this.surplusVertices.Add(v);
                    this.preFlow.Add(edge, 0);
                    this.capacities.Add(edge, -balacingIndex);
                    OnSurplusVertexAdded(v);
                    OnEdgeAdded(edge);
                }
                else
                {
                    // deficient vertex
                    IEdge edge = this.VisitedGraph.AddEdge(v, this.BalancingSink);
                    this.deficientEdges.Add(edge);
                    this.deficientVertices.Add(v);
                    this.preFlow.Add(edge, 0);
                    this.capacities.Add(edge, balacingIndex);
                    OnDeficientVertexAdded(v);
                    OnEdgeAdded(edge);
                }
            }

            this.balanced = true;
        }

        public void UnBalance()
        {
            if (!this.Balanced)
                throw new InvalidOperationException("Graph is not balanced");
            foreach (IEdge edge in this.surplusEdges)
            {
                this.VisitedGraph.RemoveEdge(edge);
                this.capacities.Remove(edge);
                this.preFlow.Remove(edge);
            }
            foreach (IEdge edge in this.deficientEdges)
            {
                this.VisitedGraph.RemoveEdge(edge);
                this.capacities.Remove(edge);
                this.preFlow.Remove(edge);
            }

            this.capacities.Remove(this.BalancingSinkEdge);
            this.capacities.Remove(this.BalancingSourceEdge);
            this.preFlow.Remove(this.BalancingSinkEdge);
            this.preFlow.Remove(this.BalancingSourceEdge);
            this.VisitedGraph.RemoveEdge(this.BalancingSourceEdge);
            this.VisitedGraph.RemoveEdge(this.BalancingSinkEdge);
            this.VisitedGraph.RemoveVertex(this.BalancingSource);
            this.VisitedGraph.RemoveVertex(this.BalancingSink);

            this.balancingSource = null;
            this.balancingSink = null;
            this.balancingSourceEdge = null;
            this.balancingSinkEdge = null;

            this.surplusEdges.Clear();
            this.deficientEdges.Clear();
            this.surplusVertices.Clear();
            this.deficientVertices.Clear();

            this.balanced = false;
        }
        #endregion
    }
}
