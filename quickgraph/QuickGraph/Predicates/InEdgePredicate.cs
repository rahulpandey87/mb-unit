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

namespace QuickGraph.Predicates
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// In edge predicate
	/// </summary>
	/// <remarks>
	/// <para>
	/// Applies predicates to an edge and to it's source.
	/// </para>
	/// <para>
	/// Given <c>ep</c>, the edge predicate, and <c>vp</c>, the vertex
	/// predicate, the predicate result is computed, for a given edge <c>e</c>, 
	/// as:
	/// <code>
	/// ep(e) &amp;&amp; vp(e.Source)
	/// </code>
	/// </para>
	/// </remarks>
	public class InEdgePredicate : IEdgePredicate
	{
		private IEdgePredicate m_EdgePredicate;
		private IVertexPredicate m_VertexPredicate;

		/// <summary>
		/// Construct a new predicate.
		/// </summary>
		/// <param name="ep">the edge predicate</param>
		/// <param name="vp">the source vertex predicate</param>
		/// <exception cref="ArgumentNullException">ep or vp is null</exception>
		public InEdgePredicate(IEdgePredicate ep, IVertexPredicate vp)
		{
			if (ep == null)
				throw new ArgumentNullException("Edge predicate");
			if (vp == null)
				throw new ArgumentNullException("Vertex predicate");

			m_EdgePredicate = ep;
			m_VertexPredicate = vp;
		}

		/// <summary>
		/// Applies the edge predicate to e and to it's source
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>EdgePredicate(e) and VertexPredicate(e.Source)</returns>
		/// <exception cref="ArgumentNullException">e is null</exception>
		public bool Test(IEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			return m_EdgePredicate.Test(e) && m_VertexPredicate.Test(e.Source);
		}
	}
}
