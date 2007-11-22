using System;

namespace QuickGraph.Algorithms.TestGames
{
	/// <summary>
	/// Summary description for PairPreOrderPerformanceComparer.
	/// </summary>
	public class PairPreOrderPerformanceComparer : IPerformanceComparer
	{
		#region IPerformanceComparer Members

		public bool Compare(double leftProb, double leftCost, double rightProb, double rightCost)
		{
			// 0,c greatest
			if (rightProb==0)
				return true;

			if (leftProb > rightProb)
				return true;
			
			if (Math.Abs(leftProb - rightProb) < double.Epsilon 
				&& leftCost < rightCost)
				return true;
			return false;
		}

		#endregion
	}
}
