using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Algorithms.Layout;

	public class EdgeRenderer : IEdgeRenderer
	{
		private Pen pen = Pens.LightGray;
		
		public Color StrokeColor
		{
			get
			{
				return this.pen.Color;
			}
			set
			{
				this.pen = new Pen(value);
			}
		}
		
		public virtual void Render(Graphics g, IEdge e, PointF sourcePosition, PointF targetPosition)
		{
			g.DrawLine(this.pen, sourcePosition, targetPosition);
		}
	}
}
