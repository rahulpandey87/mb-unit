using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MbUnit.Core;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Remoting;
using System.Reflection;

namespace MbUnit.Forms
{
	/// <summary>
	/// Summary description for ExceptionBrowser.
	/// </summary>
	public sealed class ExceptionBrowser : System.Windows.Forms.UserControl
	{
		private ReflectorTreeView tree = null;
		private System.Windows.Forms.TreeView exceptionTreeView;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.RichTextBox textBox1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ExceptionBrowser()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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
            this.exceptionTreeView = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.textBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
// 
// exceptionTreeView
// 
            this.exceptionTreeView.Dock = System.Windows.Forms.DockStyle.Top;
            this.exceptionTreeView.Location = new System.Drawing.Point(0, 0);
            this.exceptionTreeView.Name = "exceptionTreeView";
            this.exceptionTreeView.Size = new System.Drawing.Size(500, 56);
            this.exceptionTreeView.TabIndex = 0;
            this.exceptionTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.exceptionTreeView_AfterSelect);
// 
// splitter1
// 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 56);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(500, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
// 
// textBox1
// 
            this.textBox1.AcceptsTab = true;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 59);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(500, 229);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "";
            this.textBox1.WordWrap = false;
// 
// ExceptionBrowser
// 
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.exceptionTreeView);
            this.Name = "ExceptionBrowser";
            this.Size = new System.Drawing.Size(500, 288);
            this.ResumeLayout(false);

        }
		#endregion

		public ReflectorTreeView Tree
		{
			get
			{
				return this.tree;
			}
			set
			{
                if (this.tree != null && value != this.tree)
                {
                    this.tree.AfterSelect -= new TreeViewEventHandler(treeView_AfterSelect);
                    this.tree.FinishTests -= new EventHandler(treeView_FinishTests);
                }
				this.tree = value;
                if (this.tree != null)
                {
                    this.tree.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
                    this.tree.FinishTests += new EventHandler(treeView_FinishTests);
                }
			}
		}

		public void AddException(ReportException ex)
		{
			this.exceptionTreeView.Nodes.Clear();

			// adding recursively the exceptions
			ReportException current = ex;
			TreeNode parent = null;
			while(current!=null)
			{
				TreeNode node =new TreeNode(current.Type);
				node.Tag = current;

				if (parent==null)
				{
					this.exceptionTreeView.Nodes.Add(node);
					this.exceptionTreeView.SelectedNode = node;
					parent = node;
				}
				else
				{
					parent.Nodes.Add(node);
					parent = node;
				}

				current = current.Exception;
			}

			this.exceptionTreeView.ExpandAll();
			this.RefreshException();
			this.Refresh();
		}

		public void RefreshException()
		{
			this.textBox1.Text="";
			if (this.exceptionTreeView.SelectedNode==null)
			{
				this.textBox1.Text="";
				return;
			}

			ReportException ex = (ReportException)this.exceptionTreeView.SelectedNode.Tag;
			Font boldFont = new Font(this.Font.FontFamily,this.Font.SizeInPoints,FontStyle.Bold);
			Font font = new Font(this.Font.FontFamily,this.Font.SizeInPoints);

			// adding name
            this.textBox1.SelectionFont = boldFont;
            this.textBox1.SelectedText = String.Format("Message: {0}\n\n", ex.Message);
            this.textBox1.SelectionFont = font;
            this.textBox1.SelectedText = String.Format("Type: {0}\n", ex.Type);
            this.textBox1.SelectedText = String.Format("Source: {0}\n",ex.Source);
            foreach(ReportProperty property in ex.Properties)
                this.textBox1.SelectedText = String.Format("{0}: {1}\n", property.Name, property.Value);
            this.textBox1.SelectedText = String.Format("Stack:");
            this.textBox1.SelectedText = String.Format("{0}",ex.StackTrace);
		}

        private void treeView_AfterSelect( object sender, System.Windows.Forms.TreeViewEventArgs e )
        {
            SafelyDisplaySelectedNodeResult();
        }

        private void treeView_FinishTests( object sender, EventArgs e )
        {
            SafelyDisplaySelectedNodeResult();
        }

        private void SafelyDisplaySelectedNodeResult()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(this.DisplaySelectedNodeResult));
                }
                else
                {
                    DisplaySelectedNodeResult();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplaySelectedNodeResult()
        {
            // retreive current exceptoin
            this.exceptionTreeView.Nodes.Clear();
            this.textBox1.Text = "";
            UnitTreeNode node = this.tree.TypeTree.SelectedNode as UnitTreeNode;
            if (node == null)
                return;

            ReportRun result = this.Tree.TestDomains.GetResult(node);
            if (result != null && result.Result == ReportRunResult.Failure)
            {
                AddException(result.Exception);
            }
        }

        private void exceptionTreeView_AfterSelect( object sender, System.Windows.Forms.TreeViewEventArgs e )
        {
            RefreshException();
        }

        public void Clear()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Clear));
            else
            {
                exceptionTreeView.Nodes.Clear();
                textBox1.Clear();
            }
        }
	}
}
