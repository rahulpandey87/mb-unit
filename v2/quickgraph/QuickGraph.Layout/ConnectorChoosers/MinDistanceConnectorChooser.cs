using System;
using System.Drawing;
using Netron;
using QuickGraph.Concepts;

namespace QuickGraph.Layout.ConnectorChoosers
{
	public class MinDistanceConnectorChooser : IConnectorChooser
	{
		public double SqrDistance(PointF source, PointF target)
		{
			return Math.Pow(target.X-source.X,2)
				+Math.Pow(target.Y-source.Y,2);
		}

		public void Connect(IEdge e, Connection conn, Shape source, Shape target)
		{
			if (conn==null)
				throw new ArgumentNullException("conn");
			if (source==null)
				throw new ArgumentNullException("source");
			if (target==null)
				throw new ArgumentNullException("target");

			PointF c = conn.FromKnot;
			PointF sp;
			PointF tp;
			float dist = float.MaxValue;
			float distTemp;
			foreach(Connector sc in source.Connectors)
			{
				sp = source.ConnectionPoint(sc);
				distTemp = (float)SqrDistance(sp,c);
				if (distTemp<dist)
				{
					conn.From = sc;
					dist = distTemp;						
				}
			}

			// get connectors from source
			c = conn.ToKnot;
			dist = float.MaxValue;
				foreach(Connector tc in target.Connectors)
				{
					tp = target.ConnectionPoint(tc);
					distTemp = (float)SqrDistance(c,tp);
					if (distTemp<dist)
					{
						conn.To = tc;
						dist = distTemp;						
					}
				}

			if (conn.From==null || conn.To==null)
				throw new InvalidProgramException("Connector chooser failed");
		}	
	}
}
