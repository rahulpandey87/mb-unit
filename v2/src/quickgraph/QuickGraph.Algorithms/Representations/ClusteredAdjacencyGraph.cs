using System;
using System.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Predicates;
using QuickGraph.Concepts.Modifications;
using QuickGraph.Concepts.MutableTraversals;
using QuickGraph.Concepts.Providers;
using QuickGraph.Concepts.Serialization;
using QuickGraph.Concepts.Collections;
using QuickGraph.Collections;

namespace QuickGraph.Representations
{

	/// <summary>
	/// A clustered adjacency graph
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class implements a clustered <see cref="AdjacencyGraph"/>: 
	/// an <see cref="AdjacencyGraph"/>
	/// that has sub-graphs (clusters). Each cluster is a 
	/// <see cref="ClusteredAdjacencyGraph"/> which can also have sub-graphs.
	/// </para>
	/// <para>
	/// Suppose that <c>G=(V,E)</c> is the main graph and 
	/// that <c>G1=(V1,E1)</c> is a cluster:
	/// <list type="bullet">
	/// <item>
	/// <description>If <c>v</c> is added to <c>V1</c>, then <c>v</c> also
	/// belongs to V.</description>
	/// </item>
	/// <item>
	/// <description>If <c>v</c> is added to <c>V</c>, then <c>v</c> does not
	/// belong to <c>V1</c>.</description>
	/// </item>
	/// </list>
	/// </para>
	/// <para>
	/// <b>Threading:</b>
	/// Not thread safe.
	/// </para>
	/// </remarks>
	public class ClusteredAdjacencyGraph : 
		IFilteredVertexAndEdgeListGraph
		,IFilteredIncidenceGraph
		,IMutableEdgeListGraph
		,IEdgeMutableGraph
		,IMutableIncidenceGraph
		,IEdgeListAndIncidenceGraph
		,ISerializableVertexAndEdgeListGraph
		,IMutableVertexAndEdgeListGraph
		,IClusteredGraph
	{
		private ClusteredAdjacencyGraph parent;
		private AdjacencyGraph wrapped;
		private ArrayList clusters;
		private bool colapsed;

		/// <summary>
		/// Constructs a <see cref="ClusteredAdjacencyGraph"/> on top of
		/// the <see cref="AdjacencyGraph"/> object.
		/// </summary>
		/// <param name="wrapped">parent graph</param>
		public ClusteredAdjacencyGraph(AdjacencyGraph wrapped)
		{
			if(wrapped==null)
				throw new ArgumentNullException("parent");
			this.parent = null;
			this.wrapped = wrapped;
			this.clusters = new ArrayList();
			this.colapsed=false;
		}

		/// <summary>
		/// Construct a cluster inside another cluster
		/// </summary>
		/// <param name="parent"></param>
		public ClusteredAdjacencyGraph(ClusteredAdjacencyGraph parent)
		{
			if(parent==null)
				throw new ArgumentNullException("parent");
			this.parent = parent;
			this.wrapped = new AdjacencyGraph(
				Parent.VertexProvider,
				Parent.EdgeProvider,
				Parent.AllowParallelEdges
				);
			this.clusters = new ArrayList();
		}

		/// <summary>
		/// Gets the parent <see cref="AdjacencyGraph"/>.
		/// </summary>
		/// <value>
		/// Parent <see cref="AdjacencyGraph"/>.
		/// </value>
		public ClusteredAdjacencyGraph Parent
		{
			get
			{
				return parent;
			}
		}

		/// <summary>
		/// Not implemented yet.
		/// </summary>
		public bool Colapsed
		{
			get
			{
				return colapsed;
			}
			set
			{
				colapsed = value;
			}
		}

		/// <summary>
		/// Gets the wrapped <see cref="AdjacencyGraph"/> object.
		/// </summary>
		/// <remarks>
		/// Wrapped <see cref="AdjacencyGraph"/> object. Can be a 
		/// <see cref="ClusteredAdjacencyGraph"/>.
		/// </remarks>
		protected AdjacencyGraph Wrapped
		{
			get
			{
				return wrapped;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the graph is directed.
		/// </summary>
		/// <value>
		/// true if the graph is directed, false otherwize.
		/// </value>
		public bool IsDirected
		{
			get
			{
				return Wrapped.IsDirected;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the graph allows parallel edges.
		/// </summary>
		/// <value>
		/// true if the graph allows parallel edges, false otherwize.
		/// </value>
		public bool AllowParallelEdges
		{
			get
			{
				return Wrapped.AllowParallelEdges;
			}
		}

		/// <summary>
		/// Gets the <see cref="IVertexProvider"/> used to generate the vertices.
		/// </summary>
		/// <value>
		/// <see cref="IVertexProvider"/> instance used to generate the new 
		/// vertices.
		/// </value>
		public IVertexProvider VertexProvider
		{
			get
			{
				return Wrapped.VertexProvider;
			}
		}

		/// <summary>
		/// Gets the <see cref="IEdgeProvider"/> used to generate the edges.
		/// </summary>
		/// <value>
		/// <see cref="IEdgeProvider"/> instance used to generate the new 
		/// edges.
		/// </value>
		public IEdgeProvider EdgeProvider
		{
			get
			{
				return Wrapped.EdgeProvider;
			}
		}

		/// <summary>
		/// Gets a value indicating if the vertex set is empty
		/// </summary>
		/// <remarks>
		/// <para>
		/// Usually faster that calling <see cref="EdgesCount"/>.
		/// </para>
		/// </remarks>
		/// <value>
		/// true if the vertex set is empty, false otherwise.
		/// </value>
		public bool EdgesEmpty
		{
			get
			{
				return Wrapped.EdgesEmpty;
			}
		}

		/// <summary>
		/// Gets an enumerable collection of edges.
		/// </summary>
		/// <value>
		/// <see cref="IEdgeEnumerable"/> collection of edges.
		/// </value>
		public IEdgeEnumerable Edges
		{
			get
			{
				return Wrapped.Edges;
			}
		}

		/// <summary>
		/// Gets the edge count.
		/// </summary>
		/// <value>
		/// Edge count.
		/// </value>
		/// <remarks>
		/// Complexity: O(E)
		/// </remarks>
		public int EdgesCount
		{
			get
			{
				return Wrapped.EdgesCount;
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
				return Wrapped.VerticesEmpty;
			}
		}

		/// <summary>
		/// Gets an enumerable collection of vertices.
		/// </summary>
		/// <value>
		/// <see cref="IVertexEnumerable"/> collection of vertices.
		/// </value>
		public IVertexEnumerable Vertices
		{
			get
			{
				return Wrapped.Vertices;
			}
		}

		/// <summary>
		/// Gets the vertex count.
		/// </summary>
		/// <value>
		/// Vertex count.
		/// </value>
		/// <remarks>
		/// Complexity: O(V)
		/// </remarks>
		public int VerticesCount
		{
			get
			{
				return Wrapped.VerticesCount;
			}
		}

		/// <summary>
		/// Determines whether the <see cref="ClusteredAdjacencyGraph"/> 
		/// contains the edge <paramref name="e"/>.
		/// </summary>
		/// <param name="e">
		/// The edge to locate in <see cref="ClusteredAdjacencyGraph"/>.
		/// </param>
		/// <returns>
		/// true if the <see cref="ClusteredAdjacencyGraph"/> contains 
		/// the edge <paramref name="e"/>; otherwise, false.
		///	</returns>
		public bool ContainsEdge(IEdge e)
		{
			return Wrapped.ContainsEdge(e);
		}

		/// <summary>
		/// Determines whether the <see cref="ClusteredAdjacencyGraph"/> 
		/// contains an edge from the vertex <paramref name="u"/> to
		/// the vertex <paramref name="v"/>.
		/// </summary>
		/// <param name="u">
		/// The source vertex of the edge(s) to locate in <see cref="ClusteredAdjacencyGraph"/>.
		/// </param>
		/// <param name="v">
		/// The target vertex of the edge(s) to locate in <see cref="ClusteredAdjacencyGraph"/>.
		/// </param>
		/// <returns>
		/// true if the <see cref="ClusteredAdjacencyGraph"/> contains 
		/// the edge (<paramref name="u"/>, <paramref name="v"/>); otherwise, false.
		///	</returns>
		public bool ContainsEdge(IVertex u, IVertex v)
		{
			return Wrapped.ContainsEdge(u,v);
		}

		/// <summary>
		/// Determines whether the <see cref="ClusteredAdjacencyGraph"/> 
		/// contains the vertex <paramref name="v"/>.
		/// </summary>
		/// <param name="v">
		/// The vertex to locate in <see cref="ClusteredAdjacencyGraph"/>.
		/// </param>
		/// <returns>
		/// true if the <see cref="ClusteredAdjacencyGraph"/> contains 
		/// the vertex <paramref name="v"/>; otherwise, false.
		///	</returns>
		public bool ContainsVertex(IVertex v)
		{
			return Wrapped.ContainsVertex(v);
		}

		/// <summary>
		/// Gets a filtered <see cref="IEdgeEnumerable"/> collection of edges.
		/// </summary>
		/// <param name="ep">edge predicate</param>
		/// <returns>filetered collection</returns>
		public IEdgeEnumerable SelectEdges(IEdgePredicate ep)
		{
			return Wrapped.SelectEdges(ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep"></param>
		/// <returns></returns>
		public IEdgeEnumerable SelectOutEdges(IVertex v, IEdgePredicate ep)
		{
			return Wrapped.SelectOutEdges(v,ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public IEdgeEnumerable OutEdges(IVertex v)
		{
			return Wrapped.OutEdges(v);
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
			return Wrapped.OutEdgesEmpty(v);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public int OutDegree(IVertex v)
		{
			return Wrapped.OutDegree(v);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ep"></param>
		/// <returns></returns>
		public IEdge SelectSingleEdge(IEdgePredicate ep)
		{
			return Wrapped.SelectSingleEdge(ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep"></param>
		/// <returns></returns>
		public IEdge SelectSingleOutEdge(IVertex v, IEdgePredicate ep)
		{
			return Wrapped.SelectSingleOutEdge(v,ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vp"></param>
		/// <returns></returns>
		public IVertex SelectSingleVertex(IVertexPredicate vp)
		{
			return Wrapped.SelectSingleVertex(vp);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="vp"></param>
		/// <returns></returns>
		public IVertexEnumerable SelectVertices(IVertexPredicate vp)
		{
			return Wrapped.SelectVertices(vp);
		}

		/// <summary>
		/// Gets an enumerable collection of clusters
		/// </summary>
		/// <value>
		/// Enumerable collection of clusters
		/// </value>
		public IEnumerable Clusters
		{
			get
			{
				return clusters;
			}
		}

		/// <summary>
		/// Gets the number of clusters
		/// </summary>
		/// <value>
		/// Number of clusters
		/// </value>
		public int ClustersCount
		{
			get
			{
				return clusters.Count;
			}
		}

		/// <summary>
		/// Adds a new cluster.
		/// </summary>
		/// <returns>New cluster</returns>
		public ClusteredAdjacencyGraph AddCluster()
		{
			ClusteredAdjacencyGraph cluster = new ClusteredAdjacencyGraph(this);
			clusters.Add(cluster);
			return cluster;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IClusteredGraph IClusteredGraph.AddCluster()
		{
			return this.AddCluster();
		}

		/// <summary>
		/// Removes a cluster
		/// </summary>
		/// <param name="cluster">cluster to remove</param>
		/// <exception cref="ArgumentNullException">cluster is a null reference.</exception>
		public void RemoveCluster(IClusteredGraph cluster)
		{
			if (cluster==null)
				throw new ArgumentNullException("cluster");
			clusters.Remove(cluster);
		}

		/// <summary>
		/// Adds a new vertex to the cluster
		/// </summary>
		/// <returns>new vertex</returns>
		public virtual IVertex AddVertex()
		{
			if (Parent!=null)
			{
				// add new vertex in the parent
				IVertex v = Parent.AddVertex();
				// add new vertex in the cluster
				Wrapped.AddVertex(v);
				// return vertex
				return v;
			}
			else
				return Wrapped.AddVertex();
		}

		/// <summary>
		/// Adds an existing vertex to the cluster
		/// </summary>
		/// <param name="v">vertex to add</param>
		public virtual void AddVertex(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			if (Parent!=null && !Parent.ContainsVertex(v))
				Parent.AddVertex(v);
			Wrapped.AddVertex (v);
		}

		/// <summary>
		/// Adds a new edge
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <param name="v">target edge</param>
		/// <returns>added edge</returns>
		/// <exception cref="ArgumentNullException">u or v is a null reference</exception>
		public virtual IEdge AddEdge(IVertex u, IVertex v)
		{
			if (u==null)
				throw new ArgumentNullException("u");
			if (v==null)
				throw new ArgumentNullException("v");
		
			// check if u,v in vertex set
			if (!ContainsVertex(u))
				throw new ArgumentException("graph does not contain u");
			if (!ContainsVertex(v))
				throw new ArgumentException("graph does not contain v");		
		
			// add edge
			if (Parent!=null)
			{
				IEdge e = Parent.AddEdge(u,v);
				Wrapped.AddEdge(e);
				return e;
			}
			else
				return Wrapped.AddEdge(u,v);		
		}

		/// <summary>
		/// Adds an existing edge to the cluster
		/// </summary>
		/// <param name="e">edge to add</param>
		public virtual void AddEdge(IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("e");

			if (Parent!=null && !Parent.ContainsEdge(e))
				Parent.AddEdge(e);
			Wrapped.AddEdge (e);
		}

		/// <summary>
		/// Removes a vertex from the cluster
		/// </summary>
		/// <param name="u"></param>
		public virtual void RemoveVertex(IVertex u)
		{
			if (u==null)
				throw new ArgumentNullException("u");
			Wrapped.RemoveVertex(u);
			if (Parent!=null)
				Parent.RemoveVertex(u);
		}

		/// <summary>
		/// Clears vertex out-edges
		/// </summary>
		/// <param name="u"></param>
		public virtual void ClearVertex(IVertex u)
		{
			Parent.ClearVertex(u);
			Wrapped.ClearVertex(u);
		}

		/// <summary>
		/// Remove a specific edge
		/// </summary>
		/// <param name="e"></param>
		public virtual void RemoveEdge(IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("e");
			Wrapped.RemoveEdge(e);
			if (Parent!=null)
				Parent.RemoveEdge(e);
		}
	
		/// <summary>
		/// Remove edges from u to v
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>	
		public virtual void RemoveEdge(IVertex u,IVertex v)
		{
			if (u==null)
				throw new ArgumentNullException("u");
			if (v==null)
				throw new ArgumentNullException("v");
			Wrapped.RemoveEdge(u,v);
			if (Parent!=null)
				Parent.RemoveEdge(u,v);
		}
	
		/// <summary>
		/// Remove out edge satisfying the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep"></param>
		public virtual void RemoveOutEdgeIf(IVertex v, IEdgePredicate ep)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			if (ep==null)
				throw new ArgumentNullException("ep");
		
			Wrapped.RemoveOutEdgeIf(v,ep);
			if (Parent!=null)
				Parent.RemoveOutEdgeIf(v,ep);
		}

		/// <summary>
		/// Remove edge satifying the predicate
		/// </summary>
		/// <param name="ep"></param>
		public virtual void RemoveEdgeIf(IEdgePredicate ep)
		{
			if (ep==null)
				throw new ArgumentNullException("ep");
		
			Wrapped.RemoveEdgeIf(ep);
			if (Parent!=null)
				Parent.RemoveEdgeIf(ep);
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

		public virtual void Clear()
		{
			this.wrapped.Clear();
			this.clusters.Clear();
		}

	}
}
