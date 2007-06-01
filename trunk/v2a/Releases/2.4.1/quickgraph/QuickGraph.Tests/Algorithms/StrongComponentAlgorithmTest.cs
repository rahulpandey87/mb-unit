using System;
using System.Collections;
using QuickGraph.Concepts;
using QuickGraph.Algorithms;
using QuickGraph.Representations;
using QuickGraph.Concepts.Traversals;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace QuickGraph.UnitTests.Algorithms
{
	[TestFixture]
	public class StrongComponentAlgorithmTest
	{
		[Test]
		public void EmptyGraph()
		{
			IVertexListGraph g = GraphFactory.EmptyParallelEdgesAllowed();
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);

			Assert.AreEqual(0, strong.Compute());

			checkStrong(strong);
		}

		[Test]
		public void OneVertex()
		{
			AdjacencyGraph g =new AdjacencyGraph(true);
			IVertex v1 = g.AddVertex();
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);
			Assert.AreEqual(1, strong.Compute());

			checkStrong(strong);
		}

		[Test]
		public void TwoVertex()
		{
			AdjacencyGraph g =new AdjacencyGraph(true);
			IVertex v1 = g.AddVertex();
			IVertex v2 = g.AddVertex();
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);
			Assert.AreEqual(2, strong.Compute());

			checkStrong(strong);
		}	

		[Test]
		public void TwoVertexOnEdge()
		{
			AdjacencyGraph g =new AdjacencyGraph(true);
			IVertex v1 = g.AddVertex();
			IVertex v2 = g.AddVertex();
			IEdge e = g.AddEdge(v1,v2);
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);
			Assert.AreEqual(2, strong.Compute());

			checkStrong(strong);
		}


		[Test]
		public void TwoVertexCycle()
		{
			AdjacencyGraph g =new AdjacencyGraph(true);
			IVertex v1 = g.AddVertex();
			IVertex v2 = g.AddVertex();
			IEdge e1 = g.AddEdge(v1,v2);
			IEdge e2 = g.AddEdge(v2,v1);
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);
			Assert.AreEqual(1, strong.Compute());

			checkStrong(strong);
		}

		[Test]
		public void Loop()
		{
			IVertexListGraph g = GraphFactory.Loop();
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);
			Assert.AreEqual(1, strong.Compute());

			strong.Compute();
			checkStrong(strong);
		}

		[Test]
		public void Simple()
		{
			IVertexListGraph g = GraphFactory.Simple();
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);

			strong.Compute();
			checkStrong(strong);
		}

		[Test]
		public void FileDependency()
		{
			IVertexListGraph g = GraphFactory.FileDependency();
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);

			strong.Compute();
			checkStrong(strong);
		}


		[Test]
		public void Fsm()
		{
			IVertexListGraph g = GraphFactory.Fsm();
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);
	
			strong.Compute();
			checkStrong(strong);
		}


		[Test]
		public void RegularLattice()
		{
			IVertexListGraph g = GraphFactory.RegularLattice(10,10);
			StrongComponentsAlgorithm strong = new StrongComponentsAlgorithm(g);

			strong.Compute();
			checkStrong(strong);
		}

		private void checkStrong(StrongComponentsAlgorithm strong)
		{
			Assert.AreEqual(strong.VisitedGraph.VerticesCount, strong.Components.Count);
			Assert.AreEqual(strong.VisitedGraph.VerticesCount, strong.DiscoverTimes.Count);
			Assert.AreEqual(strong.VisitedGraph.VerticesCount, strong.Roots.Count);

			foreach(IVertex v in strong.VisitedGraph.Vertices)
			{
				Assert.IsTrue(strong.Components.Contains(v));
				Assert.IsTrue(strong.DiscoverTimes.Contains(v));
			}

			foreach(DictionaryEntry de in strong.Components)
			{
				Assert.IsNotNull(de.Key);
				Assert.IsNotNull(de.Value);
				Assert.LowerEqualThan((int)de.Value,strong.Count);
			}

			foreach(DictionaryEntry de in strong.DiscoverTimes)
			{
				Assert.IsNotNull(de.Key);
				Assert.IsNotNull(de.Value);
			}
		}
	}
}
