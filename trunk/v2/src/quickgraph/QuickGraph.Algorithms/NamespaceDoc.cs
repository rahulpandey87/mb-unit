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

namespace QuickGraph.Algorithms
{
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Algorithms.ShortestPath;
	using QuickGraph.Algorithms.AllShortestPath;
	using QuickGraph.Algorithms.Clone;
	using QuickGraph.Algorithms.MaximumFlow;
	using QuickGraph.Algorithms.PerfectMatching;
	using QuickGraph.Algorithms.Travelling;
	using QuickGraph.Algorithms.Visitors;

	/// <summary>
	/// The <b>QuickGraph.Algorithms</b> namespace is the base namespace
	/// for graph algorithms.
	/// </summary>
	/// <remarks>
	/// This namespace contains all the graph algorithms implements by quickgraph.
	/// Algorithms are classified by the type of problem they solve, and each
	/// problem is separated in it's own namespace:
	/// <list type="bulleted">
	/// <item>
	/// <term>Search</term>
	/// <description>Basic algorithms such as the <see cref="DepthFirstSearchAlgorithm"/> or
	/// the <see cref="BreadthFirstSearchAlgorithm"/>.
	/// </description>
	/// </item>
	/// <item>
	/// <term>ShortestPath</term>
	/// <description>Computes the single source shortest path problem.</description>
	/// </item>
	/// <item>
	/// <term>Clone</term>
	/// <description>Cloning of graph related algorithms</description>
	/// </item>
	/// <item>
	/// <term>MaximumFLow</term>
	/// <description>Netword maximu flow algorithms</description>
	/// </item>
	/// </list>
	/// <para>
	/// A number of algorithm supports visitors defined in the Visitor namespace.
	/// </para>
	/// </remarks>
	internal class NamespaceDoc
	{}
}
