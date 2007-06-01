using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Netron;

namespace QuickGraph.Layout.Shapes
{
	/// <summary>
	/// Summary description for BasicShape.
	/// </summary>
	public class BasicShape : Netron.Shape
	{
		private Color       backColor = Color.White;
		private int         opacity   = 100;

		private bool        enableBorder = true;
		private Color       borderColor  = Color.Black;
		private DashStyle   borderStyle  = DashStyle.Solid;
		private float		borderWidth  = 1;

		private bool		enableShadow = true;
		private float		shadowWidth  = 3;
		private Color		shadowColor  = Color.FromArgb(150,200,200,200);

		private bool		enableToolTip = true;
		private String		toolTip = "";

		private bool		sizeDirty = true;

		public BasicShape()
		{
			Rectangle = new RectangleF(0, 0, 120, 40);
		}

		[Category("Appearance")]
		[Description("Background color")]
		public Color BackColor
		{
			get { return backColor; }
			set { backColor = value; }
		}

		[Category("Appearance")]
		[Description("Transparency: 0, transparent, 100, opaque.")]
		public int Opacity
		{
			get { return opacity; }
			set 
			{
				if( value < 0 || value > 100 )
					throw new ArgumentOutOfRangeException("opacity");
				opacity = value; 
			}
		}

		[Category("Appearance")]
		public bool EnableBorder
		{
			get { return enableBorder; }
			set { enableBorder = value; }
		}

		[Category("Appearance")]
		public float BorderWidth
		{
			get { return borderWidth; }
			set { borderWidth = value; }
		}

		[Category("Appearance")]
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; }
		}

		[Category("Appearance")]
		public DashStyle BorderStyle
		{
			get { return borderStyle; }
			set { borderStyle = value; }
		}

		[Category("Appearance")]
		public bool EnableShadow
		{
			get { return enableShadow; }
			set { enableShadow = value; }
		}

		[Category("Appearance")]
		public float ShadowWidth
		{
			get { return shadowWidth; }
			set { shadowWidth = value; }
		}

		[Category("Appearance")]
		public Color ShadowColor
		{
			get { return this.shadowColor;}
			set  {this.shadowColor = value;}
		}

		[Category("Appearance")]
		public bool EnableToolTip
		{
			get { return enableToolTip; }
			set { enableToolTip = value; }
		}

		[Category("Appearance")]
		public String ToolTip
		{
			get { return toolTip; }
			set { toolTip = value; }
		}

		[Browsable(false)]
		public bool SizeDirty
		{
			get{	return this.sizeDirty;}
			set{	this.sizeDirty = value;}
		}

		public virtual void ResetColors()
		{
			BasicShape shape = new BasicShape();
			this.backColor = shape.BackColor;
			this.borderColor = shape.BorderColor;
		}

		public virtual void FitSize(Graphics g)
		{
			this.sizeDirty =false;
		}
	}
}
