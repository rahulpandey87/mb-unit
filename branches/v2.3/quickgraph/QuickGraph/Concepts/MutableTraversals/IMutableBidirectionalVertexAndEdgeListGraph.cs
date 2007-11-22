using System;

namespace QuickGraph.Concepts.MutableTraversals
{
	using QuickGraph.Concepts.Modifications;

	/// <summary>
	/// A fusion of <see cref="IBidirectionalGraph"/>,
	/// 
	/// </summary>
	public interface IMutableBidirectionalVertexAndEdgeListGraph : 
		IMutableBidirectionalGraph,
		IMutableVertexAndEdgeListGraph
	{
	}
}
