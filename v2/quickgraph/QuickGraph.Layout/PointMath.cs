using System;
using System.Drawing;

namespace QuickGraph.Layout
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
	}
}
