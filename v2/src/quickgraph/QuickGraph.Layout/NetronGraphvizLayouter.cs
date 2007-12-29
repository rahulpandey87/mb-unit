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
using System.Drawing;
using Netron;
using NGraphviz.Layout.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Algorithms.Graphviz;
using QuickGraph.Collections;
using System.Diagnostics;
using System.Collections;
using NGraphviz.Helpers;

namespace QuickGraph.Layout
{
	/// <summary>
	/// Summary description for NetronGraphvizLayouter.
	/// </summary>
	public class NetronGraphvizLayouter
	{
		private NetronAdaptorGraph graph;
		private GraphvizLayout layouter;
		private GraphvizRankDirection rankDirection;
		private IDictionary vertexArgs  = new Hashtable();
		private IDictionary edgeArgs = new Hashtable();

		private float positionScale = 1.0f;

		public NetronGraphvizLayouter(
			NetronAdaptorGraph g
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");

			this.graph = g;
			this.layouter = new GraphvizLayout(this.graph.VisitedGraph);

			this.layouter.GraphSize +=new SizeEventHandler(this.GraphSize);
			this.layouter.LayVertex +=new VertexPositionEventHandler(this.LayVertex);
			this.layouter.LayEdge += new EdgeKeysEventHandler(this.LayEdge);
		}

		public VertexSizeFDictionary VertexSizes
		{
			get
			{
				return this.layouter.VertexSizes;
			}
		}

		public GraphvizLayout Layouter
		{
			get
			{
				return this.layouter;
			}
		}
		/// <summary>
		/// The direction in which to layout the graph
		/// Valid values are GraphvizRankDirection.LR for left to right
		/// and GraphvizRankDirection.TB for top to bottom
		/// </summary>
		public GraphvizRankDirection RankDirection
		{
			get
			{
				return this.rankDirection;
			}
			set
			{
				this.rankDirection = value;
			}
		}

		public void Compute()
		{
			this.layouter.Compute();
		}

		public void PositionVertices()
		{
			foreach(DictionaryEntry de in this.VertexArgs)
			{
				VertexPositionEventArgs args = (VertexPositionEventArgs)de.Value;
				if (!this.graph.ContainsVertex(args.Vertex))
					throw new Exception("could not find shape");
				Shape shape = this.graph.VertexShapes[args.Vertex];

				PointF p = args.Position;
				p.X*=this.PositionScale;
				p.Y*=this.PositionScale;

				p.X -= shape.Size.Width/2;
				p.Y-= shape.Size.Height/2;

				shape.Location = p;
			}			
		}

		public void PositionConnection(IEdge e, Connection conn)
		{
			EdgeKeysEventArgs args = (EdgeKeysEventArgs)this.EdgeArgs[e];

			ArrayList list = new ArrayList();
			foreach(PointF p in args.Keys)
			{
				list.Add( new PointF(p.X*this.PositionScale,p.Y*this.PositionScale) );
			}
			conn.AddPoints(list);
		}

		public void PositionConnections()
		{
			// set up edges points
			foreach(DictionaryEntry de in this.EdgeArgs)
			{
				IEdge e = (IEdge)de.Key;
				EdgeKeysEventArgs args = (EdgeKeysEventArgs)de.Value;
				if (!this.graph.ContainsEdge(args.Edge))
					throw new Exception("could not find edge");

				Connection conn = this.graph.EdgeConnections[e];
				ArrayList list = new ArrayList();
				foreach(PointF p in args.Keys)
				{
					list.Add( new PointF(p.X*this.PositionScale,p.Y*this.PositionScale) );
				}

				conn.AddPoints(list);
			}			
		}

		public float PositionScale
		{
			get
			{
				return this.positionScale;
			}
			set
			{
				this.positionScale = value;
			}
		}

		public NetronAdaptorGraph AdaptorGraph
		{
			get
			{
				return this.graph;
			}
		}

		public IDictionary VertexArgs
		{
			get
			{
				return this.vertexArgs;
			}
		}

		public IDictionary EdgeArgs
		{
			get
			{
				return this.edgeArgs;
			}
		}

		private void GraphSize(object sender, SizeEventArgs e)
		{
			Debug.WriteLine(String.Format("GraphSize {0}",e.Size));
		}

		private void LayVertex(object sender, VertexPositionEventArgs e)
		{
			this.vertexArgs[e.Vertex]=e;
		}

		private void LayEdge(object sender, EdgeKeysEventArgs e)
		{
			this.edgeArgs[e.Edge] = e;
		}
	}
}
