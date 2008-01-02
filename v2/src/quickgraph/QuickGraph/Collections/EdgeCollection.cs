// QuickGraph Library 
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
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux


using System;
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	/// <summary>
	/// A collection of elements of type Edge
	/// </summary>
	public sealed class EdgeCollection: 
		CollectionBase,
		IEdgeCollection
	{
		/// <summary>
		/// Initializes a new empty instance of the EdgeCollection class.
		/// </summary>
		public EdgeCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the EdgeCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new EdgeCollection.
		/// </param>
		public EdgeCollection(IEdge[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the EdgeCollection class, containing elements
		/// copied from another instance of EdgeCollection
		/// </summary>
		/// <param name="items">
		/// The EdgeCollection whose elements are to be added to the new EdgeCollection.
		/// </param>
		public EdgeCollection(IEdgeEnumerable items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this EdgeCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this EdgeCollection.
		/// </param>
		public void AddRange(IEdge[] items)
		{
			foreach (IEdge item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another EdgeCollection to the end of this EdgeCollection.
		/// </summary>
		/// <param name="items">
		/// The EdgeCollection whose elements are to be added to the end of this EdgeCollection.
		/// </param>
		public void AddRange(IEdgeEnumerable items)
		{
			foreach (IEdge item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type Edge to the end of this EdgeCollection.
		/// </summary>
		/// <param name="value">
		/// The Edge to be added to the end of this EdgeCollection.
		/// </param>
		public void Add(IEdge value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic Edge value is in this EdgeCollection.
		/// </summary>
		/// <param name="value">
		/// The Edge value to locate in this EdgeCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this EdgeCollection;
		/// false otherwise.
		/// </returns>
		public bool Contains(IEdge value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this EdgeCollection
		/// </summary>
		/// <param name="value">
		/// The Edge value to locate in the EdgeCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public int IndexOf(IEdge value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the EdgeCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the Edge is to be inserted.
		/// </param>
		/// <param name="value">
		/// The Edge to insert.
		/// </param>
		public void Insert(int index, IEdge value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the Edge at the given index in this EdgeCollection.
		/// </summary>
		public IEdge this[int index]
		{
			get
			{
				return (IEdge) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific Edge from this EdgeCollection.
		/// </summary>
		/// <param name="value">
		/// The Edge value to remove from this EdgeCollection.
		/// </param>
		public void Remove(IEdge value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this EdgeCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new EdgeCollection.Enumerator GetEnumerator()
		{
			return new EdgeCollection.Enumerator(this);
		}

		IEdgeEnumerator IEdgeEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Type-specific enumeration class, used by EdgeCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: IEdgeEnumerator
		{
			private IEnumerator wrapped;

			/// <summary>
			/// Create a new enumerator on the collection
			/// </summary>
			/// <param name="collection">collection to enumerate</param>
			public Enumerator(EdgeCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			/// <summary>
			/// The current element. 
			/// </summary>
			public IEdge Current
			{
				get
				{
					return (IEdge) (this.wrapped.Current);
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return (Object)(this.wrapped.Current);
				}
			}

			/// <summary>
			/// Moves cursor to next element.
			/// </summary>
			/// <returns>true if current is valid, false otherwize</returns>
			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			/// <summary>
			/// Resets the cursor to the position before the first element.
			/// </summary>
			public void Reset()
			{
				this.wrapped.Reset();
			}
		}
	}
}
