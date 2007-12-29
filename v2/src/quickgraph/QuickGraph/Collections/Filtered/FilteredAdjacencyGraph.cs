using System;

namespace QuickGraph.Collections.Filtered
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Predicates;

	/// <summary>
	/// A filtered adjacency graph
	/// </summary>
	public class FilteredAdjacencyGraph : IAdjacencyGraph
	{
		private IVertexPredicate m_VertexPredicate;
		private IAdjacencyGraph m_AdjacencyGraph;

		/// <summary>
		/// Create an adjacency filtered graph
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="vp">vertex predicate</param>
		public FilteredAdjacencyGraph(
			IAdjacencyGraph g,
			IVertexPredicate vp
			)
		{
			if (g==null)
				throw new ArgumentNullException("g is null");
			m_AdjacencyGraph = g;
			VertexPredicate = vp;
		}

		/// <summary>
		/// Filtered adjacency graph
		/// </summary>
		public IAdjacencyGraph AdjacencyGraph
		{
			get
			{
				return m_AdjacencyGraph;
			}
		}

		/// <summary>
		/// Vertex predicate used to filter the vertices
		/// </summary>
		public IVertexPredicate VertexPredicate 
		{
			get
			{
				return m_VertexPredicate;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("vertex predicate");
				m_VertexPredicate = value;
			}
		}

		/// <summary>
		/// Returns a filtered enumerable collection of adjacent vertices
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public FilteredVertexEnumerable AdjacentVertices(IVertex v)
		{
			return new FilteredVertexEnumerable(
				AdjacencyGraph.AdjacentVertices(v),
				VertexPredicate
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		IVertexEnumerable IAdjacencyGraph.AdjacentVertices(IVertex v)
		{
			return this.AdjacentVertices(v);
		}

	}
}
