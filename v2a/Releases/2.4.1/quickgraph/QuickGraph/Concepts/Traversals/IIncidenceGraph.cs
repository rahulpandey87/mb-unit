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



namespace QuickGraph.Concepts.Traversals
{
	using System;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Access to each vertex out-edges.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The IncidenceGraph concept provides an interface for efficient access 
	/// to the out-edges of each vertex in the graph. 
	/// </para>
	/// <seealso cref="IGraph"/>
	/// </remarks>
	public interface IIncidenceGraph : IGraph, IAdjacencyGraph, IImplicitGraph
	{
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
		bool ContainsEdge(IVertex u, IVertex v);

	}
}
