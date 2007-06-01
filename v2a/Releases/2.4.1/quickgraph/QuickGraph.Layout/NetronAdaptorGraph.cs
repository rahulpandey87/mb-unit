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
using System.Collections;

using Netron;
using Netron.UI;
using Netron.Commands;

using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Collections;
using QuickGraph.Concepts.MutableTraversals;
using QuickGraph.Collections;
using QuickGraph.Layout.Collections;
using QuickGraph.Layout.ConnectorChoosers;
using QuickGraph.Layout.Providers;
using QuickGraph.Layout.Shapes;

using QuickGraph.Algorithms.Graphviz;
using NGraphviz.Helpers;

namespace QuickGraph.Layout
{
	/// <summary>
	/// This class populate a <see cref="Netron.Panel"/> with 
	/// the context of a <see cref="IVertexListGraph"/>.
	/// </summary>
	/// <remarks>
	/// TODO adapt populator to work with IVertexListGraph;
	/// </remarks>
	public class NetronAdaptorGraph
	{
		private IMutableBidirectionalVertexAndEdgeListGraph visitedGraph;
		private NetronPanel panel;
		private IShapeVertexProvider shapeProvider;
		private IConnectionEdgeProvider connectionProvider;
		private IConnectorChooser connectorChooser;

		private VertexShapeDictionary vertexShapes = new VertexShapeDictionary();
		private ShapeVertexDictionary shapeVertices = new ShapeVertexDictionary();
		private EdgeConnectionDictionary edgeConnections = new EdgeConnectionDictionary();
		private ConnectionEdgeDictionary connectionEdges = new ConnectionEdgeDictionary();
		private GraphvizRankDirection rankDirection = GraphvizRankDirection.TB;

		private bool graphDirty = true;

		public NetronAdaptorGraph(
			IMutableBidirectionalVertexAndEdgeListGraph g,
			NetronPanel panel,
			IShapeVertexProvider shapeProvider,
			IConnectionEdgeProvider connectionProvider,
			IConnectorChooser connectorChooser
			)
		{
			if (g==null)
				throw new ArgumentNullException("g");
			if (panel==null)
				throw new ArgumentNullException("panel");
			if (shapeProvider==null)
				throw new ArgumentNullException("shapeArgument");
			if (connectionProvider==null)
				throw new ArgumentNullException("connectionProvider");
			if (connectorChooser == null)
				throw new ArgumentNullException("connectorChooser");

			this.visitedGraph = g;
			this.panel = panel;
			this.shapeProvider = shapeProvider;
			this.connectionProvider = connectionProvider;
			this.connectorChooser = connectorChooser;
		}

		#region Properties
		public IMutableBidirectionalVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		public NetronPanel Panel
		{
			get
			{
				return this.panel;
			}
		}
		/// <summary>
		/// This property allows specification of the class when will be used to draw a Vertex
		/// The Shape class must inherit from Netron.Shape
		/// </summary>
		public IShapeVertexProvider ShapeProvider
		{
			get
			{
				return this.shapeProvider;
			}
			set
			{
				this.shapeProvider = value;
			}
		}
		/// <summary>
		/// This property allows specification of the class when will be used to draw an Edge
		/// The Connection class must inherit from Netron.Connection
		/// </summary>
		public IConnectionEdgeProvider ConnectionProvider
		{
			get
			{
				return this.connectionProvider;
			}
			set
			{
				this.connectionProvider = value;
			}
		}

		public IConnectorChooser ConnectorChooser
		{
			get
			{
				return this.connectorChooser;
			}
			set
			{
				this.connectorChooser = value;
			}
		}

		public VertexShapeDictionary VertexShapes
		{
			get
			{
				return this.vertexShapes;
			}
		}

		public ShapeVertexDictionary ShapeVertices
		{
			get
			{
				return this.shapeVertices;
			}
		}

		public EdgeConnectionDictionary EdgeConnections
		{
			get
			{
				return this.edgeConnections;
			}
		}

		public ConnectionEdgeDictionary ConnectionEdges
		{
			get
			{
				return this.connectionEdges;
			}
		}

		public bool GraphDirty
		{
			get
			{
				return this.graphDirty;
			}
			set
			{
				this.graphDirty = value;
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
		#endregion

		#region Events
		public event EventHandler LayGraph;

		protected void OnLayGraph()
		{
			if (LayGraph!=null)
				LayGraph(this, new EventArgs());
		}

		public event ShapeVertexEventHandler LayVertex;

		protected void OnLayVertex(Shape shape, IVertex v)
		{
			if (LayVertex!=null)
				LayVertex(this,new ShapeVertexEventArgs(shape,v));
		}


		public event ConnectionEdgeEventHandler LayEdge;

		protected void OnLayEdge(Connection conn, IEdge e)
		{
			if (LayEdge!=null)
				LayEdge(this,new ConnectionEdgeEventArgs(conn,e));
		}

		public event ShapeVertexEventHandler ConnectVertex;

		protected void OnConnectVertex(Shape shape, IVertex v)
		{
			if (ConnectVertex!=null)
				ConnectVertex(this, new ShapeVertexEventArgs(shape,v));
		}

		public event ConnectionEdgeEventHandler ConnectEdge;

		protected void OnConnectEdge(Connection conn, IEdge e)
		{
			if (ConnectEdge!=null)
				ConnectEdge(this, new ConnectionEdgeEventArgs(conn,e));
		}
		#endregion

		public void PopulatePanel(Graphics g)
		{
			// add shapes
			foreach(IVertex v in VisitedGraph.Vertices)
			{
				// do not add vertex if already in panel
				if (VertexShapes.Contains(v))
					continue;
				AttachShape(v);
			}
			
			// udpate shape sizes if possible and build size dictionary
			NetronGraphvizLayouter layout = new NetronGraphvizLayouter(this);
			layout.Layouter.DpiX = g.DpiX;
			layout.Layouter.DpiY = g.DpiY;
			layout.Layouter.RankDirection = this.RankDirection;
			foreach(DictionaryEntry de in this.ShapeVertices)
			{
				if (de.Key is Netron.Shape)
				{
					Netron.Shape shape = (Netron.Shape)de.Key;
					layout.VertexSizes[(IVertex)de.Value] = shape.Size;
				}
				else
					throw new Exception("Shapes must derive from Netron.Shape");
			}
			layout.Compute();
			layout.PositionVertices();

			foreach(IEdge e in VisitedGraph.Edges)
			{
				// check if edge alreay in graph
				if (EdgeConnections.Contains(e))
					continue;
				AttachConnection(layout,e);
			}

			// reposition connections
		//	layout.PositionConnections();

			// redraw panel
			this.Panel.Invalidate();
			this.graphDirty = false;
		}

		public void LayoutPanel()
		{
			// layout shapes and connections
			OnLayGraph();

			// lay vertices
			if (LayVertex!=null)
			{
				foreach(IVertex v in this.visitedGraph.Vertices)
				{
					Shape shape = this.VertexShapes[v];
					if (shape!=null)
						OnLayVertex(shape,v);
				}
			}

			if (LayEdge!=null)
			{
				foreach(IEdge e in this.visitedGraph.Edges)
				{
					Connection conn = this.EdgeConnections[e];
					if (conn!=null)
						OnLayEdge(conn,e);
				}
			}
			this.Panel.Invalidate();
		}

		public void MoveShape(Shape shape)
		{
/*
			// retreive vertex
			IVertex v = this.ShapeVertices[shape];

			// create rectangle collection
			ArrayList boxes =new ArrayList();

			// create planner
			foreach(Shape s in this.ShapeVertices.Keys)
			{
				RectangleF r = s.Rectangle;
				PointF[] corners = new PointF[4];
				corners[0] = new PointF(r.X,r.Y);
				corners[1] = new PointF(r.X,r.Y+r.Height);
				corners[2] = new PointF(r.X+r.Width,r.Y+r.Height);
				corners[3] = new PointF(r.X+r.Width,r.Y);
				boxes.Add( corners );
			}
			NPathPlan.PathPlanner planner = new NPathPlan.PathPlanner(boxes);
			
			// iterating over the out-edges
			foreach(IEdge e in this.VisitedGraph.OutEdges(v))
			{
				// v is the start edge
				Connection conn = this.EdgeConnections[e];
				PointF start = shape.ConnectionPoint(conn.From);

				Shape target = this.VertexShapes[e.Target];
				PointF end = target.ConnectionPoint(conn.To);

				conn.RemoveAllPoints();
				conn.AddPoints(planner.SplineRoute(
					start,end,
					new PointF(0,0),
					new PointF(0,0)
					));
			}

			// iterating over the in-edges
			foreach(IEdge e in this.VisitedGraph.InEdges(v))
			{
				// v is the start edge
				Connection conn = this.EdgeConnections[e];
				Shape source = this.VertexShapes[e.Target];
				PointF start = source.ConnectionPoint(conn.To);

				PointF end = shape.ConnectionPoint(conn.From);
				conn.RemoveAllPoints();
				conn.AddPoints(planner.SplineRoute(
					start,end,
					new PointF(0,0),
					new PointF(0,0)
					));
			}
*/			
		}

		public bool ContainsVertex(IVertex v)
		{
			return this.VisitedGraph.ContainsVertex(v);
		}

		public bool ContainsVertex(Shape shape)
		{
			return this.shapeVertices.Contains(shape);
		}

		public bool ContainsEdge(IEdge e)
		{
			return this.VisitedGraph.ContainsEdge(e);
		}

		public bool ContainsEdge(Connection conn)
		{
			return this.connectionEdges.Contains(conn);

		}

		public bool ContainsEdge(IVertex u, IVertex v)
		{
			return this.VisitedGraph.ContainsEdge(u,v);

		}

		protected void Connect(IVertex v, Shape shape)
		{
			// storing in hashtable
			VertexShapes.Add(v,shape);
			ShapeVertices.Add(shape,v);		
	
			OnConnectVertex(shape,v);
		}

		protected void Connect(IEdge e, Connection conn)
		{
			// getting source and target shapes
			Shape source = VertexShapes[e.Source];
			Shape target = VertexShapes[e.Target];

			ConnectorChooser.Connect(
				e,	
				conn, 
				source, 
				target);

			// store in tables
			EdgeConnections.Add(e,conn);
			ConnectionEdges.Add(conn,e);

			OnConnectEdge(conn,e);
		}

		public Shape AttachShape(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			if (!this.VisitedGraph.ContainsVertex(v))
				throw new ArgumentException("vertex is not part of the graph");
			if (this.vertexShapes.Contains(v))
				throw new ArgumentException("shape already associtated with this vertex");

			Shape shape = this.ShapeProvider.ProvideShape(v);
			this.Panel.Insert(shape);

			Modify m = new Modify();
			m.AddInsertShape(shape,this.Panel.AbstractShape);
			this.Panel.AbstractShape.History.Do(m);

			Connect(v,shape);

			return shape;
		}

		public Connection AttachConnection(NetronGraphvizLayouter layout,IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("e");
			if (!this.VisitedGraph.ContainsEdge(e))
				throw new ArgumentException("edge already in graph");
			if (this.EdgeConnections.Contains(e))
				throw new ArgumentException("connection already associtated with this edge");

			// create edge
			Connection conn = this.ConnectionProvider.ProvideConnection(e);
			// set points
			layout.PositionConnection(e,conn);
			Connect(e,conn);

			// connect to target
			Modify m = new Modify();
			m.AddInsertConnection(conn, conn.From, conn.To,this.Panel.AbstractShape);
			this.Panel.AbstractShape.History.Do(m);

			return conn;
		}

		public IVertex AddVertex(Shape shape)
		{
			if (shape==null)
				throw new ArgumentNullException("shape");
			if (ContainsVertex(shape))
				throw new ArgumentException("shape already in graph");

			IVertex v = this.VisitedGraph.AddVertex();
			Connect(v,shape);

			return v;
		}

		public IEdge AddEdge(Connection conn)
		{
			if (conn==null)
				throw new ArgumentNullException("conn");
			if (ContainsEdge(conn))
				throw new ArgumentException("connection already in graph");

			// find shapes
			IVertex source = this.ShapeVertices[conn.From.Shape];
			if(source==null)
				throw new ArgumentException("source shape is not in the graph");
			IVertex target = this.ShapeVertices[conn.To.Shape];
			if (target==null)
				throw new ArgumentException("target shape is not in the graph");

			IEdge e = this.VisitedGraph.AddEdge(source,target);
			Connect(e,conn);

			return e;
		}

		public void RemoveVertex(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			if (!this.ContainsVertex(v))
				throw new ArgumentException("v is not part of the graph");

			if (this.VisitedGraph.Degree(v)!=0)
				throw new ArgumentNullException("Vertex has connections, delete them first");

			// remove vertex
			Shape shape = this.VertexShapes[v];
			RemoveShapeVertex(v,shape);
		}

		public void RemoveVertex(Shape shape)
		{
			if (shape==null)
				throw new ArgumentNullException("shape");
			if (!this.ContainsVertex(shape))
				throw new ArgumentException("shape is not part of the graph");

			IVertex v = this.ShapeVertices[shape];
			if (this.VisitedGraph.Degree(v)!=0)
				throw new ArgumentNullException("Vertex has connections, delete them first");

			// remove vertex
			RemoveShapeVertex(v,shape);
		}

		private void RemoveShapeVertex(IVertex v, Shape shape)
		{
			this.VisitedGraph.RemoveVertex(v);
			this.ShapeVertices.Remove(shape);
			this.VertexShapes.Remove(v);

			Modify m = new Modify();
			m.AddDeleteShape(shape);
			this.Panel.AbstractShape.History.Do(m);
		}

		public void ClearVertex(IVertex v)
		{
			if (v==null)
				throw new ArgumentNullException("v");
			if (!this.ContainsVertex(v))
				throw new ArgumentException("v is not part of the graph");

			EdgeCollection edges = new EdgeCollection();
			foreach(IEdge e in this.VisitedGraph.OutEdges(v))
				edges.Add(e);
			foreach(IEdge e in this.VisitedGraph.InEdges(v))
				edges.Add(e);
			
			foreach(IEdge e in edges)
			{
				if (this.VisitedGraph.ContainsEdge(e))
				{
					RemoveEdge(e);
				}
			}
		}

		public void RemoveEdge(IEdge e)
		{
			if (e==null)
				throw new ArgumentNullException("e");
			if (!this.VisitedGraph.ContainsEdge(e))
				throw new ArgumentException("e is not part of the graph");

			Connection conn = this.EdgeConnections[e];
			this.VisitedGraph.RemoveEdge(e);
			this.EdgeConnections.Remove(e);
			this.ConnectionEdges.Remove(conn);

			Modify m = new Modify();
			m.AddDeleteConnection(conn);
			this.Panel.AbstractShape.History.Do(m);
		}

		public void RemoveEdge(Connection conn)
		{
			if (conn==null)
				throw new ArgumentNullException("conn");
			if (!this.ContainsEdge(conn))
				throw new ArgumentException("conn is not part of the graph");

			IEdge e = this.ConnectionEdges[conn];
			RemoveConnectionEdge(e,conn);
		}

		private void RemoveConnectionEdge(IEdge e, Connection conn)
		{
			this.VisitedGraph.RemoveEdge(e);
			this.EdgeConnections.Remove(e);
			this.ConnectionEdges.Remove(conn);

			Modify m = new Modify();
			m.AddDeleteConnection(conn);
			this.Panel.AbstractShape.History.Do(m);
		}
	}
}
