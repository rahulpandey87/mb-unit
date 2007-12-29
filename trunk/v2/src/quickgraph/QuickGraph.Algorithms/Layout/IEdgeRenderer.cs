using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Algorithms.Layout;

	public interface IEdgeRenderer
	{
		void Render(Graphics g, IEdge e, PointF sourcePosition, PointF targetPosition);		
	}
}
