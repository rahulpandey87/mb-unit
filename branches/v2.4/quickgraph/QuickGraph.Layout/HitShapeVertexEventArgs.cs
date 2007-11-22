using System;
using System.Drawing;

namespace QuickGraph.Layout
{
	using Netron;
	using QuickGraph.Concepts;

	/// <summary>
	/// Summary description for HitShapeVertexEventArgs.
	/// </summary>
	public class HitShapeVertexEventArgs : ShapeVertexEventArgs
	{
		private PointF location;

		public HitShapeVertexEventArgs(Shape shape, IVertex v, PointF location)
			:base(shape,v)
		{
			this.location = location;
		}

		public PointF Location
		{
			get
			{
				return this.location;
			}
		}
	}

	public delegate void HitShapeVertexEventHandler(
		Object sender,
		HitShapeVertexEventArgs e);
}
