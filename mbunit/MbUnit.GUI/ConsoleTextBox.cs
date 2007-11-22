using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MbUnit.Forms
{
	using MbUnit.Core;
	using MbUnit.Core.Remoting;
    using MbUnit.Core.Reports.Serialization;

    public enum ConsoleStream
	{
		Out,
		Error
	}

	public sealed class ConsoleTextBox : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.RichTextBox textBox;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;

		private ConsoleStream consoleStream=ConsoleStream.Out;
		private ReflectorTreeView tree = null;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ConsoleTextBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		public override String Text
		{
			get
			{
				return this.textBox.Text;
			}
			set
			{
				this.textBox.Text = value;
			}
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
			this.textBox = new System.Windows.Forms.RichTextBox();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.BackColor = System.Drawing.Color.White;
			this.textBox.ContextMenu = this.contextMenu1;
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.Name = "textBox";
			this.textBox.ReadOnly = true;
			this.textBox.Size = new System.Drawing.Size(440, 368);
			this.textBox.TabIndex = 2;
			this.textBox.Text = "";
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1,
																						 this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Save";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "Copy To Clipboard";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// XmlTextBox
			// 
			this.ContextMenu = this.contextMenu1;
			this.Controls.Add(this.textBox);
			this.Name = "XmlTextBox";
			this.Size = new System.Drawing.Size(440, 368);
			this.ResumeLayout(false);

		}
		#endregion

		[Browsable(false)]
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

		[Category("Data")]
		public ConsoleStream ConsoleStream
		{
			get
			{
				return this.consoleStream;
			}
			set
			{
				this.consoleStream = value;
			}
		}

		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
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
            this.textBox.Text = "";
            UnitTreeNode node = this.tree.TypeTree.SelectedNode as UnitTreeNode;
            if (node == null)
                return;

            ReportRun result = this.tree.TestDomains.GetResult(node);
            if (result != null)
            {
                switch (this.consoleStream)
                {
                    case ConsoleStream.Out:
                        this.textBox.Text = result.ConsoleOut;
                        break;
                    case ConsoleStream.Error:
                        this.textBox.Text = result.ConsoleError;
                        break;
                }
            }
            this.Refresh();
        }

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();
			if(dlg.ShowDialog() != DialogResult.OK)
				return;
			using (StreamWriter sw = new StreamWriter(dlg.FileName))
			{
				sw.Write(this.textBox.Text);
			}		
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			Clipboard.SetDataObject( this.textBox.Text );
		}

        public void Clear()
        {
            this.textBox.Text = "";
        }
	}
}
