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
	/// A filtered bidirectional graph
	/// </summary>
	public class FilteredBidirectionalGraph : 
		FilteredIncidenceGraph,
		IBidirectionalGraph
	{
		/// <summary>
		/// Construct a graph that filters in and out edges
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g or edgePredicate is null
		/// </exception>
		public FilteredBidirectionalGraph(
			IBidirectionalGraph g, 
			IEdgePredicate edgePredicate)
			: base(g,edgePredicate)
		{}

		/// <summary>
		/// Construct a filtered graph with an edge and a vertex predicate.
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <param name="vertexPredicate">vertex predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g, edgePredicate or vertexPredicate are null
		/// </exception>
		public FilteredBidirectionalGraph(
			IBidirectionalGraph g,
			IEdgePredicate edgePredicate, 
			IVertexPredicate vertexPredicate)
			: base(g,edgePredicate,vertexPredicate)
		{}

		/// <summary>
		/// Underlying incidence graph
		/// </summary>
		public IBidirectionalGraph BidirectionalGraph
		{
			get
			{
				return (IBidirectionalGraph)Graph;
			}
		}

		/// <summary>
		/// Gets a value indicating if the set of edges connected to v is empty
		/// </summary>
		/// <remarks>
		/// <para>
		/// Usually faster that calling <see cref="Degree"/>.
		/// </para>
		/// </remarks>
		/// <value>
		/// true if the adjacent edge set is empty, false otherwise.
		/// </value>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		public bool AdjacentEdgesEmpty(IVertex v)
		{
			return OutDegree(v)==0 && InDegree(v)==0;
		}

		/// <summary>
		/// Gets a value indicating if the set of in-edges is empty
		/// </summary>
		/// <remarks>
		/// <para>
		/// Usually faster that calling <see cref="InDegree"/>.
		/// </para>
		/// </remarks>
		/// <value>
		/// true if the in-edge set is empty, false otherwise.
		/// </value>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		public bool InEdgesEmpty(IVertex v)
		{
			if (BidirectionalGraph.InEdgesEmpty(v))
				return true;

			return InDegree(v)==0;
		}


		/// <summary>
		/// Returns the number of out-degree edges of v
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>out-degree</returns>
		public int InDegree(IVertex v)
		{
			int n=0;
			IEnumerator e = InEdges(v).GetEnumerator();
			while(e.MoveNext())
				++n;
			return n;
		}

		/// <summary>
		/// Vertex filtered degre
		/// </summary>
		/// <param name="v">v to compute degree of</param>
		/// <returns>filtered degree</returns>
		public int Degree(IVertex v)
		{
			return OutDegree(v)+InDegree(v);
		}

		/// <summary>
		/// Returns an iterable collection of the out edges of v
		/// </summary>
		public FilteredEdgeEnumerable InEdges(IVertex v)
		{
			return new FilteredEdgeEnumerable(
				BidirectionalGraph.InEdges(v),
				new InEdgePredicate(EdgePredicate,VertexPredicate)
				);
		}

		/// <summary>
		/// Implentens IIncidenceGraph interface.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		IEdgeEnumerable IBidirectionalGraph.InEdges(IVertex v)
		{
			return this.InEdges(v);
		}
	}
}
