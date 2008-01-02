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


namespace QuickGraph.Algorithms
{
	using System;
	using System.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Algorithms;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Exceptions;

	/// <summary>
	/// Topological sort of the graph.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The topological sort algorithm creates a linear ordering of the 
	/// vertices such that if edge (u,v) appears in the graph, then v comes 
	/// before u in the ordering.
	/// </para>
	/// <para>
	/// The graph must be a directed acyclic graph 
	/// (DAG). The implementation consists mainly of a call to 
	/// <see cref="Search.DepthFirstSearchAlgorithm"/>.
	/// </para>
	/// <para>This algorithm is directly inspired from the
	/// BoostGraphLibrary implementation.
	/// </para> 
	/// </remarks>
	public class TopologicalSortAlgorithm
	{
		private IVertexListGraph m_VisitedGraph;
		private IList m_Vertices;

		/// <summary>
		/// Builds a new sorter
		/// </summary>
		/// <param name="g">Graph to sort</param>
		public TopologicalSortAlgorithm(IVertexListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");

			m_VisitedGraph = g;
			m_Vertices = new ArrayList();
		}

		/// <summary>
		/// Builds a new sorter
		/// </summary>
		/// <param name="g">Graph to sort</param>
		/// <param name="vertices">vertices list</param>
		public TopologicalSortAlgorithm(IVertexListGraph g, IList vertices)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (vertices == null)
				throw new ArgumentNullException("vertices");

			m_VisitedGraph = g;
			m_Vertices = vertices;
		}

		/// <summary>
		/// Visited vertex list
		/// </summary>
		public IVertexListGraph VisitedGraph
		{
			get
			{
				return m_VisitedGraph;
			}
		}

		/// <summary>
		/// Sorted vertices list
		/// </summary>
		public IList SortedVertices
		{
			get
			{
				return m_Vertices;
			}
		}

		/// <summary>
		/// Delegate event that detects cycle. <seealso cref="EdgeEventHandler"/>.
		/// </summary>
		/// <param name="sender">DepthFirstSearch algorithm</param>
		/// <param name="args">Edge that produced the error</param>
		/// <exception cref="Exception">Will always throw an exception.</exception>
		public void BackEdge(Object sender, EdgeEventArgs args)
		{
			throw new NonAcyclicGraphException();
		}

		/// <summary>
		/// Delegate that adds the vertex to the vertex list. <seealso cref="VertexEventHandler"/>.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FinishVertex(Object sender, VertexEventArgs args)
		{
			m_Vertices.Insert(0,args.Vertex);
		}

		/// <summary>
		/// Computes the topological sort and stores it in the list.
		/// </summary>
		public void Compute()
		{
			Compute(null);
		}

		/// <summary>
		/// Computes the topological sort and stores it in the list.
		/// </summary>
		/// <param name="vertices">Vertex list that will contain the results</param>
		public void Compute(IList vertices)
		{
			if (vertices != null)
				m_Vertices = vertices;

			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(VisitedGraph);

			dfs.BackEdge += new EdgeEventHandler(this.BackEdge);
			dfs.FinishVertex += new VertexEventHandler(this.FinishVertex);

			m_Vertices.Clear();
			dfs.Compute();
		}
	}
}
