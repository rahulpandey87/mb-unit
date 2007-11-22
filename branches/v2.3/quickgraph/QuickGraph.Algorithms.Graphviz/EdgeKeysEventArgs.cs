using System;
using System.Collections;

namespace QuickGraph.Algorithms.Graphviz
{
	using QuickGraph.Concepts;

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class EdgeKeysEventArgs : EventArgs
	{
		private IEdge edge;
		private EdgePort sourcePort;
		private EdgePort targetPort;
		private NGraphviz.Layout.Collections.PointFCollection keys;

		public EdgeKeysEventArgs(
			IEdge e,
			EdgePort sourcePort,
			EdgePort targetPort,
			NGraphviz.Layout.Collections.PointFCollection keys
			) 
		{
			if (e==null)
				throw new ArgumentNullException("e");
			this.edge = e;
			this.keys = keys;
			this.sourcePort = sourcePort;
			this.targetPort = targetPort;
		}

		public IEdge Edge
		{
			get
			{
				return this.edge;
			}
		}

		public EdgePort SourcePort
		{
			get
			{
				return this.sourcePort;
			}
		}


		public EdgePort TargetPort
		{
			get
			{
				return this.targetPort;
			}
		}

		public NGraphviz.Layout.Collections.PointFCollection Keys
		{
			get
			{
				return this.keys;
			}
		}
	}

	public delegate void EdgeKeysEventHandler(
		Object sender,
		EdgeKeysEventArgs e);
}
