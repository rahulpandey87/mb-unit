using System;

namespace QuickGraph.Algorithms.AllShortestPath.Testers
{
	using QuickGraph.Concepts;

	public interface IFloydWarshallTester
	{
		bool Test(IVertex source, IVertex target, IVertex intermediate);	
	}
}
