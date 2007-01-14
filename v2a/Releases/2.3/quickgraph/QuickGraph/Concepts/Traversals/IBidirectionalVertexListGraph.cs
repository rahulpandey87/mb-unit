using System;

namespace QuickGraph.Concepts.Traversals
{
	/// <summary>
	/// A fusion of <see cref="IBidirectionalGraph"/> and
	/// <see cref="IVertexListGraph"/>.
	/// </summary>
	public interface IBidirectionalVertexListGraph :
		IVertexListGraph,
		IBidirectionalGraph
	{
	}
}
