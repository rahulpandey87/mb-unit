// QuickGraph Library 
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		QuickGraph Library HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux


namespace QuickGraph.Representations
{
	using System;
	using System.Collections;
	using System.Runtime.Serialization;

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
	/// A mutable incidence graph implemetation
	/// </summary>
	/// <remarks>
	/// <seealso cref="IVertexMutableGraph"/>
	/// <seealso cref="IMutableIncidenceGraph"/>
	/// </remarks>
	public class AdjacencyGraph :
		IFilteredVertexAndEdgeListGraph
		,IFilteredIncidenceGraph
		,IMutableEdgeListGraph
		,IEdgeMutableGraph
		,IMutableIncidenceGraph
		,IEdgeListAndIncidenceGraph
		,ISerializableVertexAndEdgeListGraph
		,IMutableVertexAndEdgeListGraph
		,IAdjacencyGraph
		,IIndexedVertexListGraph
	{
		private IVertexProvider vertexProvider;
		private IEdgeProvider edgeProvider;
		private VertexEdgesDictionary vertexOutEdges;
		private bool allowParallelEdges;

		/// <summary>
		/// Builds a new empty directed graph with default vertex and edge
		/// provider.
		/// </summary>
		/// <remarks>
		/// </remarks>
		public AdjacencyGraph()
		{
			this.vertexProvider = new QuickGraph.Providers.VertexProvider();
			this.edgeProvider = new QuickGraph.Providers.EdgeProvider();
			this.allowParallelEdges = true;
			this.vertexOutEdges = new VertexEdgesDictionary();
		}

		/// <summary>
		/// Builds a new empty directed graph with default vertex and edge
		/// provider.
		/// </summary>
		/// <param name="allowParallelEdges">true if parallel edges are allowed</param>
		public AdjacencyGraph(bool allowParallelEdges)
		{
			this.vertexProvider = new QuickGraph.Providers.VertexProvider();
			this.edgeProvider = new QuickGraph.Providers.EdgeProvider();
			this.allowParallelEdges = allowParallelEdges;
			this.vertexOutEdges = new VertexEdgesDictionary();
		}

		/// <summary>
		/// Builds a new empty directed graph with custom providers
		/// </summary>	
		/// <param name="allowParallelEdges">true if the graph allows
		/// multiple edges</param>	
		/// <param name="edgeProvider">custom edge provider</param>
		/// <param name="vertexProvider">custom vertex provider</param>
		/// <exception cref="ArgumentNullException">
		/// vertexProvider or edgeProvider is a null reference
		/// </exception>
		public AdjacencyGraph(
			IVertexProvider vertexProvider,
			IEdgeProvider edgeProvider,
			bool allowParallelEdges
			)
		{
			if (vertexProvider == null)
				throw new ArgumentNullException("vertex provider");
			if (edgeProvider == null)
				throw new ArgumentNullException("edge provider");

			this.vertexProvider = vertexProvider;
			this.edgeProvider = edgeProvider;
			this.allowParallelEdges = allowParallelEdges;
			this.vertexOutEdges = new VertexEdgesDictionary();
		}

		/// <summary>
		/// Gets a value indicating if the graph is directed.
		/// </summary>
		/// <value>
		/// true if the graph is directed, false if undirected.
		/// </value>
		/// <remarks>
		/// <seealso cref="IGraph"/>
		/// </remarks>
		public bool IsDirected
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets a value indicating if the graph allows parralell edges.
		/// </summary>
		/// <value>
		/// true if the graph is a multi-graph, false otherwise
		/// </value>
		/// <remarks>
		/// <seealso cref="IGraph"/>
		/// </remarks>
		public bool AllowParallelEdges
		{
			get
			{
				return IsDirected && this.allowParallelEdges;
			}
		}

		/// <summary>
		/// Vertex Out edges dictionary
		/// </summary>
		/// <value>
		/// Dictionary of <see cref="IVertex"/> to out edge collection.
		/// </value>
		protected VertexEdgesDictionary VertexOutEdges
		{
			get
			{
				return this.vertexOutEdges;
			}
		}

		/// <summary>
		/// Gets the <see cref="IVertex"/> provider
		/// </summary>
		/// <value>
		/// <see cref="IVertex"/> provider
		/// </value>
		public IVertexProvider VertexProvider
		{
			get
			{
				return this.vertexProvider;
			}
		}

		/// <summary>
		/// Gets the <see cref="IEdge"/> provider
		/// </summary>
		/// <value>
		/// <see cref="IEdge"/> provider
		/// </value>
		public IEdgeProvider EdgeProvider
		{
			get
			{
				return this.edgeProvider;
			}
		}

		/// <summary>
		/// Remove all of the edges and vertices from the graph.
		/// </summary>
		public virtual void Clear()
		{
			VertexOutEdges.Clear();
		}

		/// <summary>
		/// Add a new vertex to the graph and returns it.
		/// 
		/// Complexity: 1 insertion.
		/// </summary>
		/// <returns>Create vertex</returns>
		public virtual IVertex AddVertex()
		{
			IVertex v = VertexProvider.ProvideVertex();
			VertexOutEdges.Add(v, new EdgeCollection());
			return v;
		}

		/// <summary>
		/// Add a new vertex to the graph and returns it.
		/// 
		/// Complexity: 1 insertion.
		/// </summary>
		/// <returns>Create vertex</returns>
		public virtual void AddVertex(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("vertex");
			if (v.GetType() != VertexProvider.VertexType)
				throw new ArgumentNullException("vertex type not valid");
			if (VertexOutEdges.Contains(v))
				throw new ArgumentException("vertex already in graph");

			VertexProvider.UpdateVertex(v);
			VertexOutEdges.Add(v, new EdgeCollection());
		}

		/// <summary>
		/// Add a new vertex from source to target
		///  
		/// Complexity: 2 search + 1 insertion
		/// </summary>
		/// <param name="source">Source vertex</param>
		/// <param name="target">Target vertex</param>
		/// <returns>Created Edge</returns>
		/// <exception cref="ArgumentNullException">source or target is null</exception>
		/// <exception cref="Exception">source or target are not part of the graph</exception>
		public virtual IEdge AddEdge(
			IVertex source,
			IVertex target
			)
		{
			// look for the vertex in the list
			if (!VertexOutEdges.ContainsKey(source))
				throw new VertexNotFoundException("Could not find source vertex");
			if (!VertexOutEdges.ContainsKey(target))
				throw new VertexNotFoundException("Could not find target vertex");

			// if parralel edges are not allowed check if already in the graph
			if (!this.AllowParallelEdges)
			{
				if (ContainsEdge(source,target))
					throw new Exception("Parallel edge not allowed");
			}

			// create edge
			IEdge e = EdgeProvider.ProvideEdge(source,target);
			VertexOutEdges[source].Add(e);

			return e;
		}

		/// <summary>
		/// Used for serialization. Not for private use.
		/// </summary>
		/// <param name="e">edge to add.</param>
		public virtual void AddEdge(IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("vertex");
			if (e.GetType() != EdgeProvider.EdgeType)
				throw new ArgumentNullException("vertex type not valid");
			if (!VertexOutEdges.ContainsKey(e.Source))
				throw new VertexNotFoundException("Could not find source vertex");
			if (!VertexOutEdges.ContainsKey(e.Target))
				throw new VertexNotFoundException("Could not find target vertex");

			// if parralel edges are not allowed check if already in the graph
			if (!this.AllowParallelEdges)
			{
				if (ContainsEdge(e.Source,e.Target))
					throw new ArgumentException("graph does not allow duplicate edges");
			}
			// create edge
			EdgeProvider.UpdateEdge(e);
			VertexOutEdges[e.Source].Add(e);
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
			if (v == null)
				throw new ArgumentNullException("v");
			return VertexOutEdges[v].Count==0;
		}

		/// <summary>
		/// Returns the number of out-degree edges of v
		/// </summary>
		/// <param name="v">vertex</param>
		/// <returns>number of out-edges of the <see cref="IVertex"/> v</returns>
		public int OutDegree(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			EdgeCollection ec=VertexOutEdges[v];
			if (ec==null)
				throw new VertexNotFoundException(v.ToString());
			return ec.Count;
		}

		/// <summary>
		/// Returns an iterable collection over the edge connected to v
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public EdgeCollection OutEdges(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");

			EdgeCollection ec=VertexOutEdges[v];
			if (ec==null)
				throw new VertexNotFoundException(v.ToString());
			return ec;
		}

		/// <summary>
		/// Incidence graph implementation
		/// </summary>
		IEdgeEnumerable IImplicitGraph.OutEdges(IVertex v)
		{
			return this.OutEdges(v);
		}

		IEdgeCollection IIndexedIncidenceGraph.OutEdges(IVertex v)
		{
			return this.OutEdges(v);
		}

		/// <summary>
		/// Returns the first out-edge that matches the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep">Edge predicate</param>
		/// <returns>null if not found, otherwize the first Edge that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">v or ep is null</exception>
		public IEdge SelectSingleOutEdge(IVertex v, IEdgePredicate ep)
		{
			if (ep==null)
				throw new ArgumentNullException("edge predicate");
			
			foreach(IEdge e in SelectOutEdges(v,ep))
				return e;

			return null;
		}

		/// <summary>
		/// Returns the collection of out-edges that matches the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep">Edge predicate</param>
		/// <returns>enumerable colleciton of vertices that matches the 
		/// criteron</returns>
		/// <exception cref="ArgumentNullException">v or ep is null</exception>
		public FilteredEdgeEnumerable SelectOutEdges(IVertex v, IEdgePredicate ep)
		{
			if (v==null)
				throw new ArgumentNullException("vertex");
			if (ep==null)
				throw new ArgumentNullException("edge predicate");
			
			return new FilteredEdgeEnumerable(OutEdges(v),ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep"></param>
		/// <returns></returns>
		IEdgeEnumerable IFilteredIncidenceGraph.SelectOutEdges(IVertex v, IEdgePredicate ep)
		{
			return this.SelectOutEdges(v,ep);
		}

		/// <summary>
		/// Removes the vertex from the graph.
		/// </summary>
		/// <param name="v">vertex to remove</param>
		/// <exception cref="ArgumentNullException">v is null</exception>
		public virtual void RemoveVertex(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("vertex");
			if (!ContainsVertex(v))
				throw new VertexNotFoundException("v");

			ClearVertex(v);

			// removing vertex
			VertexOutEdges.Remove(v);
		}

		/// <summary>
		/// Remove all edges to and from vertex u from the graph.
		/// </summary>
		/// <param name="v"></param>
		public virtual void ClearVertex(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("vertex");

			// removing edges touching v
			RemoveEdgeIf(new IsAdjacentEdgePredicate(v));

			// removing edges
			VertexOutEdges[v].Clear();
		}

		/// <summary>
		/// Removes an edge from the graph.
		/// 
		/// Complexity: 2 edges removed from the vertex edge list + 1 edge
		/// removed from the edge list.
		/// </summary>
		/// <param name="e">edge to remove</param>
		/// <exception cref="ArgumentNullException">e is null</exception>
		public virtual void RemoveEdge(IEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("edge");
			// removing edge from vertices
			EdgeCollection outEdges = VertexOutEdges[e.Source];
			if (outEdges==null || !outEdges.Contains(e))
				throw new EdgeNotFoundException();
			outEdges.Remove(e);
		}

		/// <summary>
		/// Remove the edge (u,v) from the graph. 
		/// If the graph allows parallel edges this remove all occurrences of 
		/// (u,v).
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <param name="v">target vertex</param>
		public virtual void RemoveEdge(IVertex u, IVertex v)
		{
			if (u == null)
				throw new ArgumentNullException("source vertex");
			if (v == null)
				throw new ArgumentNullException("targetvertex");

			EdgeCollection edges = VertexOutEdges[u];
			if (edges==null)
				throw new EdgeNotFoundException();
			// marking edges to remove
			EdgeCollection removedEdges = new EdgeCollection();
			foreach(IEdge e in edges)
			{
				if (e.Target == v)
					removedEdges.Add(e);
			}
			//removing edges
			foreach(IEdge e in removedEdges)
				edges.Remove(e);
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
			EdgeCollection removedEdges = new EdgeCollection();
			foreach(IEdge e in Edges)
			{
				if (pred.Test(e))
					removedEdges.Add(e);
			}

			// removing edges
			foreach(IEdge e in removedEdges)
				RemoveEdge(e);
		}

		/// <summary>
		/// Remove all the out-edges of vertex u for which the predicate pred 
		/// returns true.
		/// </summary>
		/// <param name="u">vertex</param>
		/// <param name="pred">edge predicate</param>
		public virtual void RemoveOutEdgeIf(IVertex u, IEdgePredicate pred)
		{
			if (u==null)
				throw new ArgumentNullException("vertex u");
			if (pred == null)
				throw new ArgumentNullException("predicate");

			EdgeCollection edges = VertexOutEdges[u];
			EdgeCollection removedEdges = new EdgeCollection();
			foreach(IEdge e in edges)
				if (pred.Test(e))
					removedEdges.Add(e);

			foreach(IEdge e in removedEdges)
				RemoveEdge(e);
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
				return VertexOutEdges.Count==0;
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
				return VertexOutEdges.Count;
			}
		}

		/// <summary>
		/// Enumerable collection of vertices.
		/// </summary>
		public VertexEnumerable Vertices
		{
			get
			{
				return new VertexEnumerable(VertexOutEdges.Keys);
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
		/// Returns the first vertex that matches the predicate
		/// </summary>
		/// <param name="vp">vertex predicate</param>
		/// <returns>null if not found, otherwize the first vertex that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">vp is null</exception>
		public IVertex SelectSingleVertex(IVertexPredicate vp)
		{
			if (vp == null)
				throw new ArgumentNullException("vertex predicate");

			foreach(IVertex v in SelectVertices(vp))
				return v;
			return null;
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

		/// <summary>
		/// Tests if a vertex is part of the graph
		/// </summary>
		/// <param name="v">Vertex to test</param>
		/// <returns>true if is part of the graph, false otherwize</returns>
		public bool ContainsVertex(IVertex v)
		{
			return VertexOutEdges.Contains(v);
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
				foreach(DictionaryEntry d in VertexOutEdges)
				{
					n+=((EdgeCollection)d.Value).Count;
				}
				return n;
			}
		}

		/// <summary>
		/// Enumerable collection of edges.
		/// </summary>
		public VertexEdgesEnumerable Edges
		{
			get
			{
				return new VertexEdgesEnumerable(VertexOutEdges);
			}
		}

		/// <summary>
		/// IEdgeListGraph implementation
		/// </summary>
		IEdgeEnumerable IEdgeListGraph.Edges
		{
			get
			{
				return this.Edges;
			}
		}

		/// <summary>
		/// Returns the first Edge that matches the predicate
		/// </summary>
		/// <param name="ep">Edge predicate</param>
		/// <returns>null if not found, otherwize the first Edge that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">ep is null</exception>
		public IEdge SelectSingleEdge(IEdgePredicate ep)
		{
			if (ep == null)
				throw new ArgumentNullException("edge predicate");
			foreach(IEdge e in SelectEdges(ep))
				return e;
			return null;
		}

		/// <summary>
		/// Returns the collection of edges that matches the predicate
		/// </summary>
		/// <param name="ep">Edge predicate</param>
		/// <returns>enumerable colleciton of vertices that matches the 
		/// criteron</returns>
		/// <exception cref="ArgumentNullException">ep is null</exception>
		public FilteredEdgeEnumerable SelectEdges(IEdgePredicate ep)
		{
			if (ep == null)
				throw new ArgumentNullException("edge predicate");

			return new FilteredEdgeEnumerable(Edges,ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ep"></param>
		/// <returns></returns>
		IEdgeEnumerable IFilteredEdgeListGraph.SelectEdges(IEdgePredicate ep)
		{
			return this.SelectEdges(ep);
		}

		/// <summary>
		/// Tests if a edge is part of the graph
		/// </summary>
		/// <param name="e">Edge to test</param>
		/// <returns>true if is part of the graph, false otherwize</returns>
		public bool ContainsEdge(IEdge e)
		{
			foreach(DictionaryEntry di in VertexOutEdges)
			{
				EdgeCollection es = (EdgeCollection)di.Value;
				if (es.Contains(e))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Test if an edge (u,v) is part of the graph
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <param name="v">target vertex</param>
		/// <returns>true if part of the graph</returns>
		public bool ContainsEdge(IVertex u, IVertex v)
		{
			if (!this.ContainsVertex(u))
				return false;
			if (!this.ContainsVertex(v))
				return false;
			// try to find the edge
			foreach(IEdge e in this.OutEdges(u))
			{
				if (e.Target == v)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Gets an enumerable collection of adjacent vertices
		/// </summary>
		/// <param name="v"></param>
		/// <returns>Enumerable collection of adjacent vertices</returns>
		public IVertexEnumerable AdjacentVertices(IVertex v)
		{
			return new TargetVertexEnumerable(OutEdges(v));
		}
	}
}
