using System;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Representations;
using QuickGraph.Algorithms;
using System.Collections;
using QuickGraph.Concepts.Collections;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using QuickGraph.Collections;
using QuickGraph.Exceptions;

namespace QuickGraph.Tests.Algorithms
{
    [TestFixture]
    public class SourceFirstTopologicalSortAlgorithmTest
    {
        private AdjacencyGraph g;
        private SourceFirstTopologicalSortAlgorithm topo;
        private VertexIntDictionary vertexRanks = new VertexIntDictionary();

        [SetUp]
        public void SetUp()
        {
            this.g = new AdjacencyGraph();
            this.topo = new SourceFirstTopologicalSortAlgorithm(g);
        }
        [Test]
        public void OneVertex()
        {
            this.vertexRanks.Add(g.AddVertex(),0);

            topo.Compute();
            this.CheckResult();
        }

        [Test]
        public void SelfLoop()
        {
            IVertex v = g.AddVertex();
            this.vertexRanks.Add(v, 0);

            g.AddEdge(v, v);

            topo.Compute();
            this.CheckResult();
        }

        [Test]
        public void TwoVerticesNoEdge()
        {
            this.vertexRanks.Add(g.AddVertex(),0);
            this.vertexRanks.Add(g.AddVertex(),0);

            topo.Compute();
            this.ShowResult();
            this.CheckResult();
        }

        [Test]
        public void TwoVerticesOneEdge()
        {
            IVertex u = g.AddVertex();
            this.vertexRanks.Add(u, 0);
            IVertex v = g.AddVertex();
            this.vertexRanks.Add(v, 1);
            IEdge e = g.AddEdge(u, v);

            topo.Compute();
            this.ShowResult();
            this.CheckResult();
        }

        [Test]
        public void ReversedTree()
        {
            IVertex u = g.AddVertex();
            this.vertexRanks.Add(u, 0);
            IVertex v = g.AddVertex();
            this.vertexRanks.Add(v, 1);
            IVertex w = g.AddVertex();
            this.vertexRanks.Add(w, 0);

            g.AddEdge(u, v);
            g.AddEdge(w, v);

            topo.Compute();
            this.ShowResult();
            this.CheckResult();
        }

        [Test]
        public void NonBalancedTree()
        {
            IVertex u = g.AddVertex();
            this.vertexRanks.Add(u, 0);
            IVertex o = g.AddVertex();
            this.vertexRanks.Add(o, 1);
            IVertex p = g.AddVertex();
            this.vertexRanks.Add(p, 0);
            IVertex op = g.AddVertex();
            this.vertexRanks.Add(op, 2);

            g.AddEdge(u, o);
            g.AddEdge(p, op);
            g.AddEdge(o, op);

            topo.Compute();
            this.ShowResult();
            this.CheckResult();
        }

        [Test]
        [ExpectedException(typeof(NonAcyclicGraphException))]
        public void Cycle()
        {
            IVertex o = g.AddVertex();
            this.vertexRanks.Add(o, -1);
            IVertex u = g.AddVertex();
            this.vertexRanks.Add(u, 0);
            IVertex v = g.AddVertex();
            this.vertexRanks.Add(v, 0);
            IVertex w = g.AddVertex();
            this.vertexRanks.Add(w, 0);
            IVertex c = g.AddVertex();
            this.vertexRanks.Add(c, 1);

            g.AddEdge(u, v);
            g.AddEdge(v, w);
            g.AddEdge(w, u);

            g.AddEdge(o, u);
            g.AddEdge(w, c);

            topo.Compute();
        }

        protected void ShowResult()
        {
            Console.WriteLine("Result:");
            foreach (IVertex v in this.topo.SortedVertices)
                Console.WriteLine("\t{0}",v);
        }

        protected void CheckResult()
        {
            CollectionAssert.AreCountEqual(g.VerticesCount, topo.SortedVertices);
            IVertex u = null;
            foreach (IVertex v in this.topo.SortedVertices)
            {
                if (u != null)
                    CheckOrder(u, v);
                u = v;
            }
        }

        protected void CheckOrder(IVertex u, IVertex v)
        {
            Assert.LowerEqualThan(
                this.vertexRanks[u],
                this.vertexRanks[v]
                );
        }
    }
}
