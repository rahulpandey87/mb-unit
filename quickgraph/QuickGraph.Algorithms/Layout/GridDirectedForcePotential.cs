using System;

namespace QuickGraph.Algorithms.Layout
{
	/// <summary>
	/// Summary description for GridDirectedForcePotential.
	/// </summary>
	public class GridDirectedForcePotential : DirectedForcePotential
	{
		public GridDirectedForcePotential(IIteratedLayoutAlgorithm algorithm)
			:base(algorithm)
		{}

		protected override double RepulsionForce(double distance)
		{
			if (distance < 2*this.Algorithm.EdgeLength)
				return base.RepulsionForce(distance);
			else
				return 0;
		}

	}
}
