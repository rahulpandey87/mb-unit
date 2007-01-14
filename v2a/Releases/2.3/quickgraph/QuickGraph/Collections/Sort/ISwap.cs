using System;
using System.Collections;

namespace QuickGraph.Collections.Sort
{
	/// <summary>
	/// Object swapper interface
	/// </summary>
	public interface ISwap
	{
		/// <summary>
		/// Swaps left and right in the list
		/// </summary>
		/// <param name="array"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		void Swap(IList array, int left, int right);
	}
}
