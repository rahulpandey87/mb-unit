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

namespace QuickGraph.Concepts.Modifications
{
	using QuickGraph.Concepts.Providers;
	/// <summary>
	/// </summary>
	public interface IEdgeMutableGraph : 
		IVertexMutableGraph
	{
		/// <summary>
		/// Returns the vertex provider
		/// </summary>
		IEdgeProvider EdgeProvider {get;}

		/// <summary>
		/// Remove all edges to and from vertex u from the graph.
		/// </summary>
		/// <param name="u"></param>
		void ClearVertex(IVertex u);

		/// <summary>
		/// Inserts the edge (u,v) into the graph, and returns the new edge.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method attemps to insert the edge (u,v) into the graph, 
		/// returning the inserted edge or a parrallel edge. If the insertion
		/// was not successful, the returned edge is null.
		/// </para>
		/// </remarks>
		IEdge AddEdge(IVertex u, IVertex v);

		/// <summary>
		/// Remove the edge (u,v) from the graph. 
		/// If the graph allows parallel edges this remove all occurrences of 
		/// (u,v).
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <param name="v">target vertex</param>
		void RemoveEdge(IVertex u, IVertex v);

		/// <summary>
		/// Removes the edge e
		/// </summary>
		/// <param name="e">edge to remove</param>
		/// <exception cref="ArgumentException">Edge not found</exception>
		void RemoveEdge(IEdge e);
	}
}
