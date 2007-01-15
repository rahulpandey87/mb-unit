using System;

namespace QuickGraph.Concepts.Serialization
{
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Traversals;

	/// <summary>
	/// Summary description for ISerializableEdgeListGraph.
	/// </summary>
	public interface ISerializableEdgeListGraph :
		IEdgeMutableGraph,
		IEdgeListGraph
	{
		/// <summary>
		/// Adds an edge to the graph
		/// </summary>
		/// <param name="e">edge to add</param>
		void AddEdge(IEdge e);	
	}
}
