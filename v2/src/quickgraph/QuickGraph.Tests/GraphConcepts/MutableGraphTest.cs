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
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Exceptions;

	public class DummyEdgeEqualPredicate : IEdgePredicate
	{
		IEdge re;
		bool th;

		public DummyEdgeEqualPredicate(IEdge e, bool throwIfTrue)
		{
			re = e;
			th = throwIfTrue;
		}
		public bool Test(IEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("e");

			if (th && e==re)
				throw new Exception("e == re");
			return e==re;
		}
	}

	[TypeFixture(typeof(IEdgeMutableGraph),"Fixture for the IEdgeMutableGraph concept")]
	[ProviderFactory(typeof(AdjacencyGraphGenerator),typeof(IEdgeMutableGraph))]
	[ProviderFactory(typeof(BidirectionalGraphGenerator),typeof(IEdgeMutableGraph))]
	[ProviderFactory(typeof(CustomAdjacencyGraphGenerator),typeof(IEdgeMutableGraph))]
	[Pelikhan]
	public class MutableGraphTest
	{
		[Test]
		public void AddRemoveVertex(IEdgeMutableGraph g)
		{
			IVertex v = g.AddVertex();
			g.RemoveVertex(v);
		}

		[Test]
		[ExpectedException(typeof(VertexNotFoundException))]
		public void AddRemoveVertexNotFound(IEdgeMutableGraph g)
		{
			IVertex v = g.AddVertex();
			g.RemoveVertex(v);
			g.RemoveVertex(v);
		}

		[Test]
		public void AddEdge(IEdgeMutableGraph g)
		{
			IVertex v = g.AddVertex();
			IVertex u  = g.AddVertex();
			IEdge e = g.AddEdge(u,v);

			Assert.AreEqual(e.Source,u);
			Assert.AreEqual(e.Target,v);
		}

		[Test]
		[ExpectedException(typeof(VertexNotFoundException))]
		public void AddEdgeSourceNotFound(IEdgeMutableGraph g)
		{
			IVertex u  = g.AddVertex();
			g.RemoveVertex(u);
			IVertex v = g.AddVertex();
			IEdge e = g.AddEdge(u,v);
		}


		[Test]
		[ExpectedException(typeof(VertexNotFoundException))]
		public void AddEdgeTargetNotFound(IEdgeMutableGraph g)
		{
			IVertex u  = g.AddVertex();
			IVertex v = g.AddVertex();
			g.RemoveVertex(v);
			IEdge e = g.AddEdge(u,v);
		}

		[Test]
		public void RemoveEdge(IEdgeMutableGraph g)
		{
			IVertex v = g.AddVertex();
			IVertex u  = g.AddVertex();
			IEdge e = g.AddEdge(u,v);
			g.RemoveEdge(e);
		}

		[Test]
		[ExpectedException(typeof(EdgeNotFoundException))]
		public void RemoveEdgeNotFound(IEdgeMutableGraph g)
		{
			IVertex v = g.AddVertex();
			IVertex u  = g.AddVertex();
			IEdge e = g.AddEdge(u,v);
			g.RemoveEdge(e);
			g.RemoveEdge(e);
		}

		[Test]
		public void ClearVertexSourceTarget(IEdgeMutableGraph g)
		{
			IVertex v = g.AddVertex();
			IVertex u  = g.AddVertex();
			g.AddEdge(u,v);
			g.ClearVertex(u);
			g.ClearVertex(v);
		}

		[Test]
		public void ClearVertexTargetSource(IEdgeMutableGraph g)
		{
			IVertex u  = g.AddVertex();
			IVertex v = g.AddVertex();
			g.AddEdge(u,v);
			g.ClearVertex(v);
			g.ClearVertex(u);
		}
	}
}
