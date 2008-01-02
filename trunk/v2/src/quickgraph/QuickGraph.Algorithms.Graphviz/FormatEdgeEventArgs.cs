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

namespace QuickGraph.Algorithms.Graphviz
{
	using NGraphviz.Helpers;
	using QuickGraph.Concepts;

	/// <summary>
	/// Argument for graphviz edge formating.
	/// </summary>
	public class FormatEdgeEventArgs : EdgeEventArgs
	{
		private GraphvizEdge m_EdgeFormatter;

		/// <summary>
		/// Graphviz format edge parameter object
		/// </summary>
		/// <param name="edgeFormatter">edge formatter</param>
		/// <param name="e">edge</param>
		public FormatEdgeEventArgs(GraphvizEdge edgeFormatter, IEdge e)
			: base(e)
		{
			if (edgeFormatter==null)
				throw new ArgumentNullException("edgeFormatter");
			m_EdgeFormatter = edgeFormatter;
		}

		/// <summary>
		/// Edge formatter
		/// </summary>
		public GraphvizEdge EdgeFormatter
		{
			get
			{
				return m_EdgeFormatter;
			}
		}
	}

	/// <summary>
	/// Graphviz format edge delegate
	/// </summary>
	public delegate void FormatEdgeEventHandler(object sender, FormatEdgeEventArgs e);
}
