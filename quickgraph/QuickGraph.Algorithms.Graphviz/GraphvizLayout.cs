using System;
using System.IO;
using System.Collections;
using System.Drawing;

namespace QuickGraph.Algorithms.Graphviz
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using NGraphviz.Helpers;
	using NGraphviz.Layout;
	using QuickGraph.Collections;

	/// <summary>
	/// Uses Graphviz to layout vertices and edges.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class GraphvizLayout
	{
		private IVertexAndEdgeListGraph visitedGraph;
		private VertexSizeFDictionary vertexSizes = new VertexSizeFDictionary();
		private GraphvizRankDirection rankDirection;
		private double dpiX = 72.0;
		private double dpiY = 72.0;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="visitedGraph"></param>
		public GraphvizLayout(IVertexAndEdgeListGraph visitedGraph)
		{
			if (visitedGraph==null)
				throw new ArgumentNullException("visitedGraph");
			this.visitedGraph = visitedGraph;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="visitedGraph"></param>
		public GraphvizLayout(
			IVertexAndEdgeListGraph visitedGraph,
			VertexSizeFDictionary vertexSizes)
		{
			if (visitedGraph==null)
				throw new ArgumentNullException("visitedGraph");
			if (vertexSizes==null)
				throw new ArgumentNullException("vertexSizes");

			this.visitedGraph = visitedGraph;
			this.vertexSizes = vertexSizes;
		}

		/// <summary>
		/// Visited Graph
		/// </summary>
		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		/// <summary>
		/// Vertex size (pixels) dictionary
		/// </summary>
		public VertexSizeFDictionary VertexSizes
		{
			get
			{
				return this.vertexSizes;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public double DpiX
		{
			get
			{
				return this.dpiX;
			}
			set
			{
				this.dpiX = value;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		public double DpiY
		{
			get
			{
				return this.dpiY;
			}
			set
			{
				this.dpiY = value;
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
		/// <summary>
		/// Raised while setting the graph size
		/// </summary>
		public event SizeEventHandler GraphSize;

		/// <summary>
		/// Raises the <see cref="GraphSize"/> event.
		/// </summary>
		/// <param name="size">size of the graph layout(in pixels)</param>
		protected void OnGraphSize(Size size)
		{
			if (GraphSize!=null)
				GraphSize(this,new SizeEventArgs(size));
		}

		/// <summary>
		/// Raised for each layout-out vertex.
		/// </summary>
		public event VertexPositionEventHandler LayVertex;

		/// <summary>
		/// Raises the <see cref="LayVertex"/> event.
		/// </summary>
		/// <param name="v">vertex layer</param>
		/// <param name="pos">vertex position</param>
		protected void OnLayVertex(IVertex v, PointF pos)
		{
			if (LayVertex!=null)
				LayVertex(this,new VertexPositionEventArgs(v,pos));
		}

		/// <summary>
		/// Raised for each layed-out edge.
		/// </summary>
		public event EdgeKeysEventHandler LayEdge;

		/// <summary>
		/// Raises the <see cref="LayEdge"/> event.
		/// </summary>
		/// <param name="e">edge to lay</param>
		/// <param name="sourcePort">source port</param>
		/// <param name="targetPort">target port</param>
		/// <param name="keys">key positions of the spline</param>
		protected void OnLayEdge(
			IEdge e, 
			QuickGraph.Concepts.EdgePort sourcePort, 
			QuickGraph.Concepts.EdgePort targetPort, 
			NGraphviz.Layout.Collections.PointFCollection keys
			)
		{
			if (LayEdge!=null)
				LayEdge(this,new EdgeKeysEventArgs(e,sourcePort,targetPort,keys));
		}

		/// <summary>
		/// Computes the layout of the graph using Graphviz
		/// </summary>
		/// <remarks>
		/// </remarks>
		public void Compute()
		{
			StringWriter sw = new StringWriter();

			sw.WriteLine("digraph G{");
			sw.WriteLine("rankdir={0}",this.RankDirection);

			foreach(IVertex v in this.VisitedGraph.Vertices)
			{
				if (this.vertexSizes.Count==0)
				{
					sw.WriteLine("{0} [shape=box];",v.ID);
				}
				else
				{
					SizeF size = this.vertexSizes[v];
					sw.WriteLine("{0} [shape=box, width={1}, height={2}, fixedsize=true];",
						v.ID,
						size.Width/this.DpiX,
						size.Height/this.DpiY
						);
				}
			}

			foreach(IEdge e in this.VisitedGraph.Edges)
			{
				sw.WriteLine("{0} -> {1} [label=\"{2}\"];",e.Source.ID,e.Target.ID, e.ID);
			}

			sw.WriteLine("}");

			System.Diagnostics.Debug.WriteLine(sw.ToString());

			// running graphviz
			NGraphviz.Helpers.Dot dot =new NGraphviz.Helpers.Dot(".");
			GraphLayout gl=dot.RunData(sw.ToString());

			// build vertex id map

			OnGraphSize(gl.Size);
			if (LayVertex!=null)
			{
				foreach(IVertex v in this.VisitedGraph.Vertices)
				{
					VertexLayout vl = (VertexLayout)gl.Vertices[v.ID.ToString()];
					OnLayVertex(v,vl.Pos);
				}
			}

			if (LayEdge!=null)
			{
				foreach(IEdge e in this.VisitedGraph.Edges)
				{
					EdgeLayout el = (EdgeLayout)gl.Edges[e.ID.ToString()];
					OnLayEdge(
						e,
						ConvertEdgePort(el.SourcePort),
						ConvertEdgePort(el.TargetPort),
						el.Keys);
				}
			}
		}

		internal QuickGraph.Concepts.EdgePort ConvertEdgePort(NGraphviz.Layout.EdgePort ep)
		{
			return (QuickGraph.Concepts.EdgePort)Enum.Parse(
				typeof(QuickGraph.Concepts.EdgePort),
				ep.ToString(),
				true
				);
		}
	}
}
