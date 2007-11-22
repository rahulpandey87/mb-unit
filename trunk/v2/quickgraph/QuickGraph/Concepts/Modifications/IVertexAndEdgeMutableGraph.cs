using System;

namespace QuickGraph.Concepts.Modifications
{
	/// <summary>
	/// Fusion of <see cref="IEdgeMutableGraph"/>
	/// and <see cref="IVertexMutableGraph"/>.
	/// </summary>
	public interface IVertexAndEdgeMutableGraph :
		IEdgeMutableGraph,
		IVertexMutableGraph
	{
	}
}
