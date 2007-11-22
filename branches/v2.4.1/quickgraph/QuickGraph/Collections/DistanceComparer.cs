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


namespace QuickGraph.Collections
{
	using System;
	using System.Collections;
	using QuickGraph.Concepts;

	/// <summary>
	/// Given a Distance map, compare two vertex distance
	/// </summary>
	public class DistanceComparer : IComparer
	{
		VertexDoubleDictionary m_Distances;

		/// <summary>
		/// Builds a vertex distance comparer
		/// </summary>
		/// <param name="distances"></param>
		public DistanceComparer(VertexDoubleDictionary distances)
		{
			m_Distances = distances;
		}

		/// <summary>
		/// Compare the distance between vertex x and y
		/// </summary>
		/// <param name="x">First vertex</param>
		/// <param name="y">Second vertex</param>
		/// <returns>-1 if d[x]&lt;d[y], 0 if d[x] equals d[y] and +1 if d[x] &gt; d[y]</returns>
		public int Compare(Object x, Object y)
		{
			if (!(x is IVertex))
				throw new ArgumentException("x is not a vertex");
			if (!(y is IVertex))
				throw new ArgumentException("y is not a vertex");

			return m_Distances[(IVertex)x].CompareTo( m_Distances[(IVertex)y] );
		}
	}
}
