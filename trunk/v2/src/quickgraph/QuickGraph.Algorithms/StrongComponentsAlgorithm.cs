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
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux


using System;

namespace QuickGraph.Algorithms
{
	using System.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms.Search;

	/// <summary>
	/// Computes the graph strong components.
	/// </summary>
	/// <remarks>
	/// This class compute the strongly connected 
	/// components of a directed graph using Tarjan's algorithm based on DFS.
	/// </remarks>
	public class StrongComponentsAlgorithm
	{
		private IVertexListGraph visitedGraph;
		private VertexIntDictionary components;
		private VertexIntDictionary discoverTimes;
		private VertexVertexDictionary roots;
		private Stack stack;
		int count;
		int dfsTime;

		/// <summary>
		/// Construct a strong component algorithm
		/// </summary>
		/// <param name="g">graph to apply algorithm on</param>
		/// <exception cref="ArgumentNullException">graph is null</exception>
		public StrongComponentsAlgorithm(IVertexListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			this.visitedGraph = g;
			this.components = new VertexIntDictionary();
			this.roots = new VertexVertexDictionary();
			this.discoverTimes = new VertexIntDictionary();
			this.stack = new Stack();
			this.count = 0;
			this.dfsTime = 0;
		}

		/// <summary>
		/// Construct a strong component algorithm
		/// </summary>
		/// <param name="g">graph to apply algorithm on</param>
		/// <param name="components">component map to record results</param>
		/// <exception cref="ArgumentNullException">graph is null</exception>
		public StrongComponentsAlgorithm(
			IVertexListGraph g,
			VertexIntDictionary components)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (components==null)
				throw new ArgumentNullException("components");

			this.visitedGraph = g;
			this.components = components;
			this.roots = new VertexVertexDictionary();
			this.discoverTimes = new VertexIntDictionary();
			this.stack = new Stack();
			this.count = 0;
			this.dfsTime = 0;
		}

		/// <summary>
		/// Visited graph
		/// </summary>
		public IVertexListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		/// <summary>
		/// Component map
		/// </summary>
		public VertexIntDictionary Components
		{
			get
			{
				return this.components;
			}
		}

		/// <summary>
		/// Root map
		/// </summary>
		public VertexVertexDictionary Roots
		{
			get
			{
				return this.roots;
			}
		}

		/// <summary>
		/// Vertex discory times
		/// </summary>
		public VertexIntDictionary DiscoverTimes
		{
			get
			{
				return this.discoverTimes;
			}
		}

		/// <summary>
		/// Gets the number of strongly connected components in the graph
		/// </summary>
		/// <value>
		/// Number of strongly connected components
		/// </value>
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		/// <summary>
		/// Used internally
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void DiscoverVertex(Object sender, VertexEventArgs args)
		{
			IVertex v = args.Vertex;

			this.Roots[v]=v;
			this.Components[v]=int.MaxValue;
			this.DiscoverTimes[v]=dfsTime++;
			this.stack.Push(v);
		}

		/// <summary>
		/// Used internally
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void FinishVertex(Object sender, VertexEventArgs args)
		{
			IVertex v = args.Vertex;
			foreach(IEdge e in VisitedGraph.OutEdges(v))
			{
				IVertex w = e.Target;

				if (this.Components[w] == int.MaxValue)
					this.Roots[v]=MinDiscoverTime(this.Roots[v], this.Roots[w]);
			}

			if (Roots[v] == v) 
			{
				IVertex w=null;
				do 
				{
					w = (IVertex)this.stack.Peek(); 
					this.stack.Pop();
					this.Components[w]=count;
				} 
				while (w != v);
				++count;
			}	
		}

		internal IVertex MinDiscoverTime(IVertex u, IVertex v)
		{
			if (this.DiscoverTimes[u]<this.DiscoverTimes[v])
				return u;
			else
				return v;
		}

		/// <summary>
		/// Executes the algorithm
		/// </summary>
		/// <remarks>
		/// The output of the algorithm is recorded in the component property 
		/// Components, which will contain numbers giving the component ID 
		/// assigned to each vertex. 
		/// </remarks>
		/// <returns>The number of components is the return value of the function.</returns>
		public int Compute()
		{
			this.Components.Clear();
			this.Roots.Clear();
			this.DiscoverTimes.Clear();
			count = 0;
			dfsTime = 0;

			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(VisitedGraph);
			dfs.DiscoverVertex += new VertexEventHandler(this.DiscoverVertex);
			dfs.FinishVertex += new VertexEventHandler(this.FinishVertex);

			dfs.Compute();

			return count;
		}
	}
}
