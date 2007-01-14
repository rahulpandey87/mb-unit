using System;

namespace QuickGraph.Algorithms.TestGames
{
	using QuickGraph.Concepts;
	/// <summary>
	/// Summary description for Strategy.
	/// </summary>
	public interface IBoundedReachabilityGamePlayer
	{
		IEdge ChooseEdge(IVertex state, int i);
	}
}
