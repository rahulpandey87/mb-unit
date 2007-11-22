using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Netron;
using Netron.UI;

using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.MutableTraversals;
using QuickGraph.Representations;
using QuickGraph;
using QuickGraph.Layout.Providers;
using QuickGraph.Layout.ConnectorChoosers;
using QuickGraph.Layout.Shapes;
using QuickGraph.Layout.Connections;
using NGraphviz.Helpers;

namespace QuickGraph.Layout.Forms
{
	/// <summary>
	/// Summary description for QuickNetronPanel.
	/// </summary>
	public class QuickNetronPanel : NetronPanel
	{
		private IShapeVertexProvider shapeProvider = null;
		private IConnectionEdgeProvider connectionProvider = null;
		private IConnectorChooser connectorChooser = null;
		private NetronAdaptorGraph populator = null;
		private GraphvizRankDirection rankDirection = GraphvizRankDirection.TB;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public QuickNetronPanel(System.ComponentModel.IContainer container)
		{
			container.Add(this);
			InitializeComponent();
			InitializeQuickNetronPanel();
		}

		public QuickNetronPanel()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();
			InitializeQuickNetronPanel();
		}


		private void InitializeQuickNetronPanel()
		{
			this.HitEvent += new HitEventHandler(QuickNetronPanel_HitEvent);
			this.EntitySelected +=new StatusEventHandler(QuickNetronPanel_SelectEvent);

			this.shapeProvider= new TypeShapeVertexProvider(typeof(PropertyGridShape));
			this.connectionProvider = new TypeConnectionEdgeProvider(typeof(SplineConnection));
			this.connectorChooser = new MinDistanceConnectorChooser();

			ZoomWheelMouseBehavior zoom = new ZoomWheelMouseBehavior();
			zoom.Attach(this);
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
				if ( this.populator != null )
					populator.ConnectorChooser = value;
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
				if ( this.populator != null )
					populator.ShapeProvider = value;
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
				if ( this.populator != null )
					populator.ConnectionProvider = value;
			}
		}

		public NetronAdaptorGraph Populator
		{
			get
			{
				return this.populator;
			}
		}

		public IMutableBidirectionalVertexAndEdgeListGraph Graph
		{
			get
			{
				if (this.populator==null)
					return null;
				else
					return this.populator.VisitedGraph;
			}
			set
			{
				if (value==null)
				{
					this.populator = null;
					return;
				}

				// populator
				this.populator = new NetronAdaptorGraph(
					value,
					this,
					shapeProvider,
					connectionProvider,
					connectorChooser
					);
				// attaching delegates
				this.populator.ConnectEdge +=new ConnectionEdgeEventHandler(populator_ConnectEdge);
				this.populator.ConnectVertex +=new ShapeVertexEventHandler(populator_ConnectVertex);
				this.populator.RankDirection = this.rankDirection;
				this.Invalidate();
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
				return rankDirection;
			}
			set
			{
				rankDirection = value;
				if ( this.populator != null )
					this.populator.RankDirection = rankDirection;
			}
		}

		public virtual void Clear()
		{
			this.populator = null;
			base.AbstractShape.Shapes.Clear();
			base.AbstractShape.Connectors.Clear();
			base.AbstractShape.Update();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// QuickNetronPanel
			// 
			this.Click += new System.EventHandler(this.QuickNetronPanel_Click);

		}
		#endregion

		#region Events
		
		public event HitShapeVertexEventHandler HitVertex;

		protected void OnHitVertex(Shape shape, PointF location)
		{
			if (HitVertex!=null)
			{
				IVertex v = this.populator.ShapeVertices[shape];
				if (v==null)
					throw new ArgumentException("shape is not part of the graph");
				HitVertex(this, new HitShapeVertexEventArgs(shape,v,location));
			}
		}

		public event HitConnectionEdgeEventHandler HitEdge;

		protected void OnHitEdge(Connection conn,PointF location)
		{
			if (HitEdge!=null)
			{
				IEdge e = this.populator.ConnectionEdges[conn];
				if (e==null)
					throw new ArgumentException("connection is not part of the graph");
				HitEdge(this, new HitConnectionEdgeEventArgs(conn,e,location));
			}
		}

		#endregion

		#region Message handler overrides
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs args)
		{
			if (this.Populator!=null)
			{
				if (this.Populator.GraphDirty)
				{
					this.Populator.PopulatePanel(args.Graphics);
					this.Populator.LayoutPanel();
					this.Invalidate();
				}
			}

			Rectangle r = new Rectangle(new Point(0,0), new Size(this.Width,this.Height));
			base.OnPaint(new PaintEventArgs(args.Graphics,r));
		}

		#endregion


		private void QuickNetronPanel_Click(object sender, System.EventArgs e)
		{
		
		}

		private void populator_ConnectEdge(Object sender, ConnectionEdgeEventArgs e)
		{

		}

		private void populator_ConnectVertex(Object sender, ShapeVertexEventArgs args)
		{

		}

		private void QuickNetronPanel_HitEvent(object sender, HitEventArgs e)
		{
			switch(e.HitType)
			{
				case EnumHitType.Connection:
					if (this.populator.ContainsEdge((Connection)e.Entity))
						OnHitEdge((Connection)e.Entity,e.Location);
					break;
				case EnumHitType.Shape:
					if (this.populator.ContainsVertex((Shape)e.Entity))
						OnHitVertex((Shape)e.Entity,e.Location);
					break;
			}
		}

		private void QuickNetronPanel_SelectEvent(object sender, StatusEventArgs e)
		{
			switch(e.Status)
			{
				case EnumStatusType.Deselected:
				switch(e.Type)
				{
					case EnumEntityType.Connection:
						if (this.populator.ContainsEdge((Connection)e.Entity))
							this.populator.RemoveEdge((Connection)e.Entity);
						break;
					case EnumEntityType.Shape:
						if (this.populator.ContainsVertex((Shape)e.Entity))
							this.populator.RemoveVertex((Shape)e.Entity);
						break;
				}
				break;
				case EnumStatusType.Inserted:
				switch(e.Type)
				{
					case EnumEntityType.Connection:
						if (!this.populator.ContainsEdge((Connection)e.Entity))
							this.populator.AddEdge((Connection)e.Entity);
						break;
					case EnumEntityType.Shape:
						if (!this.populator.ContainsVertex((Shape)e.Entity))
							this.populator.AddVertex((Shape)e.Entity);
						break;
				}
				break;
			}
		}

		public void MoveToFront(Shape shape)
		{
			Shape top = this.AbstractShape.Shapes[ this.AbstractShape.Shapes.Count - 1];
			int index = this.AbstractShape.Shapes.IndexOf(shape);
			this.AbstractShape.Shapes[index]=top;
			this.AbstractShape.Shapes[this.AbstractShape.Shapes.Count - 1]=shape;
		}
	}
}
