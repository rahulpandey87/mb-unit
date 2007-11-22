using System;

namespace QuickGraph.Algorithms.TestGames
{
	public interface IPerformanceComparer
	{
		bool Compare(
			double leftProb, double leftCost,
			double rightProb, double rightCost
			);
	}
}
