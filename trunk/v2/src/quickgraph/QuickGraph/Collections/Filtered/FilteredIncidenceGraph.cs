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
	/// A filtered incidence graph
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class FilteredIncidenceGraph : 
		FilteredGraph,
		IIncidenceGraph
	{
		/// <summary>
		/// Construct a graph that filters in-edges
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g or edgePredicate is null
		/// </exception>
		public FilteredIncidenceGraph(
			IIncidenceGraph g, 
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
		public FilteredIncidenceGraph(
			IIncidenceGraph g,
			IEdgePredicate edgePredicate, 
			IVertexPredicate vertexPredicate)
			: base(g,edgePredicate,vertexPredicate)
		{}

		/// <summary>
		/// Underlying incidence graph
		/// </summary>
		public IIncidenceGraph IncidenceGraph
		{
			get
			{
				return (IIncidenceGraph)Graph;
			}
		}

		/// <summary>
		/// Gets a value indicating if the set of out-edges is empty
		/// </summary>
		/// <remarks>
		/// <para>
		/// Usually faster that calling <see cref="OutDegree"/>.
		/// </para>
		/// </remarks>
		/// <value>
		/// true if the out-edge set is empty, false otherwise.
		/// </value>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		public bool OutEdgesEmpty(IVertex v)
		{
			if (IncidenceGraph.OutEdgesEmpty(v))
				return true;

			return OutDegree(v)==0;
		}

		/// <summary>
		/// Returns the number of out-degree edges of v
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>out-degree</returns>
		public int OutDegree(IVertex v)
		{
			int n=0;
			IEnumerator e = OutEdges(v).GetEnumerator();
			while(e.MoveNext())
				++n;
			return n;
		}

		/// <summary>
		/// Returns an iterable collection of the out edges of v
		/// </summary>
		public FilteredEdgeEnumerable OutEdges(IVertex v)
		{
			return new FilteredEdgeEnumerable(
				IncidenceGraph.OutEdges(v),
				new OutEdgePredicate(EdgePredicate,VertexPredicate)
				);
		}

		/// <summary>
		/// Implentes IIncidenceGraph interface.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		IEdgeEnumerable IImplicitGraph.OutEdges(IVertex v)
		{
			return this.OutEdges(v);
		}

		/// <summary>
		/// Gets a value indicating if there is an edge between the vertices 
		/// <paramref name="u"/>, <paramref name="v"/>.
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <param name="v">target vertex</param>
		/// <returns>true if (<paramref name="u"/>, <paramref name="v"/>) exists.</returns>
		/// <exception cref="ArgumentNullException">u or v is a null reference</exception>
		/// <remarks>
		/// This method checks wheter an edge exists between the two vertices.
		/// <para>
		/// Complexity: O(E)
		/// </para>
		/// </remarks>
		public bool ContainsEdge(IVertex u, IVertex v)
		{
			foreach(IEdge e in OutEdges(u))
				if (e.Source==u && e.Target==v)
					return true;
			return false;
		}

		/// <summary>
		/// Gets an enumerable collection of the v adjacent vertices
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public IVertexEnumerable AdjacentVertices(IVertex v)
		{
			return new TargetVertexEnumerable(OutEdges(v));
		}
	}
}
