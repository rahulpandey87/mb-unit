// Automatically generated code, dot edit!

namespace TestFu.Data.Graph
{
	using System;
	using System.Collections;
	using System.Diagnostics;
	using System.Data;

	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.MutableTraversals;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Providers;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Collections;
	using QuickGraph.Exceptions;
    using QuickGraph.Predicates;


	/// <summary>
	/// A graph of <see cref="DataTable"/> (vertices) and <see cref="DataRelation"/>
	/// (edges).
	/// </summary>
	public class DataGraph :
		 IMutableGraph
		,IFilteredVertexAndEdgeListGraph
		,IFilteredIncidenceGraph
		,IMutableEdgeListGraph
		,IEdgeMutableGraph
		,IMutableIncidenceGraph
		,IEdgeListAndIncidenceGraph
		,ISerializableVertexAndEdgeListGraph
		,IMutableVertexAndEdgeListGraph
		,IAdjacencyGraph
		,IIndexedVertexListGraph
		,IFilteredBidirectionalGraph
		,IMutableBidirectionalGraph
		,IBidirectionalVertexAndEdgeListGraph
		,IMutableBidirectionalVertexAndEdgeListGraph
	{
		private int version=0;
		private bool allowParallelEdges=true;
		private DataTableVertexProvider vertexProvider = new DataTableVertexProvider();
		private DataRelationEdgeProvider edgeProvider = new DataRelationEdgeProvider();
		private DataTableVertexDataRelationEdgeCollectionDictionary vertexOutEdges = new DataTableVertexDataRelationEdgeCollectionDictionary();
		private DataTableVertexDataRelationEdgeCollectionDictionary vertexInEdges = new DataTableVertexDataRelationEdgeCollectionDictionary();
		
		private Hashtable tableVertices =new Hashtable();
		private Hashtable relationEdges = new Hashtable();

        #region Helper constructor
        public static DataGraph Create(DataSet dataSource)
        {
            if (dataSource == null)
                throw new ArgumentNullException("dataSource");
            DataGraphPopulator pop = new DataGraphPopulator();
            pop.DataSource = dataSource;
            pop.Populate();
            return pop.Graph;
        }
        #endregion

        #region Data related methods
        public DataTable[] GetSortedTables()
        {
            DataTableSortAlgorithm topo = new DataTableSortAlgorithm(this);
            topo.Compute();
            return topo.GetSortedTables();
        }
        #endregion

		#region Constructors
		/// <summary>
		/// Builds a new empty directed graph with default vertex and edge
		/// provider.
		/// </summary>
		/// <remarks>
		/// </remarks>
		public DataGraph()
		{}
		#endregion

        #region Data to Vertex/Edge methods
        public DataTableVertex GetVertex(DataTable table)
		{
			return (DataTableVertex)this.tableVertices[table];
		}
		public DataRelationEdge GetEdge(DataRelation relation)
		{
			return (DataRelationEdge)this.relationEdges[relation];
        }
        #endregion

		#region IMutableGraph
		/// <summary>
		/// Remove all of the edges and vertices from the graph.
		/// </summary>
		public virtual void Clear()
		{
			this.version++;
			this.vertexOutEdges.Clear();
			this.vertexInEdges.Clear();
		}
		#endregion
		
		#region IGraph
		/// <summary>
		/// Gets a value indicating if the <see cref="DataGraph"/> 
		/// is directed.
		/// </summary>
		/// <value>
		/// true if the graph is directed, false if undirected.
		/// </value>
		public bool IsDirected
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating if the <see cref="DataGraph"/> allows parallel edges.
		/// </summary>
		/// <value>
		/// true if the <see cref="DataGraph"/> is a multi-graph, false otherwise
		/// </value>
		public bool AllowParallelEdges
		{
			get
			{
				return this.IsDirected && this.allowParallelEdges;
			}
		}
		#endregion
			
		#region IVertexMutableGraph		
		/// <summary>
		/// Gets the <see cref="DataTableVertex"/> provider
		/// </summary>
		/// <value>
		/// <see cref="DataTableVertex"/> provider
		/// </value>
		public DataTableVertexProvider VertexProvider
		{
			get
			{
				return this.vertexProvider;
			}
		}	
		
		IVertexProvider IVertexMutableGraph.VertexProvider
		{
			get
			{
				return this.VertexProvider;
			}
		}	
		
		/// <summary>
		/// Add a new DataTableVertex to the graph and returns it.
		/// </summary>
		/// <returns>
		/// Created vertex
		/// </returns>
		public virtual DataTableVertex AddVertex(DataTable table)
		{
			if (table==null)
				throw new ArgumentNullException("table");
			if (this.tableVertices.Contains(table))
				throw new ArgumentNullException("Duplicate table");
			
			this.version++;
			DataTableVertex v = (DataTableVertex)this.VertexProvider.ProvideVertex();
			this.vertexOutEdges.Add(v);
			this.vertexInEdges.Add(v);
			
			v.Table = table;
			this.tableVertices.Add(table, v);
			
			return v;
		}
		
		IVertex IVertexMutableGraph.AddVertex()
		{
			throw new NotSupportedException();
		}
		
		
		public virtual void RemoveVertex(DataTable table)
		{
			this.RemoveVertex(GetVertex(table));
		}
		
		/// <summary>
		/// Removes the vertex from the graph.
		/// </summary>
		/// <param name="v">vertex to remove</param>
		/// <exception cref="ArgumentNullException">v is null</exception>
		public virtual void RemoveVertex(DataTableVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			if (!ContainsVertex(v))
				throw new VertexNotFoundException("v");

			this.version++;
			this.ClearVertex(v);

			// removing vertex
			this.vertexOutEdges.Remove(v);
			this.vertexInEdges.Remove(v);
		}	
		
		void IVertexMutableGraph.RemoveVertex(IVertex v)
		{
			this.RemoveVertex((DataTableVertex)v);
		}
		#endregion
		
		#region IEdgeMutableGraph
		/// <summary>
		/// Gets the <see cref="DataRelationEdge"/> provider
		/// </summary>
		/// <value>
		/// <see cref="DataRelationEdge"/> provider
		/// </value>
		public DataRelationEdgeProvider EdgeProvider
		{
			get
			{
				return this.edgeProvider;
			}
		}
		
		IEdgeProvider IEdgeMutableGraph.EdgeProvider
		{
			get
			{
				return this.EdgeProvider;
			}
		}
		
		public virtual DataRelationEdge AddEdge(DataRelation relation)
		{
			return this.AddEdge(GetVertex(relation.ParentTable),
			                    GetVertex(relation.ChildTable),
			                    relation);
		}
		
		/// <summary>
		/// Add a new vertex from source to target
		///  
		/// Complexity: 2 search + 1 insertion
		/// </summary>
		/// <param name="source">Source vertex</param>
		/// <param name="target">Target vertex</param>
		/// <returns>Created Edge</returns>
		/// <exception cref="ArgumentNullException">
		/// source or target is a null reference
		/// </exception>
		/// <exception cref="Exception">source or target are not part of the graph</exception>
		protected virtual  DataRelationEdge AddEdge(
			DataTableVertex source,
			DataTableVertex target,
			DataRelation relation
			)
		{
			// look for the vertex in the list
			if (!this.vertexOutEdges.Contains(source))
				throw new VertexNotFoundException("Could not find source vertex");
			if (!this.vertexOutEdges.Contains(target))
				throw new VertexNotFoundException("Could not find target vertex");
			if (relation==null)
				throw new ArgumentNullException("relation");

			// if parralel edges are not allowed check if already in the graph
			if (!this.AllowParallelEdges)
			{
				if (ContainsEdge(source,target))
					throw new Exception("Parallel edge not allowed");
			}

			this.version++;
			// create edge
			DataRelationEdge e = (DataRelationEdge)this.EdgeProvider.ProvideEdge(source,target);
			this.vertexOutEdges[source].Add(e);
			this.vertexInEdges[target].Add(e);
			
			e.Relation = relation;
			this.relationEdges.Add(relation,e);

			return e;
		}
		
		IEdge IEdgeMutableGraph.AddEdge(
			IVertex source,
			IVertex target
			)
		{
			throw new NotSupportedException();
		}
		

		/// <summary>
		/// Remove all edges to and from vertex u from the graph.
		/// </summary>
		/// <param name="v"></param>
		public virtual void ClearVertex(DataTableVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("vertex");

			this.version++;
			// removing edges touching v
			this.RemoveEdgeIf(new IsAdjacentEdgePredicate(v));

			// removing edges
			this.vertexOutEdges[v].Clear();
			this.vertexInEdges[v].Clear();
		}
		
		void IEdgeMutableGraph.ClearVertex(IVertex v)
		{
			this.ClearVertex((DataTableVertex)v);
		}
		

		/// <summary>
		/// Removes an edge from the graph.
		/// 
		/// Complexity: 2 edges removed from the vertex edge list + 1 edge
		/// removed from the edge list.
		/// </summary>
		/// <param name="e">edge to remove</param>
		/// <exception cref="ArgumentNullException">
		/// e is a null reference (Nothing in Visual Basic)
		/// </exception>
		/// <exception cref="EdgeNotFoundException">
		/// <paramref name="e"/> is not part of the graph
		/// </exception>
		public virtual void RemoveEdge(DataRelationEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("e");
			if (!this.ContainsEdge(e))
				throw new EdgeNotFoundException("e");
			
			this.version++;
			// removing edge from vertices
			DataTableVertex source= (DataTableVertex)e.Source;
			DataRelationEdgeCollection outEdges = this.vertexOutEdges[source];
			if (outEdges==null)
				throw new VertexNotFoundException(source.ToString());
			outEdges.Remove(e);
			DataTableVertex target= (DataTableVertex)e.Target;
			DataRelationEdgeCollection inEdges = this.vertexInEdges[target];
			if (inEdges==null)
				throw new VertexNotFoundException(target.ToString());
			inEdges.Remove(e);
		}

		void IEdgeMutableGraph.RemoveEdge(IEdge e)
		{
			this.RemoveEdge((DataRelationEdge)e);
		}
		
		/// <summary>
		/// Remove the edge (u,v) from the graph. 
		/// If the graph allows parallel edges this remove all occurrences of 
		/// (u,v).
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <param name="v">target vertex</param>
		public virtual void RemoveEdge(DataTableVertex u, DataTableVertex v)
		{
			if (u == null)
				throw new ArgumentNullException("u");
			if (v == null)
				throw new ArgumentNullException("v");

			this.version++;
			// getting out-edges
			DataRelationEdgeCollection outEdges = this.vertexOutEdges[u];
			
			// marking edges to remove
			DataRelationEdgeCollection removedEdges = new DataRelationEdgeCollection();
			foreach(DataRelationEdge e in outEdges)
			{
				if (e.Target == v)
					removedEdges.Add(e);
			}
			//removing out-edges
			foreach(DataRelationEdge e in removedEdges)
				outEdges.Remove(e);
			
			removedEdges.Clear();
			DataRelationEdgeCollection inEdges = this.vertexInEdges[v];
			foreach(DataRelationEdge e in inEdges)
			{
				if (e.Source == u)
					removedEdges.Add(e);
			}
			//removing in-edges
			foreach(DataRelationEdge e in removedEdges)
				inEdges.Remove(e);
		}
		
		void IEdgeMutableGraph.RemoveEdge(IVertex u, IVertex v)
		{
			this.RemoveEdge((DataTableVertex) u, (DataTableVertex) v);
		}
		
		#endregion 
		
		#region ISerializableVertexListGraph
		/// <summary>
		/// Add a new vertex to the graph and returns it.
		/// </summary>
		/// <returns>Create vertex</returns>
		internal virtual void AddVertex(DataTableVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("vertex");
			if (this.vertexOutEdges.Contains(v))
				throw new ArgumentException("vertex already in graph");

			this.version++;
			this.VertexProvider.UpdateVertex(v);
			this.vertexOutEdges.Add(v);
			this.vertexInEdges.Add(v);
		}
		
		void ISerializableVertexListGraph.AddVertex(IVertex v)
		{
			this.AddVertex((DataTableVertex) v);
		}
		#endregion
		
		#region ISerializableEdgeListGraph
		/// <summary>
		/// Used for serialization. Not for private use.
		/// </summary>
		/// <param name="e">edge to add.</param>
		internal virtual void AddEdge(DataRelationEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("vertex");
			if (e.GetType().IsAssignableFrom(EdgeProvider.EdgeType))
				throw new ArgumentNullException("vertex type not valid");
			
			DataTableVertex source= (DataTableVertex)e.Source;
			if (!this.vertexOutEdges.Contains(source))
				throw new VertexNotFoundException(source.ToString());
			DataTableVertex target= (DataTableVertex)e.Target;
			if (!this.vertexOutEdges.Contains(target))
				throw new VertexNotFoundException(target.ToString());

			// if parralel edges are not allowed check if already in the graph
			if (!this.AllowParallelEdges)
			{
				if (ContainsEdge(source,target))
					throw new ArgumentException("graph does not allow duplicate edges");
			}
			// create edge
			this.EdgeProvider.UpdateEdge(e);
			this.vertexOutEdges[source].Add(e);
			this.vertexInEdges[target].Add(e);
		}		
		
		void ISerializableEdgeListGraph.AddEdge(IEdge e)
		{
			this.AddEdge((DataRelationEdge)e);
		}		
		#endregion

		#region IIncidenceGraph
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
		/// <exception cref="ArgumentNullException">
		/// v is a null reference (Nothing in Visual Basic)
		/// </exception>
		/// <exception cref="VertexNotFoundException">
		/// v is not part of the graph.
		/// </exception>
		public bool OutEdgesEmpty(DataTableVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			DataRelationEdgeCollection edges = this.vertexOutEdges[v];
			if (edges==null)
				throw new VertexNotFoundException(v.ToString());
			return edges.Count==0;
		}
		
		bool IImplicitGraph.OutEdgesEmpty(IVertex v)
		{
			return this.OutEdgesEmpty((DataTableVertex)v);
		}

		/// <summary>
		/// Returns the number of out-degree edges of v
		/// </summary>
		/// <param name="v">vertex</param>
		/// <returns>number of out-edges of the <see cref="DataTableVertex"/> v</returns>
		/// <exception cref="ArgumentNullException">
		/// v is a null reference (Nothing in Visual Basic)
		/// </exception>
		/// <exception cref="VertexNotFoundException">
		/// v is not part of the graph.
		/// </exception>		
		public int OutDegree(DataTableVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			DataRelationEdgeCollection edges = this.vertexOutEdges[v];
			if (edges==null)
				throw new VertexNotFoundException(v.ToString());
			return edges.Count;
		}

		int IImplicitGraph.OutDegree(IVertex v)
		{
			return this.OutDegree((DataTableVertex)v);
		}

		/// <summary>
		/// Returns an iterable collection over the edge connected to v
		/// </summary>
		/// <param name="v"></param>
		/// <returns>out-edges of v</returns>
		/// <exception cref="ArgumentNullException">
		/// v is a null reference.
		/// </exception>
		/// <exception cref="VertexNotFoundException">
		/// v is not part of the graph.
		/// </exception>
		public IEdgeCollection OutEdges(DataTableVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");

			DataRelationEdgeCollection edges = this.vertexOutEdges[v];
			if (edges==null)
				throw new VertexNotFoundException(v.ToString());
			return edges;
		}		

		IEdgeEnumerable IImplicitGraph.OutEdges(IVertex v)
		{
			return this.OutEdges((DataTableVertex)v);
		}

		/// <summary>
		/// Test is an edge (u,v) is part of the graph
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <param name="v">target vertex</param>
		/// <returns>true if part of the graph</returns>
		public bool ContainsEdge(DataTableVertex u,DataTableVertex v)
		{
			// try to find the edge
			foreach(DataRelationEdge e in this.OutEdges(u))
			{
				if (e.Target == v)
					return true;
			}
			return false;
		}
		
		bool IIncidenceGraph.ContainsEdge(IVertex u, IVertex v)
		{
			return this.ContainsEdge((DataTableVertex)u,(DataTableVertex)v);
		}		
		#endregion 
		
		#region IFilteredIncidenceGraph  
		/// <summary>
		/// Returns the first out-edge that matches the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep">Edge predicate</param>
		/// <returns>null if not found, otherwize the first Edge that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">v or ep is null</exception>
		public DataRelationEdge SelectSingleOutEdge(DataTableVertex v, IEdgePredicate ep)
		{
			if (ep==null)
				throw new ArgumentNullException("ep");
			
			foreach(DataRelationEdge e in this.SelectOutEdges(v,ep))
				return e;

			return null;
		}

		IEdge IFilteredIncidenceGraph.SelectSingleOutEdge(IVertex v, IEdgePredicate ep)
		{
			return this.SelectSingleOutEdge((DataTableVertex)v,ep);
		}

		/// <summary>
		/// Returns the collection of out-edges that matches the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep">Edge predicate</param>
		/// <returns>enumerable colleciton of vertices that matches the 
		/// criteron</returns>
		/// <exception cref="ArgumentNullException">v or ep is null</exception>
		public IEdgeEnumerable SelectOutEdges(DataTableVertex v, IEdgePredicate ep)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			if (ep==null)
				throw new ArgumentNullException("ep");
			
			return new FilteredEdgeEnumerable(this.OutEdges(v),ep);
		}

		IEdgeEnumerable IFilteredIncidenceGraph.SelectOutEdges(IVertex v, IEdgePredicate ep)
		{
			return this.SelectOutEdges((DataTableVertex)v,ep);
		}


		/// <summary>
		/// Remove all the edges from graph g for which the predicate pred
		/// returns true.
		/// </summary>
		/// <param name="pred">edge predicate</param>
		public virtual void RemoveEdgeIf(IEdgePredicate pred)
		{
			if (pred == null)
				throw new ArgumentNullException("predicate");

			// marking edge for removal
			DataRelationEdgeCollection removedEdges = new DataRelationEdgeCollection();
			foreach(DataRelationEdge e in Edges)
			{
				if (pred.Test(e))
					removedEdges.Add(e);
			}

			// removing edges
			foreach(DataRelationEdge e in removedEdges)
				this.RemoveEdge(e);
		}
		#endregion

		#region IMutableIncidenceGraph
		/// <summary>
		/// Remove all the out-edges of vertex u for which the predicate pred 
		/// returns true.
		/// </summary>
		/// <param name="u">vertex</param>
		/// <param name="pred">edge predicate</param>
		public virtual void RemoveOutEdgeIf(DataTableVertex u, IEdgePredicate pred)
		{
			if (u==null)
				throw new ArgumentNullException("u");
			if (pred == null)
				throw new ArgumentNullException("pred");

			DataRelationEdgeCollection edges = this.vertexOutEdges[u];
			DataRelationEdgeCollection removedEdges = new DataRelationEdgeCollection();
			foreach(DataRelationEdge e in edges)
			{
				if (pred.Test(e))
					removedEdges.Add(e);
			}

			foreach(DataRelationEdge e in removedEdges)
				this.RemoveEdge(e);
		}

		void IMutableIncidenceGraph.RemoveOutEdgeIf(IVertex u, IEdgePredicate pred)
		{
			this.RemoveOutEdgeIf((DataTableVertex)u,pred);
		}
		#endregion
		
		#region IIndexedIncidenceGraph
		IEdgeCollection IIndexedIncidenceGraph.OutEdges(IVertex v)
		{
			return this.OutEdges((DataTableVertex)v);
		}
		#endregion

		#region IVertexListGraph
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
				return this.vertexOutEdges.Count==0;
			}
		}

		/// <summary>
		/// Gets the number of vertices
		/// </summary>
		/// <value>
		/// Number of vertices in the graph
		/// </value>
		public int VerticesCount
		{
			get
			{
				return this.vertexOutEdges.Count;
			}
		}

		/// <summary>
		/// Enumerable collection of vertices.
		/// </summary>
		public IVertexEnumerable Vertices
		{
			get
			{
				return this.vertexOutEdges.Vertices;
			}
		}
		
		/// <summary>
		/// Tests if a <see cref="DataTableVertex"/> is part of the graph
		/// </summary>
		/// <param name="v">Vertex to test</param>
		/// <returns>true if is part of the graph, false otherwize</returns>
		public bool ContainsVertex(DataTableVertex v)
		{
			return this.vertexOutEdges.Contains(v);
		}		
		
		bool IVertexListGraph.ContainsVertex(IVertex v)	
		{
			return this.ContainsVertex((DataTableVertex)v);
		}
		#endregion
		
		#region IFilteredVertexListGraph
		/// <summary>
		/// Returns the first <see cref="DataTableVertex"/> that matches the predicate
		/// </summary>
		/// <param name="vp">vertex predicate</param>
		/// <returns>null if not found, otherwize the first vertex that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">vp is null</exception>
		public DataTableVertex SelectSingleVertex(IVertexPredicate vp)
		{
			if (vp == null)
				throw new ArgumentNullException("vertex predicate");

			foreach(DataTableVertex v in this.SelectVertices(vp))
				return v;
			return null;
		}

		IVertex IFilteredVertexListGraph.SelectSingleVertex(IVertexPredicate vp)
		{
			return this.SelectSingleVertex(vp);
		}

		/// <summary>
		/// Returns the collection of vertices that matches the predicate
		/// </summary>
		/// <param name="vp">vertex predicate</param>
		/// <returns>enumerable colleciton of vertices that matches the 
		/// criteron</returns>
		/// <exception cref="ArgumentNullException">vp is null</exception>
		public IVertexEnumerable SelectVertices(IVertexPredicate vp)
		{
			if (vp == null)
				throw new ArgumentNullException("vertex predicate");

			return new FilteredVertexEnumerable(Vertices,vp);
		}
		#endregion

		#region EdgeListGraph
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
				return this.EdgesCount==0;
			}
		}

		/// <summary>
		/// Gets the edge count
		/// </summary>
		/// <remarks>
		/// Edges count
		/// </remarks>
		public int EdgesCount
		{
			get
			{
				int n = 0;
				foreach(DictionaryEntry d in vertexOutEdges)
				{
					n+=((DataRelationEdgeCollection)d.Value).Count;
				}
				return n;
			}
		}

		/// <summary>
		/// Enumerable collection of edges.
		/// </summary>
		public IEdgeEnumerable Edges
		{
			get
			{
				return this.vertexOutEdges.Edges;
			}
		}
		
		/// <summary>
		/// Tests if a (<see cref="DataRelationEdge"/>) is part of the graph
		/// </summary>
		/// <param name="e">Edge to test</param>
		/// <returns>true if is part of the graph, false otherwize</returns>
		public bool ContainsEdge(DataRelationEdge e)
		{
			foreach(DictionaryEntry di in this.vertexOutEdges)
			{
				DataRelationEdgeCollection es = (DataRelationEdgeCollection)di.Value;
				if (es.Contains(e))
					return true;
			}
			return false;
		}

		bool IEdgeListGraph.ContainsEdge(IEdge e)
		{	
			return this.ContainsEdge((DataRelationEdge)e);
		}
		#endregion

		#region IFileteredEdgeListGraph
		/// <summary>
		/// Returns the first Edge that matches the predicate
		/// </summary>
		/// <param name="ep">Edge predicate</param>
		/// <returns>null if not found, otherwize the first Edge that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">ep is null</exception>
		public DataRelationEdge SelectSingleEdge(IEdgePredicate ep)
		{
			if (ep == null)
				throw new ArgumentNullException("edge predicate");
			foreach(DataRelationEdge e in this.SelectEdges(ep))
				return e;
			return null;
		}

		IEdge IFilteredEdgeListGraph.SelectSingleEdge(IEdgePredicate ep)
		{
			return this.SelectSingleEdge(ep);
		}

		/// <summary>
		/// Returns the collection of edges that matches the predicate
		/// </summary>
		/// <param name="ep">Edge predicate</param>
		/// <returns>enumerable colleciton of vertices that matches the 
		/// criteron</returns>
		/// <exception cref="ArgumentNullException">ep is null</exception>
		public IEdgeEnumerable SelectEdges(IEdgePredicate ep)
		{
			if (ep == null)
				throw new ArgumentNullException("edge predicate");

			return new FilteredEdgeEnumerable(Edges,ep);
		}
		#endregion

		#region IAdjacencyGraph
		/// <summary>
		/// Gets an enumerable collection of adjacent vertices
		/// </summary>
		/// <param name="v"></param>
		/// <returns>Enumerable collection of adjacent vertices</returns>
		public IVertexEnumerable AdjacentVertices(DataTableVertex v)
		{
			return new TargetVertexEnumerable(this.OutEdges(v));
		}
		
		IVertexEnumerable IAdjacencyGraph.AdjacentVertices(IVertex v)
		{
			return AdjacentVertices((DataTableVertex)v);
		}
		#endregion
		
		#region IBidirectionalGraph
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
		/// <exception cref="ArgumentNullException">
		/// v is a null reference (Nothing in Visual Basic)
		/// </exception>
		/// <exception cref="VertexNotFoundException">
		/// <paramref name="v"/> is not part of the graph.
		/// </exception>
		public bool InEdgesEmpty(DataTableVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			DataRelationEdgeCollection edges = this.vertexInEdges[v];
			if (edges==null)
				throw new VertexNotFoundException("v");
			return edges.Count==0;
		}

		bool IBidirectionalGraph.InEdgesEmpty(IVertex v)
		{
			return this.InEdgesEmpty((DataTableVertex)v);
		}

		/// <summary>
		/// Returns the number of in-degree edges of v
		/// </summary>
		/// <param name="v"></param>
		/// <returns>number of in-edges of the vertex v</returns>
		/// <exception cref="ArgumentNullException">
		/// v is a null reference (Nothing in Visual Basic)
		/// </exception>
		/// <exception cref="VertexNotFoundException">
		/// <paramref name="v"/> is not part of the graph.
		/// </exception>		
		public int InDegree(DataTableVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			DataRelationEdgeCollection edges = this.vertexInEdges[v];
			if (edges==null)
				throw new VertexNotFoundException("v");
			return edges.Count;
		}
		int IBidirectionalGraph.InDegree(IVertex v)
		{
			return this.InDegree((DataTableVertex)v);
		}

		/// <summary>
		/// Returns an iterable collection over the in-edge connected to v
		/// </summary>
		/// <param name="v"></param>
		/// <returns>in-edges of v</returns>
		/// <exception cref="ArgumentNullException">
		/// v is a null reference (Nothing in Visual Basic)
		/// </exception>
		/// <exception cref="VertexNotFoundException">
		/// <paramref name="v"/> is not part of the graph.
		/// </exception>		
		public IEdgeCollection InEdges(DataTableVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			DataRelationEdgeCollection edges = this.vertexInEdges[v];
			if (edges==null)
				throw new VertexNotFoundException(v.ToString());
			return edges;
		}

		/// <summary>
		/// Incidence graph implementation
		/// </summary>
		IEdgeEnumerable IBidirectionalGraph.InEdges(IVertex v)
		{
			return this.InEdges((DataTableVertex)v);
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
		public bool AdjacentEdgesEmpty(DataTableVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			return this.OutEdgesEmpty(v) && this.InEdgesEmpty(v);
		}

		bool IBidirectionalGraph.AdjacentEdgesEmpty(IVertex v)
		{
			return this.AdjacentEdgesEmpty((DataTableVertex)v);
		}

		/// <summary>
		/// Returns the number of in-edges plus out-edges.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public int Degree(DataTableVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			DataRelationEdgeCollection outEdges = this.vertexOutEdges[v];
			if (outEdges==null)
				throw new VertexNotFoundException("v");
			DataRelationEdgeCollection inEdges = this.vertexInEdges[v];
			Debug.Assert(inEdges!=null);
			return outEdges.Count + inEdges.Count;
		}
		
		int IBidirectionalGraph.Degree(IVertex v)
		{
			return this.Degree((DataTableVertex)v);
		}
		#endregion

		#region IFilteredBidirectionalGraph
		/// <summary>
		/// Returns the first in-edge that matches the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep">Edge predicate</param>
		/// <returns>null if not found, otherwize the first Edge that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">v or ep is null</exception>
		public DataRelationEdge SelectSingleInEdge(DataTableVertex v, IEdgePredicate ep)
		{
			if (ep==null)
				throw new ArgumentNullException("edge predicate");
			
			foreach(DataRelationEdge e in this.SelectInEdges(v,ep))
				return e;

			return null;
		}
		IEdge IFilteredBidirectionalGraph.SelectSingleInEdge(IVertex v, IEdgePredicate ep)
		{
			return this.SelectSingleInEdge((DataTableVertex)v, ep);
		}

		/// <summary>
		/// Returns the collection of in-edges that matches the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep">Edge predicate</param>
		/// <returns>enumerable colleciton of vertices that matches the 
		/// criteron</returns>
		/// <exception cref="ArgumentNullException">v or ep is null</exception>
		public IEdgeEnumerable SelectInEdges(DataTableVertex v, IEdgePredicate ep)
		{
			if (v==null)
				throw new ArgumentNullException("vertex");
			if (ep==null)
				throw new ArgumentNullException("edge predicate");
			
			return new FilteredEdgeEnumerable(this.InEdges(v),ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep"></param>
		/// <returns></returns>
		IEdgeEnumerable IFilteredBidirectionalGraph.SelectInEdges(IVertex v, IEdgePredicate ep)
		{
			return this.SelectInEdges((DataTableVertex)v,ep);
		}
		#endregion
		
		#region IMutableBidirectionalGraph

		/// <summary>
		/// Remove all the out-edges of vertex u for which the predicate pred 
		/// returns true.
		/// </summary>
		/// <param name="u">vertex</param>
		/// <param name="pred">edge predicate</param>
		public void RemoveInEdgeIf(DataTableVertex u, IEdgePredicate pred)
		{
			if (u==null)
				throw new ArgumentNullException("vertex u");
			if (pred == null)
				throw new ArgumentNullException("predicate");

			DataRelationEdgeCollection edges = this.vertexInEdges[u];
			DataRelationEdgeCollection removedEdges = new DataRelationEdgeCollection();
			foreach(DataRelationEdge e in edges)
			{
				if (pred.Test(e))
					removedEdges.Add(e);
			}

			foreach(DataRelationEdge e in removedEdges)
				this.RemoveEdge(e);
		}

		void IMutableBidirectionalGraph.RemoveInEdgeIf(IVertex u, IEdgePredicate pred)
		{
			this.RemoveInEdgeIf((DataTableVertex)u,pred);
		}

		#endregion

		#region DataRelationEdgeCollection
		private class DataRelationEdgeCollection : 
			CollectionBase
			,IEdgeCollection
		{
			/// <summary>
			/// Initializes a new empty instance of the 
			/// <see cref="DataRelationEdgeCollection"/> class.
			/// </summary>
			public DataRelationEdgeCollection()
			{}
			
			/// <summary>
			/// Adds an instance of type <see cref="DataRelationEdge"/> to the end of this 
			/// <see cref="DataRelationEdgeCollection"/>.
			/// </summary>
			/// <param name="value">
			/// The Edge to be added to the end of this EdgeCollection.
			/// </param>
			internal void Add(DataRelationEdge value)
			{
				this.List.Add(value);
			}
			
			/// <summary>
			/// Removes the first occurrence of a specific Edge from this EdgeCollection.
			/// </summary>
			/// <param name="value">
			/// The Edge value to remove from this EdgeCollection.
			/// </param>
			internal void Remove(IEdge value)
			{
				this.List.Remove(value);
			}

			#region IEdgeCollection
			/// <summary>
			/// Determines whether a specfic <see cref="DataRelationEdge"/> value is in this EdgeCollection.
			/// </summary>
			/// <param name="value">
			/// edge value to locate in this <see cref="DataRelationEdgeCollection"/>.
			/// </param>
			/// <returns>
			/// true if value is found in this collection;
			/// false otherwise.
			/// </returns>
			public bool Contains(DataRelationEdge value)
			{	
				return this.List.Contains(value);
			}
			
			bool IEdgeCollection.Contains(IEdge value)
			{
				return this.Contains((DataRelationEdge)value);
			}
			
			/// <summary>
			/// Gets or sets the Edge at the given index in this EdgeCollection.
			/// </summary>
			public DataRelationEdge this[int index]
			{
				get
				{
					return (DataRelationEdge)this.List[index];
				}
				set
				{
					this.List[index] = value;
				}
			}
			
			IEdge IEdgeCollection.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					this[index] = (DataRelationEdge)value;
				}
			}		
			#endregion
			
			#region IEdgeEnumerable
			/// <summary>
			/// Returns an enumerator that can iterate through the elements of this EdgeCollection.
			/// </summary>
			/// <returns>
			/// An object that implements System.Collections.IEnumerator.
			/// </returns>        
			public new IEdgeEnumerator GetEnumerator()
			{
				return new DataRelationEdgeEnumerator(this);
			}
			
			private class DataRelationEdgeEnumerator  : IEdgeEnumerator
			{
				private IEnumerator wrapped;

				/// <summary>
				/// Create a new enumerator on the collection
				/// </summary>
				/// <param name="collection">collection to enumerate</param>
				public DataRelationEdgeEnumerator(DataRelationEdgeCollection collection)
				{
					this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
				}

				/// <summary>
				/// The current element. 
				/// </summary>
				public DataRelationEdge Current
				{
					get
					{
						return (DataRelationEdge)this.wrapped.Current;
					}
				}
				#region IEdgeEnumerator
				IEdge IEdgeEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}				
				#endregion
				#region IEnumerator
				object IEnumerator.Current
				{
					get
					{
						return this.Current;
					}
				}

				/// <summary>
				/// Moves cursor to next element.
				/// </summary>
				/// <returns>true if current is valid, false otherwize</returns>
				public bool MoveNext()
				{
					return this.wrapped.MoveNext();
				}
	
				/// <summary>
				/// Resets the cursor to the position before the first element.
				/// </summary>
				public void Reset()
				{
					this.wrapped.Reset();
				}				
				#endregion
			}
			#endregion
		}
		#endregion

		#region DataTableVertexDataRelationEdgeCollectionDictionary
		private class DataTableVertexDataRelationEdgeCollectionDictionary :
			DictionaryBase
		{
			public DataTableVertexDataRelationEdgeCollectionDictionary()
			{}
			
			public void Add(DataTableVertex u)
			{
				Debug.Assert(u!=null);
				this.Dictionary.Add(u, new DataRelationEdgeCollection() );
			}
			
			public  bool Contains(DataTableVertex key)
			{
				return this.Dictionary.Contains(key);
			}
			
			public void Remove(DataTableVertex key)
			{
				this.Dictionary.Remove(key);
			}
			
			public IVertexEnumerable Vertices
			{
				get
				{
					return new DataTableVertexDataRelationEdgeCollectionVertexEnumerable(this.Dictionary.Keys);
				}
			}
			
			public IEdgeEnumerable Edges
			{
				get
				{
					return new DataTableVertexDataRelationEdgeCollectionEdgeEnumerable(this.Dictionary.Values);
				}			
			}
			
			public DataRelationEdgeCollection this[DataTableVertex v]
			{
				get
				{
					return (DataRelationEdgeCollection)this.Dictionary[v];
				}
			}
			
			#region Vertex Enumerable/Enumerator
			private class DataTableVertexDataRelationEdgeCollectionVertexEnumerable : 
				IVertexEnumerable
			{
				private IEnumerable en;
				public DataTableVertexDataRelationEdgeCollectionVertexEnumerable(IEnumerable en)
				{
					Debug.Assert(en!=null);
					this.en = en;
				}
				public IVertexEnumerator GetEnumerator()
				{
					return new DataTableVertexDataRelationEdgeCollectionVertexEnumerator(en);
				}
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				
				#region Enumerator
				private class DataTableVertexDataRelationEdgeCollectionVertexEnumerator :
					IVertexEnumerator
				{
					private IEnumerator en;
					public DataTableVertexDataRelationEdgeCollectionVertexEnumerator(IEnumerable col)
					{
						Debug.Assert(col!=null);
						this.en = col.GetEnumerator();
					}
					
					public DataTableVertex Current
					{
						get
						{
							return (DataTableVertex)this.en.Current;
						}
					}
					IVertex IVertexEnumerator.Current
					{
						get
						{
							return this.Current;
						}						
					}
					Object IEnumerator.Current
					{
						get
						{
							return this.Current;
						}						
					}
					public void Reset()
					{
						this.en.Reset();
					}
					public bool MoveNext()
					{
						return this.en.MoveNext();
					}
				}
				#endregion
			}
			#endregion
			
			#region Edge Enumerable/Enumerator
			private class DataTableVertexDataRelationEdgeCollectionEdgeEnumerable : 
				IEdgeEnumerable
			{
				private IEnumerable en;
				public DataTableVertexDataRelationEdgeCollectionEdgeEnumerable(IEnumerable en)
				{
					Debug.Assert(en!=null);
					this.en = en;
				}
				public IEdgeEnumerator GetEnumerator()
				{
					return new DataTableVertexDataRelationEdgeCollectionEdgeEnumerator(en);
				}
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}
				
				#region Edge Enumerator
				private class DataTableVertexDataRelationEdgeCollectionEdgeEnumerator :
					IEdgeEnumerator
				{
					private IEnumerator edges;
					private IEdgeEnumerator edge;

					public DataTableVertexDataRelationEdgeCollectionEdgeEnumerator(IEnumerable en)
					{
						Debug.Assert(en!=null);
						this.edges = en.GetEnumerator();
						this.edge = null;
					}

					public void Reset()
					{
						this.edges.Reset();
						this.edge=null;
					}

					public bool MoveNext()
					{
						// check if first time.
						if (this.edge == null)
						{
							if (!moveNextVertex())
								return false;
						}

						// getting next valid entry
						do 
						{
							// try getting edge in the current out edge list
							if (edge.MoveNext())
								return true;

							// move to the next outedge list
							if (!moveNextVertex())
								return false;
						} 
						while(true);
					}

					public DataRelationEdge Current
					{
						get
						{
							if (this.edge == null)
								throw new InvalidOperationException();
							return (DataRelationEdge)this.edge.Current;
						}
					}
					
					IEdge IEdgeEnumerator.Current
					{
						get
						{
							return this.Current;
						}
					}
					
					Object IEnumerator.Current
					{
						get
						{
							return this.Current;
						}
					}
					
					private bool moveNextVertex()
					{
						// check if empty vertex set
						if (!this.edges.MoveNext())
						{
							this.edges=null;
							return false;
						}

						// getting enumerator
						this.edge = ((DataRelationEdgeCollection)this.edges.Current).GetEnumerator();
						return true;
					}
				}
				#endregion
			}
			#endregion			
		}
		#endregion

		#region DataTableVertexEnumerator
		private class DataTableVertexEnumerator : IVertexEnumerator
		{
			private IEnumerator en;
			public DataTableVertexEnumerator(IEnumerable enumerable)
			{
				Debug.Assert(en!=null);
				this.en = enumerable.GetEnumerator();
			}
			
			public void Reset()
			{
				this.en.Reset();
			}			
			public bool MoveNext()
			{
				return this.en.MoveNext();
			}
			public DataTableVertex Current
			{
				get
				{				
					return (DataTableVertex)this.en.Current;
				}
			}
			IVertex IVertexEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}
			Object IEnumerator.Current
			{
				get
				{
					return this.en.Current;
				}
			}
		}
		#endregion
	}
}

