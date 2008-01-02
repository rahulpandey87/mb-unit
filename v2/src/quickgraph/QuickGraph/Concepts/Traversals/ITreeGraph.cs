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
	using QuickGraph.Exceptions;

	/// <summary>
	/// A tree-like interface definition
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface defines a DOM like tree node structure.
	/// </para>
	/// <para>
	/// Graphs used with this interface must be directed, not
	/// allowing parrallel edges. However, they can be cylic
	/// but the in-degree of each vertex must be equal to 1.
	/// </para>
	/// </remarks>
	public interface ITreeGraph
	{
		/// <summary>
		/// Gets the <see cref="IVertex"/> parent.
		/// </summary>
		/// <param name="v">current vertex</param>
		/// <returns>
		/// parent vertex if any, null reference otherwize
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		/// <exception cref="MultipleInEdgeException">
		/// <paramref name="v"/> has multiple in-edges
		/// </exception>
		IVertex ParentVertex(IVertex v);

		/// <summary>
		/// Gets the first adjacent vertex
		/// </summary>
		/// <param name="v">current vertex</param>
		/// <returns>first out-vertex</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		IVertex FirstChild(IVertex v);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		IVertex LastChild(IVertex v);

		/// <summary>
		/// Gets a value indicating if the <see cref="IVertex"/> has out-edges
		/// </summary>
		/// <param name="v"><see cref="IVertex"/> to test</param>
		/// <returns>true if <paramref name="v"/> has out-edges.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		bool HasChildVertices(IVertex v);

		/// <summary>
		/// Gets an enumerable collection of child <see cref="IVertex"/>
		/// </summary>
		/// <param name="v">current <see cref="IVertex"/></param>
		/// <returns>An enumerable collection of adjacent vertices</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="v"/> is a null reference
		/// </exception>
		IVertexEnumerable ChildVertices(IVertex v);
	}
}
