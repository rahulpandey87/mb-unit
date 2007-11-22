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

	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Providers;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts.MutableTraversals;
	using QuickGraph.Collections;
	using QuickGraph.Exceptions;
	using QuickGraph.Predicates;

	/// <summary>
	/// A mutable bidirectional graph implemetation
	/// </summary>
	/// <remarks>
	/// <seealso cref="AdjacencyGraph"/>
	/// <seealso cref="IBidirectionalGraph"/>
	/// <seealso cref="IMutableBidirectionalGraph"/>
	/// </remarks>
	public class BidirectionalGraph :
		AdjacencyGraph,
		IFilteredBidirectionalGraph,
		IMutableBidirectionalGraph,
		IBidirectionalVertexAndEdgeListGraph,
		IMutableBidirectionalVertexAndEdgeListGraph
	{
		private VertexEdgesDictionary m_VertexInEdges;

		/// <summary>
		/// Builds a new empty graph with default vertex and edge providers
		/// </summary>
		public BidirectionalGraph(bool allowParallelEdges)
			: base(allowParallelEdges)
		{
			m_VertexInEdges = new VertexEdgesDictionary();
		}

		/// <summary>
		/// Builds a new empty graph
		/// </summary>
		public BidirectionalGraph(
			IVertexProvider vertexProvider,
			IEdgeProvider edgeProvider,
			bool allowParallelEdges
			)
			: base(vertexProvider,edgeProvider,allowParallelEdges)
		{
			m_VertexInEdges = new VertexEdgesDictionary();
		}
		/// <summary>
		/// Vertex Out edges dictionary
		/// </summary>
		protected VertexEdgesDictionary VertexInEdges
		{
			get
			{
				return m_VertexInEdges;
			}
		}

		/// <summary>
		/// Remove all of the edges and vertices from the graph.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			VertexInEdges.Clear();
		}

		/// <summary>
		/// Add a new vertex to the graph and returns it.
		/// 
		/// Complexity: 1 insertion.
		/// </summary>
		/// <returns>Create vertex</returns>
		public override IVertex AddVertex()
		{
			IVertex v = base.AddVertex();
			VertexInEdges.Add(v, new EdgeCollection());

			return v;
		}

		/// <summary>
		/// Adds a new vertex to the graph.
		/// </summary>
		/// <param name="v"></param>
		public override void AddVertex(IVertex v)
		{
			base.AddVertex(v);
			VertexInEdges.Add(v, new EdgeCollection());
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
		public override IEdge AddEdge(
			IVertex source,
			IVertex target
			)
		{
			// look for the vertex in the list
			if (!VertexInEdges.ContainsKey(source))
				throw new VertexNotFoundException("Could not find source vertex");
			if (!VertexInEdges.ContainsKey(target))
				throw new VertexNotFoundException("Could not find target vertex");

			// create edge
			IEdge e = base.AddEdge(source,target);
			VertexInEdges[target].Add(e);

			return e;
		}

		/// <summary>
		/// Adds a new edge to the graph
		/// </summary>
		/// <param name="e"></param>
		public override void AddEdge(IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("edge");
			if (!VertexInEdges.ContainsKey(e.Source))
				throw new VertexNotFoundException("Could not find source vertex");
			if (!VertexInEdges.ContainsKey(e.Target))
				throw new VertexNotFoundException("Could not find target vertex");

			// create edge
			base.AddEdge(e);
			VertexInEdges[e.Target].Add(e);
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
			return this.VertexInEdges[v].Count==0;
		}

		/// <summary>
		/// Returns the number of in-degree edges of v
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public int InDegree(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			return VertexInEdges[v].Count;
		}

		/// <summary>
		/// Returns an iterable collection over the in-edge connected to v
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public EdgeCollection InEdges(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");
			return VertexInEdges[v];
		}

		/// <summary>
		/// Incidence graph implementation
		/// </summary>
		IEdgeEnumerable IBidirectionalGraph.InEdges(IVertex v)
		{
			return this.InEdges(v);
		}

		/// <summary>
		/// Returns the first in-edge that matches the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep">Edge predicate</param>
		/// <returns>null if not found, otherwize the first Edge that
		/// matches the predicate.</returns>
		/// <exception cref="ArgumentNullException">v or ep is null</exception>
		public IEdge SelectSingleInEdge(IVertex v, IEdgePredicate ep)
		{
			if (ep==null)
				throw new ArgumentNullException("edge predicate");
			
			foreach(IEdge e in SelectInEdges(v,ep))
				return e;

			return null;
		}

		/// <summary>
		/// Returns the collection of in-edges that matches the predicate
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep">Edge predicate</param>
		/// <returns>enumerable colleciton of vertices that matches the 
		/// criteron</returns>
		/// <exception cref="ArgumentNullException">v or ep is null</exception>
		public FilteredEdgeEnumerable SelectInEdges(IVertex v, IEdgePredicate ep)
		{
			if (v==null)
				throw new ArgumentNullException("vertex");
			if (ep==null)
				throw new ArgumentNullException("edge predicate");
			
			return new FilteredEdgeEnumerable(InEdges(v),ep);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v"></param>
		/// <param name="ep"></param>
		/// <returns></returns>
		IEdgeEnumerable IFilteredBidirectionalGraph.SelectInEdges(IVertex v, IEdgePredicate ep)
		{
			return this.SelectInEdges(v,ep);
		}

		/// <summary>
		/// Removes the vertex from the graph.
		/// </summary>
		/// <param name="v">vertex to remove</param>
		/// <exception cref="ArgumentNullException">v is null</exception>
		public override void RemoveVertex(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("vertex");
			base.RemoveVertex(v);
			// removing vertex
			VertexInEdges.Remove(v);
		}

		/// <summary>
		/// Remove all edges to and from vertex u from the graph.
		/// </summary>
		/// <param name="v"></param>
		public override void ClearVertex(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("vertex");

			base.ClearVertex(v);
			VertexInEdges[v].Clear();
		}

		/// <summary>
		/// Removes an edge from the graph.
		/// 
		/// Complexity: 2 edges removed from the vertex edge list + 1 edge
		/// removed from the edge list.
		/// </summary>
		/// <param name="e">edge to remove</param>
		/// <exception cref="ArgumentNullException">e is null</exception>
		public override void RemoveEdge(IEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("edge");
			base.RemoveEdge(e);
			EdgeCollection inEdges = VertexInEdges[e.Target];
			if (inEdges==null || !inEdges.Contains(e))
				throw new EdgeNotFoundException();
			inEdges.Remove(e);
		}

		/// <summary>
		/// Remove the edge (u,v) from the graph. 
		/// If the graph allows parallel edges this remove all occurrences of 
		/// (u,v).
		/// </summary>
		/// <param name="u">source vertex</param>
		/// <param name="v">target vertex</param>
		public override void RemoveEdge(IVertex u, IVertex v)
		{
			if (u == null)
				throw new ArgumentNullException("source vertex");
			if (v == null)
				throw new ArgumentNullException("targetvertex");

			EdgeCollection outEdges = VertexOutEdges[u];
			EdgeCollection inEdges = VertexInEdges[v];
			// marking edges to remove
			EdgeCollection removedEdges = new EdgeCollection();
			foreach(IEdge e in outEdges)
			{
				if (e.Target == v)
					removedEdges.Add(e);
			}
			foreach(IEdge e in inEdges)
			{
				if (e.Source == u)
					removedEdges.Add(e);
			}

			//removing edges
			foreach(IEdge e in removedEdges)
				RemoveEdge(e);
		}

		/// <summary>
		/// Remove all the out-edges of vertex u for which the predicate pred 
		/// returns true.
		/// </summary>
		/// <param name="u">vertex</param>
		/// <param name="pred">edge predicate</param>
		public void RemoveInEdgeIf(IVertex u, IEdgePredicate pred)
		{
			if (u==null)
				throw new ArgumentNullException("vertex u");
			if (pred == null)
				throw new ArgumentNullException("predicate");

			EdgeCollection edges = VertexInEdges[u];
			EdgeCollection removedEdges = new EdgeCollection();
			foreach(IEdge e in edges)
				if (pred.Test(e))
					removedEdges.Add(e);

			foreach(IEdge e in removedEdges)
				RemoveEdge(e);
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
		/// Returns the number of in-edges plus out-edges.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public int Degree(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("v");

			return VertexInEdges[v].Count + VertexOutEdges[v].Count;
		}
	}
}
