// QuickGraph Library 
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//        1. The origin of this software must not be misrepresented; 
//        you must not claim that you wrote the original software. 
//        If you use this software in a product, an acknowledgment in the product 
//        documentation would be appreciated but is not required.
//
//        2. Altered source versions must be plainly marked as such, and must 
//        not be misrepresented as being the original software.
//
//        3. This notice may not be removed or altered from any source 
//        distribution.
//        
//        QuickGraph Library HomePage: http://www.mbunit.com
//        Author: Jonathan de Halleux



namespace QuickGraph.Algorithms.Search
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using QuickGraph.Collections;
    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Algorithms;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Concepts.Visitors;


    /// <summary>
    /// The EdgeDepthFirstSearchAlgorithm performs a depth-first traversal of the 
    /// edges in a directed graph.
    /// </summary>
    public class EdgeDepthFirstSearchAlgorithm :
        IAlgorithm
        ,ITreeEdgeBuilderAlgorithm
        ,IEdgeColorizerAlgorithm
		,IEdgePredecessorRecorderAlgorithm
    {
        private IEdgeListAndIncidenceGraph visitedGraph;
        private EdgeColorDictionary edgeColors;
		private int maxDepth = int.MaxValue;

        /// <summary>
        /// A depth first search algorithm on a directed graph
        /// </summary>
        /// <param name="g">The graph to traverse</param>
        /// <exception cref="ArgumentNullException">g is null</exception>
        public EdgeDepthFirstSearchAlgorithm(IEdgeListAndIncidenceGraph g)
            :this(g,new EdgeColorDictionary())
        {}

        /// <summary>
        /// A depth first search algorithm on a directed graph
        /// </summary>
        /// <param name="g">The graph to traverse</param>
        /// <param name="colors">vertex color map</param>
        /// <exception cref="ArgumentNullException">g or colors are null</exception>
        public EdgeDepthFirstSearchAlgorithm(
            IEdgeListAndIncidenceGraph g, 
            EdgeColorDictionary colors
            )
        {
            if (g == null)
                throw new ArgumentNullException("g");
            if (colors == null)
                throw new ArgumentNullException("Colors");

            visitedGraph = g;
            edgeColors = colors;
        }

        /// <summary>
        /// Gets the <see cref="IEdgeListAndIncidenceGraph"/> visited graph
        /// </summary>
        /// <value>
        /// The <see cref="IEdgeListAndIncidenceGraph"/> visited graph
        /// </value>
        public IEdgeListAndIncidenceGraph VisitedGraph
        {
            get
            {
                return visitedGraph;
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
        /// Gets the edge <see cref="GraphColor"/> dictionary
        /// </summary>
        /// <value>
        /// Edge <see cref="GraphColor"/> dictionary
        /// </value>
        public EdgeColorDictionary EdgeColors
        {
            get
            {
                return edgeColors;
            }
        }

        IDictionary IEdgeColorizerAlgorithm.EdgeColors
        {
            get
            {
                return this.EdgeColors;
            }
        }

		/// <summary>
		/// Gets or sets the maximum exploration depth, from
		/// the start edge.
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
		/// Invoked on every vertex of the graph before the start of the graph 
		/// search.
		/// </summary>
		public event EdgeEventHandler InitializeEdge;

		/// <summary>
		/// Triggers the ForwardOrCrossEdge event.
		/// </summary>
		/// <param name="e"></param>
		protected void OnInitializeEdge(IEdge e)
		{
			if (this.InitializeEdge!=null)
				InitializeEdge(this,new EdgeEventArgs(e));
		}

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
			if (this.StartVertex!=null)
				StartVertex(this,new VertexEventArgs(v));
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
			if (this.StartEdge!=null)
				StartEdge(this,new EdgeEventArgs(e));
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
				DiscoverTreeEdge(this, new EdgeEdgeEventArgs(se,e));
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
			if (this.ForwardOrCrossEdge!=null)
				ForwardOrCrossEdge(this,new EdgeEventArgs(e));
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
			if (this.FinishEdge!=null)
				FinishEdge(this,new EdgeEventArgs(e));
		}

		#endregion


		/// <summary>
		/// Compute the algorithm starting at the first vertex.
		/// </summary>
		public void Compute()
		{
			this.Compute(Traversal.FirstEdge(this.VisitedGraph).Source);
		}

        /// <summary>
        /// Execute the EDFS starting with the vertex s
        /// </summary>
        /// <param name="v">Starting vertex</param>
        public void Compute(IVertex v)
        {
            if (v==null)
                throw new ArgumentNullException("entry point");

			Initialize();

            // start whith him:
            OnStartVertex(v);

            // process each out edge of v
            foreach(IEdge e in VisitedGraph.OutEdges(v))
            {
                if (EdgeColors[e] == GraphColor.White)
                {
                    OnStartEdge(e);
                    Visit(e,0);
                }
            }

			// process the rest of the graph edges
			foreach(IEdge e in VisitedGraph.Edges)
			{
				if (EdgeColors[e]==GraphColor.White)
				{
					OnStartEdge(e);
					Visit(e,0);
				}
			}
        }

		/// <summary>
		/// Initiliaze color map
		/// </summary>
		public void Initialize()
		{
			// put all vertex to white
			foreach(IEdge e in VisitedGraph.Edges)
			{
				EdgeColors[e] = GraphColor.White;
				OnInitializeEdge(e);
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
			if (se==null)
				throw new ArgumentNullException("se");

			// mark edge as gray
            EdgeColors[se] = GraphColor.Gray;
            // add edge to the search tree
            OnTreeEdge(se);

            // iterate over out-edges
            foreach(IEdge e in VisitedGraph.OutEdges(se.Target))
            {
                // check edge is not explored yet,
                // if not, explore it.
				if (EdgeColors[e] == GraphColor.White)
				{
					OnDiscoverTreeEdge(se,e);
					Visit(e, depth+1);
				}
				else if (EdgeColors[e] == GraphColor.Gray)
				{
					// edge is being explored
					OnBackEdge(e);
				}
				else 
					// edge is black
					OnForwardOrCrossEdge(e);
            }

            // all out-edges have been explored
            EdgeColors[se] = GraphColor.Black;
            OnFinishEdge(se);
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

		/// <summary>
		/// Registers the handlers of a <see cref="IEdgeColorizerVisitor"/>
		/// visitor.
		/// </summary>
		/// <param name="vis">visitor to "attach"</param>
		public void RegisterEdgeColorizerHandlers(IEdgeColorizerVisitor vis)
		{
			if (vis==null)
				throw new ArgumentNullException("vis");

			this.InitializeEdge +=new EdgeEventHandler(vis.InitializeEdge);
			this.TreeEdge += new EdgeEventHandler(vis.TreeEdge);
			this.FinishEdge += new EdgeEventHandler(vis.FinishEdge);
		}

		public void RegisterEdgePredecessorRecorderHandlers(IEdgePredecessorRecorderVisitor vis)
		{
			this.InitializeEdge += new EdgeEventHandler(vis.InitializeEdge);
			this.DiscoverTreeEdge +=new EdgeEdgeEventHandler(vis.DiscoverTreeEdge);
			this.FinishEdge +=new EdgeEventHandler(vis.FinishEdge);
		}
    }
}
