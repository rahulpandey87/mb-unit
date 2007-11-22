using System;

namespace QuickGraph.Concepts.Serialization
{
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Modifications;

	/// <summary>
	/// Union of the <see cref="IVertexListGraph"/> 
	/// , <see cref="IVertexMutableGraph"/> and
	/// <see cref="IEdgeMutableGraph"/>
	/// interfaces.
	/// </summary>
	public interface ISerializableVertexAndEdgeListGraph :
		ISerializableEdgeListGraph,
		ISerializableVertexListGraph,
		IVertexAndEdgeListGraph
	{}
}
