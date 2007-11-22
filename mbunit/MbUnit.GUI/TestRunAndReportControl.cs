using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MbUnit.Forms
{
	/// <summary>
	/// Summary description for TestRunAndReportControl.
	/// </summary>
	public sealed class TestRunAndReportControl : System.Windows.Forms.UserControl
	{
		private MbUnit.Forms.TestRunControl testRunControl1;
		private MbUnit.Forms.TestResultControl testResultControl1;
		private System.Windows.Forms.Splitter splitter1;
		private ReflectorTreeView treeView;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestRunAndReportControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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
				this.testResultControl1.TreeView=value;
				this.testRunControl1.TreeView=value;
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
			this.testRunControl1 = new MbUnit.Forms.TestRunControl();
			this.testResultControl1 = new MbUnit.Forms.TestResultControl();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.SuspendLayout();
			// 
			// testRunControl1
			// 
			this.testRunControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.testRunControl1.Location = new System.Drawing.Point(0, 0);
			this.testRunControl1.Name = "testRunControl1";
			this.testRunControl1.Size = new System.Drawing.Size(688, 112);
			this.testRunControl1.TabIndex = 0;
			// 
			// testResultControl1
			// 
			this.testResultControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.testResultControl1.Location = new System.Drawing.Point(0, 112);
			this.testResultControl1.Name = "testResultControl1";
			this.testResultControl1.Size = new System.Drawing.Size(688, 280);
			this.testResultControl1.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 112);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(688, 3);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// TestRunAndReportControl
			// 
			this.Controls.Add(this.splitter1);
			this.Controls.Add(this.testResultControl1);
			this.Controls.Add(this.testRunControl1);
			this.Name = "TestRunAndReportControl";
			this.Size = new System.Drawing.Size(688, 392);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
