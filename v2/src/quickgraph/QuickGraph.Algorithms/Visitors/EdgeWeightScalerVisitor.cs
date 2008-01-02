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

namespace QuickGraph.Algorithms.Visitors
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;

	/// <summary>
	/// Scales the edge weights at each call
	/// </summary>
	public class EdgeWeightScalerVisitor
	{
		private EdgeDoubleDictionary weights;
		private double factor;

		/// <summary>
		/// Constructs a edge weight scaler
		/// </summary>
		/// <param name="weights">edge weight dictionary</param>
		/// <param name="factor">weight scale factor</param>
		public EdgeWeightScalerVisitor(EdgeDoubleDictionary weights, double factor)
		{
			if (weights==null)
				throw new ArgumentNullException("weights");
			this.weights=weights;
			this.factor = factor;
		}

		/// <summary>
		/// Gets the edge weight dictionary
		/// </summary>
		/// <value>
		/// Edge weight dictionary
		/// </value>
		public EdgeDoubleDictionary Weights
		{
			get
			{
				return this.weights;
			}
		}

		/// <summary>
		/// Gets or sets the scale factor
		/// </summary>
		/// <value>
		/// Scale factor
		/// </value>
		public double Factor
		{
			get
			{
				return this.factor;
			}
			set
			{
				this.factor = value;
			}
		}

		/// <summary>
		/// Event handler that applies the factor the edge weight
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e">event arguement containing the edge</param>
		public void TreeEdge(Object sender, EdgeEventArgs e)
		{
			this.weights[e.Edge]=this.weights[e.Edge]*factor;
		}
	}
}
