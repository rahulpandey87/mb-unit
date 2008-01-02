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



namespace QuickGraph.Algorithms.Visitors
{
	using System;
	using System.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Visitors;

	/// <summary>
	/// Visitor that computes the vertices predecessors.
	/// </summary>
	/// <remarks>
	///  The visitor applies to any algorithm that implements the 
	///  <seealso cref="QuickGraph.Concepts.Algorithms.IPredecessorRecorderAlgorithm"/> model.
	///  </remarks>
	///  <example>
	///  This sample shows how to use the find the predecessor map using a
	///  <seealso cref="Search.DepthFirstSearchAlgorithm"/>:
	///  <code>
	///  Graph g = ...;
	///  
	///  // creating dfs algorithm
	///  DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(g);
	///  
	///  // creating predecessor visitor
	///  PredecessorVisitor pred = new PredecessorVisitor();
	///  
	///  // registering event handlers
	///  pred.RegisterHandlers(dfs);
	///  
	///  //executing...
	///  dfs.Compute();
	///  
	///  // pred.Predecessors now contains the map of predecessors.
	///  </code>
	///  </example>
	public class PredecessorRecorderVisitor :
		IPredecessorRecorderVisitor
	{
		private VertexEdgeDictionary predecessors;
		private VertexCollection endPathVertices;

		/// <summary>
		/// Default constructor
		/// </summary>
		public PredecessorRecorderVisitor()
		{
			this.predecessors = new VertexEdgeDictionary();
			this.endPathVertices = new VertexCollection();
		}

		/// <summary>
		/// Constructor, uses the given predecessor map.
		/// </summary>
		/// <param name="predecessors">Predecessor map</param>
		/// <exception cref="ArgumentNullException">
		/// predecessors is null
		/// </exception>
		public PredecessorRecorderVisitor(VertexEdgeDictionary predecessors)
		{
			if (predecessors == null)
				throw new ArgumentNullException("predecessors");
			this.predecessors = predecessors;
			this.endPathVertices = new VertexCollection();
		}

		/// <summary>
		/// Vertex Edge predecessor map.
		/// </summary>
		public VertexEdgeDictionary Predecessors
		{
			get
			{
				return predecessors;
			}
		}

		/// <summary>
		/// End of path vertices
		/// </summary>
		public VertexCollection EndPathVertices
		{
			get
			{
				return endPathVertices;
			}
		}

		/// <summary>
		/// Returns the path leading to the vertex v.
		/// </summary>
		/// <param name="v">end of the path</param>
		/// <returns>path leading to v</returns>
		public EdgeCollection Path(IVertex v)
		{
			EdgeCollection path = new EdgeCollection();

			IVertex vc = v;
			while (Predecessors.Contains(v))
			{
				IEdge e = Predecessors[v];
				path.Insert(0,e);
				v=e.Source;
			}

			return path;
		}

		/// <summary>
		/// Returns the minimal set of path from the entry point that
		/// executes all actions
		/// </summary>
		/// <returns></returns>
		public EdgeCollectionCollection AllPaths()
		{
			EdgeCollectionCollection es = new EdgeCollectionCollection();

			foreach(IVertex v in EndPathVertices)
				es.Add( Path( v ) );

			return es;
		}

		/// <summary>
		/// Let e = (u,v), p[v]=u
		/// </summary>
		public void TreeEdge(Object sender, EdgeEventArgs args)
		{
			Predecessors[args.Edge.Target]=args.Edge;
		}

		/// <summary>
		/// Records end of path vertex
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FinishVertex(Object sender, VertexEventArgs args)
		{
			foreach(DictionaryEntry de in Predecessors)
			{
				IEdge e = (IEdge)de.Value;
				if (e.Source==args.Vertex)
					return;
			}
			
//			if (!Predecessors.Contains(args.Vertex))
				EndPathVertices.Add(args.Vertex);
		}
	}
}
