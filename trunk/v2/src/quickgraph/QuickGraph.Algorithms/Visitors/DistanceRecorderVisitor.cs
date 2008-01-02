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


namespace QuickGraph.Algorithms.Visitors
{
	using System;

	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Visitors;

	/// <summary>
	/// Records the vertex distance
	/// </summary>
	public class DistanceRecorderVisitor :
		IDistanceRecorderVisitor
	{
		private VertexIntDictionary distances;

		/// <summary>
		/// Default constructor
		/// </summary>
		public DistanceRecorderVisitor()
		{
			this.distances = new VertexIntDictionary();
		}

		/// <summary>
		/// Uses the dictionary to record the distance
		/// </summary>
		/// <param name="distances">Distance dictionary</param>
		/// <exception cref="ArgumentNullException">distances is null</exception>
		public DistanceRecorderVisitor(VertexIntDictionary distances)
		{
			if (distances == null)
				throw new ArgumentNullException("distances");

			this.distances = distances;
		}

		/// <summary>
		/// Vertex distance dictionary
		/// </summary>
		public VertexIntDictionary Distances
		{
			get
			{
				return this.distances;
			}
		}

		/// <summary>
		/// d[u] = + intfy
		/// </summary>
		/// <param name="sender">Algorithm using the visitor</param>
		/// <param name="args">Contains the vertex</param>
		public void InitializeVertex(Object sender, VertexEventArgs args)
		{
			this.distances[args.Vertex] = int.MaxValue;
		}
		
		/// <summary>
		/// d[u] = 0;
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void DiscoverVertex(Object sender, VertexEventArgs args)
		{
			this.distances[args.Vertex]=0;
		}

		/// <summary>
		/// Let e = (u,v), d[ v ] = d[ u ] + 1; 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void TreeEdge(Object sender, EdgeEventArgs args)
		{
			this.distances[args.Edge.Target] = this.distances[args.Edge.Source] + 1;
		}
	}
}
