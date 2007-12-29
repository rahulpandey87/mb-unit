using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	public class VertexRenderer : IVertexRenderer
	{
		private SolidBrush brush = new SolidBrush(Color.Black);
		private float radius = 4;
		
		public Color FillColor
		{
			get
			{
				return this.brush.Color;
			}
			set
			{
				this.brush=new SolidBrush(value);
			}
		}
		
		public float Radius
		{
			get
			{
				return this.radius;
			}
			set
			{
				this.radius=value;
			}
		}

		public virtual void PreRender()
		{}

		public virtual void Render(Graphics g, IVertex u, PointF p)
		{
			g.FillEllipse(this.brush, 
				p.X-radius, p.Y-radius, 2*radius, 2*radius
				);
		}
	}
}
