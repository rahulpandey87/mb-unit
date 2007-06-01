using System;
using System.Collections;

namespace QuickGraph.Collections.Sort
{
	/// <summary>
	/// Summary description for ISorter.
	/// </summary>
	public interface ISorter
	{
		/// <summary>
		/// Sorts the <paramref name="list"/>.
		/// </summary>
		/// <param name="list"></param>
		void Sort(IList list);
	}
}
