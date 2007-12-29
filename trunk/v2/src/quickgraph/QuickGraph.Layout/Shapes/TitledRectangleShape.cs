using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace QuickGraph.Layout.Shapes
{
	/// <summary>
	/// Summary description for TitleShape.
	/// </summary>
	public class TitledRectangleShape : RectangleShape
	{
		private String title = "Untitled";
		private Font   titleFont = new Font("Tahoma",9f);
		private Color  titleColor = Color.Black;
		private Color  titleBackColor = Color.WhiteSmoke;
		private SizeF  titlePadding = new SizeF(3,3);

		private bool	enableTitleIcon = true;
		private Color	iconBackColor = Color.Transparent;

		private SizeF	titleSize;

		[Category("Appearance")]
		public String Title
		{
			get { return title; }
			set 
			{ 
				if(value==title)
					return;
				this.SizeDirty=true;
				title = value; 
			}
		}

		[Category("Appearance")]
		public Font TitleFont
		{
			get { return titleFont; }
			set 
			{ 
				if(value==titleFont)
					return;
				this.SizeDirty=true;
				titleFont = value; 
			}
		}

		[Category("Appearance")]
		public Color TitleColor
		{
			get { return titleColor; }
			set { titleColor = value; }
		}

		[Category("Appearance")]
		public Color TitleBackColor
		{
			get { return titleBackColor; }
			set { titleBackColor = value; }
		}

		[Category("Appearance")]
		public SizeF TitlePadding
		{
			get { return titlePadding; }
			set 
			{
				if(value==titlePadding)
					return;
				this.SizeDirty=true;
				titlePadding = value; 
			}
		}

		[Category("Appearance")]
		public bool EnableTitleIcon
		{
			get { return enableTitleIcon; }
			set 
			{ 
				if (this.enableTitleIcon==value)
					return;
				this.SizeDirty=true;
				enableTitleIcon = value; 
			}
		}

		[Category("Appearance")]
		public Color IconBackColor
		{
			get {return iconBackColor;}
			set {iconBackColor=value;}
		}

		[Browsable(false)]
		public SizeF TitleSize
		{
			get{return this.titleSize;}
			set{this.titleSize=value;}
		}

		public override void ResetColors()
		{
			base.ResetColors();
			TitledRectangleShape shape = new TitledRectangleShape();
			this.titleBackColor = shape.titleBackColor;
			this.titleColor = shape.titleColor;
		}


		protected virtual void DrawTitle(Graphics g, RectangleF r)
		{
			SolidBrush brush = new SolidBrush(this.titleColor);
			SolidBrush backBrush = new SolidBrush(this.titleBackColor);

			float yCur = r.Top + this.TitlePadding.Height;
			float xCur = r.X + this.TitlePadding.Width;

			// draw background
			g.FillRectangle(backBrush,
				r.Left,r.Top,
				r.Width,this.TitleSize.Height
				);

			if (this.EnableTitleIcon && this.Icon!=null)
			{
				if (this.iconBackColor.A>0)
				{
					SolidBrush iconBrush = new SolidBrush(this.iconBackColor);
					g.FillRectangle(brush,
						r.X,r.Y,
						this.Icon.Width+this.TitlePadding.Width,
						this.TitleSize.Height
						);
				}

				g.DrawImage(this.Icon.ToBitmap(),
					xCur,
					yCur
					);
				xCur += this.Icon.Width+this.TitlePadding.Width*2;
			}

			g.DrawString(
				this.title,
				this.titleFont,
				brush,
				xCur,
				yCur
				);
		}	

		public override void FitSize(Graphics g)
		{
			base.FitSize (g);
			titleSize = g.MeasureString(this.title,this.titleFont);
			// apply padding...
			titleSize.Width += this.titlePadding.Width*2;
			titleSize.Height += this.titlePadding.Height*2;

			// check if icon is added to the title
			if (this.EnableTitleIcon && this.Icon!=null)
			{
				titleSize.Width += this.Icon.Width + this.titlePadding.Width*2;
				titleSize.Height = Math.Max(
					titleSize.Height,
					this.Icon.Height + this.titlePadding.Height*2
					);
			}

			this.Size = this.titleSize;
		}

	}
}
