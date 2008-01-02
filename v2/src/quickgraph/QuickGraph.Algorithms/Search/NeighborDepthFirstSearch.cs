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
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux



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
	/// The DepthFirstSearchAlgorithm performs a depth-first traversal of the 
	/// vertices in a directed graph.
	/// </summary>
	public class NeighborDepthFirstSearchAlgorithm :
		IAlgorithm,
		IPredecessorRecorderAlgorithm,
		ITimeStamperAlgorithm,
		IVertexColorizerAlgorithm,
		ITreeEdgeBuilderAlgorithm
	{
		private IBidirectionalVertexListGraph visitedGraph;
		private VertexColorDictionary colors;
		private int maxDepth = int.MaxValue;

		/// <summary>
		/// A depth first search algorithm on a directed graph
		/// </summary>
		/// <param name="g">The graph to traverse</param>
		/// <exception cref="ArgumentNullException">g is null</exception>
		public NeighborDepthFirstSearchAlgorithm(IBidirectionalVertexListGraph g)
		{
			if (g == null)
				throw new ArgumentNullException("g");
			this.visitedGraph = g;
			this.colors = new VertexColorDictionary();
		}

		/// <summary>
		/// A depth first search algorithm on a directed graph
		/// </summary>
		/// <param name="g">The graph to traverse</param>
		/// <param name="colors">vertex color map</param>
		/// <exception cref="ArgumentNullException">g or colors are null</exception>
		public NeighborDepthFirstSearchAlgorithm(
			IBidirectionalVertexListGraph g, 
			VertexColorDictionary colors
			)
		{
			if (g == null)
				throw new ArgumentNullException("g");
			if (colors == null)
				throw new ArgumentNullException("Colors");

			this.visitedGraph = g;
			this.colors = colors;
		}

		/// <summary>
		/// Visited graph
		/// </summary>
		public IBidirectionalVertexListGraph VisitedGraph
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
		/// Invoked on every vertex of the graph before the start of the graph 
		/// search.
		/// </summary>
		public event VertexEventHandler InitializeVertex;

		/// <summary>
		/// Raises the <see cref="InitializeVertex"/> event.
		/// </summary>
		/// <param name="v">vertex that raised the event</param>
		protected void OnInitializeVertex(IVertex v)
		{
			if (InitializeVertex!=null)
				InitializeVertex(this, new VertexEventArgs(v));
		}

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
			if (StartVertex!=null)
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
			if (DiscoverVertex!=null)
				DiscoverVertex(this, new VertexEventArgs(v));
		}

		/// <summary>
		/// Invoked on every out-edge of each vertex after it is discovered. 
		/// </summary>
		public event EdgeEventHandler ExamineOutEdge;


		/// <summary>
		/// Raises the <see cref="ExamineOutEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnExamineOutEdge(IEdge e)
		{
			if (ExamineOutEdge!=null)
				ExamineOutEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on every out-edge of each vertex after it is discovered. 
		/// </summary>
		public event EdgeEventHandler ExamineInEdge;


		/// <summary>
		/// Raises the <see cref="ExamineInEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnExamineInEdge(IEdge e)
		{
			if (ExamineInEdge!=null)
				ExamineInEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on each edge as it becomes a member of the edges that form 
		/// the search tree. If you wish to record predecessors, do so at this 
		/// event point. 
		/// </summary>
		public event EdgeEventHandler TreeOutEdge;


		/// <summary>
		/// Raises the <see cref="TreeOutEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnTreeOutEdge(IEdge e)
		{
			if (TreeOutEdge!=null)
				TreeOutEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on each edge as it becomes a member of the edges that form 
		/// the search tree. If you wish to record predecessors, do so at this 
		/// event point. 
		/// </summary>
		public event EdgeEventHandler TreeInEdge;


		/// <summary>
		/// Raises the <see cref="TreeInEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnTreeInEdge(IEdge e)
		{
			if (TreeInEdge!=null)
				TreeInEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on the back edges in the graph. 
		/// </summary>
		public event EdgeEventHandler BackOutEdge;


		/// <summary>
		/// Raises the <see cref="BackOutEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnBackOutEdge(IEdge e)
		{
			if (BackOutEdge!=null)
				BackOutEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on the back edges in the graph. 
		/// </summary>
		public event EdgeEventHandler BackInEdge;


		/// <summary>
		/// Raises the <see cref="BackInEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnBackInEdge(IEdge e)
		{
			if (BackInEdge!=null)
				BackInEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on forward or cross edges in the graph. 
		/// (In an undirected graph this method is never called.) 
		/// </summary>
		public event EdgeEventHandler ForwardOrCrossOutEdge;


		/// <summary>
		/// Raises the <see cref="ForwardOrCrossOutEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnForwardOrCrossOutEdge(IEdge e)
		{
			if (ForwardOrCrossOutEdge!=null)
				ForwardOrCrossOutEdge(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on forward or cross edges in the graph. 
		/// (In an undirected graph this method is never called.) 
		/// </summary>
		public event EdgeEventHandler ForwardOrCrossInEdge;


		/// <summary>
		/// Raises the <see cref="ForwardOrCrossInEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnForwardOrCrossInEdge(IEdge e)
		{
			if (ForwardOrCrossInEdge!=null)
				ForwardOrCrossInEdge(this, new EdgeEventArgs(e));
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
			if (FinishVertex!=null)
				FinishVertex(this, new VertexEventArgs(v));
		}
		#endregion

		/// <summary>
		/// Execute the DFS search.
		/// </summary>
		public void Compute()
		{
			Compute(null);
		}

		/// <summary>
		/// Execute the DFS starting with the vertex s
		/// </summary>
		/// <param name="s">Starting vertex</param>
		public void Compute(IVertex s)
		{
			// put all vertex to white
			Initialize();

			// if there is a starting vertex, start whith him:
			if (s != null)
			{
				OnStartVertex(s);
				Visit(s,0);
			}

			// process each vertex 
			foreach(IVertex u in VisitedGraph.Vertices)
			{
				if (Colors[u] == GraphColor.White)
				{
					OnStartVertex(u);
					Visit(u,0);
				}
			}
		}

		/// <summary>
		/// Initializes the vertex color map
		/// </summary>
		/// <remarks>
		/// </remarks>
		public void Initialize()
		{
			foreach(IVertex u in VisitedGraph.Vertices)
			{
				Colors[u] = GraphColor.White;
				OnInitializeVertex(u);
			}
		}

		/// <summary>
		/// Does a depth first search on the vertex u
		/// </summary>
		/// <param name="u">vertex to explore</param>
		/// <param name="depth">current recursion depth</param>
		/// <exception cref="ArgumentNullException">u cannot be null</exception>
		public void Visit(IVertex u, int depth)
		{
			if (depth > this.maxDepth)
				return;
			if (u==null)
				throw new ArgumentNullException("u");

			Colors[u] = GraphColor.Gray;
			OnDiscoverVertex(u);

			IVertex v = null;

			foreach(IEdge e in VisitedGraph.OutEdges(u))
			{
				OnExamineOutEdge(e);
				v = e.Target;
				GraphColor c=Colors[v];
				if (c == GraphColor.White)
				{
					OnTreeOutEdge(e);
					Visit(v,depth+1);
				}
				else if (c == GraphColor.Gray)
				{
					OnBackOutEdge(e);
				}
				else
				{
					OnForwardOrCrossOutEdge(e);
				}
			}

			foreach(IEdge e in VisitedGraph.InEdges(u))
			{
				OnExamineInEdge(e);
				v = e.Source;
				GraphColor c=Colors[v];
				if (c == GraphColor.White)
				{
					OnTreeInEdge(e);
					Visit(v,depth+1);
				}
				else if (c == GraphColor.Gray)
				{
					OnBackInEdge(e);
				}
				else
				{
					OnForwardOrCrossInEdge(e);
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
			TreeOutEdge += new EdgeEventHandler(vis.TreeEdge);
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
		public void RegisterVertexColorizerHandlers(IVertexColorizerVisitor vis)
		{
			if (vis == null)
				throw new ArgumentNullException("visitor");

			InitializeVertex += new VertexEventHandler(vis.InitializeVertex);
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

			TreeOutEdge += new EdgeEventHandler(vis.TreeEdge);
			TreeInEdge += new EdgeEventHandler(vis.TreeEdge);
		}
	}
}
