using System;

namespace QuickGraph.Algorithms.AllShortestPath.Testers
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	public interface IFloydWarshallDistanceTester
	{
		bool TestDistance(
			IVertexDistanceMatrix distances, 
			IVertex source, 
			IVertex target, 
			IVertex intermediate
			);	
	}
}
