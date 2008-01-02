// MbUnit Test Framework
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using System;
using System.Collections;

namespace MbUnit.Core.Remoting
{
	/// <summary>
	/// A collection of elements of type TestTreeNode
	/// </summary>
	[Serializable]
	public class TestTreeNodeCollection: ICollection
	{
		private ArrayList list = new ArrayList();
		private WeakReference owner;
		/// <summary>
		/// Initializes a new empty instance of the TestTreeNodeCollection class.
		/// </summary>
		public TestTreeNodeCollection( TestTreeNode owner)
		{
			this.owner = new WeakReference(owner);
		}

		private ArrayList List
		{
			get
			{
				return this.list;
			}
		}

        public TestTreeNode this[int index]
        {
            get
            {
                return (TestTreeNode)this.list[index];
            }
        }

        public virtual void Clear()
		{
			foreach(TestTreeNode node in this.List)
			{
				node.Parent=null;
				node.Nodes.Clear();
			}
			this.List.Clear();
		}

		/// <summary>
		/// Adds an instance of type TestTreeNode to the end of this TestTreeNodeCollection.
		/// </summary>
		/// <param name="value">
		/// The TestTreeNode to be added to the end of this TestTreeNodeCollection.
		/// </param>
		public virtual void Add(TestTreeNode value)
		{
			value.Parent = (TestTreeNode)this.owner.Target;
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic TestTreeNode value is in this TestTreeNodeCollection.
		/// </summary>
		/// <param name="value">
		/// The TestTreeNode value to locate in this TestTreeNodeCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this TestTreeNodeCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(TestTreeNode value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific TestTreeNode from this TestTreeNodeCollection.
		/// </summary>
		/// <param name="value">
		/// The TestTreeNode value to remove from this TestTreeNodeCollection.
		/// </param>
		public virtual void Remove(TestTreeNode value)
		{
			value.Parent = null;
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by TestTreeNodeCollection.GetEnumerator.
		/// </summary>
		[Serializable]
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(TestTreeNodeCollection collection)
			{
				this.wrapped = collection.List.GetEnumerator();
			}

			public TestTreeNode Current
			{
				get
				{
					return (TestTreeNode) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (TestTreeNode) (this.wrapped.Current);
				}
			}

			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this TestTreeNodeCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public virtual TestTreeNodeCollection.Enumerator GetEnumerator()
		{
			return new TestTreeNodeCollection.Enumerator(this);
		}
		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return this.List.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			this.List.CopyTo(array,index);
		}

		public object SyncRoot
		{
			get
			{
				return this.List.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
