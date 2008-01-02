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

namespace QuickGraph.Algorithms.MaximumFlow
{

	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;
    using QuickGraph.Concepts.MutableTraversals;
    using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Abstract base class for maximum flow algorithms.
	/// </summary>
	public abstract class MaximumFlowAlgorithm
	{
		private IVertexListGraph visitedGraph;
		private VertexEdgeDictionary predecessors;
		private EdgeDoubleDictionary capacities;
		private EdgeDoubleDictionary residualCapacities;
		private EdgeEdgeDictionary reversedEdges;
		private VertexColorDictionary colors;
	
		/// <summary>Constructs a maximum flow algorithm.</summary>
		/// <param name="g">Graph to compute maximum flow on.</param>
		/// <param name="capacities">edge capacities</param>
		/// <param name="reversedEdges">reversed edge map</param>
		/// <exception cref="ArgumentNullException"><paramref name="g"/> or
		/// <paramref name="capacities"/> or <paramref name="reversedEdges"/> is a null
		/// reference.
		/// </exception>
		public MaximumFlowAlgorithm(
			IVertexListGraph g,
			EdgeDoubleDictionary capacities,
			EdgeEdgeDictionary reversedEdges
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (capacities==null)
				throw new ArgumentNullException("capacities");
			if (reversedEdges==null)
				throw new ArgumentNullException("reversedEdges");
		
			this.visitedGraph=g;
			this.capacities=capacities;
			this.reversedEdges=reversedEdges;
		
			this.predecessors=new VertexEdgeDictionary();
			this.residualCapacities=new EdgeDoubleDictionary();
			this.colors = new VertexColorDictionary();
        }
	
		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public IVertexListGraph VisitedGraph
		{
			get
			{
				return visitedGraph;
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		public VertexEdgeDictionary Predecessors
		{
			get
			{
				return predecessors;
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		public EdgeDoubleDictionary Capacities
		{
			get
			{
				return capacities;
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		public EdgeDoubleDictionary ResidualCapacities
		{
			get
			{
				return residualCapacities;
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		public EdgeEdgeDictionary ReversedEdges
		{
			get
			{
				return reversedEdges;
			}
		}
	
		/// <summary>
		/// 
		/// </summary>
		public VertexColorDictionary Colors
		{
			get
			{
				return colors;
			}
		}
		#endregion	

		/// <summary>
		/// 
		/// </summary>
		/// <param name="src"></param>
		/// <param name="sink"></param>
		/// <returns></returns>
		public abstract double Compute(IVertex src, IVertex sink);
	}

}
