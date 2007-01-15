using System;

namespace QuickGraph.Algorithms.Metrics
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;

	/// <summary>
	/// Summary description for VertexCoverageMetric.
	/// </summary>
	public class EdgeCoverageMetric
	{
		private IEdgeListGraph graph;
		private EdgeIntDictionary passCounts = new EdgeIntDictionary();
		private double coverage = 0;

		public EdgeCoverageMetric(IEdgeListGraph graph)
		{
			if (graph==null)
				throw new ArgumentNullException("graph");
			this.graph = graph;
		}

		public IEdgeListGraph Graph
		{
			get
			{
				return this.graph;
			}
		}

		public double Coverage
		{
			get
			{
				return this.coverage;
			}
		}

		public EdgeIntDictionary PassCounts
		{
			get
			{
				return this.PassCounts;
			}
		}

		public void Measure(IEdgeEnumerable edges)
		{
			Clear();

			// count the number of passages
			int count=0;
			foreach(IEdge e in edges)
			{
				if (this.passCounts[e]==0)
					++count;
				this.passCounts[e]++;
			}

			// compute covergage
			if (this.graph.EdgesEmpty)
				this.coverage =0;
			else
				this.coverage = count / (double)this.graph.EdgesCount;
		}

		public void Clear()
		{
			this.passCounts.Clear();
			this.coverage = 0;

			// initialize
			foreach(IEdge e in this.graph.Edges)
			{
				this.passCounts[e]=0;				
			}
		}
	}
}
