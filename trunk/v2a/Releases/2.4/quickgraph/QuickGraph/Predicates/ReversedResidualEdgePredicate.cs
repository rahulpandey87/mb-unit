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
	using System.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// Predicate that test if an edge's reverse is residual
	/// </summary>
	/// <remarks>
	/// <para>
	/// Given a capacity map and reversed edge map, the predicate returns true if the 
	/// reversed edge's capacity is positive: <c>0 &lt; Capacities[ReversedEdges[e]]</c>
	/// </para>
	/// </remarks>
	public class ReversedResidualEdgePredicate :
		IEdgePredicate
	{
		private IDictionary residualCapacities;
		private IDictionary reversedEdges;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="residualCapacities">Residual Edge capacities map</param>
		/// <param name="reversedEdges">Reversed Edge map</param>
		/// <exception cref="ArgumentNullException">residualCapacities is null</exception>
		public ReversedResidualEdgePredicate(IDictionary residualCapacities, IDictionary reversedEdges)
		{
			if (residualCapacities == null)
				throw new ArgumentNullException("residualCapacities");
			if (reversedEdges == null)
				throw new ArgumentNullException("reversedEdges");
			this.residualCapacities = residualCapacities;
			this.reversedEdges = reversedEdges;
		}

		/// <summary>
		/// Residual capacities map
		/// </summary>
		public IDictionary ResidualCapacities
		{
			get
			{
				return this.residualCapacities;
			}
		}

		/// <summary>
		/// Reversed edges map
		/// </summary>
		public IDictionary ReversedEdges
		{
			get
			{
				return this.reversedEdges;
			}
		}

		/// <summary>
		/// Test if edge e has a positive residual capacity
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>0 &lt; ResidualCapacities[e]</returns>
		/// <exception cref="ArgumentNullException">e is null</exception>
		public bool Test(IEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			return 0 < (double)this.residualCapacities[(IEdge)reversedEdges[e]];
		}
	}
}
