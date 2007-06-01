using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using Netron;
using System.Reflection;
using System.Windows.Forms;

namespace QuickGraph.Layout.Shapes
{
	[Netron.Shape(
		 "Property Grid",
		 Description="A shape containing a table of key-value pairs",
		 Author="Jonathan de Halleux")]
	[Serializable ]
	public class PropertyGridShape : TitledRectangleShape
	{
		private bool	collapseRows = true;
		private Font	rowFont = new Font("Tahoma",8f);
		private Color	rowColor = Color.Black;
		private SizeF	rowPadding = new SizeF(3,3);

		private bool	enableGrid = true;
		private Color	gridColor = Color.Gray;
		private float	gridWidth = 1;
		private DashStyle gridStyle = DashStyle.Solid;

		private ArrayList rows = new ArrayList();

		private Connector west;
		private Connector north;
		private Connector south;
		private Connector east;
		private Connector southWest;
		private Connector northWest;
		private Connector southEast;
		private Connector northEast;

		// box sizes
		private SizeF keySize;
		private SizeF valueSize;
		private SizeF rowSize;
		private SizeF rowsSize;

		public PropertyGridShape()
		{
			this.Resizable = false;

			Assembly a = Assembly.GetExecutingAssembly();
			this.west = new Netron.Connector(this, "West");
			this.Connectors.Add(this.west);
			this.east = new Netron.Connector(this, "East");
			this.Connectors.Add(this.east);
			this.north = new Netron.Connector(this, "North");
			this.Connectors.Add(this.north);
			this.south = new Netron.Connector(this, "South");
			this.Connectors.Add(this.south);
			this.southWest = new Netron.Connector(this, "SouthWest");
			this.Connectors.Add(this.southWest);
			this.northWest = new Netron.Connector(this, "NorthWest");
			this.Connectors.Add(this.northWest);
			this.southEast = new Netron.Connector(this, "SouthEast");
			this.Connectors.Add(this.southEast);
			this.northEast = new Netron.Connector(this, "NorthEast");
			this.Connectors.Add(this.northEast);

		}

		[Category("Appearance")]
		public bool CollapseRows
		{
			get { return this.collapseRows; }
			set 
			{ 
				if (this.collapseRows==value)
					return;
				this.SizeDirty = true;
				this.collapseRows = value; 
			}
		}

		[Category("Appearance")]
		public Color RowColor
		{
			get { return rowColor; }
			set { rowColor = value; }
		}

		[Category("Appearance")]
		public Font RowFont
		{
			get { return rowFont; }
			set 
			{ 
				if(value==rowFont)
					return;
				this.SizeDirty=true;
				rowFont = value; 
			}
		}

		[Category("Appearance")]
		public SizeF RowPadding
		{
			get { return rowPadding; }
			set { rowPadding = value; }
		}

		[Category("Appearance")]
		public bool EnableGrid
		{
			get { return enableGrid; }
			set { enableGrid = value; }
		}

		[Category("Appearance")]
		public Color GridColor
		{
			get { return gridColor; }
			set { gridColor = value; }
		}

		[Category("Appearance")]
		public float GridWidth
		{
			get { return gridWidth; }
			set { gridWidth = value; }
		}

		[Category("Appearance")]
		public DashStyle GridStyle
		{
			get { return gridStyle; }
			set { gridStyle = value; }
		}

		[TypeConverter(typeof(ExpandableObjectConverter))]
		public ArrayList Rows
		{
			get { return this.rows; }
			set { this.rows = value; }
		}

		[Browsable(false)]
		public override ArrayList MenuItems 
		{
			get 
			{
				ArrayList items = base.MenuItems;

				return items;
			}
		}

		public override void ResetColors()
		{
			base.ResetColors();
			PropertyGridShape shape = new PropertyGridShape();
			this.rowColor = shape.rowColor;
			this.gridColor = shape.gridColor;
		}

		public override PointF ConnectionPoint(Netron.Connector c)
		{
			RectangleF r = Rectangle;

			if (c == this.west) 
				return new PointF(r.Left, r.Top + r.Height/2);
			if (c == this.east) 
				return new PointF(r.Right, r.Top + r.Height/2);
			if (c == this.north) 
				return new PointF(r.Left+r.Width/2, r.Top);
			if (c == this.south) 
				return new PointF(r.Left+r.Width/2, r.Bottom);
			if (c==this.southWest)
				return new PointF(r.Left, r.Bottom);
			if (c==this.northWest)
				return new PointF(r.Left, r.Top);
			if (c==this.southEast)
				return new PointF(r.Right, r.Bottom);
			if (c==this.northEast)
				return new PointF(r.Right, r.Top);

			throw new Exception("Unknown connector");
		}

		public override void Paint(Graphics g)
		{
			if (this.SizeDirty)
			{
				FitSize(g);
			}
			RectangleF r = Rectangle;

			DrawBack(g,r);

			DrawTitle(g,r);
			if (!this.collapseRows)
				DrawRows(g,r);
			if (this.EnableGrid)
				DrawGrid(g,r);
			if (this.EnableBorder)
				DrawBorder(g,r);

			base.Paint(g);

		}

		public override void FitSize(Graphics g)
		{
			// compute different box sizes
			// title
			base.FitSize(g);

			if (this.collapseRows)
			{
				this.keySize = SizeF.Empty;
				this.valueSize = SizeF.Empty;
				this.rowSize = SizeF.Empty;
				this.rowSize = SizeF.Empty;
				return;
			}

			// rows
			this.keySize = SizeF.Empty;
			this.valueSize = SizeF.Empty;
			SizeF temp;
			foreach(PropertyEntry de in this.rows)
			{
				temp = g.MeasureString(de.Key,this.rowFont);
				this.keySize = new SizeF(
					Math.Max(this.keySize.Width,temp.Width),
					Math.Max(this.keySize.Height,temp.Height)
					);

				temp = g.MeasureString(de.Value,this.rowFont);
				this.valueSize = new SizeF(
					Math.Max(this.valueSize.Width,temp.Width),
					Math.Max(this.valueSize.Height,temp.Height)
					);
			}
			// apply padding...
			this.keySize.Width += this.rowPadding.Width*2;
			this.keySize.Height += this.rowPadding.Height*2;
			this.valueSize.Width += this.rowPadding.Width*2;
			this.valueSize.Height += this.rowPadding.Height*2;

			// computing the size of the key-value box
			this.rowSize = new SizeF(
				this.keySize.Width + this.valueSize.Width,
				Math.Max(this.keySize.Height,this.valueSize.Height)
				);

			// updating title and rows
			this.rowSize.Width = Math.Max(this.rowSize.Width, this.TitleSize.Width);
			this.TitleSize = new SizeF(this.rowSize.Width,this.TitleSize.Height);

			// rows
			this.rowsSize = new SizeF(
				this.rowSize.Width,this.rowSize.Height*this.rows.Count);

			// adding the title
			this.Size = new SizeF(
				this.rowSize.Width,
				this.TitleSize.Height + this.rowsSize.Height
				);

		}

		protected virtual void DrawGrid(Graphics g, RectangleF r)
		{
			Pen pen = new Pen(this.GridColor,this.GridWidth);
			pen.DashStyle = this.GridStyle;
			
			// horizontal lines
			float yCur = r.Top + this.TitleSize.Height + this.rowSize.Height;
			for(int i = 0;i<this.rows.Count-1;++i)
			{
				g.DrawLine(pen,r.Left,yCur,r.Right,yCur);	
				yCur+=this.rowSize.Height;
			}

			// vertical
			g.DrawLine(pen,
				r.Left+this.keySize.Width,r.Bottom,
				r.Left+this.keySize.Width,r.Top + this.TitleSize.Height );		
		}

		protected virtual void DrawRows(Graphics g, RectangleF r)
		{
			SolidBrush brush = new SolidBrush(this.rowColor);

			float yCur = 
				r.Top 
				+ this.TitleSize.Height 
				+ this.rowPadding.Height/2;
			float xKey = r.Left + this.rowPadding.Width/2;
			float xValue =r.Left + this.keySize.Width + this.rowPadding.Width;

			foreach(PropertyEntry de in this.rows)
			{
				g.DrawString(
					de.Key,
					this.rowFont,
					brush,
					xKey,
					yCur
					);
				g.DrawString(
					de.Value,
					this.rowFont,
					brush,
					xValue,
					yCur
					);

				yCur += this.rowSize.Height;
			}
		}
	}
}
