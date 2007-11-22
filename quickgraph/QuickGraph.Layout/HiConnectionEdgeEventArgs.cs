using System;
using System.Drawing;

namespace QuickGraph.Layout
{
	using Netron;
	using QuickGraph.Concepts;

	/// <summary>
	/// Summary description for HitConnectionEdgeEventArgs.
	/// </summary>
	public class HitConnectionEdgeEventArgs : ConnectionEdgeEventArgs
	{
		private PointF location;

		public HitConnectionEdgeEventArgs(Connection conn, IEdge e, PointF location)
			:base(conn,e)
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

	public delegate void HitConnectionEdgeEventHandler(
		Object sender,
		HitConnectionEdgeEventArgs e
		);
}
