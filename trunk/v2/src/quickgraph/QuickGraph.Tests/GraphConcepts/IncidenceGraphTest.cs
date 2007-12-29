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

namespace QuickGraph.Tests.GraphConcepts
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;
	using QuickGraph.Tests.Generators;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;

	[TypeFixture(typeof(IIncidenceGraph),"Fixture for the IIncidenceGraph concept")]
	[ProviderFactory(typeof(AdjacencyGraphGenerator),typeof(IIncidenceGraph))]
	[ProviderFactory(typeof(BidirectionalGraphGenerator),typeof(IIncidenceGraph))]
	[ProviderFactory(typeof(CustomAdjacencyGraphGenerator),typeof(IIncidenceGraph))]
	[Pelikhan]
	public class IncidenceGraphTest
	{
		[Test]
		[Ignore("Not implemented yet")]
		public void TestOutDegree(IIncidenceGraph g)
		{
		}
	}
}
