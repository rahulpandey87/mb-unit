using System;

namespace QuickGraph.Algorithms.Layout
{
	public interface IIteratedLayoutAlgorithm : ILayoutAlgorithm
	{
		event EventHandler PreIteration;
		event EventHandler PostIteration;
		
		int CurrentIteration {get;}
		void Iterate();
		Object SyncRoot {get;}
	}
}
