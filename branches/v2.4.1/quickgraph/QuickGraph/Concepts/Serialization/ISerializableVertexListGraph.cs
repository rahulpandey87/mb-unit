using System;

namespace QuickGraph.Concepts.Serialization
{
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Traversals;

	public interface ISerializableVertexListGraph :
		IVertexMutableGraph,
		IVertexListGraph
	{
		/// <summary>
		/// Add a vertex to the graph
		/// </summary>
		/// <param name="v">vertex to add</param>
		void AddVertex(IVertex v);
	}
}
