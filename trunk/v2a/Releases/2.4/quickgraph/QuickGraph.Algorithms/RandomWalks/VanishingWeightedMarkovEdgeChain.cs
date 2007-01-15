using System;

namespace QuickGraph.Algorithms.RandomWalks
{
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;

	/// <summary>
	/// Summary description for VanishingWeightedMarkovEdgeChain.
	/// </summary>
	public class VanishingWeightedMarkovEdgeChain : WeightedMarkovEdgeChain
	{
		private double factor = 0.5;

		public VanishingWeightedMarkovEdgeChain(EdgeDoubleDictionary weights, double factor)
			:base(weights)
		{
			this.factor = factor;
		}

		public double Factor
		{
			get
			{
				return this.factor;
			}
		}

        public override IEdge Successor(IImplicitGraph g, IVertex u)
        {
			// get succesor
			IEdge s = base.Successor(g,u);
			// update probabilities
			this.Weights[s]*=this.Factor;

			return s;
		}
	}
}
