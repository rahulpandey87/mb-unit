using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core;

namespace MbUnit.Forms
{
	/// <summary>
	/// Summary description for TestRunControl.
	/// </summary>
	public sealed class TestRunControl : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.Button stopButton;
        private TestProgressControl testProgressBar;
        private ReflectorTreeView treeView = null;
        /// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private EnableControlDelegate controlEnabler;

        public TestRunControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            this.controlEnabler= new EnableControlDelegate(this.EnableControl);
        }

        public ReflectorTreeView TreeView
		{
			get
			{
				return this.treeView;
			}
			set
			{
				if (this.treeView!=null)
				{
                    this.treeView.BeginLoadTests-=new EventHandler(treeView_BeginLoadTests);
                    this.treeView.TreePopulated-=new EventHandler(treeView_TreePopulated);
					this.treeView.StartTests-=new EventHandler(treeView_StartTests);
					this.treeView.FinishTests-=new EventHandler(treeView_FinishTests);
				}
				this.treeView=value;
                this.testProgressBar.Tree = value;
                if (this.treeView!=null)
				{
                    this.treeView.BeginLoadTests += new EventHandler(treeView_BeginLoadTests);
                    this.treeView.TreePopulated += new EventHandler(treeView_TreePopulated);
                    this.treeView.StartTests += new EventHandler(treeView_StartTests);
                    this.treeView.FinishTests+=new EventHandler(treeView_FinishTests);
				}
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.testProgressBar = new MbUnit.Forms.TestProgressControl();
            this.stopButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.testProgressBar);
            this.groupBox1.Controls.Add(this.stopButton);
            this.groupBox1.Controls.Add(this.runButton);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 112);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tests";
            // 
            // testProgressBar
            // 
            this.testProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.testProgressBar.BackColor = System.Drawing.Color.White;
            this.testProgressBar.FailureColor = System.Drawing.Color.Red;
            this.testProgressBar.IgnoreColor = System.Drawing.Color.Yellow;
            this.testProgressBar.Location = new System.Drawing.Point(8, 80);
            this.testProgressBar.Name = "testProgressBar";
            this.testProgressBar.Size = new System.Drawing.Size(464, 23);
            this.testProgressBar.SkipColor = System.Drawing.Color.Violet;
            this.testProgressBar.SuccessColor = System.Drawing.Color.Green;
            this.testProgressBar.TabIndex = 2;
            this.testProgressBar.Tree = null;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(119, 24);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(104, 40);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // runButton
            // 
            this.runButton.Enabled = false;
            this.runButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.runButton.Location = new System.Drawing.Point(8, 24);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(104, 40);
            this.runButton.TabIndex = 0;
            this.runButton.Text = "Run";
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // TestRunControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "TestRunControl";
            this.Size = new System.Drawing.Size(480, 112);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		private void runButton_Click(object sender, System.EventArgs e)
		{
			this.treeView.ThreadedRunTests();
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(this.EnableStopButton));
            else
                this.EnableStopButton();
        }

		private void stopButton_Click(object sender, System.EventArgs e)
		{
			this.treeView.AbortWorkerThread();
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(this.EnableRunButton));
            else
                this.EnableRunButton();
        }

        private void treeView_FinishTests(object sender, EventArgs e)
		{
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(this.EnableRunButton));
            else
                this.EnableRunButton();
        }

		private void treeView_StartTests(object sender, EventArgs e)
		{
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(this.EnableStopButton));
            else
                this.EnableStopButton();
        }

        private void EnableRunButton()
        {
            this.stopButton.Enabled = false;
            this.runButton.Enabled = true;
        }

        private void EnableResumeButton()
        {
            this.stopButton.Enabled = false;
            this.runButton.Enabled = false;
        }

        private void EnableStopButton()
        {
            this.stopButton.Enabled = true;
            this.runButton.Enabled = false;
        }

        private void DisableButtons()
        {
            this.stopButton.Enabled = false;
            this.runButton.Enabled = false;
        }

        void treeView_BeginLoadTests(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(new MethodInvoker(this.DisableButtons));
            else
                this.DisableButtons();
        }

        void treeView_TreePopulated(object sender, EventArgs e)
        {
            if (this.treeView.Nodes.Count > 0)
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(this.EnableRunButton));
                else
                    this.EnableRunButton();
            }
        }

        public delegate void EnableControlDelegate(Control control, bool enabledState);

        void EnableControl(Control control, bool enabledState)
        {
            control.Enabled = enabledState;
        }
    }
}
