using System;

namespace QuickGraph.Concepts.MutableTraversals
{
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Traversals;

	/// <summary>
	/// Union of <see cref="IVertexAndEdgeListGraph"/>,
	/// <see cref="IMutableVertexAndEdgeListGraph"/>
	/// </summary>
	public interface IMutableVertexAndEdgeListGraph :
		IVertexAndEdgeListGraph,
		IVertexAndEdgeMutableGraph
	{}
}
