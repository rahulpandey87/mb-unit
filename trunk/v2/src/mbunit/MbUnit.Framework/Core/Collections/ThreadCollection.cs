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
using System.Threading;

namespace MbUnit.Core.Collections
{
	/// <summary>
	/// A collection of elements of type Thread
	/// </summary>
	public sealed class ThreadCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the ThreadCollection class.
		/// </summary>
		public ThreadCollection()
		{}

		public Thread Add(ThreadStart start)
		{
            if (start == null)
                throw new ArgumentNullException("start");
            Thread thread = new Thread(start);
            this.Add(thread);
            return thread;
        }

		/// <summary>
		/// Adds an instance of type Thread to the end of this ThreadCollection.
		/// </summary>
		/// <param name="value">
		/// The Thread to be added to the end of this ThreadCollection.
		/// </param>
		public void Add(Thread value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic Thread value is in this ThreadCollection.
		/// </summary>
		/// <param name="value">
		/// The Thread value to locate in this ThreadCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this ThreadCollection;
		/// false otherwise.
		/// </returns>
		public bool Contains(Thread value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Gets or sets the Thread at the given index in this ThreadCollection.
		/// </summary>
		public Thread this[int index]
		{
			get
			{
				return (Thread) this.List[index];
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific Thread from this ThreadCollection.
		/// </summary>
		/// <param name="value">
		/// The Thread value to remove from this ThreadCollection.
		/// </param>
		public void Remove(Thread value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by ThreadCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(ThreadCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public Thread Current
			{
				get
				{
					return (Thread) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (Thread) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this ThreadCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new ThreadCollection.Enumerator GetEnumerator()
		{
			return new ThreadCollection.Enumerator(this);
		}
	}

}
