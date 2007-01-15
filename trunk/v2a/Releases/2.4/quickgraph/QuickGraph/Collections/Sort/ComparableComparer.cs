using System;
using System.Collections;

namespace QuickGraph.Collections.Sort
{
	/// <summary>
	/// Default <see cref="IComparable"/> object comparer.
	/// </summary>
	public class ComparableComparer : IComparer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(IComparable x, Object y)
		{
			return x.CompareTo(y);
		}

		#region IComparer Members
		int IComparer.Compare(Object x, Object y)
		{
			return this.Compare((IComparable)x,y);
		}

		#endregion
	}
}
