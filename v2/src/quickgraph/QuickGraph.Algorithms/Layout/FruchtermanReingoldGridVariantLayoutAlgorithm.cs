using System;
using System.Drawing;
using QuickGraph.Concepts.Traversals;

namespace QuickGraph.Algorithms.Layout
{
	/// <summary>
	/// The grid variant of the Fruchterman-Reingold graph layout algorithm.
	/// </summary>
	public class FruchtermanReingoldGridVariantLayoutAlgorithm : FruchtermanReingoldLayoutAlgorithm
	{
		public FruchtermanReingoldGridVariantLayoutAlgorithm(IVertexAndEdgeListGraph graph, SizeF size) 
			: base(graph, size)
		{}
		protected override double RepulsiveForce(double distance)
		{
			if (distance < 2*k)
				return base.RepulsiveForce (distance);
			else
				return 0;
		}


	}
}
