using System;

namespace QuickGraph.Algorithms.AllShortestPath.Reducers
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	public class FloydWarshallNegativeCycleDistanceReducer : IFloydWarshallDistanceReducer
	{

		public void ReducePathDistance(
			IVertexDistanceMatrix distances,
			IVertex source,
			IVertex target,
			IVertex intermediate
			)		
		{
			if (source==target && distances.Distance(source,target) < 0 )
				throw new Exception(@"Negative cycle");
		}
	}
}
