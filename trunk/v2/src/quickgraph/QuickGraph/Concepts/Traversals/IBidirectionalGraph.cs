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

namespace QuickGraph.Concepts.Traversals
{
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Adds access to in-edges.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The BidirectionalGraph concept refines IncidenceGraph and adds the 
	/// requirement for efficient access to the in-edges of each vertex.
	/// </para>
	/// <para>
	/// This concept is separated from IncidenceGraph because for directed 
	/// graphs efficient access to in-edges typically requires more storage 
	/// space, and many algorithms do not require access to in-edges. 
	/// </para>
	/// <para>
	/// For undirected graphs this is not an issue, 
	/// since the InEdges and OutEdges functions are the same, 
	/// they both return the edges incident to the vertex. 
	/// </para>
	/// <para>
	/// The InEdges() function is required to be constant time. 
	/// The InDegree and Degree properties functions must be linear in the 
	/// number of in-edges (for directed graphs) or incident edges 
	/// (for undirected graphs). 
	/// </para>
	/// </remarks>
	public interface IBidirectionalGraph :
		IIncidenceGraph
	{
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
		bool InEdgesEmpty(IVertex v);

		/// <summary>
		/// Returns the number of in-edges (for directed graphs) or the number 
		/// of incident edges (for undirected graphs) of vertex v in graph g.
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>out-degree</returns>
		int InDegree(IVertex v);

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
		bool AdjacentEdgesEmpty(IVertex v);

		/// <summary>
		/// Returns the number of in-edges plus out-edges (for directed graphs) 
		/// or the number of incident edges (for undirected graphs) of 
		/// vertex v in graph g.
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>out-degree</returns>
		int Degree(IVertex v);

		/// <summary>
		/// Enumerable collection of in-edges
		/// </summary>
		/// <remarks>
		/// <para>
		/// Returns an enumerable collection of in-edges (for directed graphs) 
		/// or incident edges (for undirected graphs) of vertex v in graph g. 
		/// </para>
		/// <para>
		/// For both directed and undirected graphs, the target of an out-edge 
		/// is required to be vertex v and the source is required to be a 
		/// vertex that is adjacent to v. 
		/// </para>
		/// </remarks>
		IEdgeEnumerable InEdges(IVertex v);
	}
}
