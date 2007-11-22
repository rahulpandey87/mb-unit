using System;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Collections;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.Collections;

namespace QuickGraph.Algorithms
{
    public class SourceFirstTopologicalSortAlgorithm : IAlgorithm
    {
        private IVertexAndEdgeListGraph visitedGraph;
        private VertexDoubleDictionary inDegrees = new VertexDoubleDictionary();
        private PriorithizedVertexBuffer heap;
        private VertexCollection sortedVertices = new VertexCollection();

        public SourceFirstTopologicalSortAlgorithm(
            IVertexAndEdgeListGraph visitedGraph
            )
        {
            this.visitedGraph = visitedGraph;
            this.heap = new PriorithizedVertexBuffer(this.inDegrees);
        }

        public IVertexAndEdgeListGraph VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }

        Object IAlgorithm.VisitedGraph
        {
            get
            {
                return this.visitedGraph;
            }
        }

        public IVertexCollection SortedVertices
        {
            get
            {
                return this.sortedVertices;
            }
        }

        public PriorithizedVertexBuffer Heap
        {
            get
            {
                return this.heap;
            }
        }

        public VertexDoubleDictionary InDegrees
        {
            get
            {
                return this.inDegrees;
            }
        }

        public event VertexEventHandler AddVertex;

        protected virtual void OnAddVertex(IVertex v)
        {
            if (this.AddVertex != null)
                this.AddVertex(this, new VertexEventArgs(v));
        }


        public void Compute()
        {
            this.InitializeInDegrees();

            while (this.heap.Count != 0)
            {
                IVertex v = this.heap.Pop();
                if (this.inDegrees[v] != 0)
                    throw new QuickGraph.Exceptions.NonAcyclicGraphException();

                this.sortedVertices.Add(v);
                this.OnAddVertex(v);

                // update the count of it's adjacent vertices
                foreach (IEdge e in this.VisitedGraph.OutEdges(v))
                {
                    if (e.Source == e.Target)
                        continue;

                    this.inDegrees[e.Target]--;
                    if (this.inDegrees[e.Target] < 0)
                        throw new InvalidOperationException("InDegree is negative, and cannot be");
                    this.heap.Update(e.Target);
                }
            }
        }

        protected virtual void InitializeInDegrees()
        {
            foreach (IVertex v in this.VisitedGraph.Vertices)
            {
                this.inDegrees.Add(v,0);
                this.heap.Push(v);
            }

            foreach (IEdge e in this.VisitedGraph.Edges)
            {
                if (e.Source == e.Target)
                    continue;
                this.inDegrees[e.Target]++;
            }

            this.heap.Sort();
        }
    }
}
