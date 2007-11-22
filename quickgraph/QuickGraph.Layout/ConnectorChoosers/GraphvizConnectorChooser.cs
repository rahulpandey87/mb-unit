using System;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Collections;
using QuickGraph.Algorithms.Graphviz;
using Netron;
using System.Drawing;

namespace QuickGraph.Layout.ConnectorChoosers
{
	/// <summary>
	/// Summary description for GraphvizConnectorChooser.
	/// </summary>
	public class GraphvizConnectorChooser : IConnectorChooser
	{
		private NetronGraphvizLayouter layouter = null;
		public GraphvizConnectorChooser(NetronGraphvizLayouter layouter)
		{
			if (layouter==null)
				throw new ArgumentNullException("layout");
			this.layouter = layouter;
		}

		public GraphvizConnectorChooser()
		{}

		public NetronGraphvizLayouter Layouter
		{
			get
			{
				return this.layouter;
			}
			set
			{
				this.layouter = value;
			}
		}

		public void Connect(
			IEdge e,
			Netron.Connection conn, 
			Netron.Shape source, 
			Netron.Shape target
			)
		{
			// get ports
			EdgeKeysEventArgs args = (EdgeKeysEventArgs)layouter.EdgeArgs[e];

			if (args.Keys.Count<2)
				throw new ArgumentException("not enough keys");
			// find best from
			conn.From = FindNearestConnector(source, args.Keys[0]);
			conn.To = FindNearestConnector(target, args.Keys[args.Keys.Count-1]);
		}

		public Connector FindNearestConnector(Netron.Shape shape, PointF p)
		{
			Connector best = null;
			double distBest = double.MaxValue;
			double dist;

			foreach(Connector c in shape.Connectors)
			{
				dist = PointMath.SqrDistance(shape.ConnectionPoint(c),p);
				if (dist<distBest)
				{
					distBest = dist;
					best = c;
				}
			}

			if (best==null)
				throw new Exception("could not find port");

			return best;
		}
	}
}
