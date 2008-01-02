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

namespace QuickGraph.Tests.Algorithms.MaximumFlow
{
	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	using QuickGraph.Tests.Generators;

	using QuickGraph.Concepts;
	using QuickGraph.Algorithms;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Algorithms.MaximumFlow;
	using QuickGraph.Algorithms.Visitors;
	using QuickGraph.Collections;
	using QuickGraph.Collections.Filtered;
	using QuickGraph.Predicates;
	using QuickGraph.Representations;

	
	[TypeFixture(typeof(MaximumFlowAlgorithm))]
	[Author("Arun Bhalla", "bhalla@uiuc.edu")]
	public class MaximumFlowAlgorithmTest
	{
		private EdgeDoubleDictionary capacities;
		private EdgeEdgeDictionary reversedEdges;
		private AdjacencyGraph graph;
		private IVertex source;
		private IVertex sink;

		public EdgeDoubleDictionary Capacities
		{
			get { return capacities; }
		}

		public EdgeEdgeDictionary ReversedEdges
		{
			get { return reversedEdges; }
		}

		public AdjacencyGraph Graph
		{
			get { return graph; }
		}

		[SetUp]
		public void Init()
		{
			this.capacities = new EdgeDoubleDictionary();
			this.reversedEdges = new EdgeEdgeDictionary();
			this.graph = new AdjacencyGraph();

			this.source = graph.AddVertex();
			this.sink = graph.AddVertex();

			BuildSimpleGraph(source, sink);
		}

		[Provider(typeof(EdmondsKarpMaximumFlowAlgorithm))]
		public EdmondsKarpMaximumFlowAlgorithm EdmundsKarp()
		{
			EdmondsKarpMaximumFlowAlgorithm maxFlow = new EdmondsKarpMaximumFlowAlgorithm(
				Graph, 
				Capacities, 
				ReversedEdges
				);
			return maxFlow;
		}

		[Provider(typeof(PushRelabelMaximumFlowAlgorithm))]
		public PushRelabelMaximumFlowAlgorithm PushRelabel()
		{
			PushRelabelMaximumFlowAlgorithm maxFlow = new PushRelabelMaximumFlowAlgorithm(
				Graph, 
				Capacities, 
				ReversedEdges
				);
			return maxFlow;
		}

		[Test]
		public void SimpleGraph(MaximumFlowAlgorithm maxFlow)
		{
			double flow = maxFlow.Compute(source, sink);
			Assert.AreEqual(23, flow, double.Epsilon);
			Assert.IsTrue(IsFlow(maxFlow));
			Assert.IsTrue(IsOptimal(maxFlow));
		}

		private void BuildSimpleGraph(IVertex source, IVertex sink)
		{
			IVertex v1, v2, v3, v4;
			v1 = graph.AddVertex();
			v2 = graph.AddVertex();
			v3 = graph.AddVertex();
			v4 = graph.AddVertex();

			AddLink(source, v1, 16);
			AddLink(source, v2, 13);
			AddLink(v1, v2, 10);
			AddLink(v2, v1, 4);
			AddLink(v1, v3, 12);
			AddLink(v2, v4, 14);
			AddLink(v3, v2, 9);
			AddLink(v4, v3, 7);
			AddLink(v3, sink, 20);
			AddLink(v4, sink, 4);
		}

		private void AddLink(IVertex u, IVertex v, double capacity)
		{
			IEdge edge, reverseEdge;

			edge = Graph.AddEdge(u, v);
			Capacities[edge] = capacity;

			reverseEdge = Graph.AddEdge(v, u);
			Capacities[reverseEdge] = 0;

			ReversedEdges[edge] = reverseEdge;
			ReversedEdges[reverseEdge] = edge;
		}

		private bool IsFlow(MaximumFlowAlgorithm maxFlow)
		{
			// check edge flow values
			foreach (IVertex u in maxFlow.VisitedGraph.Vertices) 
			{
				foreach (IEdge a in maxFlow.VisitedGraph.OutEdges(u))
				{
					if (maxFlow.Capacities[a] > 0)
						if ((maxFlow.ResidualCapacities[a] + maxFlow.ResidualCapacities[maxFlow.ReversedEdges[a]] 
							!= maxFlow.Capacities[a])
							|| (maxFlow.ResidualCapacities[a] < 0)
							|| (maxFlow.ResidualCapacities[maxFlow.ReversedEdges[a]] < 0))
							return false;
				}
			}

			// check conservation
			VertexDoubleDictionary inFlows = new VertexDoubleDictionary();
			VertexDoubleDictionary outFlows = new VertexDoubleDictionary();
			foreach (IVertex u in maxFlow.VisitedGraph.Vertices)
			{
				inFlows[u] = 0;
				outFlows[u] = 0;
			}

			foreach (IVertex u in maxFlow.VisitedGraph.Vertices)
			{
				foreach (IEdge e in maxFlow.VisitedGraph.OutEdges(u))
				{
					if (maxFlow.Capacities[e] > 0) 
					{
						double flow = maxFlow.Capacities[e] - maxFlow.ResidualCapacities[e];

						inFlows[e.Target] += flow;
						outFlows[e.Source] += flow;
					}
				}
			}

			foreach (IVertex u in maxFlow.VisitedGraph.Vertices)
			{
				if (u != source && u != sink)
					if (inFlows[u] != outFlows[u])
						return false;
			}

			return true;
		}

		private bool IsOptimal(MaximumFlowAlgorithm maxFlow)
		{
			// check if mincut is saturated...
			FilteredVertexListGraph residualGraph = new FilteredVertexListGraph(
				maxFlow.VisitedGraph, 
				new ReversedResidualEdgePredicate(maxFlow.ResidualCapacities, maxFlow.ReversedEdges)
				);
			BreadthFirstSearchAlgorithm bfs = new BreadthFirstSearchAlgorithm(residualGraph);

			VertexIntDictionary distances = new VertexIntDictionary();
			DistanceRecorderVisitor vis = new DistanceRecorderVisitor(distances);
			bfs.RegisterDistanceRecorderHandlers(vis);
			bfs.Compute(sink);

			return distances[source] >= maxFlow.VisitedGraph.VerticesCount;
		}
	
	}
}
