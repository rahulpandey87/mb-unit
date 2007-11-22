using System;

namespace QuickGraph.Concepts.Collections
{
	/// <summary>
	/// 
	/// </summary>
	public interface IVertexDistanceMatrix
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		double Distance(IVertex source, IVertex target);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="distance"></param>
		void SetDistance(IVertex source, IVertex target, double distance);
	}
}
