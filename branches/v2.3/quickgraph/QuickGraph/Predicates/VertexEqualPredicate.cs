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


namespace QuickGraph.Predicates
{
	using System;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// Predicate that checks to two vertex are equal
	/// </summary>
	public class VertexEqualPredicate : IVertexPredicate
	{
		private IVertex m_ReferenceVertex;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="u">reference vertex</param>
		/// <exception cref="ArgumentNullException">u is null</exception>
		public VertexEqualPredicate(IVertex u)
		{
			if (u == null)
				throw new ArgumentNullException("u");

			m_ReferenceVertex = u;
		}

		/// <summary>
		/// Reference vertex
		/// </summary>
		public IVertex ReferenceVertex
		{
			get
			{
				return m_ReferenceVertex;
			}
		}

		/// <summary>
		/// Test if v == u
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>v == u</returns>
		public bool Test(IVertex v)
		{
			return m_ReferenceVertex == v;
		}
	}
}
