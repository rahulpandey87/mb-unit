using System;

namespace QuickGraph.Representations
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Predicates;

	using QuickGraph.Collections;
	using QuickGraph.Predicates;
	using QuickGraph.Collections.Filtered;

	/// <summary>
	/// Creates a bidirectional graph out of a 
	/// <see cref="IVertexAndEdgeListGraph"/> graph.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class adapts a <see cref="IVertexAndEdgeListGraph"/> to support
	/// in-edge traversal. Be aware, that the in-edge traversal is less 
	/// efficient that using specialized classes.
	/// </para>
	/// </remarks>
	public class BidirectionalAdaptorGraph :
		IBidirectionalVertexAndEdgeListGraph
	{
		private IVertexAndEdgeListGraph graph;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="g"></param>
		public BidirectionalAdaptorGraph(IVertexAndEdgeListGraph g)
		{
			if (g==null)
				throw new  ArgumentNullException("grah");
			this.graph = g;
		}

		/// <summary>
		/// Adapted graph
		/// </summary>
		public IVertexAndEdgeListGraph Graph
		{
			get
			{
				return graph;
			}
		}

		/// <summary>
		/// Directed state
		/// </summary>
		public bool IsDirected
		{
			get
			{
				return Graph.IsDirected;
			}
		}

		/// <summary>
		/// True if parallel edges allowed
		/// </summary>
		public bool AllowParallelEdges
		{
			get
			{
				return Graph.AllowParallelEdges;
			}
		}

		/// <summary>
		/// Gets a value indicating if the set of out-edges is empty
		/// </summary>
		/// <remarks>
		/// <para>
		/// Usually faster that calling <see cref="OutDegree"/>.
		/// </para>
		/// </remarks>
		/// <value>
		/// true if the out-edge set is empty, false otherwise.
		/// </value>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		public bool OutEdgesEmpty(IVertex v)
		{
			return Graph.OutEdgesEmpty(v);
		}

		/// <summary>
		/// Returns the number of out-degree edges of v
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>out-degree</returns>
		public int OutDegree(IVertex v)
		{
			return Graph.OutDegree(v);
		}

		/// <summary>
		/// Returns an iterable collection of the out edges of v
		/// </summary>
		public IEdgeEnumerable OutEdges(IVertex v)
		{
			return Graph.OutEdges(v);
		}

		/// <summary>
		/// Gets a value indicating if the set of in-edges is empty
		/// </summary>
		/// <remarks>
		/// <para>
		/// Usually faster that calling <see cref="InDegree"/>.
		/// </para>
		/// </remarks>
		/// <value>
		/// true if the in-edge set is empty, false otherwise.
		/// </value>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		public bool InEdgesEmpty(IVertex v)
		{
			return this.InDegree(v)==0;
		}

		/// <summary>
		/// Returns the number of in-edges (for directed graphs) or the number 
		/// of incident edges (for undirected graphs) of vertex v in graph g.
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>out-degree</returns>
		public int InDegree(IVertex v)
		{
			IEdgeEnumerator en = InEdges(v).GetEnumerator();
			int n=0;
			while(en.MoveNext())
				++n;
			return n;
		}



		/// <summary>
		/// Gets a value indicating if the set of edges connected to v is empty
		/// </summary>
		/// <remarks>
		/// <para>
		/// Usually faster that calling <see cref="Degree"/>.
		/// </para>
		/// </remarks>
		/// <value>
		/// true if the adjacent edge set is empty, false otherwise.
		/// </value>
		/// <exception cref="ArgumentNullException">v is a null reference</exception>
		public bool AdjacentEdgesEmpty(IVertex v)
		{
			return this.OutEdgesEmpty(v) && this.InEdgesEmpty(v);
		}

		/// <summary>
		/// Returns the number of in-edges plus out-edges (for directed graphs) 
		/// or the number of incident edges (for undirected graphs) of 
		/// vertex v in graph g.
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>out-degree</returns>
		public int Degree(IVertex v)
		{
			return OutDegree(v) + InDegree(v);
		}

		/// <summary>
		/// Enumerable collection of in-edges
		/// </summary>
		/// <remarks>
		/// <para>
		/// Returns an enumerable collection of in-edges (for directed graphs) 
		/// or incident edges (for undirected graphs) of vertex v in graph g. 
		/// </para>
		/// <para>
		/// For both directed and undirected graphs, the target of an out-edge 
		/// is required to be vertex v and the source is required to be a 
		/// vertex that is adjacent to v. 
		/// </para>
		/// </remarks>
		public IEdgeEnumerable InEdges(IVertex v)
		{
			IEdgePredicate ep = new IsInEdgePredicate(v);
			return new FilteredEdgeEnumerable(Graph.Edges,ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public bool ContainsEdge(IVertex u, IVertex v)
		{
			if (u==null)
				throw new ArgumentNullException(@"u");
			if (v==null)
				throw new ArgumentNullException(@"v");
			return Graph.ContainsEdge(u,v);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool VerticesEmpty
		{
			get
			{
				return this.graph.VerticesEmpty;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IVertexEnumerable Vertices
		{
			get
			{
				return this.graph.Vertices;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int VerticesCount
		{
			get
			{
				return this.graph.VerticesCount;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public bool ContainsVertex(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException(@"v");
			return Graph.ContainsVertex(v);
		}

		public bool EdgesEmpty
		{
			get
			{
				return this.graph.EdgesEmpty;
			}
		}

		public int EdgesCount
		{
			get
			{
				return this.graph.EdgesCount;
			}
		}

		public IEdgeEnumerable Edges
		{
			get
			{
				return this.graph.Edges;
			}
		}

		public bool ContainsEdge(IEdge e)
		{
			return this.graph.ContainsEdge(e);
		}

		/// <summary>
		/// Gets an enumerable collection of the v adjacent vertices
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public IVertexEnumerable AdjacentVertices(IVertex v)
		{
			return new TargetVertexEnumerable(OutEdges(v));
		}
	}
}
