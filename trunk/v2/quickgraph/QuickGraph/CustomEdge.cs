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

namespace QuickGraph
{
	using QuickGraph.Concepts;
	/// <summary>
	/// An edge that can hold a value
	/// </summary>
	public class CustomEdge : Edge
	{
		private Object m_Value;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <remarks>
		/// This constructors defaults the value to null
		/// </remarks>
		/// <param name="id">unique edge identification number</param>
		/// <param name="source">source vertex</param>
		/// <param name="target">target vertex</param>
		public CustomEdge(int id, IVertex source, IVertex target) 
			: base(id,source,target)
		{
		}

		/// <summary>
		/// associated property value
		/// </summary>
		public Object Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}
	}
}
