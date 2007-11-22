using System;

namespace QuickGraph.Concepts.Modifications
{
	using QuickGraph.Exceptions;

	/// <summary>
	/// A mutable tree-like graph
	/// </summary>
	public interface IMutableTreeGraph : IMutableGraph
	{
		/// <summary>
		/// Gets a value indicating if the tree allows cycles
		/// </summary>
		/// <value>
		/// true if it allows cycle, false otherwise
		/// </value>
		bool AllowCycles{get;}

		/// <summary>
		/// Adds a child vertex to the tree
		/// </summary>
		/// <param name="parent">parent vertex</param>
		/// <returns>created vertex</returns>
		/// <exception cref="ArgumentNullException">parent is a null reference</exception>
		/// <exception cref="NonAcyclicGraphException">
		/// if <c>AllowCycles</c> is false and the edge creates a cycle
		/// </exception>
		IVertex AddChild(IVertex parent);

		/// <summary>
		/// Removes vertex and sub-tree
		/// </summary>
		/// <param name="root">vertex to remove</param>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		/// <exception cref="GraphNotStronglyConnectedException">
		/// Removing the vertex breaks the graph connectivity
		/// </exception>
		void RemoveTree(IVertex root);
	}
}
