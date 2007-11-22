using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

using Netron.UI;
using Netron;

using QuickGraph;
using QuickGraph.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Representations;
using QuickGraph.Algorithms.Graphviz;
using QuickGraph.Algorithms.RandomWalks;
using QuickGraph.Layout.Providers;
using QuickGraph.Layout.ConnectorChoosers;
using QuickGraph.Layout.Shapes;
using QuickGraph.Algorithms;
using QuickGraph.Algorithms.Search;
using QuickGraph.Serialization;
using QuickGraph.Providers;
using QuickGraph.Layout.Forms;
using QuickGraph.Layout.Connections;

namespace QuickGraph.Layout.GUI
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private VertexColorDictionary vertexColors = null;
		private EdgeColorDictionary edgeColors = null;
		private VertexIntDictionary vertexCounts = null;
		private EdgeIntDictionary edgeCounts = null;
		private EdgeDoubleDictionary edgeWeights = null;
		private Netron.NetronDomain doc = null;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel messageBarPanel;
		private System.Windows.Forms.ToolBar toolBar;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem depthFirstSearchAlgorithmItem;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem breadthFirstSearchItem;
		private System.Windows.Forms.MenuItem edgeDepthFirstSearchItem;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.MenuItem menuItem6;
		private DockingManagerExtender.DockingManagerExtender dockingManagerExtender1;
		private QuickGraph.Layout.Forms.QuickNetronPanel netronPanel;
		private Netron.UI.NetronOverview netronOverview1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.StatusBarPanel errorBarPanel;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.doc = new NetronDomain();

			this.doc.ConfigurationFileName = ConfigurationSettings.AppSettings["NetronConfig"];
			this.doc.LoadConfiguration();

//			this.netronPalette.Document = this.doc;
			this.netronOverview1.Panel = this.netronPanel;
//			this.netronProperties.Panel = this.netronPanel;

			this.netronPanel.Domain = this.doc;
			this.netronPanel.Graph =
				new BidirectionalGraph(
					new TypedVertexProvider(typeof(SerializableVertex)),
					new TypedEdgeProvider(typeof(SerializableEdge)),
					true
					);
			this.netronPanel.EntitySelected +=new StatusEventHandler(netronPanel_SelectEvent);
			this.netronPanel.BackColor = Color.LightBlue;

			OnNew(this,new EventArgs());
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.depthFirstSearchAlgorithmItem = new System.Windows.Forms.MenuItem();
			this.edgeDepthFirstSearchItem = new System.Windows.Forms.MenuItem();
			this.breadthFirstSearchItem = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.messageBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.errorBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.toolBar = new System.Windows.Forms.ToolBar();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.dockingManagerExtender1 = new DockingManagerExtender.DockingManagerExtender(this.components);
			this.netronPanel = new QuickGraph.Layout.Forms.QuickNetronPanel(this.components);
			this.netronOverview1 = new Netron.UI.NetronOverview();
			((System.ComponentModel.ISupportInitialize)(this.messageBarPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorBarPanel)).BeginInit();
			this.netronPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem3,
																					 this.menuItem2});
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 0;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem4,
																					  this.menuItem6});
			this.menuItem3.Text = "File";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 0;
			this.menuItem4.Text = "Load GraphML";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.Text = "Load GXL";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem5,
																					  this.menuItem1});
			this.menuItem2.Text = "Algorithms";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 0;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.depthFirstSearchAlgorithmItem,
																					  this.edgeDepthFirstSearchItem,
																					  this.breadthFirstSearchItem});
			this.menuItem5.Text = "Search";
			// 
			// depthFirstSearchAlgorithmItem
			// 
			this.depthFirstSearchAlgorithmItem.Index = 0;
			this.depthFirstSearchAlgorithmItem.Text = "DepthFirstSearchAlgorithm";
			this.depthFirstSearchAlgorithmItem.Click += new System.EventHandler(this.depthFirstSearchAlgorithmItem_Click);
			// 
			// edgeDepthFirstSearchItem
			// 
			this.edgeDepthFirstSearchItem.Index = 1;
			this.edgeDepthFirstSearchItem.Text = "EdgeDepthFirstSearchAlgorithm";
			this.edgeDepthFirstSearchItem.Click += new System.EventHandler(this.edgeDepthFirstSearchItem_Click);
			// 
			// breadthFirstSearchItem
			// 
			this.breadthFirstSearchItem.Index = 2;
			this.breadthFirstSearchItem.Text = "BreadthFirstSearchAlgorithm";
			this.breadthFirstSearchItem.Click += new System.EventHandler(this.breadthFirstSearchItem_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem7,
																					  this.menuItem8});
			this.menuItem1.Text = "Walks";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 0;
			this.menuItem7.Text = "Uniform walk";
			this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "Weighted walk";
			this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 403);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.messageBarPanel,
																						 this.errorBarPanel});
			this.statusBar.Size = new System.Drawing.Size(584, 22);
			this.statusBar.TabIndex = 0;
			this.statusBar.Text = "Status Bar";
			// 
			// messageBarPanel
			// 
			this.messageBarPanel.Text = "Message";
			// 
			// errorBarPanel
			// 
			this.errorBarPanel.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
			this.errorBarPanel.Text = "Error";
			this.errorBarPanel.Width = 20;
			// 
			// toolBar
			// 
			this.toolBar.ButtonSize = new System.Drawing.Size(16, 16);
			this.toolBar.DropDownArrows = true;
			this.toolBar.ImageList = this.imageList1;
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.ShowToolTips = true;
			this.toolBar.Size = new System.Drawing.Size(584, 22);
			this.toolBar.TabIndex = 1;
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(32, 32);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// dockingManagerExtender1
			// 
			this.dockingManagerExtender1.AutomaticStatePersistence = false;
			this.dockingManagerExtender1.ContainerControl = this;
			this.dockingManagerExtender1.InnerControl = null;
			this.dockingManagerExtender1.OuterControl = null;
			this.dockingManagerExtender1.PlainTabBorder = false;
			this.dockingManagerExtender1.VisualStyle = Crownwood.Magic.Common.VisualStyle.IDE;
			// 
			// netronPanel
			// 
			this.dockingManagerExtender1.SetADockingEnable(this.netronPanel, false);
			this.netronPanel.BackColor = System.Drawing.Color.White;
			this.netronPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dockingManagerExtender1.SetCloseButton(this.netronPanel, false);
			this.dockingManagerExtender1.SetCloseOnHide(this.netronPanel, false);
			this.netronPanel.Controls.Add(this.netronOverview1);
			this.netronPanel.DataUpdateInterval = 50;
			this.netronPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockingManagerExtender1.SetDockingStyle(this.netronPanel, System.Windows.Forms.DockStyle.Left);
			this.netronPanel.Domain = null;
			this.dockingManagerExtender1.SetFullTitle(this.netronPanel, "netronPanel");
			this.netronPanel.Graph = null;
			this.dockingManagerExtender1.SetIcon(this.netronPanel, null);
			this.netronPanel.Location = new System.Drawing.Point(0, 22);
			this.netronPanel.Name = "netronPanel";
			this.netronPanel.Size = new System.Drawing.Size(584, 381);
			this.dockingManagerExtender1.SetTabbedMode(this.netronPanel, true);
			this.netronPanel.TabIndex = 4;
			this.dockingManagerExtender1.SetTitle(this.netronPanel, "netronPanel");
			this.netronPanel.Zoom = 1F;
			// 
			// netronOverview1
			// 
			this.dockingManagerExtender1.SetADockingEnable(this.netronOverview1, true);
			this.dockingManagerExtender1.SetCloseButton(this.netronOverview1, true);
			this.dockingManagerExtender1.SetCloseOnHide(this.netronOverview1, false);
			this.dockingManagerExtender1.SetDockingStyle(this.netronOverview1, System.Windows.Forms.DockStyle.Left);
			this.dockingManagerExtender1.SetFullTitle(this.netronOverview1, "NetronOverview");
			this.dockingManagerExtender1.SetIcon(this.netronOverview1, null);
			this.netronOverview1.Location = new System.Drawing.Point(24, 16);
			this.netronOverview1.Name = "netronOverview1";
			this.netronOverview1.Panel = null;
			this.netronOverview1.Size = new System.Drawing.Size(224, 248);
			this.dockingManagerExtender1.SetTabbedMode(this.netronOverview1, true);
			this.netronOverview1.TabIndex = 0;
			this.dockingManagerExtender1.SetTitle(this.netronOverview1, "NetronOverview");
			this.netronOverview1.Zoom = 0.2F;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(584, 425);
			this.Controls.Add(this.netronPanel);
			this.Controls.Add(this.toolBar);
			this.Controls.Add(this.statusBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "QuickGraph - Netron - Test Application";
			((System.ComponentModel.ISupportInitialize)(this.messageBarPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorBarPanel)).EndInit();
			this.netronPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		protected void OnNew(Object sender, EventArgs e)
		{
			MainForm mf = (MainForm)sender;

			mf.netronPanel.Initialize();

			Refresh();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainForm());
		}

		private void ResetVertexAndEdgeColors()
		{
			if (this.netronPanel.Graph==null)
				throw new Exception("Generate a graph first");

			foreach(BasicShape s in this.netronPanel.Populator.ShapeVertices.Keys)
			{
				s.ResetColors();
			}

			foreach(Connection c in this.netronPanel.Populator.ConnectionEdges.Keys)
			{
				c.StrokeColor = Color.Black;
			}
		}

		private void depthFirstSearchAlgorithmItem_Click(object sender, System.EventArgs e)
		{
			if (this.netronPanel.Graph==null)
				throw new Exception("Generate a graph first");

			// clear colors
			ResetVertexAndEdgeColors();

			// create algorithm
			this.edgeColors=new EdgeColorDictionary();
			foreach(IEdge edge in this.netronPanel.Graph.Edges)
				this.edgeColors[edge]=GraphColor.White;

			this.vertexColors = new VertexColorDictionary();
			DepthFirstSearchAlgorithm dfs = new DepthFirstSearchAlgorithm(
				this.netronPanel.Graph,
				this.vertexColors);

			// create tracer
			LayoutAlgorithmTraverVisitor tracer = new LayoutAlgorithmTraverVisitor(this.netronPanel.Populator);

			// link to algo
			dfs.RegisterTreeEdgeBuilderHandlers(tracer);
			dfs.RegisterVertexColorizerHandlers(tracer);

			dfs.TreeEdge +=new EdgeEventHandler(dfs_TreeEdge);
			dfs.BackEdge +=new EdgeEventHandler(dfs_BackEdge);
			dfs.ForwardOrCrossEdge +=new EdgeEventHandler(dfs_ForwardOrCrossEdge);

			// add handler to tracers
			tracer.UpdateVertex +=new ShapeVertexEventHandler(tracer_UpdateVertex);
			tracer.UpdateEdge +=new ConnectionEdgeEventHandler(tracer_UpdateEdge);

			// running algorithm
			Thread thread = new Thread(new ThreadStart(dfs.Compute));
			thread.Start();
		}


		private void edgeDepthFirstSearchItem_Click(object sender, System.EventArgs e)
		{
			if (this.netronPanel.Graph==null)
				throw new Exception("Generate a graph first");
			if (this.netronPanel.Populator==null)
				throw new Exception("Populator should not be null.");

			ResetVertexAndEdgeColors();

			// create algorithm
			this.vertexColors=null;
			this.edgeColors = new EdgeColorDictionary();
			EdgeDepthFirstSearchAlgorithm edfs = 
				new EdgeDepthFirstSearchAlgorithm(this.netronPanel.Graph,this.edgeColors);

			// create tracer
			LayoutAlgorithmTraverVisitor tracer = new LayoutAlgorithmTraverVisitor(this.netronPanel.Populator);

			// link to algo
			edfs.RegisterTreeEdgeBuilderHandlers(tracer);
			edfs.RegisterEdgeColorizerHandlers(tracer);

			// add handler to tracers
			tracer.UpdateVertex +=new ShapeVertexEventHandler(tracer_UpdateVertex);
			tracer.UpdateEdge +=new ConnectionEdgeEventHandler(tracer_UpdateEdge);

			// running algorithm
			Thread thread = new Thread(new ThreadStart(edfs.Compute));
			thread.Start();	
		}


		private void dfs_TreeEdge(object sender, EdgeEventArgs e)
		{
			if (this.edgeColors==null || !this.edgeColors.Contains(e.Edge))
				return;

			this.edgeColors[e.Edge]= GraphColor.Gray;
		}

		private void dfs_BackEdge(object sender, EdgeEventArgs e)
		{
			if (this.edgeColors==null || !this.edgeColors.Contains(e.Edge))
				return;

			this.edgeColors[e.Edge] = GraphColor.Black;
		}

		private void dfs_ForwardOrCrossEdge(object sender, EdgeEventArgs e)
		{
			if (this.edgeColors==null || !this.edgeColors.Contains(e.Edge))
				return;

			this.edgeColors[e.Edge] = GraphColor.Black;
		}

		private void breadthFirstSearchItem_Click(object sender, System.EventArgs e)
		{
			if (this.netronPanel.Graph==null)
				throw new Exception("Generate a graph first");
			if (this.netronPanel.Populator==null)
				throw new Exception("Populator should not be null.");

			ResetVertexAndEdgeColors();

			// create algorithm
			this.edgeColors=new EdgeColorDictionary();
			foreach(IEdge edge in this.netronPanel.Graph.Edges)
				this.edgeColors[edge]=GraphColor.White;
			this.vertexColors = new VertexColorDictionary();
			BreadthFirstSearchAlgorithm bfs = new BreadthFirstSearchAlgorithm(
				this.netronPanel.Graph,
				new VertexBuffer(),
				this.vertexColors);

			// create tracer
			LayoutAlgorithmTraverVisitor tracer = new LayoutAlgorithmTraverVisitor(this.netronPanel.Populator);

			// link to algo
			bfs.RegisterTreeEdgeBuilderHandlers(tracer);
			bfs.RegisterVertexColorizerHandlers(tracer);

			bfs.TreeEdge +=new EdgeEventHandler(dfs_TreeEdge);
			bfs.NonTreeEdge+=new EdgeEventHandler(dfs_BackEdge);
			bfs.BlackTarget +=new EdgeEventHandler(dfs_ForwardOrCrossEdge);


			// add handler to tracers
			tracer.UpdateVertex +=new ShapeVertexEventHandler(tracer_UpdateVertex);
			tracer.UpdateEdge +=new ConnectionEdgeEventHandler(tracer_UpdateEdge);

			// running algorithm
			VertexMethodCaller vm=
				new VertexMethodCaller(
					new ComputeVertexDelegate(bfs.Compute),
					Traversal.FirstVertex(this.netronPanel.Graph)
					);
			Thread thread = new Thread(new ThreadStart(vm.Run));
			thread.Start();		
		}

		private void tracer_UpdateVertex(object sender, ShapeVertexEventArgs args)
		{
			// shape is TextShape
			if (this.vertexColors==null || !this.vertexColors.Contains(args.Vertex))
				return;

			PropertyGridShape shape = (PropertyGridShape)args.Shape;
			switch(this.vertexColors[args.Vertex])
			{
				case GraphColor.White:
					shape.TitleBackColor = Color.White;
					break;
				case GraphColor.Gray:
					shape.TitleBackColor = Color.LightGray;
					break;
				case GraphColor.Black:
					shape.TitleBackColor = Color.Red;
					break;
			}
		}

		private void tracer_UpdateEdge(object sender, ConnectionEdgeEventArgs args)
		{
			// shape is TextShape
			if (this.edgeColors==null || !this.edgeColors.Contains(args.Edge))
				return;

			Connection conn = (Connection)args.Conn;
			switch(this.edgeColors[args.Edge])
			{
				case GraphColor.White:
					conn.StrokeColor = Color.Green;
					break;
				case GraphColor.Gray:
					conn.StrokeColor = Color.Black;
					break;
				case GraphColor.Black:
					conn.StrokeColor = Color.Red;
					break;
			}
		}

		private void netronPanel_SelectEvent(object sender, Netron.StatusEventArgs e)
		{
			if (e.Entity is PropertyGridShape)
			{
				PropertyGridShape shape = (PropertyGridShape)e.Entity;
				switch(e.Status)
				{
					case EnumStatusType.Selected:
						shape.CollapseRows=false;
						this.netronPanel.MoveToFront(shape);
						break;
					case EnumStatusType.Deselected:
						shape.CollapseRows=true;
						break;
				}
			}
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();

			dlg.Multiselect = false;
			dlg.DefaultExt = ".xml";
			dlg.Title="Load GraphML file";
			if(dlg.ShowDialog() != DialogResult.OK)
				return;		

			this.netronPanel.Clear();

			// create serialize
			GraphMLGraphSerializer ser = new GraphMLGraphSerializer(".");
			ser.GraphType = typeof(BidirectionalGraph);
			ser.VertexProviderType = typeof(SerializableVertexProvider);
			ser.EdgeProviderType = typeof(SerializableEdgeProvider);

			try
			{
				XmlReader reader = new XmlTextReader(dlg.FileName);
				// validate
				GraphMLGraphSerializer.Validate(reader);
				reader = new XmlTextReader(dlg.FileName);
				this.netronPanel.Graph = (BidirectionalGraph)ser.Deserialize(reader);	
			}
			catch(Exception ex)
			{
				Debug.Write(ex.ToString());
				Debug.Flush();
				throw;
			}
			this.netronPanel.Populator.PopulatePanel(this.netronPanel.Graphics);

			foreach(SerializableVertex v in this.netronPanel.Graph.Vertices)
			{
				PropertyGridShape shape = (PropertyGridShape)this.netronPanel.Populator.VertexShapes[v];
				if (v.Entries.ContainsKey("name"))
					shape.Title = v.Entries["name"];
				else
					shape.Title = v.ID.ToString();
				if (v.Entries.ContainsKey("icon"))
				{
					try
					{
						shape.Icon = new Icon(v.Entries["icon"]);
					}
					catch(Exception)
					{}
				}

				// add some properties
				foreach(DictionaryEntry de in v.Entries)
				{
					if (de.Key.ToString()=="name" || de.Key.ToString()=="icon")
						continue;
					shape.Rows.Add(new PropertyEntry(de.Key.ToString(),de.Value.ToString()));
				}
			}

			foreach(SerializableEdge edge in this.netronPanel.Graph.Edges)
			{
				if (edge.Entries.ContainsKey("name"))
				{
					SplineConnection conn = (SplineConnection)this.netronPanel.Populator.EdgeConnections[edge];
					conn.Label = edge.Entries["name"];
				}
			}

			this.netronPanel.Populator.PopulatePanel(this.netronPanel.Graphics);
		}

		private void menuItem7_Click(object sender, System.EventArgs e)
		{
			if (this.netronPanel.Graph==null)
				throw new Exception("Generate a graph first");
			if (this.netronPanel.Populator==null)
				throw new Exception("Populator should not be null.");

			ResetVertexAndEdgeColors();

			// create algorithm
			this.vertexCounts = new VertexIntDictionary();
			this.edgeCounts = new EdgeIntDictionary();
			foreach(IVertex vertex in this.netronPanel.Graph.Vertices)
				this.vertexCounts[vertex]=0;
			foreach(IEdge edge in this.netronPanel.Graph.Edges)
				this.edgeCounts[edge]=0;
			
			RandomWalkAlgorithm walker = new RandomWalkAlgorithm(
				this.netronPanel.Graph
				);

			walker.TreeEdge +=new EdgeEventHandler(walker_TreeEdge);

			LayoutAlgorithmTraverVisitor tracer = new LayoutAlgorithmTraverVisitor(this.netronPanel.Populator);
			walker.TreeEdge +=new EdgeEventHandler(tracer.TreeEdge);

			Thread thread = new Thread(new ThreadStart(walker.Generate));
			thread.Start();			
		}

		private void walker_TreeEdge(object sender, EdgeEventArgs e)
		{
			this.vertexCounts[e.Edge.Target]++;	
			this.edgeCounts[e.Edge]++;

			PropertyGridShape shape = 
				(PropertyGridShape)this.netronPanel.Populator.VertexShapes[e.Edge.Source];
			shape.TitleBackColor = Color.White;
			shape.Invalidate();

			shape = 
				(PropertyGridShape)this.netronPanel.Populator.VertexShapes[e.Edge.Target];
			shape.TitleBackColor = Color.LightGreen;
			shape.Title = this.vertexCounts[e.Edge.Target].ToString();
			shape.Invalidate();
		}

		private void walker_WeightedTreeEdge(object sender, EdgeEventArgs e)
		{
			this.vertexCounts[e.Edge.Target]++;	
			this.edgeCounts[e.Edge]++;
			this.edgeWeights[e.Edge]*=0.9;

			PropertyGridShape shape = 
				(PropertyGridShape)this.netronPanel.Populator.VertexShapes[e.Edge.Source];
			shape.TitleBackColor = Color.White;

			shape = 
				(PropertyGridShape)this.netronPanel.Populator.VertexShapes[e.Edge.Target];
			shape.TitleBackColor = Color.LightGreen;
			shape.Title = this.vertexCounts[e.Edge.Target].ToString();
			this.netronPanel.Invalidate();
		}

		private void menuItem8_Click(object sender, System.EventArgs e)
		{
			if (this.netronPanel.Graph==null)
				throw new Exception("Generate a graph first");
			if (this.netronPanel.Populator==null)
				throw new Exception("Populator should not be null.");

			ResetVertexAndEdgeColors();

			// create algorithm
			this.vertexCounts = new VertexIntDictionary();
			this.edgeCounts = new EdgeIntDictionary();
			foreach(IVertex vertex in this.netronPanel.Graph.Vertices)
				this.vertexCounts[vertex]=0;
			foreach(IEdge edge in this.netronPanel.Graph.Edges)
				this.edgeCounts[edge]=0;
			
			this.edgeWeights =new EdgeDoubleDictionary();
			foreach(IEdge edge in this.netronPanel.Graph.Edges)
				edgeWeights[edge]=1;
			WeightedMarkovEdgeChain chain = new WeightedMarkovEdgeChain(edgeWeights);
			RandomWalkAlgorithm walker = new RandomWalkAlgorithm(
				this.netronPanel.Graph
				);

			walker.TreeEdge+=new EdgeEventHandler(walker_WeightedTreeEdge);

			LayoutAlgorithmTraverVisitor tracer = new LayoutAlgorithmTraverVisitor(this.netronPanel.Populator);
			walker.TreeEdge +=new EdgeEventHandler(tracer.TreeEdge);


			Thread thread = new Thread(new ThreadStart(walker.Generate));
			thread.Start();		
		}

	}
}
