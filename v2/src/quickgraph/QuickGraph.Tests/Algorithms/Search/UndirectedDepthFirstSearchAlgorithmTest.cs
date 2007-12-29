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

namespace QuickGraph.Tests.Algorithms.Search
{
    using MbUnit.Framework;
    using MbUnit.Core.Framework;

    using QuickGraph.Tests.Generators;

    using QuickGraph.Concepts;
    using QuickGraph.Concepts.Traversals;
    using QuickGraph.Concepts.Modifications;
    using QuickGraph.Algorithms;
    using QuickGraph.Algorithms.Search;
    using QuickGraph.Collections;
    using QuickGraph.Representations;

    [TestFixture]
    public class UndirectedDepthFirstAlgorithmSearchTest
    {
        private VertexVertexDictionary parents;
        private VertexIntDictionary discoverTimes;
        private VertexIntDictionary finishTimes;
        private int time;
        private BidirectionalGraph g;
        private UndirectedDepthFirstSearchAlgorithm dfs;

        public VertexVertexDictionary Parents
        {
            get
            {
                return parents;
            }
        }

        public VertexIntDictionary DiscoverTimes
        {
            get
            {
                return discoverTimes;
            }
        }

        public VertexIntDictionary FinishTimes
        {
            get
            {
                return finishTimes;
            }
        }

        public void StartVertex(Object sender, VertexEventArgs args)
        {
            Assert.IsTrue(sender is UndirectedDepthFirstSearchAlgorithm);
            UndirectedDepthFirstSearchAlgorithm algo = (UndirectedDepthFirstSearchAlgorithm)sender;

            Assert.AreEqual(algo.Colors[args.Vertex], GraphColor.White);
        }

        public void DiscoverVertex(Object sender, VertexEventArgs args)
        {
            Assert.IsTrue(sender is UndirectedDepthFirstSearchAlgorithm);
            UndirectedDepthFirstSearchAlgorithm algo = (UndirectedDepthFirstSearchAlgorithm)sender;

            Assert.AreEqual(algo.Colors[args.Vertex], GraphColor.Gray);
            Assert.AreEqual(algo.Colors[Parents[args.Vertex]], GraphColor.Gray);

            DiscoverTimes[args.Vertex] = time++;
        }

        public void ExamineEdge(Object sender, EdgeEventArgs args)
        {
            Assert.IsTrue(sender is UndirectedDepthFirstSearchAlgorithm);
            UndirectedDepthFirstSearchAlgorithm algo = (UndirectedDepthFirstSearchAlgorithm)sender;

            bool sourceGray = algo.Colors[args.Edge.Source] == GraphColor.Gray;
            bool targetGray = algo.Colors[args.Edge.Target] == GraphColor.Gray;
            Assert.IsTrue(sourceGray || targetGray);
        }

        public void TreeEdge(Object sender, EdgeEventArgs args)
        {
            Assert.IsTrue(sender is UndirectedDepthFirstSearchAlgorithm);
            UndirectedDepthFirstSearchAlgorithm algo = (UndirectedDepthFirstSearchAlgorithm)sender;

            bool sourceWhite = algo.Colors[args.Edge.Source]== GraphColor.White;
            bool targetWhite = algo.Colors[args.Edge.Target] == GraphColor.White;

            Assert.IsTrue(sourceWhite || targetWhite);
            if (targetWhite)
                Parents[args.Edge.Target] = args.Edge.Source;
            else
                Parents[args.Edge.Source] = args.Edge.Target;
        }

        public void BackEdge(Object sender, EdgeEventArgs args)
        {
            Assert.IsTrue(sender is UndirectedDepthFirstSearchAlgorithm);
            UndirectedDepthFirstSearchAlgorithm algo = (UndirectedDepthFirstSearchAlgorithm)sender;

            bool sourceGray = algo.Colors[args.Edge.Source]== GraphColor.Gray;
            bool targetGray = algo.Colors[args.Edge.Target] == GraphColor.Gray;
            Assert.IsTrue(sourceGray || targetGray);
        }

        public void FinishVertex(Object sender, VertexEventArgs args)
        {
            Assert.IsTrue(sender is UndirectedDepthFirstSearchAlgorithm);
            UndirectedDepthFirstSearchAlgorithm algo = (UndirectedDepthFirstSearchAlgorithm)sender;

            Assert.AreEqual(algo.Colors[args.Vertex], GraphColor.Black);
            FinishTimes[args.Vertex] = time++;
        }

        public bool IsDescendant(IVertex u, IVertex v)
        {
            IVertex t = null;
            IVertex p = u;
            do
            {
                t = p;
                p = Parents[t];
                if (p == v)
                    return true;
            }
            while (t != p);

            return false;
        }

        [SetUp]
        public void Init()
        {

            parents = new VertexVertexDictionary();
            discoverTimes = new VertexIntDictionary();
            finishTimes = new VertexIntDictionary();
            time = 0;
            g = new BidirectionalGraph(true);
            dfs = new UndirectedDepthFirstSearchAlgorithm(g);

            dfs.StartVertex += new VertexEventHandler(this.StartVertex);
            dfs.DiscoverVertex += new VertexEventHandler(this.DiscoverVertex);
            dfs.ExamineEdge += new EdgeEventHandler(this.ExamineEdge);
            dfs.TreeEdge += new EdgeEventHandler(this.TreeEdge);
            dfs.BackEdge += new EdgeEventHandler(this.BackEdge);
            dfs.FinishVertex += new VertexEventHandler(this.FinishVertex);
        }

        [Test]
        public void GraphWithSelfEdges()
        {
            RandomGraph.Graph(g, 20, 100, new Random(), true);

            foreach (IVertex v in g.Vertices)
                Parents[v] = v;

            // compute
            dfs.Compute();

            CheckDfs();
        }

        [Test]
        public void GraphWithoutSelfEdges()
        {
            RandomGraph.Graph(g, 20, 100, new Random(), false);

            foreach (IVertex v in g.Vertices)
                Parents[v] = v;

            // compute
            dfs.Compute();

            CheckDfs();
        }

        protected void CheckDfs()
        {
            // check
            // all vertices should be black
            foreach (IVertex v in g.Vertices)
            {
                Assert.IsTrue(dfs.Colors.Contains(v));
                Assert.AreEqual(dfs.Colors[v], GraphColor.Black);
            }

            // check parenthesis structure of discover/finish times
            // See CLR p.480
/*            foreach (IVertex u in g.Vertices)
            {
                foreach (IVertex v in g.Vertices)
                {
                    if (u != v)
                    {
                        Assert.IsTrue(
                            FinishTimes[u] < DiscoverTimes[v]
                            || FinishTimes[v] < DiscoverTimes[u]
                            || (
                            DiscoverTimes[v] < DiscoverTimes[u]
                            && FinishTimes[u] < FinishTimes[v]
                            && IsDescendant(u, v)
                            )
                            || (
                            DiscoverTimes[u] < DiscoverTimes[v]
                            && FinishTimes[v] < FinishTimes[u]
                            && IsDescendant(v, u)
                            )
                            );
                    }
                }
            }
*/
        }
    }
}
