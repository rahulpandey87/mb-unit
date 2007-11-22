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



using System;
using System.Collections;

namespace QuickGraph.Algorithms
{
	using QuickGraph.Concepts;
	using QuickGraph.Exceptions;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Algorithms.Visitors;
	using QuickGraph.Predicates;
	using QuickGraph.Concepts.Collections;
//	using QuickGraph.Algorithms.MatrixAlgebra;

	/// <summary>
	/// A static class with some helper methods
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class contains a number of small and usefull methods.
	/// </para>
	/// <para>
	/// All the method are thread safe.
	/// </para>
	/// </remarks>	
	public sealed class AlgoUtility
	{
		/// <summary>
		/// No constructor
		/// </summary>
		private AlgoUtility()
		{}

		/// <summary>
		/// Returns true if edge is a self edge
		/// </summary>
		/// <param name="e">edge to test</param>
		/// <returns>true if self edge</returns>
		/// <exception cref="ArgumentNullException">e is null</exception>
		public static bool IsSelfLoop(IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("e");
			return e.Source == e.Target;
		}

		/// <summary>
		/// Returns the vertex opposite to v on the edge e.
		/// </summary>
		/// <remarks>
		/// Given an edge and a vertex which must be incident to the edge, 
		/// this function returns the opposite vertex. 
		/// So if v is the source vertex, this function returns the target 
		/// vertex. I
		/// f v is the target, then this function returns the source vertex. 
		/// </remarks>
		/// <param name="e"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">e or v is null</exception>
		/// <exception cref="VertexNotConnectedByEdgeException">v is not incident to e</exception>
		public static IVertex Opposite(IEdge e, IVertex v)
		{
			if (e==null)
				throw new ArgumentNullException("e");
			if (e.Source==v)
				return e.Target;
			if (e.Target==v)
				return e.Source;
			// if no opposite, throw
			throw new VertexNotConnectedByEdgeException();
		}

		/// <summary>
		/// Checks wheter an edge belongs to the edge set
		/// </summary>
		/// <param name="g">graph containing the edge set</param>
		/// <param name="e">edge to test</param>
		/// <returns>true if e is in the graph edge set</returns>
		public static bool IsInEdgeSet(IEdgeListGraph g, IEdge e)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (e==null)
				throw new ArgumentNullException("e");			
	
			foreach(IEdge ei in g.Edges)
				if (ei==e)
					return true;
			return false;
		}

		/// <summary>
		/// Checks wheter a vertex belongs to the vertex set
		/// </summary>
		/// <param name="g">graph containing the vertex set</param>
		/// <param name="v">vertex to test</param>
		/// <returns>true if v is in the graph vertex set</returns>
		public static bool IsInVertexSet(IVertexListGraph g, IVertex v)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (v==null)
				throw new ArgumentNullException("v");	
	
			foreach(IVertex vi in g.Vertices)
				if (vi==v)
					return true;
			return false;
		}

		/// <summary>
		/// Checks wheter an edge that goes from source to target 
		/// belongs to the edge set
		/// </summary>
		/// <param name="g">graph containing the edge set</param>
		/// <param name="source">edge source</param>
		/// <param name="target">edge target</param>
		/// <returns>true if e is in the graph edge set</returns>
		public static bool IsInEdgeSet(
			IEdgeListGraph g, 
			IVertex source, 
			IVertex target)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (source==null)
				throw new ArgumentNullException("source");
			if (target==null)
				throw new ArgumentNullException("target");
	
			foreach(IEdge e in g.Edges)
			{
				if (g.IsDirected)
					if (e.Source==source && e.Target==target)
						return true;
					else
						if ((e.Source==source && e.Target==target)
						|| (e.Source==target && e.Target==source))
						return true;
			}
			return false;
		}

		/// <summary>
		/// Checks if the child vertex is a child of the parent vertex 
		/// using the predecessor map.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="child"></param>
		/// <param name="predecessors"></param>
		/// <returns></returns>
		public static bool IsChild(
			IVertex parent, 
			IVertex child,
			VertexVertexDictionary predecessors
			)
		{
			Object o = predecessors[child]; 
			if (o == null || o==child) // child is the root of the tree
				return false;
			else if (o== parent)
				return true;
			else
				return IsChild(parent,(IVertex)o,predecessors);
		}

		/// <summary>
		/// Checks if there exists a path between source and target
		/// </summary>
		/// <param name="source">source vertex</param>
		/// <param name="target">target vertex</param>
		/// <param name="g">graph</param>
		/// <returns>true if target is reachable from source</returns>
		public static bool IsReachable(
			IVertex source,
			IVertex target,
			IVertexListGraph g)
		{
			if (source==null)
				throw new ArgumentNullException("source");
			if (target==null)
				throw new ArgumentNullException("target");
			if (g==null)
				throw new ArgumentNullException("g");	
	
			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(g);
			dfs.Compute(source);
			return dfs.Colors[target]!=GraphColor.White;
		}

		/// <summary>
		/// Applies a topological sort to the graph
		/// </summary>
		/// <param name="g">graph to sort</param>
		/// <param name="vertices">sorted vertices</param>
		public static void TopologicalSort(IVertexListGraph g, IList vertices)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (vertices==null)
				throw new ArgumentNullException("vertices");
	
			vertices.Clear();	
	
			TopologicalSortAlgorithm topo = 
				new TopologicalSortAlgorithm(g,vertices);
			topo.Compute();
		}

		/// <summary>
		/// Computes the connected components.
		/// </summary>
		/// <param name="g">graph to explore</param>
		/// <param name="components">component map where results are recorded</param>
		/// <returns>number of components</returns>
		public static int ConnectedComponents(
			IVertexListGraph g, 
			VertexIntDictionary components)
		{
			ConnectedComponentsAlgorithm conn = 
				new ConnectedComponentsAlgorithm(g,components);
			return conn.Compute();
		}

		/// <summary>
		/// Computes the strong components.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Thread safe.
		/// </para>
		/// </remarks>
		/// <param name="g">graph to explore</param>
		/// <param name="components">component map where results are recorded</param>
		/// <returns>number of strong components</returns>
		public static int StrongComponents(
			IVertexListGraph g, 
			VertexIntDictionary components)
		{
			StrongComponentsAlgorithm strong = 
				new StrongComponentsAlgorithm(g,components);
			return strong.Compute();
		}

		/// <summary>
		/// Returns an enumerable collection of the leaf vertices of the graph
		/// </summary>
		/// <param name="g">graph to visit</param>
		/// <returns>enumerable of leaf vertices</returns>
		/// <remarks>
		/// <para>
		/// Thread safe.
		/// </para>
		/// </remarks>
		public static IVertexEnumerable Sinks(
			IVertexListGraph g
			)
		{
            SinkVertexPredicate leaf = new SinkVertexPredicate(g);
            return new FilteredVertexEnumerable(
				g.Vertices,
				leaf
				);
		}


		/// <summary>
		/// Returns an enumerable collection of the root vertices of the graph
		/// </summary>
		/// <param name="g">graph to visit</param>
		/// <returns>enumerable of root vertices</returns>
		/// <remarks>
		/// <para>
		/// Thread safe.
		/// </para>
		/// </remarks>
		public static IVertexEnumerable Sources(
			IBidirectionalVertexListGraph g
			)
		{
			SourceVertexPredicate root = new SourceVertexPredicate(g);
			return new FilteredVertexEnumerable(
				g.Vertices,
				root
				);
		}

		/// <summary>
		/// Computes the leaves from the <paramref name="root"/> vertex.
		/// </summary>
		/// <param name="g">graph containing the vertex</param>
		/// <param name="root">root of the tree</param>
		/// <returns>leaf vertices</returns>
		public static IVertexEnumerable Sinks(
			IVertexListGraph g,
			IVertex root
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (root==null)
				throw new ArgumentNullException("root");

			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(g);
			SinkRecorderVisitor sinks = new SinkRecorderVisitor(g);

			dfs.RegisterVertexColorizerHandlers(sinks);
			dfs.Initialize();
			dfs.Visit(root,0);

			return sinks.Sinks;
		}

		/// <summary>
		/// Checks that the graph does not have cyclies
		/// </summary>
		/// <param name="g">graph to test</param>
		/// <exception cref="ArgumentNullException">g is a null reference</exception>
		/// <exception cref="NonAcyclicGraphException">graph contains a cycle</exception>
		public static void CheckAcyclic(IVertexListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");

			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(g);
			dfs.BackEdge +=new EdgeEventHandler(dfs_BackEdge);
			dfs.Compute();
		}

		/// <summary>
		/// Used in OutEdgeTree
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal static void dfs_BackEdge(object sender, EdgeEventArgs e)
		{
			throw new NonAcyclicGraphException();	
		}

		/// <summary>
		/// Checks that the sub graph rooted at <paramref name="ref"/> does not have cyclies
		/// </summary>
		/// <param name="g">graph to test</param>
		/// <exception cref="ArgumentNullException">g is a null reference</exception>
		/// <exception cref="NonAcyclicGraphException">graph contains a cycle</exception>
		public static void CheckAcyclic(IVertexListGraph g, IVertex root)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (root==null)
				throw new ArgumentNullException("root");

			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(g);
			dfs.BackEdge +=new EdgeEventHandler(dfs_BackEdge);
			dfs.Initialize();
			dfs.Visit(root,0);
			dfs.Compute();
		}

		/// <summary>
		/// Create a collection of odd vertices
		/// </summary>
		/// <param name="g">graph to visit</param>
		/// <returns>colleciton of odd vertices</returns>
		/// <exception cref="ArgumentNullException">g is a null reference</exception>
		public static VertexCollection OddVertices(IVertexAndEdgeListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");

			VertexIntDictionary counts = new VertexIntDictionary();
			foreach(IVertex v in g.Vertices)
			{
				counts[v]=0;
			}
	
			foreach(IEdge e in g.Edges)
			{
				++counts[e.Source];
				--counts[e.Target];
			}

			VertexCollection odds= new VertexCollection();
			foreach(DictionaryEntry de in counts)
			{
				if ((int)de.Value%2!=0)
					odds.Add((IVertex)de.Key);
			}

			return odds;
		}
	}
}
