using System;
using System.Collections;
using QuickGraph.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Visitors;

namespace QuickGraph.Algorithms.Search
{
    public class ImplicitDepthFirstSearchAlgorithm :
        IPredecessorRecorderAlgorithm,
        ITimeStamperAlgorithm,
        ITreeEdgeBuilderAlgorithm
    {
        private IIncidenceGraph visitedGraph;
        private int maxDepth = int.MaxValue;
        private VertexColorDictionary colors = new VertexColorDictionary();

        public ImplicitDepthFirstSearchAlgorithm(IIncidenceGraph visitedGraph)
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
        public VertexColorDictionary Colors
        {
            get
            {
                return this.colors;
            }
        }
/*
        /// <summary>
        /// IVertexColorizerAlgorithm implementation
        /// </summary>
        IDictionary IVertexColorizerAlgorithm.Colors
        {
            get
            {
                return this.Colors;
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
        /// Raises the <see cref="StartVertex"/> event.
        /// </summary>
        /// <param name="v">vertex that raised the event</param>
        protected void OnStartVertex(IVertex v)
        {
            if (StartVertex != null)
                StartVertex(this, new VertexEventArgs(v));
        }

        /// <summary>
        /// Invoked when a vertex is encountered for the first time. 
        /// </summary>
        public event VertexEventHandler DiscoverVertex;


        /// <summary>
        /// Raises the <see cref="DiscoverVertex"/> event.
        /// </summary>
        /// <param name="v">vertex that raised the event</param>
        protected void OnDiscoverVertex(IVertex v)
        {
            if (DiscoverVertex != null)
                DiscoverVertex(this, new VertexEventArgs(v));
        }

        /// <summary>
        /// Invoked on every out-edge of each vertex after it is discovered. 
        /// </summary>
        public event EdgeEventHandler ExamineEdge;


        /// <summary>
        /// Raises the <see cref="ExamineEdge"/> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        protected void OnExamineEdge(IEdge e)
        {
            if (ExamineEdge != null)
                ExamineEdge(this, new EdgeEventArgs(e));
        }

        /// <summary>
        /// Invoked on each edge as it becomes a member of the edges that form 
        /// the search tree. If you wish to record predecessors, do so at this 
        /// event point. 
        /// </summary>
        public event EdgeEventHandler TreeEdge;


        /// <summary>
        /// Raises the <see cref="TreeEdge"/> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
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
        /// Raises the <see cref="BackEdge"/> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
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
        /// Raises the <see cref="ForwardOrCrossEdge"/> event.
        /// </summary>
        /// <param name="e">edge that raised the event</param>
        protected void OnForwardOrCrossEdge(IEdge e)
        {
            if (ForwardOrCrossEdge != null)
                ForwardOrCrossEdge(this, new EdgeEventArgs(e));
        }

        /// <summary>
        /// Invoked on a vertex after all of its out edges have been added to 
        /// the search tree and all of the adjacent vertices have been 
        /// discovered (but before their out-edges have been examined). 
        /// </summary>
        public event VertexEventHandler FinishVertex;

        /// <summary>
        /// Raises the <see cref="FinishVertex"/> event.
        /// </summary>
        /// <param name="v">vertex that raised the event</param>
        protected void OnFinishVertex(IVertex v)
        {
            if (FinishVertex != null)
                FinishVertex(this, new VertexEventArgs(v));
        }
        #endregion

        /// <summary>
        /// Does an implicit depth first search on the graph
        /// </summary>
        /// <param name="startVertex">
        /// Start vertex of the depth first search
        /// </param>
        public void Compute(IVertex startVertex)
        {
            if (startVertex==null)
                throw new ArgumentNullException("startVertex");

            this.Initialize();
            this.Visit(startVertex, 0);
        }

        /// <summary>
        /// Initializes the algorithm before computation.
        /// </summary>
        public virtual void Initialize()
        {
            this.Colors.Clear();
        }

        /// <summary>
        /// Visit vertex <paramref name="u"/>.
        /// </summary>
        /// <param name="u"></param>
        /// <param name="depth"></param>
        public virtual void Visit(IVertex u, int depth)
        {
            if (depth > this.MaxDepth)
                return;

            Colors[u] = GraphColor.Gray;
            OnDiscoverVertex(u);

            IVertex v = null;
            foreach (IEdge e in VisitedGraph.OutEdges(u))
            {
                OnExamineEdge(e);
                v = e.Target;

                if (!this.Colors.Contains(v))
                {
                    OnTreeEdge(e);
                    Visit(v, depth + 1);
                }
                else
                {
                    GraphColor c = Colors[v];
                    if (c == GraphColor.Gray)
                    {
                        OnBackEdge(e);
                    }
                    else
                    {
                        OnForwardOrCrossEdge(e);
                    }
                }
            }

            Colors[u] = GraphColor.Black;
            OnFinishVertex(u);
        }

        /// <summary>
        /// Registers the predecessors handler
        /// </summary>
        /// <param name="vis"></param>
        public void RegisterPredecessorRecorderHandlers(IPredecessorRecorderVisitor vis)
        {
            if (vis == null)
                throw new ArgumentNullException("visitor");
            TreeEdge += new EdgeEventHandler(vis.TreeEdge);
            FinishVertex += new VertexEventHandler(vis.FinishVertex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vis"></param>
        public void RegisterTimeStamperHandlers(ITimeStamperVisitor vis)
        {
            if (vis == null)
                throw new ArgumentNullException("visitor");

            DiscoverVertex += new VertexEventHandler(vis.DiscoverVertex);
            FinishVertex += new VertexEventHandler(vis.FinishVertex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vis"></param>
        public void RegisterTreeEdgeBuilderHandlers(ITreeEdgeBuilderVisitor vis)
        {
            if (vis == null)
                throw new ArgumentNullException("visitor");

            TreeEdge += new EdgeEventHandler(vis.TreeEdge);
        }
    }
}