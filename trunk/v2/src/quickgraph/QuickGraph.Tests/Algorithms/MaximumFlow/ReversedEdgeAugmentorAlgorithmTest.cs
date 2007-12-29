using System;
using QuickGraph;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.MaximumFlow;
using QuickGraph.Concepts;
using QuickGraph.Representations;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace QuickGraph.Tests.Algorithms.MaximumFlow
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="ReversedEdgeAugmentorAlgorithm"/> 
    /// class
    /// </summary>
    [TestFixture]
    [CurrentFixture]
    public class ReversedEdgeAugmentorAlgorithmTest
    {
        private ReversedEdgeAugmentorAlgorithm target = null;
    
        [Test]
        [ExpectedArgumentNullException]
        public void ConstructorWithNullGraph()
        {
            this.target = new ReversedEdgeAugmentorAlgorithm(null);
        }

        [Test]
        public void AddAndRemoveAndCheckAugmented()
        {
            this.target = new ReversedEdgeAugmentorAlgorithm(new AdjacencyGraph());
            Assert.IsFalse(target.Augmented);
            target.AddReversedEdges();
            Assert.IsTrue(target.Augmented);
            target.RemoveReversedEdges();
            Assert.IsFalse(target.Augmented);
        }

        [Test]
        public void AddAndRemoveOnEmptyGraph()
        {
            this.target = new ReversedEdgeAugmentorAlgorithm(new AdjacencyGraph());
            target.AddReversedEdges();
            Assert.AreEqual(0, this.target.VisitedGraph.VerticesCount);
            Assert.AreEqual(0, this.target.VisitedGraph.EdgesCount);
        }


        [Test]
        public void AddOneEdge()
        {
            AdjacencyGraph g = new AdjacencyGraph();
            IVertex v = g.AddVertex();
            IVertex u = g.AddVertex();
            IEdge edge = g.AddEdge(u, v);

            this.target = new ReversedEdgeAugmentorAlgorithm(g);
            target.AddReversedEdges();

            Assert.AreEqual(2, this.target.VisitedGraph.VerticesCount);
            Assert.AreEqual(2, this.target.VisitedGraph.EdgesCount);
            CollectionAssert.AreCountEqual(1, this.target.AugmentedEdges);
            VerifyReversedEdges();

            IEdge reversedEdge = this.target.ReversedEdges[edge];
            Assert.IsNotNull(reversedEdge);
            Assert.IsTrue(this.target.AugmentedEdges.Contains(reversedEdge));
        }


        [Test]
        public void AddAndRemoveOneEdge()
        {
            AdjacencyGraph g = new AdjacencyGraph();
            IVertex v = g.AddVertex();
            IVertex u = g.AddVertex();
            IEdge edge = g.AddEdge(u, v);

            this.target = new ReversedEdgeAugmentorAlgorithm(g);
            target.AddReversedEdges();
            target.RemoveReversedEdges();
            Assert.AreEqual(2, this.target.VisitedGraph.VerticesCount);
            Assert.AreEqual(1, this.target.VisitedGraph.EdgesCount);
            CollectionAssert.AreCountEqual(0, this.target.AugmentedEdges);
            Assert.AreEqual(0, this.target.ReversedEdges.Count);
        }

        [Test]
        public void AddAndRemoveOnFsm()
        {
            this.target = new ReversedEdgeAugmentorAlgorithm(GraphFactory.Fsm());
            this.target.AddReversedEdges();
            this.VerifyReversedEdges();
            this.target.RemoveReversedEdges();
        }

        [Test]
        public void AddAndRemoveOnFileDependency()
        {
            this.target = new ReversedEdgeAugmentorAlgorithm(GraphFactory.FileDependency());
            this.target.AddReversedEdges();
            this.VerifyReversedEdges();
            this.target.RemoveReversedEdges();
        }

        [Test]
        public void AddAndRemoveOnUnBalancedFlow()
        {
            this.target = new ReversedEdgeAugmentorAlgorithm(GraphFactory.UnBalancedFlow());
            this.target.AddReversedEdges();
            this.VerifyReversedEdges();
            this.target.RemoveReversedEdges();
        }

        private void VerifyReversedEdges()
        {
            Assert.AreEqual(this.target.VisitedGraph.EdgesCount, this.target.ReversedEdges.Count);
            foreach (IEdge edge in this.target.VisitedGraph.Edges)
            {
                Assert.IsTrue(this.target.ReversedEdges.Contains(edge), "Reversed edge map does not contains {0}", edge);
            }
        }
    }
}