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
	public class VertexCoverageMetric
	{
		private IVertexListGraph graph;
		private VertexIntDictionary passCounts = new VertexIntDictionary();
		private double coverage = 0;

		public VertexCoverageMetric(IVertexListGraph graph)
		{
			if (graph==null)
				throw new ArgumentNullException("graph");
			this.graph = graph;
		}

		public IVertexListGraph Graph
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

		public VertexIntDictionary PassCounts
		{
			get
			{
				return this.PassCounts;
			}
		}

		public void Measure(IVertexEnumerable vertices)
		{
			if (vertices == null)
				throw new ArgumentNullException("vertices");
			Clear();

			// count the number of passages
			int count=0;
			foreach(IVertex v in vertices)
			{
				if (this.passCounts[v]==0)
					++count;
				this.passCounts[v]++;
			}

			// compute covergage
			if (this.graph.VerticesEmpty)
				this.coverage =0;
			else
				this.coverage = count / (double)this.graph.VerticesCount;
		}

		public void MeasurePath(IEdgeEnumerable edges)
		{
			Clear();

			// count the number of passages
			int count=0;
			bool isFirst = true;
			foreach(IEdge e in edges)
			{
				if (isFirst)
				{
					this.passCounts[e.Source]++;
					isFirst = false;
				}
				if (this.passCounts[e.Target]==0)
					++count;
				this.passCounts[e.Target]++;
			}

			// compute covergage
			if (this.graph.VerticesEmpty)
				this.coverage =0;
			else
				this.coverage = count / (double)this.graph.VerticesCount;
		}

		public void Clear()
		{
			this.passCounts.Clear();
			this.coverage = 0;

			// initialize
			foreach(IVertex v in this.graph.Vertices)
			{
				this.passCounts[v]=0;				
			}
		}
	}
}
