using System;

namespace QuickGraph.Algorithms.AllShortestPath.Reducers
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	public class FloydWarshallTransitiveHullDistanceReducer : IFloydWarshallDistanceReducer
	{
		public void ReducePathDistance(
			IVertexDistanceMatrix distances,
			IVertex source,
			IVertex target,
			IVertex intermediate
			)		
		{
			if (distances.Distance(source,target)==0)
			{
				distances.SetDistance(source,target, 
					Convert.ToDouble(
						(distances.Distance(source,intermediate)!=0) &&
						(distances.Distance(intermediate,target)!=0)
					)
					);
			}
		}
	}
}
