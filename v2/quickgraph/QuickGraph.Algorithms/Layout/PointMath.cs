using System;
using System.Drawing;

namespace QuickGraph.Algorithms.Layout
{
	/// <summary>
	/// Useful point algebra function.
	/// </summary>
	public sealed class PointMath
	{
		private PointMath()
		{}

		/// <summary>
		/// Computes the Euclidian distance between two points
		/// </summary>
		/// <param name="p1">first point</param>
		/// <param name="p2">second point</param>
		/// <returns><c>|p1-p2|_2</c></returns>
		public static double Distance(PointF p1, PointF p2)
		{
			return Math.Sqrt(SqrDistance(p1,p2));
		}

		/// <summary>
		/// Computes the square of the Euclidian distance between two points
		/// </summary>
		/// <param name="p1">first point</param>
		/// <param name="p2">second point</param>
		/// <returns><c>(p1.x-p2.x)^2+(p1.y-p2.y)^2</c></returns>
		public static double SqrDistance(PointF p1, PointF p2)
		{
			return (p1.X-p2.X)*(p1.X-p2.X)+(p1.Y-p2.Y)*(p1.Y-p2.Y);
		}

		public static PointF Add(PointF p1, PointF p2)
		{
			return new PointF(p1.X+p2.X, p1.Y+p2.Y);
		}

		public static PointF Add(PointF p1, float x, float y)
		{
			return new PointF(p1.X+x, p1.Y+y);
		}
		
		public static PointF Sub(PointF p1, PointF p2)
		{
			return new PointF(p1.X-p2.X, p1.Y-p2.Y);
		}
		
		public static PointF Mul(float m, PointF p)
		{
			return new PointF(m*p.X, m*p.Y);
		}
		public static PointF Combili(PointF p0, double m, PointF p)
		{
			return new PointF(p0.X+(float)m*p.X, p0.Y+(float)m*p.Y);
		}
		public static PointF Combili(PointF p0, float m, PointF p)
		{
			return new PointF(p0.X+m*p.X, p0.Y+m*p.Y);
		}	
		public static PointF Combili(float a, PointF p0, float b, PointF p)
		{
			return new PointF(a*p0.X+b*p.X, a*p0.Y+b*p.Y);
		}	
		
		public static PointF ScaleSaturate(PointF o, PointF p, float scale, float saturation)
		{
			float x=p.X*scale;
			x=Math.Max(-saturation, Math.Min( saturation, x));
			float y=p.Y*scale;
			y=Math.Max(-saturation, Math.Min( saturation, y));
			return new PointF(o.X+x,o.Y+y);
		}
	}
}
