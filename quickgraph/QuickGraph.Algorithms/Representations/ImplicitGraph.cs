using System;
using System.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Collections;

namespace QuickGraph.Representations
{
    public abstract class ImplicitGraph : IImplicitGraph
    {
		private bool allowParallelEdges=true;

        public abstract TransitionDelegateCollection Transitions(IVertex v);

		public virtual bool IsDirected
		{
			get
			{
				return true;
			}
		}

		public virtual bool AllowParallelEdges
		{
			get
			{
				return this.allowParallelEdges;
			}
		}

        public bool ContainsEdgeEdges(IVertex v)
        {
            if (v == null)
                throw new ArgumentNullException("v");
            return this.Transitions(v).Count > 0;
        }

		public bool OutEdgesEmpty(IVertex v)
		{
			return this.OutDegree(v)==0;
		}

        public int OutDegree(IVertex v)
        {
            if (v == null)
                throw new ArgumentNullException("v");
            return this.Transitions(v).Count;
        }

        public IEdgeEnumerable OutEdges(IVertex v)
        {
            if (v == null)
                throw new ArgumentNullException("v");
            return new ImplicitEdgeEnumerable(this,v);
        }

        private class ImplicitEdgeEnumerable : IEdgeEnumerable
        {
            private ImplicitGraph graph;
            private IVertex vertex;
            public ImplicitEdgeEnumerable(ImplicitGraph graph, IVertex vertex)
            {
                this.graph = graph;
                this.vertex = vertex;
            }

            public IEdgeEnumerator GetEnumerator()
            {
                return new ImplicitEdgeEnumerator(graph,vertex);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        private class ImplicitEdgeEnumerator : IEdgeEnumerator
        {
            private IVertex vertex;
            private IEnumerator transitions = null;
            private IEdge current = null;

            public ImplicitEdgeEnumerator(ImplicitGraph graph, IVertex vertex)
            {
                this.vertex = vertex;
                this.transitions = graph.Transitions(vertex).GetEnumerator();
            }

            public void Reset()
            {
                this.transitions.Reset();
                this.current = null;
            }

            public bool MoveNext()
            {
                if (!this.transitions.MoveNext())
                {
                    this.current = null;
                    return false;
                }

                TransitionDelegate transition = (TransitionDelegate)this.transitions.Current;
                this.current = (IEdge)transition.DynamicInvoke( new Object[] { this.vertex });
                return true;
            }

            public IEdge Current
            {
                get
                {
                    return this.current;
                }
            }

            Object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
        }
     }
}