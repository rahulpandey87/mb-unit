using System;

namespace QuickGraph.Algorithms.AllShortestPath.Reducers
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Distance reducer interface
	/// </summary>
	public interface IFloydWarshallDistanceReducer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="distances"></param>
		/// <param name="source"></param>
		/// <param name="target"></param>
		/// <param name="intermediate"></param>
		void ReducePathDistance(
			IVertexDistanceMatrix distances,
			IVertex source,
			IVertex target,
			IVertex intermediate
			);
	}

}
