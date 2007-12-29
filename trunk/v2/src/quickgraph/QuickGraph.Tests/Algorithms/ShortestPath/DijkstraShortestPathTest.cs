using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace QuickGraph.Tests.Algorithms.ShortestPath
{
	using QuickGraph.Algorithms.ShortestPath;
	using QuickGraph.Concepts;
	using QuickGraph;
	using QuickGraph.Representations;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms.Visitors;

	[TestFixture]
	public class DijkstraShortestPathTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateAlgorithmWithNullGraph()
		{
			DijkstraShortestPathAlgorithm dij = new DijkstraShortestPathAlgorithm(null,null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateAlgorithmWithNullWeights()
		{
			AdjacencyGraph g = new AdjacencyGraph(true); 
			DijkstraShortestPathAlgorithm dij = new DijkstraShortestPathAlgorithm(g,null);
		}

		[Test]
		public void UnaryWeights()
		{
			AdjacencyGraph g = new AdjacencyGraph(true); 
			EdgeDoubleDictionary weights = DijkstraShortestPathAlgorithm.UnaryWeightsFromEdgeList(g);
		}

		[Test]
		public void AttachPredecessorRecorderVisitor()
		{
			AdjacencyGraph g = new AdjacencyGraph(true); 
			EdgeDoubleDictionary weights = DijkstraShortestPathAlgorithm.UnaryWeightsFromEdgeList(g);
			DijkstraShortestPathAlgorithm dij = new DijkstraShortestPathAlgorithm(g,weights);
			PredecessorRecorderVisitor vis = new PredecessorRecorderVisitor();
			dij.RegisterPredecessorRecorderHandlers(vis);
		}

		[Test]
		public void AttachDistanceRecorderVisitor()
		{
			AdjacencyGraph g = new AdjacencyGraph(true); 
			EdgeDoubleDictionary weights = DijkstraShortestPathAlgorithm.UnaryWeightsFromEdgeList(g);
			DijkstraShortestPathAlgorithm dij = new DijkstraShortestPathAlgorithm(g,weights);
			DistanceRecorderVisitor vis = new DistanceRecorderVisitor();
			dij.RegisterDistanceRecorderHandlers(vis);
		}

		[Test]
		public void RunOnLineGraph()
		{
			AdjacencyGraph g = new AdjacencyGraph(true); 
			IVertex v1 = g.AddVertex();
			IVertex v2 = g.AddVertex();
			IVertex v3 = g.AddVertex();

			IEdge e12 = g.AddEdge(v1,v2);
			IEdge e23 = g.AddEdge(v2,v3);

			EdgeDoubleDictionary weights = DijkstraShortestPathAlgorithm.UnaryWeightsFromEdgeList(g);
			DijkstraShortestPathAlgorithm dij = new DijkstraShortestPathAlgorithm(g,weights);
			dij.Compute(v1);

			Assert.AreEqual(0, dij.Distances[v1]);
			Assert.AreEqual(1, dij.Distances[v2]);
			Assert.AreEqual(2, dij.Distances[v3]);
		}

		[Test]
		public void CheckPredecessorLineGraph()
		{
			AdjacencyGraph g = new AdjacencyGraph(true); 
			IVertex v1 = g.AddVertex();
			IVertex v2 = g.AddVertex();
			IVertex v3 = g.AddVertex();

			IEdge e12 = g.AddEdge(v1,v2);
			IEdge e23 = g.AddEdge(v2,v3);

			EdgeDoubleDictionary weights = DijkstraShortestPathAlgorithm.UnaryWeightsFromEdgeList(g);
			DijkstraShortestPathAlgorithm dij = new DijkstraShortestPathAlgorithm(g,weights);
			PredecessorRecorderVisitor vis = new PredecessorRecorderVisitor();
			dij.RegisterPredecessorRecorderHandlers(vis);

			dij.Compute(v1);

			EdgeCollection col = vis.Path(v2);
			Assert.AreEqual(1,col.Count);
			Assert.AreEqual(e12,col[0]);

			col = vis.Path(v3);
			Assert.AreEqual(2,col.Count);
			Assert.AreEqual(e12,col[0]);
			Assert.AreEqual(e23,col[1]);
		}


		[Test]
		public void RunOnDoubleLineGraph()
		{
			AdjacencyGraph g = new AdjacencyGraph(true); 
			IVertex v1 = g.AddVertex();
			IVertex v2 = g.AddVertex();
			IVertex v3 = g.AddVertex();

			IEdge e12 = g.AddEdge(v1,v2);
			IEdge e23 = g.AddEdge(v2,v3);
			IEdge e13 = g.AddEdge(v1,v3);

			EdgeDoubleDictionary weights = DijkstraShortestPathAlgorithm.UnaryWeightsFromEdgeList(g);
			DijkstraShortestPathAlgorithm dij = new DijkstraShortestPathAlgorithm(g,weights);
			dij.Compute(v1);

			Assert.AreEqual(0, dij.Distances[v1]);
			Assert.AreEqual(1, dij.Distances[v2]);
			Assert.AreEqual(1, dij.Distances[v3]);
		}

		[Test]
		public void CheckPredecessorDoubleLineGraph()
		{
			AdjacencyGraph g = new AdjacencyGraph(true); 
			IVertex v1 = g.AddVertex();
			IVertex v2 = g.AddVertex();
			IVertex v3 = g.AddVertex();

			IEdge e12 = g.AddEdge(v1,v2);
			IEdge e23 = g.AddEdge(v2,v3);
			IEdge e13 = g.AddEdge(v1,v3);

			EdgeDoubleDictionary weights = DijkstraShortestPathAlgorithm.UnaryWeightsFromEdgeList(g);
			DijkstraShortestPathAlgorithm dij = new DijkstraShortestPathAlgorithm(g,weights);
			PredecessorRecorderVisitor vis = new PredecessorRecorderVisitor();
			dij.RegisterPredecessorRecorderHandlers(vis);

			dij.Compute(v1);

			EdgeCollection col = vis.Path(v2);
			Assert.AreEqual(1,col.Count);
			Assert.AreEqual(e12,col[0]);

			col = vis.Path(v3);
			Assert.AreEqual(1,col.Count);
			Assert.AreEqual(e13,col[0]);
		}
	}
}
