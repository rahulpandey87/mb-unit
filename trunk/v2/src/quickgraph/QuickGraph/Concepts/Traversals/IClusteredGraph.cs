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
using System.Collections;

namespace QuickGraph.Concepts.Traversals
{
	/// <summary>
	/// A graph with clusters.
	/// </summary>
	public interface IClusteredGraph
	{
		/// <summary>
		/// Gets an enumerable collection of <see cref="IClusteredGraph"/>.
		/// </summary>
		IEnumerable Clusters {get;}

		/// <summary>
		/// Gets the number of clusters
		/// </summary>
		/// <remarks>
		/// Number of clusters.
		/// </remarks>
		int ClustersCount 
		{get;}

		/// <summary>
		/// Gets a value indicating wheter the cluster is collapsed
		/// </summary>
		/// <value>
		/// true if the cluster is colapsed; otherwize, false.
		/// </value>
		bool Colapsed {get; set;}

		/// <summary>
		/// Adds a new cluster to the graph.
		/// </summary>
		/// <returns>Added cluster</returns>
		IClusteredGraph AddCluster();

		/// <summary>
		/// Removes a cluster from the graph
		/// </summary>
		/// <param name="g">cluster to remove</param>
		/// <exception cref="ArgumentNullException">g is null</exception>
		void RemoveCluster(IClusteredGraph g);
	}
}
