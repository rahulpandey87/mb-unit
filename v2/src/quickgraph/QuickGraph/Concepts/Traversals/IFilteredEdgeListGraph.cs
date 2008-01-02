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
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// A edge list graph that supports filtered traversals
	/// </summary>
	public interface IFilteredEdgeListGraph : IEdgeListGraph
	{
		/// <summary>
		/// Returns the first Edge that matches the predicate
		/// </summary>
		/// <param name="vp">Edge predicate</param>
		/// <returns>null if not found, otherwize the first Edge that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">vp is null</exception>
		IEdge SelectSingleEdge(IEdgePredicate vp);

		/// <summary>
		/// Returns the collection of edges that matches the predicate
		/// </summary>
		/// <param name="vp">Edge predicate</param>
		/// <returns>enumerable colleciton of vertices that matches the 
		/// criteron</returns>
		/// <exception cref="ArgumentNullException">vp is null</exception>
		IEdgeEnumerable SelectEdges(IEdgePredicate vp);
	}
}
