using System;

namespace QuickGraph.Algorithms.RandomWalks
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;

	/// <summary>
	/// Markov <see cref="IEdge"/> chain generator with the propability vector
	/// distributed over the out-edges weights. 
	/// </summary>
	/// <remarks>
	/// <value>
	/// This class can be used to generate a Markov Chain of <see cref="IEdge"/>
	/// instance. The probability vector is computed such that each
	/// out-edge weight sum is 1 (to have a stochastic vector):
	/// <code>
	/// outWeight = \sum weight[e_i]
	/// Pr[e_i] = weight[e_i]/outWeight
	/// </code>
	/// </value>
	/// </remarks>
	public class WeightedMarkovEdgeChain : IMarkovEdgeChain
	{
		private Random rnd = new Random((int)DateTime.Now.Ticks);
		private EdgeDoubleDictionary weights = null;

		/// <summary>
		/// Construct a markov <see cref="IEdge"/> chain based on the
		/// <see cref="EdgeDoubleDictionary"/> edge weight dictionary.
		/// </summary>
		/// <param name="weights">edge weight dictionary</param>
		/// <exception cref="ArgumentNullException">weights is a null reference
		/// </exception>
		public WeightedMarkovEdgeChain(EdgeDoubleDictionary weights)
		{
			if (weights == null)
				throw new ArgumentNullException("weights");
			this.weights=weights;
		}

		/// <summary>
		/// Gets or sets the random generator
		/// </summary>
		/// <value>
		/// Random number generator
		/// </value>
		public Random Rnd
		{
			get
			{
				return this.rnd;
			}
			set
			{
				this.rnd = value;
			}
		}

		/// <summary>
		/// Gets the edge-weight dictionary
		/// </summary>
		/// <value>
		/// Edge weight dictionary
		/// </value>
		public EdgeDoubleDictionary Weights
		{
			get
			{
				return this.weights;
			}
		}

		/// <summary>
		/// Selects the next out-<see cref="IEdge"/> in the Markov Chain.
		/// </summary>
		/// <param name="g">visted graph</param>
		/// <param name="u">source vertex</param>
		/// <returns>Random next out-edge</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="g"/> or <paramref name="u"/> is a null reference
		/// </exception>
        public virtual IEdge Successor(IImplicitGraph g, IVertex u)
        {
			if (g==null)
				throw new ArgumentNullException("g");
			if (u==null)
				throw new ArgumentNullException("u");

			// get number of out-edges
			int n = g.OutDegree(u);
			// compute out-edge su
			double outWeight = 0;
			foreach(IEdge e in g.OutEdges(u))
			{
				outWeight+=this.weights[e];
			}

			// scale and get next edge
			double r = rnd.NextDouble() * outWeight;

			double pos = 0;
			double nextPos = 0;
			foreach(IEdge e in g.OutEdges(u))
			{
				nextPos = pos+this.weights[e];
				if (r>=pos && r<=nextPos)
					return e;
				pos = nextPos;
			}
			return null;
		}
	}
}
