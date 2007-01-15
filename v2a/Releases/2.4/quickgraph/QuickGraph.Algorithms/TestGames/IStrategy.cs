using System;

namespace QuickGraph.Algorithms.TestGames
{
	using QuickGraph.Concepts;

	/// <summary>
	/// A <b>Strategy</b> as defined in section 3 of the article.
	/// </summary>
	public interface IStrategy : IBoundedReachabilityGamePlayer
	{
		void SetChooseEdge(IVertex v, int k, IEdge e);
	}
}
