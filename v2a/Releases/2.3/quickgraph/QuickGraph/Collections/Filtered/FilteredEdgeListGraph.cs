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
	public class FilteredEdgeListGraph : 
		FilteredGraph,
		IEdgeListGraph
	{
		/// <summary>
		/// Construct a graph that filters edges
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g or edgePredicate is null
		/// </exception>
		public FilteredEdgeListGraph(
			IEdgeListGraph g, 
			IEdgePredicate edgePredicate)
			: base(g,edgePredicate)
		{}

		/// <summary>
		/// Construct a graph that filters edges
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <param name="vertexPredicate">vertex predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g or edgePredicate or vertexPredicate is null
		/// </exception>
		public FilteredEdgeListGraph(
			IEdgeListGraph g, 
			IEdgePredicate edgePredicate,
			IVertexPredicate vertexPredicate
			)
			: base(g,edgePredicate,vertexPredicate)
		{}

		/// <summary>
		/// Underlying incidence graph
		/// </summary>
		public IEdgeListGraph EdgeListGraph
		{
			get
			{
				return (IEdgeListGraph)Graph;
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
				if (this.EdgeListGraph.EdgesEmpty)
					return true;

				return EdgesCount==0;
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
				int n=0;
				IEnumerator e = Edges.GetEnumerator();
				while(e.MoveNext())
					++n;
				return n;
			}
		}

		/// <summary>
		/// Returns an iterable collection of filtered edges
		/// </summary>
		public FilteredEdgeEnumerable Edges
		{
			get
			{
				return new FilteredEdgeEnumerable(
					EdgeListGraph.Edges,
					new EdgePredicate(EdgePredicate,VertexPredicate)
					);
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
		/// Gets a value indicating if the edge <paramref name="e"/> is part
		/// of the list.
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>true if part of the list, false otherwize</returns>
		/// <exception cref="ArgumentNullException">e is a null reference</exception>
		/// <remarks>
		/// This method checks wheter a particular edge is part of the set.
		/// <para>
		/// Complexity: O(E).
		/// </para>
		/// </remarks>
		public bool ContainsEdge(IEdge e)
		{
			foreach(IEdge el in Edges)
				if (el==e)
					return true;
			return false;
		}

	}
}
