using System;

namespace QuickGraph.Concepts.Modifications
{
	public interface IMutableGraph  : IGraph
	{
		/// <summary>
		/// Clears the graph.
		/// </summary>
		void Clear();
	}
}
