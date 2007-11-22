using System;
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts.Collections;

	public struct HeapEntry 
	{
		private object item;
		private IComparable priority;
		public HeapEntry(object item, IComparable priority) 
		{
			this.item = item;
			this.priority = priority;
		}
		public object Item 
		{
			get {return item;}
		}
		public IComparable Priority 
		{
			get {return priority;}
		}
	}

	/// <summary>
	/// Summary description for PriorityQueue.
	/// </summary>
	public class PriorityQueue : IPriorityQueue
	{
		private int count;
		private int capacity;
		private HeapEntry[] heap;

		public PriorityQueue() 
		{
			this.Clear();
		}

		public int Count
		{
			get
			{
				return this.count;
			}
		}

		public Object SyncRoot
		{
			get
			{
				return this;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public IEnumerator GetEnumerator()
		{
			return this.heap.GetEnumerator();
		}

		public void CopyTo(Array array, int index)
		{
			this.heap.CopyTo(array,index);
		}

		public void Clear()
		{
			this.count = 0;
			this.capacity = 15; // 15 is equal to 4 complete levels
			this.heap = new HeapEntry[capacity];
		}

		public object Pop() 
		{
			if (this.count==0)
				throw new InvalidOperationException("Heap is empty");

			object result = this.heap[0].Item;
			this.count--;
			trickleDown(0, this.heap[count]);
			return result;
		}

		public void Push(object item, IComparable priority) 
		{
			if (this.count == this.capacity)  
				growHeap();

			this.count++;
			bubbleUp(this.count - 1, new HeapEntry(item, priority));
		}

		public void Update(Object item, IComparable priority)
		{
			throw new Exception("not implemented");
		}

		private void bubbleUp(int index, HeapEntry he) 
		{
			int parent = (index - 1) / 2;
			// note: (index > 0) means there is a parent
			while ((index > 0) && (heap[parent].Priority.CompareTo(he.Priority)<0)) 
			{
				this.heap[index] = this.heap[parent];
				index = parent;
				parent = (index - 1) / 2;
			}
			heap[index] = he;
		}

		private void trickleDown(int index, HeapEntry he) 
		{
			int child = (index * 2) + 1;
			while (child < count) 
			{
				if (((child + 1) < count) && 
					(this.heap[child].Priority.CompareTo(this.heap[child + 1].Priority)<0))
				{
					child++;
				}
				this.heap[index] = this.heap[child];
				index = child;
				child = (index * 2) + 1;
			}
			bubbleUp(index, he);
		}

		private void growHeap() 
		{
			this.capacity = (this.capacity * 2) + 1;
			HeapEntry[] newHeap = new HeapEntry[this.capacity];
			System.Array.Copy(heap, 0, newHeap, 0, count);
			heap = newHeap;
		}
	}
}
