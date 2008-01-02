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

namespace QuickGraph.Predicates
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// Edge predicate
	/// </summary>
	/// <remarks>
	/// <para>
	/// Applies predicates to an edge, to it's source and to it's target.
	/// </para>
	/// <para>
	/// Given <c>ep</c>, the edge predicate, and <c>vp</c>, the vertex
	/// predicate, the predicate result is computed, for a given edge <c>e</c>, 
	/// as:
	/// <code>
	/// ep(e) &amp;&amp; vp(e.Source) &amp;&amp; vp(e.Target)
	/// </code>
	/// </para>
	/// </remarks>	
	public class EdgePredicate : IEdgePredicate
	{
		private IEdgePredicate m_EdgePredicate;
		private IVertexPredicate m_VertexPredicate;

		/// <summary>
		/// Constructs a new edge predicate
		/// </summary>
		/// <param name="ep">the edge predicate object</param>
		/// <param name="vp">the vertex predicate object</param>
		/// <exception cref="ArgumentNullException">ep or vp are null</exception>
		public EdgePredicate(IEdgePredicate ep, IVertexPredicate vp)
		{
			if (ep == null)
				throw new ArgumentNullException("Edge predicate");
			if (vp == null)
				throw new ArgumentNullException("Vertex predicate");

			m_EdgePredicate = ep;
			m_VertexPredicate = vp;
		}

		/// <summary>
		/// Applies the edge predicate to e and to it's vertices?
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>EdgePredicate(e) &amp;&amp; VertexPredicate(e.Source)
		/// &amp;&amp; VertexPredicate(e.Target)
		/// </returns>
		/// <exception cref="ArgumentNullException">e is null</exception>
		public bool Test(IEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			return m_EdgePredicate.Test(e) 
				&& m_VertexPredicate.Test(e.Source)
				&& m_VertexPredicate.Test(e.Target)
				;
		}
	}
}
