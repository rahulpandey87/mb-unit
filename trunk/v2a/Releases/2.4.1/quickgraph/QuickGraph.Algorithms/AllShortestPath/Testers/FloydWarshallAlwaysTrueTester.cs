using System;

namespace QuickGraph.Algorithms.AllShortestPath.Testers
{
	using QuickGraph.Concepts;

	public class FloydWarshallAlwaysTrueTester : IFloydWarshallTester
	{
		public bool Test(IVertex source, IVertex target, IVertex intermediate)
		{
			return true;
		}
	}
}
