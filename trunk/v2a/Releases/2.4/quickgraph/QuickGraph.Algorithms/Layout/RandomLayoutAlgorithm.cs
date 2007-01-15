using System;
using System.Drawing;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Concepts.Traversals;

	public class RandomLayoutAlgorithm : LayoutAlgorithm
	{
		private Random rnd = new Random((int)DateTime.Now.Ticks);
		
		public RandomLayoutAlgorithm(
			IVertexAndEdgeListGraph visitedGraph)
			:base(visitedGraph)
		{}
		public RandomLayoutAlgorithm(
			IVertexAndEdgeListGraph visitedGraph,
			IVertexPointFDictionary positions)
			:base(visitedGraph,positions)
		{}
		
		public override void Compute()
		{
			this.OnPreCompute();
			foreach(IVertex v in this.VisitedGraph.Vertices)
			{
				this.Positions[v]=new PointF(nextX(), nextY());
			}
			this.OnPostCompute();
		}
		
		private float nextX()
		{
			return (float)(rnd.NextDouble() * this.LayoutSize.Width);
		}
		
		private float nextY()
		{
			return (float)(rnd.NextDouble() * this.LayoutSize.Height);
		}
	}
}
