using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Configuration;
using System.Windows.Forms;
using System.Threading;

using MbUnit.Forms;
using MbUnit.Core;
using MbUnit.Core.Cons;
using MbUnit.Core.Cons.CommandLine;

namespace MbUnit.GUI
{
    /// <summary>
	/// Summary description for Form1.
	/// </summary>
	public sealed class MbUnitForm : System.Windows.Forms.Form
	{
        private MbUnitFormArguments arguments;
		private IContainer components = null;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItemFile;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItemFileNew;
		private System.Windows.Forms.MenuItem menuItemFileOpen;
		private System.Windows.Forms.MenuItem menuItemFileSaveAs;
		private System.Windows.Forms.MenuItem menuItemFileExit;
		private System.Windows.Forms.MenuItem menuItem29;
		private System.Windows.Forms.MenuItem menuItemTests;
		private System.Windows.Forms.MenuItem menuItemTestsRun;
		private System.Windows.Forms.MenuItem menuItemTestsStop;
		private System.Windows.Forms.MenuItem menuItemTree;
		private System.Windows.Forms.MenuItem menuItemAssemblies;
		private System.Windows.Forms.MenuItem menuItemAssembliesAddAssemblies;
		private System.Windows.Forms.MenuItem menuItemAssembliesRemoveAssemblies;
		private System.Windows.Forms.MenuItem menuItemAssembliesReload;
		private System.Windows.Forms.MenuItem menuItemReports;
		private System.Windows.Forms.MenuItem menuItemReportsHTML;
		private System.Windows.Forms.MenuItem menuItemReportsXML;
		private System.Windows.Forms.MenuItem menuItemTreeExpandAll;
		private System.Windows.Forms.MenuItem menuItemTreeCollapseAll;
		private System.Windows.Forms.MenuItem menuItemTreeExpandCurrent;
		private System.Windows.Forms.MenuItem menuItemTreeCollapseCurrent;
		private System.Windows.Forms.ImageList toolbarImageList;
		private System.Windows.Forms.MenuItem menuItemTreeExpandCurrentFailures;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItemTreeExpandAllFailures;
		private System.Windows.Forms.MenuItem menuItemTreeExpandAllIgnored;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItemTreeExpandCurrentIgnored;
		private System.Windows.Forms.MenuItem textMenuItem;
		private System.Windows.Forms.MenuItem doxMenuItem;
		private System.Windows.Forms.MenuItem menuItemTreeClearResults;

		private ReflectorTreeView treeView = null;
		private TestRunAndReportControl testResult;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel timePanel;
		private System.Windows.Forms.StatusBarPanel infoPanel;
		private System.Windows.Forms.StatusBarPanel workerThreadPanel;
		private Splitter splitter1;

		private System.Timers.Timer statusBarTimer;

        private string windowTitle = string.Empty;
        
		public MbUnitForm()
		{
			// set title.
			this.SuspendLayout();
			this.treeView=new ReflectorTreeView();
			this.testResult=new TestRunAndReportControl();
			this.splitter1=new Splitter();

			this.treeView.Dock=DockStyle.Left;
			this.splitter1.Dock=DockStyle.Left;
			this.testResult.Dock=DockStyle.Fill;

			this.Controls.AddRange(
				new Control[]{ this.testResult, this.splitter1, this.treeView}
				);

			this.ResumeLayout(true);
			InitializeComponent();

            Assembly exeAssembly = Assembly.GetExecutingAssembly();

            foreach (Attribute a in exeAssembly.GetCustomAttributes(true))
            {
                if (a is AssemblyTitleAttribute)
                    windowTitle = (a as AssemblyTitleAttribute).Title;
            }

            this.Text = String.Format("{0} (on .Net {1})", 
                                        windowTitle,
                                        typeof(Object).GetType().Assembly.GetName().Version
                                        );

            this.testResult.TreeView = this.treeView;

            this.statusBarTimer = new System.Timers.Timer(100);
			this.statusBarTimer.AutoReset=true;
			this.statusBarTimer.Enabled=true;
			this.statusBarTimer.Elapsed+=new System.Timers.ElapsedEventHandler(statusBarTimer_Elapsed);
			this.statusBarTimer.Start();
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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MbUnitForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemFileNew = new System.Windows.Forms.MenuItem();
            this.menuItemFileOpen = new System.Windows.Forms.MenuItem();
            this.menuItemFileSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem29 = new System.Windows.Forms.MenuItem();
            this.menuItemFileExit = new System.Windows.Forms.MenuItem();
            this.menuItemAssemblies = new System.Windows.Forms.MenuItem();
            this.menuItemAssembliesAddAssemblies = new System.Windows.Forms.MenuItem();
            this.menuItemAssembliesRemoveAssemblies = new System.Windows.Forms.MenuItem();
            this.menuItem23 = new System.Windows.Forms.MenuItem();
            this.menuItemAssembliesReload = new System.Windows.Forms.MenuItem();
            this.menuItemTests = new System.Windows.Forms.MenuItem();
            this.menuItemTestsRun = new System.Windows.Forms.MenuItem();
            this.menuItemTestsStop = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItemTree = new System.Windows.Forms.MenuItem();
            this.menuItemTreeExpandAll = new System.Windows.Forms.MenuItem();
            this.menuItemTreeCollapseAll = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.menuItemTreeExpandCurrent = new System.Windows.Forms.MenuItem();
            this.menuItemTreeCollapseCurrent = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.menuItemTreeExpandAllFailures = new System.Windows.Forms.MenuItem();
            this.menuItemTreeExpandCurrentFailures = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItemTreeExpandAllIgnored = new System.Windows.Forms.MenuItem();
            this.menuItemTreeExpandCurrentIgnored = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemTreeClearResults = new System.Windows.Forms.MenuItem();
            this.menuItemReports = new System.Windows.Forms.MenuItem();
            this.menuItemReportsHTML = new System.Windows.Forms.MenuItem();
            this.menuItemReportsXML = new System.Windows.Forms.MenuItem();
            this.textMenuItem = new System.Windows.Forms.MenuItem();
            this.doxMenuItem = new System.Windows.Forms.MenuItem();
            this.toolbarImageList = new System.Windows.Forms.ImageList(this.components);
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.infoPanel = new System.Windows.Forms.StatusBarPanel();
            this.timePanel = new System.Windows.Forms.StatusBarPanel();
            this.workerThreadPanel = new System.Windows.Forms.StatusBarPanel();
            ((System.ComponentModel.ISupportInitialize)(this.infoPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerThreadPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemAssemblies,
            this.menuItemTests,
            this.menuItemTree,
            this.menuItemReports});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFileNew,
            this.menuItemFileOpen,
            this.menuItemFileSaveAs,
            this.menuItem29,
            this.menuItemFileExit});
            this.menuItemFile.Text = "&File";
            // 
            // menuItemFileNew
            // 
            this.menuItemFileNew.Index = 0;
            this.menuItemFileNew.Text = "&New Project";
            this.menuItemFileNew.Click += new System.EventHandler(this.menuItemFileNew_Click);
            // 
            // menuItemFileOpen
            // 
            this.menuItemFileOpen.Index = 1;
            this.menuItemFileOpen.Text = "&Open Project...";
            this.menuItemFileOpen.Click += new System.EventHandler(this.menuItemFileOpen_Click);
            // 
            // menuItemFileSaveAs
            // 
            this.menuItemFileSaveAs.Index = 2;
            this.menuItemFileSaveAs.Text = "&Save Project As...";
            this.menuItemFileSaveAs.Click += new System.EventHandler(this.menuItemFileSaveAs_Click);
            // 
            // menuItem29
            // 
            this.menuItem29.Index = 3;
            this.menuItem29.Text = "-";
            // 
            // menuItemFileExit
            // 
            this.menuItemFileExit.Index = 4;
            this.menuItemFileExit.Text = "&Exit";
            this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
            // 
            // menuItemAssemblies
            // 
            this.menuItemAssemblies.Index = 1;
            this.menuItemAssemblies.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAssembliesAddAssemblies,
            this.menuItemAssembliesRemoveAssemblies,
            this.menuItem23,
            this.menuItemAssembliesReload});
            this.menuItemAssemblies.Text = "&Assemblies";
            // 
            // menuItemAssembliesAddAssemblies
            // 
            this.menuItemAssembliesAddAssemblies.Index = 0;
            this.menuItemAssembliesAddAssemblies.Text = "&Add Assemblies...";
            this.menuItemAssembliesAddAssemblies.Click += new System.EventHandler(this.menuItemAssembliesAddAssemblies_Click);
            // 
            // menuItemAssembliesRemoveAssemblies
            // 
            this.menuItemAssembliesRemoveAssemblies.Index = 1;
            this.menuItemAssembliesRemoveAssemblies.Text = "&Remove Assemblies...";
            this.menuItemAssembliesRemoveAssemblies.Click += new System.EventHandler(this.menuItemAssembliesRemoveAssemblies_Click);
            // 
            // menuItem23
            // 
            this.menuItem23.Index = 2;
            this.menuItem23.Text = "-";
            // 
            // menuItemAssembliesReload
            // 
            this.menuItemAssembliesReload.Index = 3;
            this.menuItemAssembliesReload.Text = "Re&load";
            this.menuItemAssembliesReload.Click += new System.EventHandler(this.menuItemAssembliesReload_Click);
            // 
            // menuItemTests
            // 
            this.menuItemTests.Index = 2;
            this.menuItemTests.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTestsRun,
            this.menuItemTestsStop,
            this.menuItem10});
            this.menuItemTests.Text = "&Tests";
            // 
            // menuItemTestsRun
            // 
            this.menuItemTestsRun.Index = 0;
            this.menuItemTestsRun.Text = "&Run";
            this.menuItemTestsRun.Click += new System.EventHandler(this.menuItemTestsRun_Click);
            // 
            // menuItemTestsStop
            // 
            this.menuItemTestsStop.Index = 1;
            this.menuItemTestsStop.Text = "&Stop";
            this.menuItemTestsStop.Click += new System.EventHandler(this.menuItemTestsStop_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 2;
            this.menuItem10.Text = "-";
            // 
            // menuItemTree
            // 
            this.menuItemTree.Index = 3;
            this.menuItemTree.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemTreeExpandAll,
            this.menuItemTreeCollapseAll,
            this.menuItem16,
            this.menuItemTreeExpandCurrent,
            this.menuItemTreeCollapseCurrent,
            this.menuItem18,
            this.menuItemTreeExpandAllFailures,
            this.menuItemTreeExpandCurrentFailures,
            this.menuItem5,
            this.menuItemTreeExpandAllIgnored,
            this.menuItemTreeExpandCurrentIgnored,
            this.menuItem2,
            this.menuItemTreeClearResults});
            this.menuItemTree.Text = "Tr&ee";
            // 
            // menuItemTreeExpandAll
            // 
            this.menuItemTreeExpandAll.Index = 0;
            this.menuItemTreeExpandAll.Text = "&Expand All";
            this.menuItemTreeExpandAll.Click += new System.EventHandler(this.menuItemTreeExpandAll_Click);
            // 
            // menuItemTreeCollapseAll
            // 
            this.menuItemTreeCollapseAll.Index = 1;
            this.menuItemTreeCollapseAll.Text = "&Collapse All";
            this.menuItemTreeCollapseAll.Click += new System.EventHandler(this.menuItemTreeCollapseAll_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 2;
            this.menuItem16.Text = "-";
            // 
            // menuItemTreeExpandCurrent
            // 
            this.menuItemTreeExpandCurrent.Index = 3;
            this.menuItemTreeExpandCurrent.Text = "Expand C&urrent";
            this.menuItemTreeExpandCurrent.Click += new System.EventHandler(this.menuItemTreeExpandCurrent_Click);
            // 
            // menuItemTreeCollapseCurrent
            // 
            this.menuItemTreeCollapseCurrent.Index = 4;
            this.menuItemTreeCollapseCurrent.Text = "C&ollapse Current";
            this.menuItemTreeCollapseCurrent.Click += new System.EventHandler(this.menuItemTreeCollapseCurrent_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 5;
            this.menuItem18.Text = "-";
            // 
            // menuItemTreeExpandAllFailures
            // 
            this.menuItemTreeExpandAllFailures.Index = 6;
            this.menuItemTreeExpandAllFailures.Text = "Expand &All Failures";
            this.menuItemTreeExpandAllFailures.Click += new System.EventHandler(this.menuItemTreeExpandAllFailures_Click);
            // 
            // menuItemTreeExpandCurrentFailures
            // 
            this.menuItemTreeExpandCurrentFailures.Index = 7;
            this.menuItemTreeExpandCurrentFailures.Text = "Expand Current Fa&ilures";
            this.menuItemTreeExpandCurrentFailures.Click += new System.EventHandler(this.menuItemTreeExpandCurrentFailures_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 8;
            this.menuItem5.Text = "-";
            // 
            // menuItemTreeExpandAllIgnored
            // 
            this.menuItemTreeExpandAllIgnored.Index = 9;
            this.menuItemTreeExpandAllIgnored.Text = "Expand All Ignored";
            this.menuItemTreeExpandAllIgnored.Click += new System.EventHandler(this.menuItemTreeExpandAllIgnored_Click);
            // 
            // menuItemTreeExpandCurrentIgnored
            // 
            this.menuItemTreeExpandCurrentIgnored.Index = 10;
            this.menuItemTreeExpandCurrentIgnored.Text = "Expand Current Ignored";
            this.menuItemTreeExpandCurrentIgnored.Click += new System.EventHandler(this.menuItemTreeExpandCurrentIgnored_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 11;
            this.menuItem2.Text = "-";
            // 
            // menuItemTreeClearResults
            // 
            this.menuItemTreeClearResults.Index = 12;
            this.menuItemTreeClearResults.Text = "Clear Results";
            this.menuItemTreeClearResults.Click += new System.EventHandler(this.menuItemTreeClearResults_Click);
            // 
            // menuItemReports
            // 
            this.menuItemReports.Index = 4;
            this.menuItemReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemReportsHTML,
            this.menuItemReportsXML,
            this.textMenuItem,
            this.doxMenuItem});
            this.menuItemReports.Text = "&Reports";
            // 
            // menuItemReportsHTML
            // 
            this.menuItemReportsHTML.Index = 0;
            this.menuItemReportsHTML.Text = "&HTML";
            this.menuItemReportsHTML.Click += new System.EventHandler(this.menuItemReportsHTML_Click);
            // 
            // menuItemReportsXML
            // 
            this.menuItemReportsXML.Index = 1;
            this.menuItemReportsXML.Text = "&XML";
            this.menuItemReportsXML.Click += new System.EventHandler(this.menuItemReportsXML_Click);
            // 
            // textMenuItem
            // 
            this.textMenuItem.Index = 2;
            this.textMenuItem.Text = "&Text";
            this.textMenuItem.Click += new System.EventHandler(this.textMenuItem_Click);
            // 
            // doxMenuItem
            // 
            this.doxMenuItem.Index = 3;
            this.doxMenuItem.Text = "&Dox";
            this.doxMenuItem.Click += new System.EventHandler(this.doxMenuItem_Click);
            // 
            // toolbarImageList
            // 
            this.toolbarImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.toolbarImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.toolbarImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 619);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.infoPanel,
            this.timePanel,
            this.workerThreadPanel});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(992, 22);
            this.statusBar1.TabIndex = 0;
            // 
            // infoPanel
            // 
            this.infoPanel.MinWidth = 400;
            this.infoPanel.Text = "Info...";
            this.infoPanel.ToolTipText = "Informations";
            this.infoPanel.Width = 400;
            // 
            // timePanel
            // 
            this.timePanel.MinWidth = 150;
            this.timePanel.Text = "Test Time:";
            this.timePanel.ToolTipText = "Duration of the current test";
            this.timePanel.Width = 150;
            // 
            // workerThreadPanel
            // 
            this.workerThreadPanel.MinWidth = 200;
            this.workerThreadPanel.Text = "Worker Thread:";
            this.workerThreadPanel.Width = 200;
            // 
            // MbUnitForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(992, 641);
            this.Controls.Add(this.statusBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "MbUnitForm";
            this.Text = "MbUnit GUI";
            ((System.ComponentModel.ISupportInitialize)(this.infoPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workerThreadPanel)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args) 
		{
            TypeHelper.InvokeFutureStaticMethod(typeof(Application), "EnableVisualStyles");
			Application.DoEvents();
            using (MbUnitForm form = new MbUnitForm())
            {
                form.ParseArguments(args);
                Thread thread = new Thread(new ThreadStart(form.ExecuteArguments));
                thread.Start();
                System.Windows.Forms.Application.Run(form);
            }

            return 100;
        }

		#region Menu Handlers

		private void menuItemFileNew_Click(object sender, System.EventArgs e)
		{
			this.treeView.NewConfig();
		}

		private void menuItemFileOpen_Click(object sender, System.EventArgs e)
		{
			this.treeView.LoadProjectByDialog();
		}

		private void menuItemFileSaveAs_Click(object sender, System.EventArgs e)
		{
			this.treeView.SaveProjectByDialog();
		}

		private void menuItemFileExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItemAssembliesAddAssemblies_Click(object sender, System.EventArgs e)
		{
			this.treeView.AddAssembliesByDialog();		
		}

		private void menuItemAssembliesRemoveAssemblies_Click(object sender, System.EventArgs e)
		{
			this.treeView.RemoveAssemblies();
		}

		private void menuItemAssembliesReload_Click(object sender, System.EventArgs e)
		{
			this.treeView.ReloadAssemblies();
		}

		private void menuItemTestsRun_Click(object sender, System.EventArgs e)
		{
			this.treeView.ThreadedRunTests();		
		}

		private void menuItemTestsStop_Click(object sender, System.EventArgs e)
		{
			this.treeView.AbortWorkerThread();
		}

		private void menuItemTreeExpandAll_Click(object sender, System.EventArgs e)
		{
			this.treeView.TypeTree.ExpandAll();
		}

		private void menuItemTreeCollapseAll_Click(object sender, System.EventArgs e)
		{
			this.treeView.TypeTree.CollapseAll();		
		}

		private void menuItemTreeExpandCurrent_Click(object sender, System.EventArgs e)
		{
			this.treeView.TypeTree.BeginUpdate();
			this.expandChildNode(this.treeView.TypeTree.SelectedNode);
			this.treeView.TypeTree.EndUpdate();
		}

		private void menuItemTreeCollapseCurrent_Click(object sender, System.EventArgs e)
		{
			this.treeView.TypeTree.BeginUpdate();
			this.collapseChildNode(this.treeView.TypeTree.SelectedNode);		
			this.treeView.TypeTree.EndUpdate();
		}

		private void expandChildNode(TreeNode node)
		{
			if (node==null)
				return;
            node.Expand();
            foreach(TreeNode child in node.Nodes)
				expandChildNode(child);
		}

		private void collapseChildNode(TreeNode node)
		{
			if (node==null)
				return;
			node.Collapse();
			foreach(TreeNode child in node.Nodes)
				collapseChildNode(child);
		}

		private void menuItemTreeClearResults_Click(object sender, System.EventArgs e)
		{
			this.treeView.ClearAllResults();
		}

		private void menuItemReportsHTML_Click(object sender, System.EventArgs e)
		{
			this.treeView.GenerateHtmlReport();
		}

		private void menuItemReportsXML_Click(object sender, System.EventArgs e)
		{
			this.treeView.GenerateXmlReport();		
		}

		private void doxMenuItem_Click(object sender, System.EventArgs e)
		{
			this.treeView.GenerateDoxReport();
		}

		private void textMenuItem_Click(object sender, System.EventArgs e)
		{
			this.treeView.GenerateTextReport();		
		}

		private void menuItemTreeExpandAllFailures_Click(object sender, System.EventArgs e)
		{
			this.treeView.ExpandAllFailures();
		}

		private void menuItemTreeExpandCurrentFailures_Click(object sender, System.EventArgs e)
		{
			this.treeView.ExpandCurrentFailures();
		}

		private void menuItemTreeExpandAllIgnored_Click(object sender, System.EventArgs e)
		{
			this.treeView.ExpandAllIgnored();		
		}

		private void menuItemTreeExpandCurrentIgnored_Click(object sender, System.EventArgs e)
		{
			this.treeView.ExpandCurrentIgnored();
		}
		#endregion

        public delegate void LoadProjectDelegate(string fileName);
        private void LoadProjectInvoker(string fileName)
        {
            this.treeView.LoadProject(fileName);
        }

        public void ExecuteArguments()
        {
            if (this.arguments==null)
                return;
            System.Threading.Thread.Sleep(500);

            try
            {
                if (this.arguments.Help)
                {
                    MessageBox.Show(
                        CommandLineUtility.CommandLineArgumentsUsage(typeof(MbUnitFormArguments))
                        );
                }

                // load fiels or project if possible
                if (this.arguments.Files != null)
                {
                    foreach (string fileName in arguments.Files)
                    {
                        if (fileName.ToLower().EndsWith(".mbunit"))
                        {
                            this.Invoke(new LoadProjectDelegate(this.LoadProjectInvoker),
                                new object[] { fileName });
                            break;
                        }
                        this.treeView.AddAssembly(fileName);
                    }
                }

                // populate tree
                this.treeView.ThreadedPopulateTree(false);
                while (treeView.WorkerThreadAlive)
                {
                    System.Threading.Thread.Sleep(100);
                }

                // run
                if (this.arguments.Run)
                {
                    this.treeView.ThreadedRunTests();
                    while (treeView.WorkerThreadAlive)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }

                // generate report
                foreach(ReportType reportType in this.arguments.ReportTypes)
                {
                    switch(reportType)
                    {
                        case ReportType.Html:
                            this.treeView.GenerateHtmlReport(); break;
                        case ReportType.Text:
                            this.treeView.GenerateTextReport(); break;
                        case ReportType.Dox:
                            this.treeView.GenerateDoxReport(); break;
                        case ReportType.Xml:
                            this.treeView.GenerateXmlReport(); break;
                    }
                }

                // exit
                if (this.arguments.Close)
                {
                    System.Threading.Thread.Sleep(1000);
                    while (treeView.WorkerThreadAlive)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failure while executing arguments");
                Console.WriteLine(ex.ToString());
                throw new ApplicationException("Failure while executing arguments", ex);
            }
        }

        public void ParseArguments(string[] args)
        {
            MbUnit.Core.Monitoring.ConsoleMonitor consoleMonitor = new MbUnit.Core.Monitoring.ConsoleMonitor();
            consoleMonitor.Start();
            try
            {
                this.arguments = new MbUnitFormArguments();
                CommandLineUtility.ParseCommandLineArguments(args, arguments);
            }
            catch (Exception)
            {
                consoleMonitor.Stop();
                MessageBox.Show(consoleMonitor.Out + consoleMonitor.Error);
                return;
            }
            finally
            {
                consoleMonitor.Stop();
            }
        }

        private void UpdateStatusBar()
		{
			this.infoPanel.Text = this.treeView.InfoMessage;
			this.timePanel.Text = String.Format("Test duration: {0:0.0}s",this.treeView.TestDuration);
			this.workerThreadPanel.Text = String.Format("Worker thread: {0}",
				(this.treeView.WorkerThreadAlive) ? "Working" : "Sleeping"
				);
		}

		private void statusBarTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(this.UpdateStatusBar));
                else
                    this.UpdateStatusBar();
            }
            catch { }
		}
	}
}
