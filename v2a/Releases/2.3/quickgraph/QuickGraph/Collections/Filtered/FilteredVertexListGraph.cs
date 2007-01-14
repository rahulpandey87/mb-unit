using System;
using System.Collections;

namespace QuickGraph.Collections.Filtered
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Predicates;
	using QuickGraph.Collections;

	/// <summary>
	/// Summary description for FilteredVertexList.
	/// </summary>
	public class FilteredVertexListGraph : 
		FilteredIncidenceGraph,
		IVertexListGraph
	{
		/// <summary>
		/// Construct a graph that filters in-edges
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="vertexPredicate">vertex predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g or vertexPredicate is null
		/// </exception>
		public FilteredVertexListGraph(
			IVertexListGraph g, 
			IVertexPredicate vertexPredicate
			)
			: base(g,new KeepAllEdgesPredicate(), vertexPredicate)
		{}

		/// <summary>
		/// Construct a graph that filters in-edges
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g or edgePredicate is null
		/// </exception>
		public FilteredVertexListGraph(
			IVertexListGraph g, 
			IEdgePredicate edgePredicate
			)
			: base(g,edgePredicate)
		{}

		/// <summary>
		/// Construct a filtered graph with an edge and a vertex predicate.
		/// </summary>
		/// <param name="g">graph to filter</param>
		/// <param name="edgePredicate">edge predicate</param>
		/// <param name="vertexPredicate">vertex predicate</param>
		/// <exception cref="ArgumentNullException">
		/// g, edgePredicate or vertexPredicate are null
		/// </exception>
		public FilteredVertexListGraph(
			IVertexListGraph g,
			IEdgePredicate edgePredicate, 
			IVertexPredicate vertexPredicate)
			: base(g,edgePredicate,vertexPredicate)
		{}

		/// <summary>
		/// Underlying incidence graph
		/// </summary>
		public IVertexListGraph VertexListGraph
		{
			get
			{
				return (IVertexListGraph)Graph;
			}
		}

		/// <summary>
		/// Gets a value indicating if the vertex set is empty
		/// </summary>
		/// <para>
		/// Usually faster (O(1)) that calling <c>VertexCount</c>.
		/// </para>
		/// <value>
		/// true if the vertex set is empty, false otherwise.
		/// </value>
		public bool VerticesEmpty 
		{
			get
			{
				if (this.VertexListGraph.VerticesEmpty)
					return true;
				return this.VerticesCount==0;
			}
		}

		/// <summary>
		/// Gets the filtered vertices count
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method is <c>O(|V|)</c>.
		/// </para>
		/// </remarks>
		public int VerticesCount
		{
			get
			{
				IEnumerator e = Vertices.GetEnumerator();
				int n=0;
				while (e.MoveNext())
					++n;
				return n;
			}
		}

		/// <summary>
		/// Filtered enumerable collection of vertices
		/// </summary>
		public FilteredVertexEnumerable Vertices
		{
			get
			{
				return new FilteredVertexEnumerable(
					VertexListGraph.Vertices,
					VertexPredicate
					);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		IVertexEnumerable IVertexListGraph.Vertices
		{
			get
			{
				return this.Vertices;
			}
		}

		/// <summary>
		/// Gets a value indicating if the vertex <paramref name="v"/> is part
		/// of the list.
		/// </summary>
		/// <param name="u">vertex to test</param>
		/// <returns>true if part of the list, false otherwize</returns>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		/// <remarks>
		/// This method checks wheter a particular vertex is part of the set.
		/// <para>
		/// Complexity: O(V)
		/// </para>
		/// </remarks>
		public bool ContainsVertex(IVertex u)
		{
			if (u==null)
				throw new ArgumentNullException("u");
			foreach(IVertex v in Vertices)
				if (u==v)
					return true;
			return false;
		}
		
	}
}
