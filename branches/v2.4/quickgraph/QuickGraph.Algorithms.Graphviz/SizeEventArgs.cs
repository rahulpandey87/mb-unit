using System;
using System.Drawing;

namespace QuickGraph.Algorithms.Graphviz
{
	/// <summary>
	/// Event argument that stores a size
	/// </summary>
	public class SizeEventArgs : EventArgs
	{
		private Size size;

		public SizeEventArgs(Size size)
		{
			this.size = size;
		}

		public Size Size
		{
			get
			{
				return this.size;
			}
		}
	}

	/// <summary>
	/// Size event handler
	/// </summary>
	public delegate void SizeEventHandler(Object sender, SizeEventArgs e);
}
