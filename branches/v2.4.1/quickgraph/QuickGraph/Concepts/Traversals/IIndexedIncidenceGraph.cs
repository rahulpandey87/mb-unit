using System;

namespace QuickGraph.Concepts.Traversals
{
	using QuickGraph.Concepts.Collections;

	public interface IIndexedIncidenceGraph : IIncidenceGraph
	{
		new IEdgeCollection OutEdges(IVertex u);
	}
}
