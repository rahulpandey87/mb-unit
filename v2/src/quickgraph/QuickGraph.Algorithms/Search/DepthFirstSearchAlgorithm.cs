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
	/// <remarks>
	/// <para>
	/// When possible, a depth-first traversal chooses a vertex adjacent to 
	/// the current vertex to visit next. If all adjacent vertices have 
	/// already been discovered, or there are no adjacent vertices, 
	/// then the algorithm backtracks to the last vertex that had undiscovered 
	/// neighbors. Once all reachable vertices have been visited, the algorithm 
	/// selects from any remaining undiscovered vertices and continues the 
	/// traversal. The algorithm finishes when all vertices have been visited. 
	/// </para>
	/// <para>
	/// Depth-first search is useful for categorizing edges in a graph, 
	/// and for imposing an ordering on the vertices.
	/// </para>
	/// <para>
	/// Similar to the <seealso cref="BreadthFirstSearchAlgorithm"/>, color 
	/// markers are used to keep track of which vertices have been discovered. 
	/// White marks vertices that have yet to be discovered, 
	/// gray marks a vertex that is discovered but still has vertices adjacent 
	/// to it that are undiscovered. A black vertex is discovered vertex that 
	/// is not adjacent to any white vertices. 
	/// </para>
	/// <para>The main loop pseudo-code is as follows:
	/// <code>
	/// IVertexListGraph g;
	/// DFS(IVertex s)
	/// {
	///     // initialize vertex colors
	///     foreach(IVertex v in g.Vertices)
	///     {
	///         Colors[v] = White;
	///         InitializeVertex(v); // event
	///     }
	/// 
	///     // if there is a starting vertex, visit it
	///     if (s != null)
	///     {
	///         StartVertex(s); // event
	///         Visit(s);
	///     }
	/// 
	///     // visit all vertices, if not previously visited
	///     foreach(IVertex v in g.Vertices)
	///     {
	///         if (Colors[v] != White)
	///         {
	///             StartVertex(v); // event
	///             Visit(v);
	///         }
	///     }
	/// }
	/// </code>
	/// </para>
	/// <para>The Visit method pseudo-code is as follows:
	/// <code>
	/// Visit(IVertexListGraph g, IVertex u)
	/// {
	///     Colors[u] = Gray;
	///     OnDiscoverVertex(u); // event
	///     
	///     // examine edges
	///     foreach(IEdge e in g.OutEdges(u))
	///     {
	///			OnExamineEdge(e); // event
	///			if (Colors[u] == White)
	///			{
	///			    OnTreeEdge(e); // event
	///			    Visit(e.Target);
	///			}
	///			else if (Colors[u] == Gray)
	///			{
	///			    OnBackEdge(e); // event
	///			}
	///			else
	///				OnForwardOrCrossEdge(e); // event
	///     }
	///     
	///     Colors[u] = Black;
	///     OnFinishVertex(u); // event
	/// }
	/// </code>
	/// </para>
	/// <para>In itself the algorithm does not take action, it is the user
	/// job to attach handlers to the different events that take part during
	/// the algorithm:
	/// <list type="bullet">
	/// <listheader>
	///		<term>Event</term>
	///		<description>When</description>
	/// </listheader>
	/// <item>
	///		<term>InitializeVertex</term>
	///		<description>Invoked on every vertex of the graph before the start of the graph 
	/// search.</description>
	/// </item>
	/// <item>
	///		<term>StartVertex</term>
	///		<description>Invoked on the source vertex once before the start of the search.</description>
	/// </item>
	/// <item>
	///		<term>DiscoverVertex</term>
	///		<description>Invoked when a vertex is encountered for the first time. </description>
	/// </item>
	/// <item>
	///		<term>ExamineEdge</term>
	///		<description>Invoked on every out-edge of each vertex after it is discovered.</description>
	/// </item>
	/// <item>
	///		<term>TreeEdge</term>
	///		<description>Invoked on each edge as it becomes a member of the edges that form 
	/// the search tree. If you wish to record predecessors, do so at this 
	/// event point. </description>
	/// </item>
	/// <item>
	///		<term>BackEdge</term>
	///		<description>Invoked on the back edges in the graph. </description>
	/// </item>
	/// <item>
	///		<term>FowardOrCrossEdge</term>
	///		<description>Invoked on forward or cross edges in the graph. 
	/// (In an undirected graph this method is never called.)</description>
	/// </item>
	/// <item>
	///		<term>FinishVertex</term>
	///		<description>Invoked on a vertex after all of its out edges have been added to 
	/// the search tree and all of the adjacent vertices have been 
	/// discovered (but before their out-edges have been examined).</description>
	/// </item>
	/// </list>
	/// </para>
	/// <para>
	/// Predifined visitors, such as <seealso cref="QuickGraph.Concepts.Visitors.IPredecessorRecorderVisitor"/>
	/// and <seealso cref="QuickGraph.Concepts.Visitors.ITimeStamperVisitor"/>
	/// can be used with this algorithm.
	/// </para>
	/// <para>This algorithm is directly inspired from the
	/// BoostGraphLibrary implementation.
	/// </para> 
	/// </remarks>
	public class DepthFirstSearchAlgorithm :
		IAlgorithm,
		IPredecessorRecorderAlgorithm,
		ITimeStamperAlgorithm,
		IVertexColorizerAlgorithm,
		ITreeEdgeBuilderAlgorithm
	{
		private IVertexListGraph visitedGraph;
		private VertexColorDictionary colors;
		private int maxDepth = int.MaxValue;

		/// <summary>
		/// A depth first search algorithm on a directed graph
		/// </summary>
		/// <param name="g">The graph to traverse</param>
		/// <exception cref="ArgumentNullException">g is null</exception>
		public DepthFirstSearchAlgorithm(IVertexListGraph g)
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
		public DepthFirstSearchAlgorithm(
			IVertexListGraph g, 
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
		public IVertexListGraph VisitedGraph
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
		public event EdgeEventHandler ExamineEdge;


		/// <summary>
		/// Raises the <see cref="ExamineEdge"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnExamineEdge(IEdge e)
		{
			if (ExamineEdge!=null)
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
			if (TreeEdge!=null)
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
			if (BackEdge!=null)
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
			if (ForwardOrCrossEdge!=null)
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
				OnExamineEdge(e);
				v = e.Target;
				GraphColor c=Colors[v];
				if (c == GraphColor.White)
				{
					OnTreeEdge(e);
					Visit(v,depth+1);
				}
				else if (c == GraphColor.Gray)
				{
					OnBackEdge(e);
				}
				else
				{
					OnForwardOrCrossEdge(e);
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

			TreeEdge += new EdgeEventHandler(vis.TreeEdge);
		}
	}
}
