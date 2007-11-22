using System;

namespace QuickGraph.Algorithms.TestGames
{
	using QuickGraph.Concepts;
	using QuickGraph.Collections;

	/// <summary>
	/// Summary description for Strategy.
	/// </summary>
	public class Strategy : IStrategy
	{
		private VertexEdgesDictionary successors = new VertexEdgesDictionary();

		#region IStrategy Members

		public void SetChooseEdge(QuickGraph.Concepts.IVertex v, int k, QuickGraph.Concepts.IEdge e)
		{
			if (k==0)
			{
				this.successors.Add(v,new EdgeCollection());
			}

			EdgeCollection col = this.successors[v];
			if (col.Count<=k)
				col.Add(e);
			else
				col[k]=e;
		}

		#endregion

		#region IBoundedReachabilityGamePlayer Members

		public QuickGraph.Concepts.IEdge ChooseEdge(QuickGraph.Concepts.IVertex state, int i)
		{
			return this.successors[state][i];
		}

		#endregion
	}
}
