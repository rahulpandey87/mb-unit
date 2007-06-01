using System;

namespace QuickGraph.Concepts.Traversals
{
	/// <summary>
	/// Union of <see cref="IFilteredVertexListGraph"/>,
	/// <see cref="IFilteredEdgeListGraph"/> and <see cref="IVertexAndEdgeListGraph"/>.
	/// </summary>
	public interface IFilteredVertexAndEdgeListGraph :
		IFilteredVertexListGraph,
		IFilteredEdgeListGraph,
		IVertexAndEdgeListGraph
	{}
}
