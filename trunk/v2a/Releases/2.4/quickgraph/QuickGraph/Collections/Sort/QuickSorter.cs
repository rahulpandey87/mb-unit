using System;
using System.Collections;
using System.IO;


namespace QuickGraph.Collections.Sort
{
	/// <summary>
	/// http://www.codeproject.com/csharp/csquicksort.asp
	/// </summary>
	public class QuickSorter : SwapSorter
	{
		/// <summary>
		/// 
		/// </summary>
		public QuickSorter()
			:base()
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="comparer"></param>
		/// <param name="swapper"></param>
		public QuickSorter(IComparer comparer, ISwap swapper)
			:base(comparer,swapper)
		{}

		/// <summary>
		/// Sorts the array.
		/// </summary>
		/// <param name="array">The array to sort.</param>
		public override void Sort(IList array)
		{
			Sort(array, 0, array.Count-1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="lower"></param>
		/// <param name="upper"></param>
		public void Sort(IList array, int lower, int upper)
		{
			// Check for non-base case
			if (lower < upper)
			{
				// Split and sort partitions
				int split=Pivot(array, lower, upper);
				Sort(array, lower, split-1);
				Sort(array, split+1, upper);
			}
		}

		#region Internal
		internal int Pivot(IList array, int lower, int upper)
		{
			// Pivot with first element
			int left=lower+1;
			object pivot=array[lower];
			int right=upper;

			// Partition array elements
			while (left <= right)
			{
				// Find item out of place
				while ( (left <= right) && (Comparer.Compare(array[left], pivot) <= 0) )
				{
					++left;
				}

				while ( (left <= right) && (Comparer.Compare(array[right], pivot) > 0) )
				{
					--right;
				}

				// Swap values if necessary
				if (left < right)
				{
					Swapper.Swap(array, left, right);
					++left;
					--right;
				}
			}

			// Move pivot element
			Swapper.Swap(array, lower, right);
			return right;
		}
		#endregion
	}
}
