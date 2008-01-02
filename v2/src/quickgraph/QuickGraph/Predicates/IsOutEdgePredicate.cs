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
	/// 
	/// </summary>
	public class IsOutEdgePredicate : IEdgePredicate
	{
		private IVertex m_Vertex;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		public IsOutEdgePredicate(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			m_Vertex  = v;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		public bool Test(IEdge e)
		{
			return e.Source == m_Vertex;
		}
	}
}