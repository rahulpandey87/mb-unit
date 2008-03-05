using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace MbUnit.Forms
{
	using MbUnit.Core;
	using MbUnit.Core.Collections;
	using MbUnit.Framework;
	using MbUnit.Core.Invokers;
	using MbUnit.Core.Config;
	using MbUnit.Core.Reports;
	using MbUnit.Core.Reports.Serialization;
	using MbUnit.Core.Remoting;
	using MbUnit.Core.Monitoring;

    public delegate void DefaultDelegate();

	/// <summary>
	/// Summary description for ReflectorTreeView.
	/// </summary>
	public class ReflectorTreeView : System.Windows.Forms.UserControl, 
		IMessageFilter,
		IUnitTreeNodeFactory
	{
		private ReflectionImageList reflectionImageList = null;
		private TreeTestDomainCollection testDomains = null;
		private TestTreeNodeFacade treeNodeFacade=null;
		private UnitTreeViewState state=null;
		private TimeMonitor testTimer = new TimeMonitor();
		private string infoMessage = "";

		private UnitTreeNodeCreatorDelegate createNode;
		private AddChildNodeDelegate addChildNode;

		private System.Windows.Forms.TreeView typeTree;
		private System.Windows.Forms.ImageList treeImageList;
		private System.Windows.Forms.ToolTip treeToolTip;
		private System.Windows.Forms.ContextMenu treeContextMenu;
		private System.Windows.Forms.MenuItem addAssemblyItem;
		private System.Windows.Forms.MenuItem removeAssembliesItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem loadConfigItem;
		private System.Windows.Forms.MenuItem saveConfigItem;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemGenerateXml;
		private System.Windows.Forms.MenuItem menuItemGenerateHtml;
		private System.Windows.Forms.MenuItem runNCoverMenuItem;
		private System.Windows.Forms.MenuItem createNAntTaskMenuItem;
        private System.Windows.Forms.MenuItem createMSBuildTaskMenuItem;

        private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem nocoverToClipboard;
		private System.Windows.Forms.MenuItem stopTestsMenuItem;
		private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem reloadAssembliesMenuItem;
		private System.Windows.Forms.MenuItem textReportMenuItem;
		private System.Windows.Forms.MenuItem doxReportMenuItem;

		private Thread workerThread = null;

		public ReflectorTreeView()
		{
			this.testDomains=new TreeTestDomainCollection(this);
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			Application.AddMessageFilter(this);
			DragAcceptFiles(this.Handle, true);

			this.state = new UnitTreeViewState(this.typeTree,
				new UnitTreeViewState.UpdateTreeNodeDelegate(this.UpdateNode)
				);

			this.treeNodeFacade= new TestTreeNodeFacade();
			this.reflectionImageList = new ReflectionImageList(this.treeImageList);

			this.createNode = new UnitTreeNodeCreatorDelegate(this.CreateNode);
			this.addChildNode = new AddChildNodeDelegate(this.AddChildNode);

			this.testDomains.Watcher.AssemblyChangedEvent+=new MbUnit.Core.Remoting.AssemblyWatcher.AssemblyChangedHandler(Watcher_AssemblyChangedEvent);

			this.SetStyle(ControlStyles.DoubleBuffer,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
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
				if (this.workerThread!=null)
				{
					this.workerThread.Abort();       
				}
			}
            if (this.testDomains != null)
            {
                this.testDomains.Watcher.AssemblyChangedEvent -= new MbUnit.Core.Remoting.AssemblyWatcher.AssemblyChangedHandler(Watcher_AssemblyChangedEvent);
                this.testDomains.Dispose();
                this.testDomains = null;
            }
            base.Dispose( disposing );
		}


		#region Properties
		public TreeTestDomainCollection TestDomains
		{
			get
			{
				return this.testDomains;
			}
		}

		public TestTreeNodeFacade Facade
		{
			get
			{
				return this.treeNodeFacade;
			}
		}

		public TreeView TypeTree
		{
			get
			{
				return this.typeTree;
			}
		}

		public TreeNodeCollection Nodes
		{
			get
			{
				return this.typeTree.Nodes;
			}
		}
		#endregion

		public event TreeViewEventHandler AfterSelect;

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.typeTree = new System.Windows.Forms.TreeView();
            this.treeContextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.stopTestsMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.addAssemblyItem = new System.Windows.Forms.MenuItem();
            this.removeAssembliesItem = new System.Windows.Forms.MenuItem();
            this.reloadAssembliesMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.loadConfigItem = new System.Windows.Forms.MenuItem();
            this.saveConfigItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemGenerateXml = new System.Windows.Forms.MenuItem();
            this.menuItemGenerateHtml = new System.Windows.Forms.MenuItem();
            this.textReportMenuItem = new System.Windows.Forms.MenuItem();
            this.doxReportMenuItem = new System.Windows.Forms.MenuItem();
            this.runNCoverMenuItem = new System.Windows.Forms.MenuItem();
            this.nocoverToClipboard = new System.Windows.Forms.MenuItem();
            this.createNAntTaskMenuItem = new System.Windows.Forms.MenuItem();
            this.createMSBuildTaskMenuItem = new System.Windows.Forms.MenuItem();
            this.treeImageList = new System.Windows.Forms.ImageList(this.components);
            this.treeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // typeTree
            // 
            this.typeTree.ContextMenu = this.treeContextMenu;
            this.typeTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeTree.HideSelection = false;
            this.typeTree.ImageIndex = 0;
            this.typeTree.ImageList = this.treeImageList;
            this.typeTree.Location = new System.Drawing.Point(0, 0);
            this.typeTree.Name = "typeTree";
            this.typeTree.SelectedImageIndex = 0;
            this.typeTree.Size = new System.Drawing.Size(488, 344);
            this.typeTree.Sorted = true;
            this.typeTree.TabIndex = 0;
            this.typeTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.typeTree_AfterSelect);
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem5,
            this.menuItem6,
            this.menuItem4,
            this.menuItem3,
            this.menuItem2,
            this.runNCoverMenuItem,
            this.createNAntTaskMenuItem,
            this.createMSBuildTaskMenuItem});
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 0;
            this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.stopTestsMenuItem});
            this.menuItem5.Text = "Tests";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Start Tests";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // stopTestsMenuItem
            // 
            this.stopTestsMenuItem.Index = 1;
            this.stopTestsMenuItem.Text = "Stop Tests";
            this.stopTestsMenuItem.Click += new System.EventHandler(this.stopTestsMenuItem_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.addAssemblyItem,
            this.removeAssembliesItem,
            this.reloadAssembliesMenuItem});
            this.menuItem6.Text = "Assemblies";
            // 
            // addAssemblyItem
            // 
            this.addAssemblyItem.Index = 0;
            this.addAssemblyItem.Text = "Add Assemblies...";
            this.addAssemblyItem.Click += new System.EventHandler(this.addAssemblyItem_Click);
            // 
            // removeAssembliesItem
            // 
            this.removeAssembliesItem.Index = 1;
            this.removeAssembliesItem.Text = "Remove Assemblies";
            this.removeAssembliesItem.Click += new System.EventHandler(this.removeAssembliesItem_Click);
            // 
            // reloadAssembliesMenuItem
            // 
            this.reloadAssembliesMenuItem.Index = 2;
            this.reloadAssembliesMenuItem.Text = "ReLoad Assemblies";
            this.reloadAssembliesMenuItem.Click += new System.EventHandler(this.reloadAssembliesMenuItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "Clear Results";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.loadConfigItem,
            this.saveConfigItem});
            this.menuItem3.Text = "Config";
            // 
            // loadConfigItem
            // 
            this.loadConfigItem.Index = 0;
            this.loadConfigItem.Text = "Load Config...";
            this.loadConfigItem.Click += new System.EventHandler(this.loadConfigItem_Click);
            // 
            // saveConfigItem
            // 
            this.saveConfigItem.Index = 1;
            this.saveConfigItem.Text = "Save Config...";
            this.saveConfigItem.Click += new System.EventHandler(this.saveConfigItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 4;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemGenerateXml,
            this.menuItemGenerateHtml,
            this.textReportMenuItem,
            this.doxReportMenuItem});
            this.menuItem2.Text = "Report";
            // 
            // menuItemGenerateXml
            // 
            this.menuItemGenerateXml.Index = 0;
            this.menuItemGenerateXml.Text = "XML";
            this.menuItemGenerateXml.Click += new System.EventHandler(this.menuItemGenerateXml_Click);
            // 
            // menuItemGenerateHtml
            // 
            this.menuItemGenerateHtml.Index = 1;
            this.menuItemGenerateHtml.Text = "HTML";
            this.menuItemGenerateHtml.Click += new System.EventHandler(this.menuItemGenerateHtml_Click);
            // 
            // textReportMenuItem
            // 
            this.textReportMenuItem.Index = 2;
            this.textReportMenuItem.Text = "&Text";
            this.textReportMenuItem.Click += new System.EventHandler(this.textReportMenuItem_Click);
            // 
            // doxReportMenuItem
            // 
            this.doxReportMenuItem.Index = 3;
            this.doxReportMenuItem.Text = "&Dox";
            this.doxReportMenuItem.Click += new System.EventHandler(this.doxReportMenuItem_Click);
            // 
            // runNCoverMenuItem
            // 
            this.runNCoverMenuItem.Index = 5;
            this.runNCoverMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.nocoverToClipboard});
            this.runNCoverMenuItem.Text = "NCover";
            // 
            // nocoverToClipboard
            // 
            this.nocoverToClipboard.Index = 0;
            this.nocoverToClipboard.Text = "Run";
            this.nocoverToClipboard.Click += new System.EventHandler(this.nocoverToClipboard_Click);
            // 
            // createNAntTaskMenuItem
            // 
            this.createNAntTaskMenuItem.Index = 6;
            this.createNAntTaskMenuItem.Text = "Create NAnt task (Clipboard)";
            this.createNAntTaskMenuItem.Click += new System.EventHandler(this.createNAntTaskMenuItem_Click);
            // 
            // createMSBuildTaskMenuItem
            // 
            this.createMSBuildTaskMenuItem.Index = 7;
            this.createMSBuildTaskMenuItem.Text = "Create MSBuild task (Clipboard)";
            this.createMSBuildTaskMenuItem.Click += new System.EventHandler(this.createMSBuildTaskMenuItem_Click);
            // 
            // treeImageList
            // 
            this.treeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.treeImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.treeImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ReflectorTreeView
            // 
            this.Controls.Add(this.typeTree);
            this.Name = "ReflectorTreeView";
            this.Size = new System.Drawing.Size(488, 344);
            this.ResumeLayout(false);

		}
		#endregion

		#region Drag & Drop support
		[DllImport("shell32.dll")]
		private static extern int DragQueryFile(IntPtr hdrop, int ifile, StringBuilder fname, int fnsize);  
		[DllImport("shell32.dll")]
		private static extern int DragAcceptFiles(IntPtr hwnd, bool accept);
		[DllImport("shell32.dll")]
		private static extern void DragFinish(IntPtr hdrop); 
		private const int WM_DROPFILES = 563; 

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_DROPFILES)
			{
				int nFiles = DragQueryFile(m.WParam,-1,null,0);
				for(int i=0;i<nFiles;++i)
				{
					StringBuilder sb = new StringBuilder(256);
					DragQueryFile(m.WParam, i, sb, 256);
					HandleDroppedFile(sb.ToString());
				}
				DragFinish(m.WParam);
                ThreadedPopulateTree(true);

                return true;
			}
			return false;
		}

		private void HandleDroppedFile(string file)
		{
			if (file!=null && file.Length>0)
				AddAssembly(file);

		}
		#endregion

		#region Assembly handling
		public void AddAssembliesByDialog()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Assemblies or Executables (*.dll,*.exe)|*.dll;*.exe|All files (*.*)|*.*||";
			dlg.FilterIndex = 0;
			dlg.Multiselect = true;

			if (dlg.ShowDialog() != DialogResult.OK)
				return;

			foreach(string file in dlg.FileNames)
				AddAssembly(file);
			this.ThreadedPopulateTree(true);
        }

		public void RemoveAssemblies()
		{
			this.AbortWorkerThread();
			this.TestDomains.Clear();
			this.ThreadedPopulateTree(false);
		}

		public void AddAssembly(string file)
		{
			if (this.TestDomains.ContainsTestAssembly(file))
			{
				MessageBox.Show(String.Format("The file {0} is already loaded.",file),"File already loaded error");
			}
			else
			{
				this.TestDomains.Add(file);
				this.TestDomains.Watcher.Start();
			}
		}

		public void Watcher_AssemblyChangedEvent(string fullPath)
		{
			this.infoMessage = String.Format("{0} modified, reloading",fullPath);
			this.ReloadAssemblies();
		}

		public void ReloadAssemblies()
		{
			this.AbortWorkerThread();
            this.ClearReports();
			this.ThreadedPopulateTree(true);
		}
		public void NewConfig()
		{
			this.AbortWorkerThread();
			this.RemoveAssemblies();
			this.ClearTree();
		}

        public void ClearReports()
        {
            MbUnit.GUI.MbUnitForm parent = (MbUnit.GUI.MbUnitForm) this.ParentForm;
            parent.ClearReports();
        }

		#endregion

		#region Tree population
        public UnitTreeNode CreateUnitTreeNode(string name, TestNodeType nodeType, Guid domainIdentifier, Guid testIdentifier)
        {
			return (UnitTreeNode)this.Invoke(this.createNode,
				new Object[]{name,nodeType,domainIdentifier,testIdentifier}
				);
		}

		public void AddChildUnitTreeNode(TreeNode node, TreeNode childNode)
		{
			this.Invoke(this.addChildNode,new Object[]{node,childNode});
		}

		public void AddChildNode(TreeNode node, TreeNode childNode)
		{
			node.Nodes.Add(childNode);
		}

        public UnitTreeNode CreateNode(string name, TestNodeType nodeType, Guid domainIdentifier, Guid testIdentifier)
        {
            return new UnitTreeNode(name, nodeType, domainIdentifier, testIdentifier);
        }

		public void UpdateNode(TreeNodeState old, UnitTreeNode node)
		{
			if (old.IsVisible)
				node.EnsureVisible();
			if (old.IsExpanded)
				node.Expand();
            if (old.IsSelected)
                this.typeTree.SelectedNode = node;
		}

		public void ClearTree()
		{
			this.typeTree.Nodes.Clear();
			this.treeNodeFacade.Clear();
			this.MessageOnStatusBar("Tree Cleared");
			OnTreeCleared();
		}

		public void PopulateTree()
		{
			try
			{
                this.OnBeginLoadTests();
                this.Invoke(new DefaultDelegate(this.ClearTree));

				this.MessageOnStatusBar("Reloading assemblies");
				this.TestDomains.Reload();
				this.MessageOnStatusBar("Build facade");
				this.TestDomains.PopulateFacade(this.treeNodeFacade);

				this.MessageOnStatusBar("Populate tree");
				this.typeTree.Invoke(new MethodInvoker(this.typeTree.BeginUpdate));
				this.TestDomains.PopulateChildTree(this.typeTree,this.Facade);
                this.typeTree.Invoke(new MethodInvoker(this.typeTree.EndUpdate));
				this.MessageOnStatusBar("Tree populated");
				this.state.Load();
				this.MessageOnStatusBar("Previous state loaded");
                OnTreePopulated();
                this.MessageOnStatusBar("");
            }
            catch (System.Runtime.Remoting.RemotingException remote)
            {
                MessageBox.Show("Could not load test domain.  Please ensure you have referenced the installed version of MbUnit.Framework within your test assembly. \r\n\r\n The error message was: \r\n" + remote.Message, "Error loading test assembly", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NewConfig();
            }
            catch(Exception ex)
			{
                if (ex is System.Threading.ThreadAbortException)
                    return;

				MessageBox.Show(ex.ToString());
			}
        }

		public void ThreadedPopulateTree(bool saveTreeState)
		{
            if (saveTreeState)
                SaveTreeState();

			this.AbortWorkerThread();
			this.workerThread =new Thread(new ThreadStart(this.PopulateTree));
			this.workerThread.Start();
        }

        private void SaveTreeState()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(SaveTreeState));
            else
                state.Save();
        }

		public event EventHandler TreeCleared;

		protected void OnTreeCleared()
		{
			if (this.TreeCleared!=null)
				TreeCleared(this, new EventArgs());
		}

        public event EventHandler BeginLoadTests;
        protected void OnBeginLoadTests()
        {
            if (this.BeginLoadTests != null)
                BeginLoadTests(this, new EventArgs());
        }

        public event EventHandler TreePopulated;
		protected void OnTreePopulated()
		{
			if (this.TreePopulated!=null)
				TreePopulated(this, new EventArgs());
		}

		public void ExpandAllFailures()
		{
			this.typeTree.BeginUpdate();
			foreach(UnitTreeNode node in this.typeTree.Nodes)
				ExpandState(node,TestState.Failure);
			this.typeTree.EndUpdate();
		}
		public void ExpandAllIgnored()
		{
			this.typeTree.BeginUpdate();
			foreach(UnitTreeNode node in this.typeTree.Nodes)
				ExpandState(node,TestState.Ignored);
			this.typeTree.EndUpdate();
		}

		public void ExpandCurrentFailures()
		{
			this.ExpandState((UnitTreeNode)this.TypeTree.SelectedNode,TestState.Failure);		
		}

		public void ExpandCurrentIgnored()
		{
			this.ExpandState((UnitTreeNode)this.TypeTree.SelectedNode,TestState.Failure);		
		}

		public void ExpandState(UnitTreeNode node, TestState state)
		{
			if (node==null)
				return;
			if (node.TestState==state)
				node.EnsureVisible();
			foreach(UnitTreeNode child in node.Nodes)
				ExpandState(child,state);
		}

		#endregion

		#region Status bar handling
		protected virtual void MessageOnStatusBar(string message, params Object[] args)
		{
			this.infoMessage=String.Format(message,args);
		}        
		#endregion

		#region Test Running
		public string InfoMessage
		{
			get
			{
				return this.infoMessage;
			}
		}

		public bool WorkerThreadAlive
		{
			get
			{
				return this.workerThread!=null && this.workerThread.IsAlive;
			}
		}

		public double TestDuration
		{
			get
			{
				return this.testTimer.Now;
			}
		}

		public void RunTests()
		{
			try
			{
				// clearing nodes
				this.MessageOnStatusBar("Clearing results");
                this.Invoke(new MethodInvoker(this.ClearSelectedResults));

				OnStartTests();
				this.MessageOnStatusBar("Starting tests");
                UnitTreeNode selectedNode = (UnitTreeNode)
                    this.Invoke(new UnitTreeNodeMethodInvoker(this.GetSelectTreeNode));
                if (selectedNode==null)
					this.TestDomains.RunPipes();
				else
					this.TestDomains.RunPipes(selectedNode);
				this.MessageOnStatusBar("Finished tests");
			}
			catch(Exception ex)
			{
				if (ex is System.Threading.ThreadAbortException)
					return;

                MessageBox.Show(ex.ToString());
                this.MessageOnStatusBar("Test execution failed: " + ex.Message);
			}
			finally
			{
				OnFinishTests();
			}
		}

		public void ThreadedRunTests()
		{
			this.AbortWorkerThread();
			this.workerThread =new Thread(new ThreadStart(this.RunTests));

            this.workerThread.IsBackground=true;
			this.workerThread.Priority=ThreadPriority.Lowest;
			this.MessageOnStatusBar("Launching worker thread");
			this.workerThread.Start();
		}

		public void AbortWorkerThread()
		{
			if (this.workerThread!=null)
			{
                try
                {
                    this.MessageOnStatusBar("Aborting worker thread");
                    this.testDomains.Stop();
                    this.workerThread.Join(1000);
                    this.workerThread.Abort();
                    this.workerThread.Join(2000);
                    this.workerThread = null;
                    this.MessageOnStatusBar("Worker thread aborted");
                }
                catch (Exception ex)
                {
                    if (ex is System.Threading.ThreadAbortException)
                        return;
                }
			}
		}

		private void typeTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (this.AfterSelect!=null)
				AfterSelect(this,e);
		}

        public delegate UnitTreeNode UnitTreeNodeMethodInvoker();
        protected UnitTreeNode GetSelectTreeNode()
        {
            return this.typeTree.SelectedNode as UnitTreeNode;
        }

        public void ClearSelectedResults()
        {
            if (this.typeTree.SelectedNode != null)
            {
                this.clearNodeResults((UnitTreeNode)this.typeTree.SelectedNode);
                this.MessageOnStatusBar("Results cleared");
            }
            else
                this.ClearAllResults();
        }

        public void ClearAllResults()
		{
			foreach(UnitTreeNode node in this.Nodes)
				this.ClearResults(node);
			this.MessageOnStatusBar("Results cleared");
		}

		public void ClearResults(UnitTreeNode node)
		{
			if (node==null)
				return;
			this.clearNodeResults(node);
		}

		private void clearNodeResults(UnitTreeNode node)
		{
			if (node==null)
				return;
			node.TestState = TestState.NotRun;
			foreach(UnitTreeNode child in node.Nodes)
				this.clearNodeResults(child);
		}

		public event EventHandler StartTests;

		protected void OnStartTests()
		{
			this.testTimer.Start();
			if (this.StartTests!=null)
				StartTests(this,new EventArgs());
		}

		public event EventHandler FinishTests;

		protected void OnFinishTests()
		{
			this.testTimer.Stop();
			if (this.FinishTests!=null)
				FinishTests(this,new EventArgs());
		}
		#endregion

		#region Context menu
		private void addAssemblyItem_Click(object sender, System.EventArgs e)
		{
			this.AddAssembliesByDialog();		
		}

		private void removeAssembliesItem_Click(object sender, System.EventArgs e)
		{
			this.RemoveAssemblies();		
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			this.ThreadedRunTests();
		}

		private void stopTestsMenuItem_Click(object sender, System.EventArgs e)
		{
			this.AbortWorkerThread();
		}

		private void loadConfigItem_Click(object sender, System.EventArgs e)
		{		
			this.LoadProjectByDialog();
		}
		private void saveConfigItem_Click(object sender, System.EventArgs e)
		{
			this.SaveProjectByDialog();
		}
		private void menuItemGenerateXml_Click(object sender, System.EventArgs e)
		{
			this.GenerateXmlReport();
		}
		private void menuItemGenerateHtml_Click(object sender, System.EventArgs e)
		{
			this.GenerateHtmlReport();
		}

		private void textReportMenuItem_Click(object sender, System.EventArgs e)
		{
			this.GenerateTextReport();		
		}

		private void doxReportMenuItem_Click(object sender, System.EventArgs e)
		{
			this.GenerateDoxReport();
		}
		private void createNAntTaskMenuItem_Click(object sender, System.EventArgs e)
		{
			this.CreateNAntTask();
		}
        private void createMSBuildTaskMenuItem_Click(object sender, System.EventArgs e)
        {
            this.CreateMSBuildTask();
        }
        private void reloadAssembliesMenuItem_Click(object sender, System.EventArgs e)
        {
			this.ReloadAssemblies();
		}
		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			this.ClearAllResults();	
		}
		private void nocoverToClipboard_Click(object sender, System.EventArgs e)
		{
			this.CreateNCoverTask();
		}

		#endregion

		#region Projects

		public void LoadProjectByDialog()
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Title = "Load MbUnit Project file";
			dlg.Multiselect = false;
			dlg.Filter = "MbUnit project files (*.mbunit)|*.mbunit|Xml files (*.xml)|*.xml|All files (*.*)|*.*";
			
			if (dlg.ShowDialog() != DialogResult.OK)
				return;

			LoadProject(dlg.FileName, false);
            
            // populate tree
            this.ThreadedPopulateTree(false);
		}

		public void LoadProject(string fileName, bool silent)
		{
			if (!File.Exists(fileName))
			{
                if (!silent)
                {
                    MessageBox.Show("Could not find file");
                }
				return;
			}

			try
			{
				this.MessageOnStatusBar("Loading {0}",fileName);
				MbUnitProject project = MbUnitProject.Load(fileName);
				// removing assemblies
				this.RemoveAssemblies();
				// adding assemblies
				foreach(string testFilePath in project.Assemblies)
				{
					this.AddAssembly(testFilePath);
				}
				// setting state
				this.state.Load(project.TreeState);
			}
			catch(Exception ex)
			{
                if (ex is System.Threading.ThreadAbortException)
                    return;

				MessageBox.Show(ex.ToString());
			}
		}

		public void SaveProjectByDialog()
		{
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Title = "Save Configuration file";
			dlg.Filter = "MbUnit project files (*.mbunit)|*.mbunit|Xml files (*.xml)|*.xml|All files (*.*)|*.*";
			dlg.DefaultExt="mbunit";
			
			if (dlg.ShowDialog() != DialogResult.OK)
				return;

			SaveProject(dlg.FileName);		
		}

		public void SaveProject(string fileName)
		{
			MbUnitProject project = new MbUnitProject();
			foreach(TestDomain domain in this.TestDomains)
				project.Assemblies.Add(domain.TestFilePath);
			this.state.Save();
			project.TreeState = this.state.GetTreeViewState();

			project.Save(fileName);
		}

		#endregion

		#region Reports

		public void GenerateXmlReport()
		{
			// create report
            string outputPath = XmlReport.RenderToXml(this.TestDomains.GetReport());
            try
            {
                System.Diagnostics.Process.Start(outputPath);
            }
            catch (Win32Exception)
            {
                MessageBox.Show("An error has occurred while trying to load the default xml viewer.  Please ensure the viewer is setup correctly.", "Viewer loading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

		public void GenerateHtmlReport()
		{
            string outputPath = HtmlReport.RenderToHtml(this.TestDomains.GetReport());
            try
            {
                System.Diagnostics.Process.Start(outputPath);
            }
            catch (Win32Exception)
            {
                MessageBox.Show("An error has occurred while trying to load the default browswer.  Please ensure the browser is setup correctly.", "Browser loading error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void GenerateTextReport()
		{
            string outputPath = TextReport.RenderToText(this.TestDomains.GetReport());
            System.Diagnostics.Process.Start(outputPath);
        }

        public void GenerateDoxReport()
		{
            string outputPath = DoxReport.RenderToDox(this.TestDomains.GetReport());
            System.Diagnostics.Process.Start(outputPath);
        }

		#endregion

		#region NAnt, NCover, etc...
		public void CreateNAntTask()
		{
			XmlDocument doc = new XmlDocument();
            
            // create project
            XmlElement project = doc.CreateElement("project");
            project.SetAttribute("default", "tests");
            doc.AppendChild(project);

            XmlElement target = doc.CreateElement("target");
            target.SetAttribute("name", "tests");
            project.AppendChild(target);

            // root element
			XmlElement mbunit = doc.CreateElement("mbunit");
			target.AppendChild(mbunit);

            // add fileset
			XmlElement fileset = doc.CreateElement("assemblies");
            mbunit.SetAttribute("report-types", "html");
            mbunit.AppendChild(fileset);

			// add include for each assembly
			foreach(string testFilePath in this.TestDomains.TestFilePaths)
			{
				XmlElement includes = doc.CreateElement("includes");
				includes.SetAttribute("asis","true");
				includes.SetAttribute("name",testFilePath);
				fileset.AppendChild(includes);
			}

			// render to clipboard
			StringWriter writer = new StringWriter();
			XmlTextWriter xWriter = new XmlTextWriter(writer);
			xWriter.Formatting = Formatting.Indented;
			doc.Save(xWriter);
			Clipboard.SetDataObject(writer.ToString(),true);
		}

        public void CreateMSBuildTask()
        {
            XmlDocument doc = new XmlDocument();

            // create project
            XmlElement project = doc.CreateElement("Project");
            project.SetAttribute("xmlns", "http://schemas.microsoft.com/developer/msbuild/2003");
            doc.AppendChild(project);

            // add using task
            XmlElement usingTask = doc.CreateElement("UsingTask");
            usingTask.SetAttribute("TaskName", "MbUnit.MSBuild.Tasks.MbUnit");
            usingTask.SetAttribute("AssemblyFile", "MbUnit.MSBuild.Tasks.dll");
            project.AppendChild(usingTask);

            XmlElement itemGroup = doc.CreateElement("ItemGroup");
            project.AppendChild(itemGroup);

            // add include for each assembly
            foreach (string testFilePath in this.TestDomains.TestFilePaths)
            {
                XmlElement includes = doc.CreateElement("TestAssemblies");
                includes.SetAttribute("Include", testFilePath);
                itemGroup.AppendChild(includes);
            }

            XmlElement target = doc.CreateElement("Target");
            target.SetAttribute("Name", "Tests");
            project.AppendChild(target);

            // root element
            XmlElement mbunit = doc.CreateElement("MbUnit");
            mbunit.SetAttribute("Assemblies", "@(TestAssemblies)");
            mbunit.SetAttribute("ReportTypes", "Html");
            target.AppendChild(mbunit);

            // render to clipboard
            StringWriter writer = new StringWriter();
            XmlTextWriter xWriter = new XmlTextWriter(writer);
            xWriter.Formatting = Formatting.Indented;
            doc.Save(xWriter);
            Clipboard.SetDataObject(writer.ToString(), true);
        }

        public void CreateNCoverTask()
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
