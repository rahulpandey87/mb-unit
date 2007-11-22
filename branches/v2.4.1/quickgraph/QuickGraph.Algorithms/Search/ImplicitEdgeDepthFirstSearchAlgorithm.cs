using System;
using System.Collections;
using QuickGraph.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Visitors;

namespace QuickGraph.Algorithms.Search
{
    public class ImplicitEdgeDepthFirstSearchAlgorithm :
        IAlgorithm
        ,ITreeEdgeBuilderAlgorithm
    {
        private IIncidenceGraph visitedGraph;
        private int maxDepth = int.MaxValue;
        private EdgeColorDictionary edgeColors = new EdgeColorDictionary();

        public ImplicitEdgeDepthFirstSearchAlgorithm(IIncidenceGraph visitedGraph)
        {
            if (visitedGraph == null)
                throw new ArgumentNullException("visitedGraph");
            this.visitedGraph = visitedGraph;
        }

        /// <summary>
        /// Gets the Visited graph
        /// </summary>
        public IIncidenceGraph VisitedGraph
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

        /// <summary>
        /// Gets the vertex color map
        /// </summary>
        /// <value>
        /// Vertex color (<see cref="GraphColor"/>) dictionary
        /// </value>
        public EdgeColorDictionary EdgeColors
        {
            get
            {
                return this.edgeColors;
            }
        }
/*
        IDictionary IEdgeColorizerAlgorithm.EdgeColors
        {
            get
            {
                return this.EdgeColors;
            }
        }
*/
        /// <summary>
        /// Gets or sets the maximum exploration depth, from
        /// the start vertex.
        /// </summary>
        /// <remarks>
        /// Defaulted at <c>int.MaxValue</c>.
        /// </remarks>
        /// <value>
        /// Maximum exploration depth.
        /// </value>
        public int MaxDepth
        {
            get
            {
                return this.maxDepth;
            }
            set
            {
                this.maxDepth = value;
            }
        }


        #region Events
        /// <summary>
        /// Invoked on the source vertex once before the start of the search. 
        /// </summary>
        public event VertexEventHandler StartVertex;

        /// <summary>
        /// Triggers the StartVertex event.
        /// </summary>
        /// <param name="v"></param>
        protected void OnStartVertex(IVertex v)
        {
            if (this.StartVertex != null)
                StartVertex(this, new VertexEventArgs(v));
        }

        /// <summary>
        /// Invoked on the first edge of a test case
        /// </summary>
        public event EdgeEventHandler StartEdge;

        /// <summary>
        /// Triggers the StartEdge event.
        /// </summary>
        /// <param name="e"></param>
        protected void OnStartEdge(IEdge e)
        {
            if (this.StartEdge != null)
                StartEdge(this, new EdgeEventArgs(e));
        }

        /// <summary>
        /// 
        /// </summary>
        public event EdgeEdgeEventHandler DiscoverTreeEdge;

        /// <summary>
        /// Triggers DiscoverEdge event
        /// </summary>
        /// <param name="se"></param>
        /// <param name="e"></param>
        public void OnDiscoverTreeEdge(IEdge se, IEdge e)
        {
            if (DiscoverTreeEdge != null)
                DiscoverTreeEdge(this, new EdgeEdgeEventArgs(se, e));
        }

        /// <summary>
        /// Invoked on each edge as it becomes a member of the edges that form 
        /// the search tree. If you wish to record predecessors, do so at this 
        /// event point. 
        /// </summary>
        public event EdgeEventHandler TreeEdge;

        /// <summary>
        /// Triggers the TreeEdge event.
        /// </summary>
        /// <param name="e"></param>
        protected void OnTreeEdge(IEdge e)
        {
            if (TreeEdge != null)
                TreeEdge(this, new EdgeEventArgs(e));
        }

        /// <summary>
        /// Invoked on the back edges in the graph. 
        /// </summary>
        public event EdgeEventHandler BackEdge;

        /// <summary>
        /// Triggers the BackEdge event.
        /// </summary>
        /// <param name="e"></param>
        protected void OnBackEdge(IEdge e)
        {
            if (BackEdge != null)
                BackEdge(this, new EdgeEventArgs(e));
        }

        /// <summary>
        /// Invoked on forward or cross edges in the graph. 
        /// (In an undirected graph this method is never called.) 
        /// </summary>
        public event EdgeEventHandler ForwardOrCrossEdge;

        /// <summary>
        /// Triggers the ForwardOrCrossEdge event.
        /// </summary>
        /// <param name="e"></param>
        protected void OnForwardOrCrossEdge(IEdge e)
        {
            if (this.ForwardOrCrossEdge != null)
                ForwardOrCrossEdge(this, new EdgeEventArgs(e));
        }

        /// <summary>
        /// Invoked on a edge after all of its out edges have been added to 
        /// the search tree and all of the adjacent vertices have been 
        /// discovered (but before their out-edges have been examined). 
        /// </summary>
        public event EdgeEventHandler FinishEdge;

        /// <summary>
        /// Triggers the ForwardOrCrossEdge event.
        /// </summary>
        /// <param name="e"></param>
        protected void OnFinishEdge(IEdge e)
        {
            if (this.FinishEdge != null)
                FinishEdge(this, new EdgeEventArgs(e));
        }

        #endregion

        /// <summary>
        /// Does an implicit depth first search on the graph
        /// </summary>
        /// <param name="startVertex">
        /// Start vertex of the depth first search
        /// </param>
        public void Compute(IVertex v)
        {
            if (v == null)
                throw new ArgumentNullException("v");

            // initialize algorithm
            this.Initialize();

            // start whith him:
            OnStartVertex(v);

            // process each out edge of v
            foreach (IEdge e in this.VisitedGraph.OutEdges(v))
            {
                if (!this.EdgeColors.Contains(e))
                {
                    OnStartEdge(e);
                    Visit(e, 0);
                }
            }
        }

        /// <summary>
        /// Does a depth first search on the vertex u
        /// </summary>
        /// <param name="se">edge to explore</param>
        /// <param name="depth">current exploration depth</param>
        /// <exception cref="ArgumentNullException">se cannot be null</exception>
        public void Visit(IEdge se, int depth)
        {
            if (depth > this.maxDepth)
                return;
            if (se == null)
                throw new ArgumentNullException("se");

            // mark edge as gray
            this.EdgeColors[se] = GraphColor.Gray;
            // add edge to the search tree
            OnTreeEdge(se);

            // iterate over out-edges
            foreach (IEdge e in this.VisitedGraph.OutEdges(se.Target))
            {
                // check edge is not explored yet,
                // if not, explore it.
                if (!this.EdgeColors.Contains(e))
                {
                    OnDiscoverTreeEdge(se, e);
                    Visit(e, depth + 1);
                }
                else
                {
                    GraphColor c = this.EdgeColors[e];
                    if (EdgeColors[e] == GraphColor.Gray)
                        OnBackEdge(e);
                    else
                        OnForwardOrCrossEdge(e);
                }
            }

            // all out-edges have been explored
            this.EdgeColors[se] = GraphColor.Black;
            OnFinishEdge(se);
        }

        /// <summary>
        /// Initializes the algorithm before computation.
        /// </summary>
        public virtual void Initialize()
        {
            this.EdgeColors.Clear();
        }

        /// <summary>
        /// Registers the handlers of a <see cref="ITreeEdgeBuilderVisitor"/>
        /// visitor.
        /// </summary>
        /// <param name="vis">visitor to "attach"</param>
        public void RegisterTreeEdgeBuilderHandlers(ITreeEdgeBuilderVisitor vis)
        {
            if (vis == null)
                throw new ArgumentNullException("visitor");

            TreeEdge += new EdgeEventHandler(vis.TreeEdge);
        }
/*
        /// <summary>
        /// Registers the handlers of a <see cref="IEdgeColorizerVisitor"/>
        /// visitor.
        /// </summary>
        /// <param name="vis">visitor to "attach"</param>
        public void RegisterEdgeColorizerHandlers(IEdgeColorizerVisitor vis)
        {
            if (vis == null)
                throw new ArgumentNullException("vis");

            this.InitializeEdge += new EdgeEventHandler(vis.InitializeEdge);
            this.TreeEdge += new EdgeEventHandler(vis.TreeEdge);
            this.FinishEdge += new EdgeEventHandler(vis.FinishEdge);
        }

        public void RegisterEdgePredecessorRecorderHandlers(IEdgePredecessorRecorderVisitor vis)
        {
            this.InitializeEdge += new EdgeEventHandler(vis.InitializeEdge);
            this.DiscoverTreeEdge += new EdgeEdgeEventHandler(vis.DiscoverTreeEdge);
            this.FinishEdge += new EdgeEventHandler(vis.FinishEdge);
        }
*/
    }
}