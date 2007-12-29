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

namespace QuickGraph.Concepts.Modifications
{
	using QuickGraph.Concepts.Providers;
	/// <summary>
	///  Defines a graph that can be modified by adding or removing vertices.
	/// </summary>
	public interface IVertexMutableGraph : 
		IMutableGraph
	{
		/// <summary>
		/// Returns the vertex provider
		/// </summary>
		IVertexProvider VertexProvider {get;}

		/// <summary>
		/// Adds a new vertex to the graph.
		/// </summary>
		/// <remarks>
		/// Creates a new <see cref="IVertex"/> instance and adds it to the
		/// graph.
		/// </remarks>
		/// <returns>new <see cref="IVertex"/> instance</returns>
		IVertex AddVertex();

		/// <summary>
		/// Remove u from the vertex set of the graph. 
		/// Note that undefined behavior may result if there are edges 
		/// remaining in the graph who's target is u. 
		/// 
		/// Typically the ClearVertex function should be called first.
		/// </summary>
		/// <param name="u">vertex to clear</param>
		/// <exception cref="ArgumentNullException">u is a null reference</exception>
		void RemoveVertex(IVertex u);
	}
}
