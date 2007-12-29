using System;

namespace QuickGraph.Algorithms.TestGames
{
	using QuickGraph.Concepts;
	using QuickGraph.Collections;

	/// <summary>
	/// Summary description for OptimalStrategy.
	/// </summary>
	public class OptimalStrategy : IStrategy
	{
		private VertexEdgeDictionary successors =new VertexEdgeDictionary();

		public OptimalStrategy()
		{}

		public OptimalStrategy(VertexEdgeDictionary successors)
		{
			if (successors==null)
				throw new ArgumentNullException("successors");
			this.successors = successors;
		}

		public VertexEdgeDictionary Successors
		{
			get
			{
				return this.successors;
			}
		}

		#region IStrategy Members

		public void SetChooseEdge(IVertex v, int k, IEdge e)
		{
			this.successors[v]=e;
		}

		public IEdge ChooseEdge(IVertex state, int i)
		{
			return this.successors[state];
		}

		#endregion
	}
}
