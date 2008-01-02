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



namespace QuickGraph.Predicates
{
	using System;
	using System.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// Predicate that test if an edge is residual
	/// </summary>
	/// <remarks>
	/// <para>
	/// Given a capacity map, the predicate returns true if the edge
	/// capacity is positive: <c>0 &lt; Capacities[e]</c>
	/// </para>
	/// </remarks>
	public class ResidualEdgePredicate :
		IEdgePredicate
	{
		private IDictionary residualCapacities;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="residualCapacities">Residual Edge capacities map</param>
		/// <exception cref="ArgumentNullException">residualCapacities is null</exception>
		public ResidualEdgePredicate(IDictionary residualCapacities)
		{
			if (residualCapacities == null)
				throw new ArgumentNullException("residualCapacities");
			this.residualCapacities = residualCapacities;
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
		/// Test if edge e has a positive residual capacity
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>0 &lt; ResidualCapacities[e]</returns>
		/// <exception cref="ArgumentNullException">e is null</exception>
		public bool Test(IEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			return 0 < (double)this.residualCapacities[e];
		}
	}
}
