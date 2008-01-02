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
	using QuickGraph.Algorithms.Search;
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;

	/// <summary>
	/// Connected component computation
	/// </summary>
	/// <remarks>
	/// <para>
	/// The ConnectedComponentsAlgorithm functions compute the connected 
	/// components of an undirected graph using a DFS-based approach. 
	/// </para>
	/// <para>
	/// A connected component of an undirected graph is a set of vertices that 
	/// are all reachable from each other. 
	/// </para>
	/// <para>
	/// If the connected components need to be maintained while a graph is 
	/// growing the disjoint-set based approach of function 
	/// IncrementalComponentsAlgorithm is faster. 
	/// For ``static'' graphs this DFS-based approach is faster. 
	/// </para>
	/// <para>
	/// The output of the algorithm is recorded in the component 
	/// property Components, which will contain numbers giving the 
	/// component number assigned to each vertex. 
	/// </para>
	/// </remarks>
	public class ConnectedComponentsAlgorithm
	{
		private int count;
		private VertexIntDictionary components;
		private IVertexListGraph visitedGraph;

		/// <summary>
		/// Constructs a connected component algorithm.
		/// </summary>
		/// <param name="g">graph to apply the algorithm on</param>
		/// <exception cref="ArgumentNullException">g is null</exception>
		public ConnectedComponentsAlgorithm(IVertexListGraph g)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			this.visitedGraph = g;
			this.components = new VertexIntDictionary();
		}

		/// <summary>
		/// Constructs a connected component algorithm, using a component map
		/// </summary>
		/// <param name="g">graph</param>
		/// <param name="components">map where the components are recorded</param>
		/// <exception cref="ArgumentNullException">g or components are null</exception>
		public ConnectedComponentsAlgorithm(IVertexListGraph g,
			VertexIntDictionary components)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (components ==null)
				throw new ArgumentNullException("components");
			this.visitedGraph = g;
			this.components = components;
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
		/// Gets the component map
		/// </summary>
		/// <value>
		/// Component map
		/// </value>
		public VertexIntDictionary Components
		{
			get
			{
				return this.components;
			}
		}

		/// <summary>
		/// Gets the connected components count
		/// </summary>
		/// <value>
		/// Connected component count
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
		private void StartVertex(Object sender, VertexEventArgs args)
		{
			++this.count;
		}

		/// <summary>
		/// Used internally
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void DiscoverVertex(Object sender, VertexEventArgs args)
		{
			Components[args.Vertex]=this.count;
		}

		/// <summary>		
		/// Executes the algorithm
		/// </summary>
		/// <returns>The total number of components is the return value of the function</returns>
		public int Compute(IVertex startVertex)
		{
            if (startVertex == null)
                throw new ArgumentNullException("startVertex");

            this.count = -1;
			this.components.Clear();

			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(VisitedGraph);
			dfs.StartVertex += new VertexEventHandler(this.StartVertex);
			dfs.DiscoverVertex += new VertexEventHandler(this.DiscoverVertex);

            dfs.Compute(startVertex);

            return ++this.count;
		}

        public int Compute()
        {
            this.count = -1;
			this.components.Clear();
            if (this.VisitedGraph.VerticesEmpty)
                return ++this.count;

            return this.Compute(QuickGraph.Concepts.Traversals.Traversal.FirstVertex(this.VisitedGraph));
        }
    }
}
