using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

using QuickGraph.Representations;
using QuickGraph.Concepts;
using QuickGraph.Algorithms.RandomWalks;

namespace QuickGraph.Tests.Algorithms.RandomWalks
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="CyclePoppingRandomTreeAlgorithm"/> 
    /// class
    /// </summary>
    [TestFixture]
    [TestsOn(typeof(CyclePoppingRandomTreeAlgorithm))]
    public class CyclePoppingRandomTreeAlgorithmTest
    {
        private CyclePoppingRandomTreeAlgorithm target = null;

        [Test]
        public void IsolatedVertex()
        {
            AdjacencyGraph g = new AdjacencyGraph();
            g.AddVertex();

            target = new CyclePoppingRandomTreeAlgorithm(g);
            target.RandomTree();
        }

        [Test]
        public void RootIsNotAccessible()
        {
            AdjacencyGraph g = new AdjacencyGraph();
            IVertex root = g.AddVertex();
            IVertex v = g.AddVertex();
            g.AddEdge(root, v);

            target = new CyclePoppingRandomTreeAlgorithm(g);
            target.RandomTreeWithRoot(root);
        }

        [Test]
        public void Loop()
        {
            target = new CyclePoppingRandomTreeAlgorithm(GraphFactory.Loop());
            target.RandomTree();
        }

    }
}