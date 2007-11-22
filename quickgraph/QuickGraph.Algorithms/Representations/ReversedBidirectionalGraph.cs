using System;

namespace QuickGraph.Representations
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;

	/// <summary>
	/// Adaptor to flip in-edges and out-edges.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This adaptor flips the in-edges and out-edges of a IBidirectionalGraph, 
	/// effectively transposing the graph. 
	/// </para>
	/// <para>
	/// The construction of the reverse graph is constant time, 
	/// providing a highly efficient way to obtain a transposed-view of a 
	/// graph. 
	/// </para>
	/// </remarks>
	public class ReversedBidirectionalGraph :
		IBidirectionalVertexAndEdgeListGraph
	{
		private IBidirectionalVertexAndEdgeListGraph wrapped;

		/// <summary>
		/// Construct a reversed graph adaptor
		/// </summary>
		/// <param name="g">Graph to adapt</param>
		public ReversedBidirectionalGraph(IBidirectionalVertexAndEdgeListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			this.wrapped = g;
		}

		/// <summary>
		/// Reversed graph
		/// </summary>
		public IBidirectionalVertexAndEdgeListGraph ReversedGraph
		{
			get
			{
				return wrapped;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsDirected
		{
			get
			{
				return ReversedGraph.IsDirected;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool AllowParallelEdges
		{
			get
			{
				return ReversedGraph.AllowParallelEdges;
			}
		}

		public bool VerticesEmpty
		{
			get
			{
				return this.ReversedGraph.VerticesEmpty;
			}
		}

		public int VerticesCount
		{
			get
			{
				return this.ReversedGraph.VerticesCount;
			}
		}

		public IVertexEnumerable Vertices
		{
			get
			{
				return this.ReversedGraph.Vertices;
			}
		}

		public bool ContainsVertex(IVertex v)
		{
			return this.ReversedGraph.ContainsVertex(v);
		}

		public bool EdgesEmpty
		{
			get
			{
				return this.wrapped.EdgesEmpty;
			}
		}

		public int EdgesCount
		{
			get
			{
				return this.wrapped.EdgesCount;
			}
		}

		public IEdgeEnumerable Edges
		{
			get
			{
				return new ReversedEdgeEnumerable(this.wrapped.Edges);
			}
		}

		public bool ContainsEdge(IEdge e)
		{
			return this.wrapped.ContainsEdge(e);
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
			if (v==null)
				throw new ArgumentNullException("v");
			return this.ReversedGraph.OutEdgesEmpty(v);
		}

		/// <summary>
		/// Flipped out-degree
		/// </summary>
		/// <param name="v">vertex to compute</param>
		/// <returns>transposed out-edgree</returns>
		public int InDegree(IVertex v)
		{
			return ReversedGraph.OutDegree(v);
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
			return this.ReversedGraph.InEdgesEmpty(v);
		}

		/// <summary>
		/// Flipped in-degree
		/// </summary>
		/// <param name="v">vertex to compute</param>
		/// <returns>transposed in-edgree</returns>
		public int OutDegree(IVertex v)
		{
			return ReversedGraph.InDegree(v);
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
			return this.ReversedGraph.AdjacentEdgesEmpty(v);
		}

		/// <summary>
		/// Vertex degree
		/// </summary>
		/// <param name="v">vertex to compute</param>
		/// <returns>vertex edgree</returns>
		public int Degree(IVertex v)
		{
			return ReversedGraph.Degree(v);
		}

		/// <summary>
		/// Returns a transposed out-edges enumerable
		/// </summary>
		/// <param name="v">vertex to compute</param>
		/// <returns>transposed out edges enumerable</returns>
		public IEdgeEnumerable InEdges(IVertex v)
		{
			return new ReversedEdgeEnumerable(ReversedGraph.OutEdges(v));
		}

		/// <summary>
		/// Returns a transposed in-edges enumerable
		/// </summary>
		/// <param name="v">vertex to compute</param>
		/// <returns>transposed in edges enumerable</returns>
		public IEdgeEnumerable OutEdges(IVertex v)
		{
			return new ReversedEdgeEnumerable(ReversedGraph.InEdges(v));
		}

		/// <summary>
		/// Check the graph contains an edge from <paramref name="u"/> 
		/// to <paramref name="v"/>.
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
			return ReversedGraph.ContainsEdge(v,u);
		}
		/// <summary>
		/// Gets an enumerable collection of the v adjacent vertices
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public IVertexEnumerable AdjacentVertices(IVertex v)
		{
			return new TargetVertexEnumerable(InEdges(v));
		}
	}
}
