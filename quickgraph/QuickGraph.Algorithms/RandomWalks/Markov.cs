using System;

namespace QuickGraph.Algorithms.RandomWalks
{
	/// <summary>
	/// Summary description for Markov.
	/// </summary>
	public sealed class Markov
	{
		public static int UniformNextEntry(int count, Random rnd)
		{
			double r = rnd.NextDouble();
			int nr = (int)Math.Floor(count * r);
			if (nr==count)
				--nr;
			return nr;
		}
	}
}
