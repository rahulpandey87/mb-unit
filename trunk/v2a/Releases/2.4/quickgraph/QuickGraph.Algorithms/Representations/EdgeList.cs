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


namespace QuickGraph.Representations
{
	using System;
	using System.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// An edge-list representation of a graph is simply a sequence of edges,
	/// where each edge is represented as a pair of vertex ID's.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The EdgeList class is an adaptor that turns an edge collection
	/// into a class that models IEdgeListGraph. 
	/// The value type of the edge collection must be be inherited form IEdge.
	/// </para>
	/// <para>
	/// An edge-list representation of a graph is simply a sequence of edges, 
	/// where each edge is represented as a pair of vertex ID's. 
	/// The memory required is only O(E). Edge insertion is typically O(1), 
	/// though accessing a particular edge is O(E) (not efficient). 
	/// </para>
	/// <seealso cref="IEdgeListGraph"/>
	/// </remarks>
	public class EdgeList : IEdgeListGraph
	{
		private IEdgeCollection edges;
		private bool isDirected;
		private bool allowParallelEdges;

		/// <summary>
		/// Builds an EdgeListGraph out of a edges collection
		/// </summary>
		/// <param name="edges"></param>
		/// <param name="isDirected"></param>
		/// <param name="allowParallelEdges"></param>
		public EdgeList(
			IEdgeCollection edges,
			bool isDirected, 
			bool allowParallelEdges
			)
		{
			if (edges == null)
				throw new ArgumentNullException("edge collection");

			this.edges = edges;
			this.isDirected = isDirected;
			this.allowParallelEdges = allowParallelEdges;
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsDirected
		{
			get
			{
				return isDirected;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowParallelEdges
		{
			get
			{
				return allowParallelEdges;
			}
		}

		/// <summary>
		/// Gets a value indicating if the vertex set is empty
		/// </summary>
		/// <remarks>
		/// <para>
		/// Usually faster that calling <see cref="EdgesCount"/>.
		/// </para>
		/// </remarks>
		/// <value>
		/// true if the vertex set is empty, false otherwise.
		/// </value>
		public bool EdgesEmpty
		{
			get
			{
				return this.edges.Count==0;
			}
		}

		/// <summary>
		/// Returns the number of edges in the graph.
		/// </summary>
		public int EdgesCount
		{
			get
			{
				return edges.Count;
			}
		}

		/// <summary>
		/// Returns an enumerator providing access to all the edges in the graph.
		/// </summary>
		public IEdgeCollection Edges
		{
			get
			{
				return edges;
			}
		}

		/// <summary>
		/// IEdgeListGraph implemetentation.
		/// </summary>
		IEdgeEnumerable IEdgeListGraph.Edges
		{
			get
			{
				return this.Edges;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public bool ContainsEdge(IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException(@"e");
			
			foreach(IEdge el in Edges)
				if (el==e)
					return true;
			return false;
		}
	}
}
