using System;

using QuickGraph;
using QuickGraph.Representations;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.MinimumFlow;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace QuickGraph.Tests.Algorithms.MinimumFlow
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="GraphBalancerAlgorithm"/> 
    /// class
    /// </summary>
    [TestFixture]
    public class GraphBalancerAlgorithmTest
    {
        BidirectionalGraph g = null;
        GraphBalancerAlgorithm algo = null;

        #region Test cases
        [TearDown]
        public void TearDown()
        {
            if (algo == null)
                return;

            algo.DeficientVertexAdded -= new VertexEventHandler(algo_DeficientVertexAdded);
            algo.SurplusVertexAdded -= new VertexEventHandler(algo_SurplusVertexAdded);
        }
        [Test]
        [ExpectedArgumentNullException]
        public void ConstructorWithNullGraph()
        {
            Vertex v = new Vertex();
            new GraphBalancerAlgorithm(null, v, v);
        }
        [Test]
        [ExpectedArgumentNullException]
        public void ConstructorWithNullSource()
        {
            Vertex v = new Vertex();
            new GraphBalancerAlgorithm(GraphFactory.EmptyParallelEdgesAllowed(), null,v);
        }
        [Test]
        [ExpectedArgumentNullException]
        public void ConstructorWithNullSink()
        {
            g = GraphFactory.UnBalancedFlow();
            Vertex v = new Vertex();
            new GraphBalancerAlgorithm(g, Traversal.FirstVertex(g),null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithSourceNotPartOfTheGraph()
        {
            g = GraphFactory.UnBalancedFlow();
            Vertex v = new Vertex();
            new GraphBalancerAlgorithm(g, v, Traversal.FirstVertex(g));
        }
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithSinkNotPartOfTheGraph()
        {
            g = GraphFactory.UnBalancedFlow();
            Vertex v = new Vertex();
            new GraphBalancerAlgorithm(g, Traversal.FirstVertex(g), v);
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnBalanceBeforeBalancing()
        {
            g = GraphFactory.UnBalancedFlow();
            algo = new GraphBalancerAlgorithm(g, Traversal.FirstVertex(g), Traversal.FirstVertex(g));
            algo.UnBalance();
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BalanceTwice()
        {
            g = GraphFactory.UnBalancedFlow();
            algo = new GraphBalancerAlgorithm(g, Traversal.FirstVertex(g), Traversal.FirstVertex(g));
            algo.Balance();
            algo.Balance();
        }
        [Test]
        public void BalanceUnBalancedFlowGraph()
        {
            g = GraphFactory.UnBalancedFlow();
            IVertex source = null;
            IVertex sink = null;
            foreach (IVertex v in g.Vertices)
            {
                if (g.InDegree(v) == 0)
                {
                    source = v;
                    continue;
                }
                if (g.OutDegree(v) == 0)
                {
                    sink = v;
                    continue;
                }
            }
            Assert.IsNotNull(source);
            Assert.IsNotNull(sink);

            int vertexCount = g.VerticesCount;
            int edgeCount = g.EdgesCount;

            algo = new GraphBalancerAlgorithm(g,source, sink);
            algo.DeficientVertexAdded+=new VertexEventHandler(algo_DeficientVertexAdded);
            algo.SurplusVertexAdded+=new VertexEventHandler(algo_SurplusVertexAdded);
            algo.Balance();

            VerifyBalanced(vertexCount, edgeCount);
        }

        private void VerifyBalanced(int vertexCount, int edgeCount)
        {
            Assert.IsNotNull(algo.Source,"Source is null");
            Assert.IsNotNull(algo.Sink,"Sink is null");
            Assert.AreEqual(vertexCount + 2, g.VerticesCount,"Vertices count do not mach");
            Assert.AreEqual(edgeCount +2+ algo.SurplusEdges.Count + algo.DeficientEdges.Count, g.EdgesCount,
                "Edges count do not match");
        }
    
        void algo_DeficientVertexAdded(Object sender, VertexEventArgs e)
        {
            Console.WriteLine("Deficient vertex: {0}", ((NamedVertex)e.Vertex).Name);
        }

        void algo_SurplusVertexAdded(Object sender, VertexEventArgs e)
        {
            Console.WriteLine("Surplus vertex: {0}", ((NamedVertex)e.Vertex).Name);
        }
        #endregion
    }
}