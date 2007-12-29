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

namespace QuickGraph.UnitTests.Algorithms
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	using QuickGraph.UnitTests.Generators;
	using QuickGraph.Exceptions;
	using QuickGraph.Representations;
	using QuickGraph.Providers;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Algorithms;
	using System.Collections;

	[TestFixture]
	public class TopologicalSortAlgorithmTest
	{
		[Test,Ignore("Test needs to be rewritten")]
		public void Sort()
		{
			AdjacencyGraph g = new AdjacencyGraph(true);
			Hashtable iv = new Hashtable();
			
			int i = 0;
			IVertex a = g.AddVertex();
			iv[i++]=a;
			IVertex b = g.AddVertex();
			iv[i++]=b;
			IVertex c = g.AddVertex();
			iv[i++]=c;
			IVertex d = g.AddVertex();
			iv[i++]=d;
			IVertex e = g.AddVertex();
			iv[i++]=e;

			g.AddEdge(a,b);
			g.AddEdge(a,c);
			g.AddEdge(b,c);
			g.AddEdge(c,d);				
			g.AddEdge(a,e);				

			TopologicalSortAlgorithm topo = new TopologicalSortAlgorithm(g);
			topo.Compute();
		}

		[Test]
		[ExpectedException(typeof(NonAcyclicGraphException))]
		public void SortCyclic()
		{
			AdjacencyGraph g = new AdjacencyGraph(true);
			
			IVertex a = g.AddVertex();
			IVertex b = g.AddVertex();
			IVertex c = g.AddVertex();
			IVertex d = g.AddVertex();
			IVertex e = g.AddVertex();

			g.AddEdge(a,b);
			g.AddEdge(a,c);
			g.AddEdge(b,c);
			g.AddEdge(c,d);				
			g.AddEdge(a,e);				
			g.AddEdge(c,a);				

			TopologicalSortAlgorithm topo = new TopologicalSortAlgorithm(g);
			topo.Compute();
		}
	}
}
