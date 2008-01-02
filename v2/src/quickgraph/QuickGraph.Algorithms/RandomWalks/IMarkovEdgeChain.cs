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

namespace QuickGraph.Algorithms.RandomWalks
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;

	/// <summary>
	/// When implemented by a class, defines methods to generate a
	/// random Markov chain of <see cref="IEdge"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This interface defines the <c>Successor</c> method which generates
	/// a Markov chain of <see cref="IEdge"/>. Implemented classes can choose
	/// the according distribution properties.
	/// </para>
	/// </remarks>
	public interface IMarkovEdgeChain
	{
		/// <summary>
		/// Selects the next out-<see cref="IEdge"/> in the Markov Chain.
		/// </summary>
		/// <param name="g">visted graph</param>
		/// <param name="u">source vertex</param>
		/// <returns>Random next out-edge</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="g"/> or <paramref name="u"/> is a null reference
		/// </exception>
		IEdge Successor(IImplicitGraph g, IVertex u);
	}
}
