using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace QuickGraph.Layout.Shapes
{
	public class RectangleShape : BasicShape
	{
		protected virtual void DrawBack(Graphics g, RectangleF r)
		{
			if (this.EnableShadow)
				FillShadow(g,r);

			SolidBrush brush = new SolidBrush(this.BackColor);
			g.FillRectangle(brush,r);
		}

		protected virtual void DrawBorder(Graphics g, RectangleF r)
		{
			Pen pen = new Pen(this.BorderColor,this.BorderWidth);
			pen.DashStyle = this.BorderStyle;

			SmoothingMode m = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			g.DrawRectangle(pen,r.X,r.Y,r.Width,r.Height);

			g.SmoothingMode = m;
		}

		protected virtual void FillShadow(Graphics g, RectangleF r)
		{
			Color sc = Color.FromArgb(100,
				this.ShadowColor.R,
				this.ShadowColor.G,
				this.ShadowColor.B
				);
			SolidBrush brush = new SolidBrush(sc);

			SmoothingMode m = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.AntiAlias;

			GraphicsContainer cont = g.BeginContainer();
				g.TranslateTransform(this.ShadowWidth,this.ShadowWidth);
				g.FillRectangle(brush,r.X,r.Y,r.Width,r.Height);
			g.EndContainer(cont);
			g.SmoothingMode = m;
		}
	}
}
