using System;
using System.Drawing;

namespace QuickGraph.Algorithms.Graphviz
{
	using QuickGraph.Concepts;
	public class VertexPositionEventArgs : EventArgs
	{
		private IVertex vertex;
		private PointF pos;

		public VertexPositionEventArgs(IVertex v,PointF pos) 
		{
			if (v==null)
				throw new ArgumentNullException("v");
			this.vertex = v;
			this.pos = pos;
		}

		public IVertex Vertex
		{
			get
			{
				return this.vertex;
			}
		}

		public PointF Position
		{
			get
			{
				return this.pos;
			}
		}
	}

	public delegate void VertexPositionEventHandler(
		Object sender,
		VertexPositionEventArgs e);
}
