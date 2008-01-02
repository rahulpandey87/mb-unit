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



namespace QuickGraph.Concepts.Traversals
{
	using System;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// A graph defined by a out-edges method.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="ImplicitGraph"/> concept provides an interface for implicitely 
	/// defining graphs through an <see cref="OutEdges"/> method.
	/// </para>
	/// <seealso cref="IGraph"/>
	/// </remarks>
	public interface IImplicitGraph : IGraph
	{
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
		bool OutEdgesEmpty(IVertex v);

		/// <summary>
		/// Returns the out-degree edges of v
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>out-degree</returns>
		int OutDegree(IVertex v);

		/// <summary>
		/// Returns an iterable collection of the out edges of v
		/// </summary>
		IEdgeEnumerable OutEdges(IVertex v);
	}
}
