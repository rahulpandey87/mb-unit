using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MbUnit.Forms
{
	using MbUnit.Core;
	using MbUnit.Core.Collections;
	using MbUnit.Framework;
	using MbUnit.Core.Invokers;
	using MbUnit.Core.Remoting;
	using MbUnit.Core.Reports.Serialization;
	using MbUnit.Core.Monitoring;

	public sealed class TestProgressControl : Panel
	{
		private ReflectorTreeView tree = null;
		private ReportCounter counter = new ReportCounter();

		private System.Timers.Timer paintTimer;

		private Color successColor = Color.Green;
		private Color failureColor = Color.Red;
        private Color skipColor = Color.Violet;
        private Color ignoreColor = Color.Yellow;
		private Color textColor = Color.Black;
		private Font textFont = new Font("Verdana",8);

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
            

		public TestProgressControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.paintTimer = new System.Timers.Timer(100);
			this.paintTimer.AutoReset=true;
			this.paintTimer.Enabled=false;
			this.paintTimer.Elapsed+=new System.Timers.ElapsedEventHandler(paintTimer_Elapsed);

			this.BackColor = Color.White;
			this.ResetTests();
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint,true);
			this.SetStyle(ControlStyles.ResizeRedraw,true);
		}

		[Category("Appearance")]
		public Color SuccessColor
		{
			get
			{
				return this.successColor;
			}
			set
			{
				this.successColor = value;
			}
		}

		[Category("Appearance")]
		public Color FailureColor
		{
			get
			{
				return this.failureColor;
			}
			set
			{
				this.failureColor = value;
			}
		}
        [Category("Appearance")]
        public Color SkipColor
        {
            get
            {
                return this.skipColor;
            }
            set
            {
                this.skipColor = value;
            }
        }
        [Category("Appearance")]
		public Color IgnoreColor
		{
			get
			{
				return this.ignoreColor;
			}
			set
			{
				this.ignoreColor = value;
			}
		}


		public ReportCounter Counter
		{
			get
			{
				return this.counter;
			}
		}	

		[Browsable(false)]
		public ReflectorTreeView Tree
		{
			get
			{
				return this.tree;
			}
			set
			{
				if (this.tree!=null && value!=this.tree)
				{
					this.tree.StartTests -=new EventHandler(tree_StartTests);
					this.tree.FinishTests-=new EventHandler(tree_FinishTests);
					this.tree.TreePopulated -=new EventHandler(this.TreePopulated);
                    this.tree.TreeCleared -= new EventHandler(this.TreePopulated);
                    this.tree.Facade.Updated-=new ResultEventHandler(Facade_Updated);
				}
				this.tree = value;
				if (this.tree!=null)
				{
					this.tree.StartTests +=new EventHandler(tree_StartTests);
					this.tree.FinishTests+=new EventHandler(tree_FinishTests);
					this.tree.TreePopulated +=new EventHandler(this.TreePopulated);
                    this.tree.TreeCleared += new EventHandler(this.TreePopulated);
                    this.tree.Facade.Updated+=new ResultEventHandler(Facade_Updated);
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
			// 
			// TestProgressControl
			// 
			this.Name = "TestProgressControl";
			this.Size = new System.Drawing.Size(24, 280);
		}
		#endregion

		public void ResetTests()
		{
			this.Counter.Clear();
			if (this.tree!=null)
			{
				this.counter = this.Tree.TestDomains.GetTestCount();
			}
		}

		private void TreePopulated(object sender, EventArgs e)
		{
			this.ResetTests();
			this.Invoke(new MethodInvoker(this.Invalidate));
		}

		private void tree_StartTests(object sender, EventArgs e)
		{
			this.Counter.SuccessCount = 0;
			this.Counter.FailureCount = 0;
            this.Counter.SkipCount = 0;
            this.Counter.IgnoreCount = 0;
            if(!this.paintTimer.Enabled)
				this.paintTimer.Enabled=true;
			this.paintTimer.Start();		
		}

		private void tree_FinishTests(object sender, EventArgs e)
		{
			this.paintTimer.Stop();
			this.paintTimer.Enabled=false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
            
			int alpha = 200;
			SolidBrush backBrush = new SolidBrush(this.BackColor);
			SolidBrush textBrush =new SolidBrush(FromColor(this.textColor,alpha));

			Rectangle r = this.ClientRectangle;
			r = new Rectangle(
				r.Location,
				new Size(r.Width-1,r.Height-1)
				);

			// draw back
			e.Graphics.FillRectangle(backBrush,r);

			// saving state
			SmoothingMode m = e.Graphics.SmoothingMode;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			if (this.Counter.RunCount!=0)
			{
				// draw boxes
				float width = r.Width * (this.Counter.SuccessCount/(float)this.Counter.RunCount);
				float left = r.Left;
				float right = r.Left+width;
				DrawBox(e.Graphics,r,left,width,successColor);

                //failures
				width = r.Width*(this.Counter.FailureCount/(float)this.Counter.RunCount);
				left = right;
				right = left + width;
				DrawBox(e.Graphics,r,left,width,failureColor);

                //skipped
                width = r.Width * (this.Counter.SkipCount / (float)this.Counter.RunCount);
                left = right;
                right = left + width;
                DrawBox(e.Graphics, r, left, width, skipColor);

                // ignored
				width = r.Width*(this.Counter.IgnoreCount/(float)this.Counter.RunCount);
				left = right;
				right = left +width;
				DrawBox(e.Graphics,r,left,width,ignoreColor);
			}
			e.Graphics.DrawRectangle(Pens.Black,r);

			// draw text
            if (this.tree != null)
            {
                string text = string.Format(
                    "{0} tests - {1} successes - {2} failures  - {3} skipped - {4} ignored - {5:0.0}s",
                    this.Counter.RunCount,
                    this.Counter.SuccessCount,
                    this.Counter.FailureCount,
                    this.Counter.SkipCount,
                    this.Counter.IgnoreCount,
                    this.tree.TestDuration
                    );

                StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString(
                    text,
                    this.textFont,
                    textBrush,
                    r.Left + r.Width / 2,
                    r.Top + r.Height / 2,
                    format
                    );
                e.Graphics.SmoothingMode = m;
            }
        }

		private void DrawBox(Graphics g, Rectangle r, float left, float width, Color c)
		{
			if (width==0)
				return;
			RectangleF re = new RectangleF(
				left,
				r.Y,
				width, 
				r.Height);

			LinearGradientBrush brush = new LinearGradientBrush(
				re,
				FromColor(c,225),
				FromColor(c,75),
				45,true);

			g.FillRectangle(brush,re);
		}

		private Color FromColor(Color c, int alpha)
		{
			return Color.FromArgb(
				alpha,
				c.R,
				c.G,
				c.B
				);
		}

		private void Facade_Updated(ResultEventArgs args)
		{
			switch(args.State)
			{
				case TestState.Failure:
					this.counter.FailureCount++;break;
				case TestState.Ignored:
					this.counter.IgnoreCount++;break;
				case TestState.Success:
					this.Counter.SuccessCount++;break;
                case TestState.Skip:
                    this.Counter.SkipCount++; break;
            }
			this.Invoke(new MethodInvoker(this.Invalidate));
		}

		private void paintTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			this.Invoke(new MethodInvoker(this.Invalidate));
		}
	}
}
