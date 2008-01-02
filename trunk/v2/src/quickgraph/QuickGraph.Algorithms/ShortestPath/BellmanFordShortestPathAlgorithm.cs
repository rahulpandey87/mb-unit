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



namespace QuickGraph.Algorithms.ShortestPath
{
	using System;
	using System.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Concepts.Traversals;

	/// <summary>
	/// Bellman Ford shortest path algorithm.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The Bellman-Ford algorithm solves the single-source shortest paths 
	/// problem for a graph with both positive and negative edge weights. 
	/// </para>
	/// <para>
	/// If you only need to solve the shortest paths problem for positive 
	/// edge weights, Dijkstra's algorithm provides a more efficient 
	/// alternative. 
	/// </para>
	/// <para>
	/// If all the edge weights are all equal to one then breadth-first search 
	/// provides an even more efficient alternative. 
	/// </para>
	/// </remarks>
	public class BellmanFordShortestPathAlgorithm
	{
		private IVertexAndEdgeListGraph m_VisitedGraph;
		private VertexColorDictionary m_Colors;
		private VertexDoubleDictionary m_Distances;
		private EdgeDoubleDictionary m_Weights;
		private VertexVertexDictionary m_Predecessors;

		/// <summary>
		/// Builds a new Bellman Ford searcher.
		/// </summary>
		/// <param name="g">The graph</param>
		/// <param name="weights">Edge weights</param>
		/// <exception cref="ArgumentNullException">Any argument is null</exception>
		/// <remarks>This algorithm uses the <seealso cref="BreadthFirstSearchAlgorithm"/>.</remarks>
		public BellmanFordShortestPathAlgorithm(
			IVertexAndEdgeListGraph g, 
			EdgeDoubleDictionary weights
			)
		{
			if (weights == null)
				throw new ArgumentNullException("Weights");

			m_VisitedGraph = g;
			m_Colors = new VertexColorDictionary();
			m_Weights = weights;
			m_Distances = new VertexDoubleDictionary();
			m_Predecessors = new VertexVertexDictionary();
		}
		
		/// <summary>
		/// 
		/// </summary>
		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return m_VisitedGraph;
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


		#region Events
		/// <summary>
		/// Invoked on each vertex in the graph before the start of the 
		/// algorithm.
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
		/// Invoked on every edge in the graph |V| times.
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
		/// Invoked when the distance label for the target vertex is decreased. 
		/// The edge that participated in the last relaxation for vertex v is 
		/// an edge in the shortest paths tree.
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
		///  Invoked if the distance label for the target vertex is not 
		///  decreased.
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
		///  Invoked during the second stage of the algorithm, 
		///  during the test of whether each edge was minimized. 
		///  
		///  If the edge is minimized then this function is invoked.
		/// </summary>
		public event EdgeEventHandler EdgeMinimized;


		/// <summary>
		/// Raises the <see cref="EdgeMinimized"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnEdgeMinimized(IEdge e)
		{
			if (EdgeMinimized!=null)
				EdgeMinimized(this, new EdgeEventArgs(e));
		}

		/// <summary>
		/// Invoked during the second stage of the algorithm, 
		/// during the test of whether each edge was minimized. 
		/// 
		/// If the edge was not minimized, this function is invoked. 
		/// This happens when there is a negative cycle in the graph. 
		/// </summary>
		public event EdgeEventHandler EdgeNotMinimized;


		/// <summary>
		/// Raises the <see cref="EdgeNotMinimized"/> event.
		/// </summary>
		/// <param name="e">edge that raised the event</param>
		protected void OnEdgeNotMinimized(IEdge e)
		{
			if (EdgeNotMinimized!=null)
				EdgeNotMinimized(this, new EdgeEventArgs(e));
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
		/// Edge weights
		/// </summary>
		public EdgeDoubleDictionary Weights
		{
			get
			{
				return m_Weights;
			}
		}

		/// <summary>
		/// Computes all the shortest path from s to the oter vertices
		/// </summary>
		/// <param name="s">Start vertex</param>
		/// <remarks>
		/// Initializes the predecessor and distance map.
		/// </remarks>
		/// <returns>true if successful, false if there was a negative cycle.</returns>
		/// <exception cref="ArgumentNullException">s is null</exception>
		public bool Compute(IVertex s)
		{
			if (s==null)
				throw new ArgumentNullException("Start vertex is null");

			
			// init color, distance
			foreach(IVertex u in VisitedGraph.Vertices)
			{
				Colors[u]=GraphColor.White;
				Predecessors[u]=u;
				Distances[u]=double.PositiveInfinity;
				OnInitializeVertex(u);
			}

			return Compute();
		}

		/// <summary>
		/// Applies the Bellman Ford algorithm
		/// </summary>
		/// <remarks>
		/// Does not initialize the predecessor and distance map.
		/// </remarks>
		/// <returns>true if successful, false if there was a negative cycle.</returns>
		public bool Compute()
		{
			// getting the number of 
			int N = VisitedGraph.VerticesCount;
			for (int k = 0; k < N; ++k) 
			{
				bool atLeastOneEdgeRelaxed = false;
				foreach(IEdge e in VisitedGraph.Edges) 
				{
					OnExamineEdge(e);

					if ( Relax(e) ) 
					{
						atLeastOneEdgeRelaxed = true;
						OnEdgeRelaxed(e);
					} 
					else
						OnEdgeNotRelaxed(e);
				}
				if (!atLeastOneEdgeRelaxed)
					break;
			}		

			
			foreach(IEdge e in VisitedGraph.Edges)
			{
				if (
					Compare(
						Combine(Distances[e.Source],Weights[e]), 
						Distances[e.Target]
						)
					)
				{
					OnEdgeMinimized(e);
					return false;
				} 
				else
					OnEdgeNotMinimized(e);
			}

			return true;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		internal bool Compare(double a, double b)
		{
			return a < b;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="d"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		internal double Combine(double d, double w)
		{
			return d+w;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		internal bool Relax(IEdge e)
		{
			double du = m_Distances[e.Source];
			double dv = m_Distances[e.Target];
			double we = m_Weights[e];

			if (Compare(Combine(du,we),dv))
			{
				Distances[e.Target]=Combine(du,we);
				Predecessors[e.Target]=e.Source;
				return true;
			}
			else
				return false;
		}
	}
}
