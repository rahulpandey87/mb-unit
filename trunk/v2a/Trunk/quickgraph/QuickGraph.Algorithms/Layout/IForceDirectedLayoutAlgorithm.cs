using System;

namespace QuickGraph.Algorithms.Layout
{
	public interface IForceDirectedLayoutAlgorithm : IIteratedLayoutAlgorithm
	{
		ILayoutAlgorithm PreLayoutAlgorithm {get;set;}
		IPotential Potential {get;set;}		
	}
}
