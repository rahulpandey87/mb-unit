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



namespace QuickGraph.Algorithms.Visitors
{
	using System;
	using System.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Visitors;

	/// <summary>
	/// Visitor that computes the edge predecessors.
	/// </summary>
	public class EdgePredecessorRecorderVisitor :
		IEdgePredecessorRecorderVisitor
	{
		private EdgeEdgeDictionary edgePredecessors;
		private EdgeCollection endPathEdges;

		/// <summary>
		/// Default constructor
		/// </summary>
		public EdgePredecessorRecorderVisitor()
		{
			edgePredecessors = new EdgeEdgeDictionary();
			endPathEdges = new EdgeCollection();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="edgePredecessors"></param>
		/// <param name="endPathEdges"></param>
		public EdgePredecessorRecorderVisitor(
			EdgeEdgeDictionary edgePredecessors,
			EdgeCollection endPathEdges
			)
		{
			if (edgePredecessors==null)
				throw new ArgumentNullException("edgePredecessors");
			if (endPathEdges==null)
				throw new ArgumentNullException("endPathEdges");

			this.edgePredecessors = edgePredecessors;
			this.endPathEdges = endPathEdges;
		}

		/// <summary>
		/// Vertex Edge predecessor map.
		/// </summary>
		public EdgeEdgeDictionary EdgePredecessors
		{
			get
			{
				return edgePredecessors;
			}
		}

		/// <summary>
		/// End path edges collection
		/// </summary>
		public EdgeCollection EndPathEdges
		{
			get
			{
				return endPathEdges;
			}
		}

		/// <summary>
		/// Returns the path leading to the vertex v.
		/// </summary>
		/// <param name="se">end of the path</param>
		/// <returns>path leading to v</returns>
		public EdgeCollection Path(IEdge se)
		{
			EdgeCollection path = new EdgeCollection();

			IEdge ec = se;
			path.Insert(0,ec);
			while (EdgePredecessors.Contains(ec))
			{
				IEdge e = EdgePredecessors[ec];
				path.Insert(0,e);
				ec=e;
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

			foreach(IEdge e in EndPathEdges)
				es.Add( Path( e ) );

			return es;
		}

		/// <summary>
		/// Create a merged path. 
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method creates an edge path that stops if an edge is not white
		/// or the edge has no more predecessors.
		/// </para>
		/// </remarks>
		/// <param name="se">end edge</param>
		/// <param name="colors">edge color dictionary</param>
		/// <returns>path to edge</returns>
		public EdgeCollection MergedPath(IEdge se,EdgeColorDictionary colors)
		{
			EdgeCollection path = new EdgeCollection();

			IEdge ec = se;
			GraphColor c = colors[ec];
			if (c!=GraphColor.White)
				return path;
			else
				colors[ec]=GraphColor.Black;

			path.Insert(0,ec);
			while (EdgePredecessors.Contains(ec))
			{
				IEdge e = EdgePredecessors[ec];
				c = colors[e];
				if (c!=GraphColor.White)
					return path;
				else
					colors[e]=GraphColor.Black;

				path.Insert(0,e);
				ec=e;
			}
			return path;
		}

		/// <summary>
		/// Returns the array of merged paths
		/// </summary>
		public EdgeCollection[] AllMergedPaths()
		{
			EdgeCollection[] es = new EdgeCollection[EndPathEdges.Count];
			EdgeColorDictionary colors = new EdgeColorDictionary();

			foreach(DictionaryEntry de in EdgePredecessors)
			{
				colors[(IEdge)de.Key]=GraphColor.White;
				colors[(IEdge)de.Value]=GraphColor.White;
			}

			for(int i=0;i<EndPathEdges.Count;++i)
				es[i] = MergedPath( EndPathEdges[i], colors );

			return es;
		}

		/// <summary>
		/// Not used
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void InitializeEdge(Object sender, EdgeEventArgs args)
		{
		}

		/// <summary>
		/// Records edge predecessor
		/// </summary>
		public void DiscoverTreeEdge(Object sender, EdgeEdgeEventArgs args)
		{
			if (args.Edge != args.TargetEdge)
				EdgePredecessors[args.TargetEdge]=args.Edge;
		}

		/// <summary>
		/// Records end path edges
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void FinishEdge(Object sender, EdgeEventArgs args)
		{
			if (!EdgePredecessors.ContainsValue(args.Edge))
				EndPathEdges.Add(args.Edge);
		}

	}
}
