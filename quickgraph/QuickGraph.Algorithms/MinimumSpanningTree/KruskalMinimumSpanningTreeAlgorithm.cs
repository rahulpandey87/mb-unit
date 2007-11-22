namespace QuickGraph.Algorithms.MinimumSpanningTree 
{
	using System;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Exceptions;

	public class KruskalMinimumSpanningTreeAlgorithm
	{
		private IVertexAndEdgeListGraph visitedGraph;
		private EdgeDoubleDictionary weights;
		private EdgeCollection spanningTreeEdges;
	
		public KruskalMinimumSpanningTreeAlgorithm(
			IVertexAndEdgeListGraph visitedGraph,
			EdgeDoubleDictionary weights
			)
		{
			if (visitedGraph==null)
				throw new ArgumentNullException("visitedGraph");
			if (weights==null)
				throw new ArgumentNullException("weights");
		
			this.visitedGraph = visitedGraph;
			this.weights=weights;
			this.spanningTreeEdges = new EdgeCollection();
		}
	
		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return visitedGraph;
			}
		}
	
		public EdgeDoubleDictionary Weights
		{
			get
			{
				return weights;
			}
		}
	
		public EdgeCollection SpannedTreeEdges
		{
			get
			{
				return spanningTreeEdges;
			}
		}
	
		public void Compute()
		{
			// clear previous results
			SpanningTreeEdges.Clear();

			// create disjoint set object
			// fill in disjoint sets		
			DisjointSet dset = new DisjointSet(VisitedGraph.Vertices);

			// create and fill in priority queue		
			PriorityQueue Q = new PriorityQueue(Weights, VisitedGraph.Edges);
		
			// iterate over edges
			while(Q.Count!=0)
			{
				IEdge e = Q.Peek();
				Q.Pop();
				IVertex u = dset.FindSet(e.Source);
				IVertex v = dset.FindSet(e.Target);
				if ( u != v ) 
				{
					SpanningTreeEdges.Add(e);
					dset.Link(u, v);
				}
			}
		}
	}
}
