using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MbUnit.Forms
{
	/// <summary>
	/// Summary description for TestResultControl.
	/// </summary>
	public sealed class TestResultControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage consoleOutTabPage;
		private System.Windows.Forms.TabPage consoleErrorTabPage;
		private System.Windows.Forms.TabPage exceptionTabPage;
		private MbUnit.Forms.ConsoleTextBox outBox;
		private MbUnit.Forms.ConsoleTextBox errorBox;
		private MbUnit.Forms.ExceptionBrowser exBrowser;
		private ReflectorTreeView treeView=null;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestResultControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.outBox=new ConsoleTextBox();
			this.errorBox=new ConsoleTextBox();
			this.exBrowser =new ExceptionBrowser();

			this.outBox.Dock=DockStyle.Fill;
			this.outBox.ConsoleStream = ConsoleStream.Out;
			this.consoleOutTabPage.Controls.Add(this.outBox);

			this.errorBox.Dock=DockStyle.Fill;
			this.errorBox.ConsoleStream = ConsoleStream.Error;
			this.consoleErrorTabPage.Controls.Add(this.errorBox);

			this.exBrowser.Dock=DockStyle.Fill;
			this.exceptionTabPage.Controls.Add(this.exBrowser);
		}

		public ReflectorTreeView TreeView
		{
			get
			{
				return this.treeView;
			}
			set
			{
				this.treeView=value;
				this.outBox.Tree=this.treeView;
				this.errorBox.Tree=this.treeView;
				this.exBrowser.Tree=this.treeView;
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.exceptionTabPage = new System.Windows.Forms.TabPage();
			this.consoleOutTabPage = new System.Windows.Forms.TabPage();
			this.consoleErrorTabPage = new System.Windows.Forms.TabPage();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.exceptionTabPage);
			this.tabControl1.Controls.Add(this.consoleOutTabPage);
			this.tabControl1.Controls.Add(this.consoleErrorTabPage);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(728, 408);
			this.tabControl1.TabIndex = 0;
			// 
			// exceptionTabPage
			// 
			this.exceptionTabPage.Location = new System.Drawing.Point(4, 22);
			this.exceptionTabPage.Name = "exceptionTabPage";
			this.exceptionTabPage.Size = new System.Drawing.Size(720, 382);
			this.exceptionTabPage.TabIndex = 2;
			this.exceptionTabPage.Text = "Exception";
			// 
			// consoleOutTabPage
			// 
			this.consoleOutTabPage.Location = new System.Drawing.Point(4, 22);
			this.consoleOutTabPage.Name = "consoleOutTabPage";
			this.consoleOutTabPage.Size = new System.Drawing.Size(720, 246);
			this.consoleOutTabPage.TabIndex = 0;
			this.consoleOutTabPage.Text = "Console.Out";
			// 
			// consoleErrorTabPage
			// 
			this.consoleErrorTabPage.Location = new System.Drawing.Point(4, 22);
			this.consoleErrorTabPage.Name = "consoleErrorTabPage";
			this.consoleErrorTabPage.Size = new System.Drawing.Size(720, 246);
			this.consoleErrorTabPage.TabIndex = 1;
			this.consoleErrorTabPage.Text = "Console.Error";
			// 
			// TestResultControl
			// 
			this.Controls.Add(this.tabControl1);
			this.Name = "TestResultControl";
			this.Size = new System.Drawing.Size(728, 408);
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

        public void ClearResults()
        {
            this.outBox.Clear();
            this.errorBox.Clear();
            this.exBrowser.Clear();
        }
	}
}
