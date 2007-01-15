using System;
using System.Collections;

namespace QuickGraph.Collections.Sort
{
	/// <summary>
	/// Default swap class
	/// </summary>
	public class DefaultSwap : ISwap
	{
		/// <summary>
		/// Default swap operation
		/// </summary>
		/// <param name="array"></param>
		/// <param name="left"></param>
		/// <param name="right"></param>
		public void Swap(IList array, int left, int right)
		{
			object swap=array[left];
			array[left]=array[right];
			array[right]=swap;
		}
	}
}
