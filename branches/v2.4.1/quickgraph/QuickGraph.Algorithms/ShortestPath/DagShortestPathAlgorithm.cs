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
	/// Directed Acyclic Graph single source shortest path algorithm.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This algorithm solves the single-source shortest-paths problem on a 
	/// weighted, directed acyclic graph (DAG).
	/// </para>
	/// <para>
	/// This algorithm is more efficient for DAG's than either the 
	/// <see cref="DijkstraShortestPathAlgorithm"/> or 
	/// <see cref="BellmanFordShortestPathAlgorithm"/>.. 
	/// Use <see cref="BreadthFirstSearchAlgorithm"/> instead of this algorithm 
	/// when all edge weights are equal to one.
	/// </para>
	/// <para>
	/// It is strongly inspired from the
	/// Boost Graph Library implementation.
	/// </para>
	/// </remarks>
	public class DagShortestPathAlgorithm : 
		IVertexColorizerAlgorithm
	{
		private IVertexListGraph m_VisitedGraph;
		private VertexColorDictionary m_Colors;
		private VertexDoubleDictionary m_Distances;
		private EdgeDoubleDictionary m_Weights;
		private VertexVertexDictionary m_Predecessors;

		/// <summary>
		/// Builds a new Dagsearcher.
		/// </summary>
		/// <param name="g">Acyclic graph</param>
		/// <param name="weights">Edge weights</param>
		/// <exception cref="ArgumentNullException">Any argument is null</exception>
		/// <remarks>This algorithm uses the <seealso cref="BreadthFirstSearchAlgorithm"/>.</remarks>
		public DagShortestPathAlgorithm(
			IVertexListGraph g, 
			EdgeDoubleDictionary weights
			)
		{
			if (g == null)
				throw new ArgumentNullException("g");
			if (weights == null)
				throw new ArgumentNullException("Weights");

			m_VisitedGraph = g;
			m_Colors = new VertexColorDictionary();
			m_Distances = new VertexDoubleDictionary();
			m_Predecessors = new VertexVertexDictionary();
			m_Weights = weights;
		}


		/// <summary>
		/// Visited graph
		/// </summary>
		public IVertexListGraph VisitedGraph
		{
			get
			{
				return m_VisitedGraph;
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
				return m_Colors;
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
		/// Triggers the InitializeVertex event
		/// </summary>
		/// <param name="v"></param>
		protected void OnInitializeVertex(IVertex v)
		{
			if (InitializeVertex!=null)
				InitializeVertex(this,new VertexEventArgs(v));
		}

		/// <summary>
		/// Invoked on vertex v when the edge (u,v) is examined and v is White. 
		/// Since a vertex is colored Gray when it is discovered, each 
		/// reachable vertex is discovered exactly once. This is also when the 
		/// vertex is inserted into the priority queue. 
		/// </summary>
		public event VertexEventHandler DiscoverVertex;

		/// <summary>
		/// Triggers the DiscoverVertex event
		/// </summary>
		/// <param name="v"></param>
		protected void OnDiscoverVertex(IVertex v)
		{
			if (DiscoverVertex!=null)
				DiscoverVertex(this,new VertexEventArgs(v));
		}

		/// <summary>
		/// Invoked on a vertex as it is added to set S. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// At this point we know that <c>(p[u],u)</c> is a shortest-paths tree 
		/// edge so <c>d[u] = delta(s,u) = d[p[u]] + w(p[u],u)</c>. 
		/// </para>
		/// <para>
		/// Also, the distances of the examined vertices is monotonically increasing 
		/// <c>d[u1] &lt;= d[u2] &lt;= d[un]</c>.  
		/// </para>
		/// </remarks>
		public event VertexEventHandler ExamineVertex;

		/// <summary>
		/// Triggers the ExamineVertex event
		/// </summary>
		/// <param name="v"></param>
		protected void OnExamineVertex(IVertex v)
		{
			if (ExamineVertex!=null)
				ExamineVertex(this,new VertexEventArgs(v));
		}

		/// <summary>
		/// Invoked on each out-edge of a vertex immediately after it has 
		/// been added to set S. 
		/// </summary>
		public event EdgeEventHandler ExamineEdge;

		/// <summary>
		/// Triggers the ExamineEdge event
		/// </summary>
		/// <param name="e"></param>
		protected void OnExamineEdge(IEdge e)
		{
			if (ExamineEdge!=null)
				ExamineEdge(this,new EdgeEventArgs(e));
		}

		/// <summary>
		/// invoked on edge (u,v) if d[u] + w(u,v) &lt; d[v]. The edge (u,v) 
		/// that participated in the last relaxation for vertex v is an edge 
		/// in the shortest paths tree. 
		/// </summary>
		public event EdgeEventHandler EdgeRelaxed;

		/// <summary>
		/// Triggers the EdgeRelaxed event
		/// </summary>
		/// <param name="e"></param>
		protected void OnEdgeRelaxed(IEdge e)
		{
			if (EdgeRelaxed!=null)
				EdgeRelaxed(this,new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked if the edge is not relaxed. <seealso cref="EdgeRelaxed"/>.
		/// </summary>
		public event EdgeEventHandler EdgeNotRelaxed;

		/// <summary>
		/// Triggers the EdgeNotRelaxed event
		/// </summary>
		/// <param name="e"></param>
		protected void OnEdgeNotRelaxed(IEdge e)
		{
			if (EdgeNotRelaxed!=null)
				EdgeNotRelaxed(this,new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked on a vertex after all of its out edges have been examined.
		/// </summary>
		public event VertexEventHandler FinishVertex;

		/// <summary>
		/// Triggers the FinishVertex event
		/// </summary>
		/// <param name="v"></param>
		protected void OnFinishVertex(IVertex v)
		{
			if (FinishVertex!=null)
				FinishVertex(this,new VertexEventArgs(v));
		}

		#endregion

		/// <summary>
		/// Constructed distance map
		/// </summary>
		public VertexDoubleDictionary Distances
		{
			get
			{
				return m_Distances;
			}
		}

		/// <summary>
		/// Constructed predecessor map
		/// </summary>
		public VertexVertexDictionary Predecessors
		{
			get
			{
				return m_Predecessors;
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
				Colors[u]=GraphColor.White;
				Predecessors[u]=u;
				Distances[u]=double.PositiveInfinity;
			}

			Colors[s]=GraphColor.Gray;
			Distances[s]=0;

			ComputeNoInit(s);
		}

		internal void ComputeNoInit(IVertex s)
		{
			VertexCollection orderedVertices = new VertexCollection();
			TopologicalSortAlgorithm topoSorter = new TopologicalSortAlgorithm(VisitedGraph,orderedVertices);
			topoSorter.Compute();

			OnDiscoverVertex(s);

			foreach(IVertex v in orderedVertices) 
			{
				OnExamineVertex(v);

				foreach(IEdge e in VisitedGraph.OutEdges(v))
				{
					OnDiscoverVertex(e.Target);
					bool decreased = Relax(e);
					if (decreased)
						OnEdgeRelaxed(e);
					else
						OnEdgeNotRelaxed(e);
				}
				OnFinishVertex(v);      
			}

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
			double du = m_Distances[e.Source];
			double dv = m_Distances[e.Target];
			double we = m_Weights[e];

			if (Compare(Combine(du,we),dv))
			{
				m_Distances[e.Target]=Combine(du,we);
				m_Predecessors[e.Target]=e.Source;
				return true;
			}
			else
				return false;
		}

		
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
	}
}
