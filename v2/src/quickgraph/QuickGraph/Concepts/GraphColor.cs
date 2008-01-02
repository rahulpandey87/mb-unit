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

namespace QuickGraph.Concepts
{
	using System.Drawing;
	/// <summary>
	/// Colors used to mark the vertex and edges in the algorithms
	/// </summary>
	public enum GraphColor
	{
		/// <summary>
		/// White color, usually describes describes vertex.
		/// </summary>
		White,
		/// <summary>
		/// Black color, usually describes finished vertex.
		/// </summary>
		Black,
		/// <summary>
		/// Gray color
		/// </summary>
		Gray
	}

	/// <summary>
	/// Utility class for graph color conversion
	/// </summary>
	public sealed class GraphColorConverter
	{
		/// <summary>
		/// No constructor
		/// </summary>
		private GraphColorConverter()
		{}

		/// <summary>
		/// Converts GraphColor to System.Drawing.Color
		/// </summary>
		/// <param name="c">graph color to convert</param>
		/// <param name="alpha">alpha component</param>
		/// <returns>corresponding Color</returns>
		public static Color Convert(GraphColor c, int alpha)
		{
			Color col;

			switch(c)
			{
				case GraphColor.Gray: 
					col=Color.Gray;
					break;
				case GraphColor.Black: 
					col=Color.Black;
					break;
				default:
					col=Color.White;
					break;
			}
			return Color.FromArgb(alpha,col.R,col.G,col.B);
		}

		/// <summary>
		/// Converts GraphColor to System.Drawing.Color
		/// </summary>
		/// <param name="c">graph color to convert</param>
		/// <returns>corresponding Color</returns>
		public static Color Convert(GraphColor c)
		{
			switch(c)
			{
				case GraphColor.Gray: return Color.Gray;
				case GraphColor.Black: return Color.Black;
				default:
					return Color.White;
			}
		}
	}
}
