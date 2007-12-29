using System;

namespace QuickGraph.Collections.Filtered
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Predicates;

	/// <summary>
	/// Base class for filtered graphs
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class FilteredGraph : IGraph
	{
		private IGraph m_Graph;
		private IEdgePredicate m_EdgePredicate;
		private IVertexPredicate m_VertexPredicate;

		/// <summary>
		/// Construct a graph that filters edges
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g or edgePredicate is null
		/// </exception>
		public FilteredGraph(IGraph g, IEdgePredicate edgePredicate)
		{
			Graph = g;
			EdgePredicate = edgePredicate;
			VertexPredicate = new KeepAllVerticesPredicate();
		}

		/// <summary>
		/// Construct a filtered graph with an edge and a vertex predicate.
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <param name="vertexPredicate">vertex predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g, edgePredicate or vertexPredicate are null
		/// </exception>
		public FilteredGraph(
			IGraph g,
			IEdgePredicate edgePredicate, 
			IVertexPredicate vertexPredicate)
		{
			Graph = g;
			EdgePredicate = edgePredicate;
			VertexPredicate = vertexPredicate;
		}

		/// <summary>
		/// Underlying filtered graph
		/// </summary>
		public IGraph Graph
		{
			get
			{
				return m_Graph;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("underlying graph");
				m_Graph = value;
			}
		}

		/// <summary>
		/// Edge predicate used to filter the edges
		/// </summary>
		public IEdgePredicate EdgePredicate 
		{
			get
			{
				return m_EdgePredicate;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("edge predicate");
				m_EdgePredicate = value;
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
		/// True if underlying graph in directed
		/// </summary>
		public bool IsDirected
		{
			get
			{
				return Graph.IsDirected;
			}
		}

		/// <summary>
		/// True if underlying graph allows parallel edges
		/// </summary>
		public bool AllowParallelEdges
		{
			get
			{
				return Graph.AllowParallelEdges;
			}
		}
	}
}
