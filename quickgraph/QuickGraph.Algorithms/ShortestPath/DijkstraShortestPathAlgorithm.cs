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
//		QuickGraph Library HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux



namespace QuickGraph.Algorithms.ShortestPath
{
	using System;
	using System.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Visitors;
	using QuickGraph.Concepts.Algorithms;

	/// <summary>
	/// Dijkstra shortest path algorithm.
	/// </summary>
	/// <remarks>
	/// This algorithm solves the single-source shortest-paths problem 
	/// on a weighted, directed for the case where all 
	/// edge weights are nonnegative. It is strongly inspired from the
	/// Boost Graph Library implementation.
	/// 
	/// Use the Bellman-Ford algorithm for the case when some edge weights are 
	/// negative. 
	/// Use breadth-first search instead of Dijkstra's algorithm when all edge 
	/// weights are equal to one. 
	/// </remarks>
	public class DijkstraShortestPathAlgorithm : 
		IVertexColorizerAlgorithm,
		IPredecessorRecorderAlgorithm,
		IDistanceRecorderAlgorithm
	{
		private IVertexListGraph visitedGraph;
		private PriorithizedVertexBuffer vertexQueue;
		private VertexColorDictionary colors;
		private VertexDoubleDictionary distances;
		private EdgeDoubleDictionary weights;

		/// <summary>
		/// Builds a new Dijsktra searcher.
		/// </summary>
		/// <param name="g">The graph</param>
		/// <param name="weights">Edge weights</param>
		/// <exception cref="ArgumentNullException">Any argument is null</exception>
		/// <remarks>This algorithm uses the <seealso cref="BreadthFirstSearchAlgorithm"/>.</remarks>
		public DijkstraShortestPathAlgorithm(
			IVertexListGraph g, 
			EdgeDoubleDictionary weights
			)
		{
			if (g == null)
				throw new ArgumentNullException("g");
			if (weights == null)
				throw new ArgumentNullException("Weights");

			this.visitedGraph = g;
			this.colors = new VertexColorDictionary();
			this.distances = new VertexDoubleDictionary();
			this.weights = weights;
			this.vertexQueue = null;
		}

		/// <summary>
		/// Create a edge unary weight dictionary.
		/// </summary>
		/// <param name="graph">graph to map</param>
		/// <returns>Dictionary where each edge wheight is 1</returns>
		public static EdgeDoubleDictionary UnaryWeightsFromEdgeList(IEdgeListGraph graph)
		{
			if (graph==null)
				throw new ArgumentNullException("graph");
			EdgeDoubleDictionary weights=new EdgeDoubleDictionary();
			foreach(IEdge e in graph.Edges)
				weights[e]=1;
			return weights;
		}

		/// <summary>
		/// Create a edge unary weight dictionary.
		/// </summary>
		/// <param name="graph">graph to map</param>
		/// <returns>Dictionary where each edge wheight is 1</returns>
		public static EdgeDoubleDictionary UnaryWeightsFromVertexList(IVertexListGraph graph)
		{
			if (graph==null)
				throw new ArgumentNullException("graph");

			EdgeDoubleDictionary weights=new EdgeDoubleDictionary();
			foreach(IVertex v in graph.Vertices)
			{
				foreach(IEdge e in graph.OutEdges(v))
				{
					weights[e]=1;
				}
			}
			return weights;
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
		/// Vertex color map
		/// </summary>
		public VertexColorDictionary Colors
		{
			get
			{
				return this.colors;
			}
		}

		IDictionary IVertexColorizerAlgorithm.Colors
		{
			get
			{
				return this.Colors;
			}
		}

		#region Events
		/// <summary>
		/// Invoked on each vertex in the graph before the start of the 
		/// algorithm.
		/// </summary>
		public event VertexEventHandler InitializeVertex;

		/// <summary>
		/// Invoked on vertex v when the edge (u,v) is examined and v is WHITE. 
		/// Since a vertex is colored GRAY when it is discovered, each 
		/// reachable vertex is discovered exactly once. This is also when the 
		/// vertex is inserted into the priority queue. 
		/// </summary>
		public event VertexEventHandler DiscoverVertex;

		/// <summary>
		/// Invoked on a vertex as it is removed from the priority queue and 
		/// added to set S. At this point we know that (p[u],u) is a 
		/// shortest-paths tree edge so 
		/// d[u] = delta(s,u) = d[p[u]] + w(p[u],u). 
		/// Also, the distances of the examined vertices is monotonically 
		/// increasing d[u1] &lt;= d[u2] &lt;= d[un]. 
		/// </summary>
		public event VertexEventHandler ExamineVertex;

		/// <summary>
		/// Invoked on each out-edge of a vertex immediately after it has 
		/// been added to set S. 
		/// </summary>
		public event EdgeEventHandler ExamineEdge;

		/// <summary>
		/// invoked on edge (u,v) if d[u] + w(u,v) &lt; d[v]. The edge (u,v) 
		/// that participated in the last relaxation for vertex v is an edge 
		/// in the shortest paths tree. 
		/// </summary>
		public event EdgeEventHandler EdgeRelaxed;

		/// <summary>
		/// Raises the <see cref="EdgeRelaxed"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnEdgeRelaxed(IEdge e)
		{
			if (EdgeRelaxed!=null)
				EdgeRelaxed(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked if the edge is not relaxed. <seealso cref="EdgeRelaxed"/>.
		/// </summary>
		public event EdgeEventHandler EdgeNotRelaxed;

		/// <summary>
		/// Raises the <see cref="EdgeNotRelaxed"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnEdgeNotRelaxed(IEdge e)
		{
			if (EdgeNotRelaxed!=null)
				EdgeNotRelaxed(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on a vertex after all of its out edges have been examined.
		/// </summary>
		public event VertexEventHandler FinishVertex;

		#endregion

		/// <summary>
		/// Checks for edge relation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void TreeEdge(Object sender, EdgeEventArgs args)
		{
			bool decreased = Relax(args.Edge);
			if (decreased)
				OnEdgeRelaxed(args.Edge);
			else
				OnEdgeNotRelaxed(args.Edge);
		}

		/// <summary>
		/// Checks for edge relation.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void GrayTarget(Object sender, EdgeEventArgs args)
		{
			bool decreased = Relax(args.Edge);
			if (decreased)
			{
				this.vertexQueue.Update(args.Edge.Target);
				OnEdgeRelaxed(args.Edge);
			}
			else
			{
				OnEdgeNotRelaxed(args.Edge);
			}
		}

		/// <summary>
		/// Constructed distance map
		/// </summary>
		public VertexDoubleDictionary Distances
		{
			get
			{
				return this.distances;
			}
		}
		
		/// <summary>
		/// Vertex priorithized queue. Used internally.
		/// </summary>
		protected VertexBuffer VertexQueue
		{
			get
			{
				return this.vertexQueue;
			}
		}

		/// <summary>
		/// Computes all the shortest path from s to the oter vertices
		/// </summary>
		/// <param name="s">Start vertex</param>
		/// <exception cref="ArgumentNullException">s is null</exception>
		public void Compute(IVertex s)
		{
			if (s==null)
				throw new ArgumentNullException("Start vertex is null");

			// init color, distance
			foreach(IVertex u in VisitedGraph.Vertices)
			{
				this.colors[u]=GraphColor.White;
				this.distances[u]=double.PositiveInfinity;
			}

			Colors[s]=GraphColor.Gray;
			Distances[s]=0;

			ComputeNoInit(s);
		}

		public void ComputeNoInit(IVertex s)
		{
			this.vertexQueue = new PriorithizedVertexBuffer(this.distances);
			BreadthFirstSearchAlgorithm bfs = new BreadthFirstSearchAlgorithm(
				VisitedGraph,
				this.vertexQueue,
				Colors
				);

			bfs.InitializeVertex += this.InitializeVertex;
			bfs.DiscoverVertex += this.DiscoverVertex;
			bfs.ExamineEdge += this.ExamineEdge;
			bfs.ExamineVertex += this.ExamineVertex;
			bfs.FinishVertex += this.FinishVertex;

			bfs.TreeEdge += new EdgeEventHandler(this.TreeEdge);
			bfs.GrayTarget += new EdgeEventHandler(this.GrayTarget);

			bfs.Visit(s);
		}

		internal bool Compare(double a, double b)
		{
			return a < b;
		}

		internal double Combine(double d, double w)
		{
			return d+w;
		}

		internal bool Relax(IEdge e)
		{
			double du = this.distances[e.Source];
			double dv = this.distances[e.Target];
			double we = this.weights[e];

			if (Compare(Combine(du,we),dv))
			{
				this.distances[e.Target]=Combine(du,we);
				return true;
			}
			else
				return false;
		}

		#region IVertexColorizerAlgorithm		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="vis"></param>
		public void RegisterVertexColorizerHandlers(IVertexColorizerVisitor vis)
		{
			InitializeVertex += new VertexEventHandler(vis.InitializeVertex);
			DiscoverVertex += new VertexEventHandler(vis.DiscoverVertex);
			FinishVertex += new VertexEventHandler(vis.FinishVertex);
		}
		#endregion
				
		#region IPredecessorRecorderAlgorithm
		/// <summary>
		/// Register the predecessor handlers
		/// </summary>
		/// <param name="vis">visitor</param>
		public void RegisterPredecessorRecorderHandlers(IPredecessorRecorderVisitor vis)
		{
			this.EdgeRelaxed += new EdgeEventHandler(vis.TreeEdge);
			this.FinishVertex += new VertexEventHandler(vis.FinishVertex);
		}		
		#endregion
		
		#region IDistanceRecorderAlgorithm
		/// <summary>
		/// Add event handlers to the corresponding events.
		/// </summary>
		/// <param name="vis">Distance recorder visitor</param>
		public void RegisterDistanceRecorderHandlers(IDistanceRecorderVisitor vis)
		{
			this.InitializeVertex += new VertexEventHandler(vis.InitializeVertex);			
			this.DiscoverVertex += new VertexEventHandler(vis.DiscoverVertex);			
			this.EdgeRelaxed += new EdgeEventHandler(vis.TreeEdge);
		}		
		#endregion
	}
}
