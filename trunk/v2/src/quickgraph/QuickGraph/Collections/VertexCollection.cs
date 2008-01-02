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
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Concepts;

	/// <summary>
	/// A collection of elements of type Vertex
	/// </summary>
	public sealed class VertexCollection: 
		CollectionBase,
		IVertexCollection
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexCollection class.
		/// </summary>
		public VertexCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the VertexCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new VertexCollection.
		/// </param>
		public VertexCollection(IVertex[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="IVertexEnumerable"/> 
		/// interface, containing elements
		/// copied from another instance of VertexCollection
		/// </summary>
		/// <param name="items">
		/// The <see cref="IVertexEnumerable"/> whose elements are to be added to the new VertexCollection.
		/// </param>
		public VertexCollection(IVertexEnumerable items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this VertexCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this VertexCollection.
		/// </param>
		public  void AddRange(IVertex[] items)
		{
			foreach (IVertex item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another <see cref="IVertexEnumerable"/> 
		/// to the end of this VertexCollection.
		/// </summary>
		/// <param name="items">
		/// The <see cref="IVertexEnumerable"/> whose elements are to 
		/// be added to the end of this VertexCollection.
		/// </param>
		public  void AddRange(IVertexEnumerable items)
		{
			foreach (IVertex item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type Vertex to the end of this VertexCollection.
		/// </summary>
		/// <param name="value">
		/// The Vertex to be added to the end of this VertexCollection.
		/// </param>
		public  void Add(IVertex value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic Vertex value is in this VertexCollection.
		/// </summary>
		/// <param name="value">
		/// The Vertex value to locate in this VertexCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this VertexCollection;
		/// false otherwise.
		/// </returns>
		public  bool Contains(IVertex value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this VertexCollection
		/// </summary>
		/// <param name="value">
		/// The Vertex value to locate in the VertexCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public  int IndexOf(IVertex value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the VertexCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the Vertex is to be inserted.
		/// </param>
		/// <param name="value">
		/// The Vertex to insert.
		/// </param>
		public  void Insert(int index, IVertex value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the Vertex at the given index in this VertexCollection.
		/// </summary>
		public  IVertex this[int index]
		{
			get
			{
				return (IVertex) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this VertexCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new  VertexCollection.Enumerator GetEnumerator()
		{
			return new VertexCollection.Enumerator(this);
		}

		IVertexEnumerator IVertexEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Removes the first occurrence of a specific Vertex from this VertexCollection.
		/// </summary>
		/// <param name="value">
		/// The Vertex value to remove from this VertexCollection.
		/// </param>
		public  void Remove(IVertex value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by VertexCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: IVertexEnumerator
		{
			private IEnumerator wrapped;

			/// <summary>
			/// Builds a enumerator on the collection
			/// </summary>
			/// <param name="collection"></param>
			public Enumerator(VertexCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			/// <summary>
			/// Current vertex
			/// </summary>
			public IVertex Current
			{
				get
				{
					return (IVertex) (this.wrapped.Current);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (IVertex) (this.wrapped.Current);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			/// <summary>
			/// 
			/// </summary>
			public void Reset()
			{
				this.wrapped.Reset();
			}
		}

	}
}
