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

namespace QuickGraph.Providers
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Providers;
	/// <summary>
	/// Custom edge provider
	/// </summary>
	public sealed class CustomEdgeProvider :	IEdgeProvider
	{
		private int m_NextID = 0;

		/// <summary>
		/// Returns typeof(CustomEdge)
		/// </summary>
		public Type EdgeType
		{
			get
			{
				return typeof(CustomEdge);
			}
		}

		/// <summary>
		/// Creates a new edge
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public CustomEdge ProvideEdge(Vertex u, Vertex v)
		{
			return new CustomEdge(m_NextID++,u,v);
		}
		IEdge IEdgeProvider.ProvideEdge(IVertex u, IVertex v)
		{
			return this.ProvideEdge((Vertex)u, (Vertex)v);
		}

		/// <summary>
		/// Updates an edge that has not been created with the provider
		/// </summary>
		/// <param name="e">vertex to update</param>
		public void UpdateEdge(CustomEdge e)
		{
			e.ID = m_NextID++;
		}

		/// <summary>
		/// Updates vertex
		/// </summary>
		/// <param name="e"></param>
		void IEdgeProvider.UpdateEdge(IEdge e)
		{
			this.UpdateEdge((CustomEdge)e);
		}
	}
}
