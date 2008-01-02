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

using System;
using System.Collections;

namespace QuickGraph.Collections.Filtered
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Predicates;

	/// <summary>
	/// A filtered edge list graph
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class FilteredVertexAndEdgeListGraph : 
		FilteredVertexListGraph,
		IVertexAndEdgeListGraph,
		IEdgeListAndIncidenceGraph
	{
		private FilteredEdgeListGraph filteredEdgeList;

		/// <summary>
		/// Construct a graph that filters edges and vertices
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <param name="vertexPredicate"></param>
		/// <exception cref="ArgumentNullException">
		/// g or edgePredicate is null
		/// </exception>
		public FilteredVertexAndEdgeListGraph(
			IVertexAndEdgeListGraph g,
			IEdgePredicate edgePredicate,
			IVertexPredicate vertexPredicate
			)
			: base(g,edgePredicate,vertexPredicate)
		{
			this.filteredEdgeList = new FilteredEdgeListGraph(g,edgePredicate);
		}


		/// <summary>
		/// Underlying incidence graph
		/// </summary>
		public IVertexAndEdgeListGraph VertexAndEdgeListGraph
		{
			get
			{
				return (IVertexAndEdgeListGraph)Graph;
			}
		}

		/// <summary>
		/// Wrapped filtered edge list
		/// </summary>
		protected FilteredEdgeListGraph FilteredEdgeList
		{
			get
			{
				return filteredEdgeList;
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
				if (this.FilteredEdgeList.EdgesEmpty)
					return true;
				return this.EdgesCount==0;
			}
		}
		/// <summary>
		/// Returns the number of filtered edges in the graph
		/// </summary>
		/// <returns>number of edges</returns>
		public int EdgesCount
		{
			get
			{
				return this.FilteredEdgeList.EdgesCount;
			}
		}

		/// <summary>
		/// Returns an iterable collection of filtered edges
		/// </summary>
		public FilteredEdgeEnumerable Edges
		{
			get
			{
				return this.FilteredEdgeList.Edges;
			}
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
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
			return FilteredEdgeList.ContainsEdge(e);
		}
	}
}
