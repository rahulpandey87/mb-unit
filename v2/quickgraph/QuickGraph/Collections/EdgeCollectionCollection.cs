using System;

namespace QuickGraph.Collections
{
	/// <summary>
	/// A collection of elements of type EdgeCollection
	/// </summary>
	public sealed class EdgeCollectionCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the EdgeCollectionCollection class.
		/// </summary>
		public EdgeCollectionCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the EdgeCollectionCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new EdgeCollectionCollection.
		/// </param>
		public EdgeCollectionCollection(EdgeCollection[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the EdgeCollectionCollection class, containing elements
		/// copied from another instance of EdgeCollectionCollection
		/// </summary>
		/// <param name="items">
		/// The EdgeCollectionCollection whose elements are to be added to the new EdgeCollectionCollection.
		/// </param>
		public EdgeCollectionCollection(EdgeCollectionCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this EdgeCollectionCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this EdgeCollectionCollection.
		/// </param>
		public void AddRange(EdgeCollection[] items)
		{
			foreach (EdgeCollection item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another EdgeCollectionCollection to the end of this EdgeCollectionCollection.
		/// </summary>
		/// <param name="items">
		/// The EdgeCollectionCollection whose elements are to be added to the end of this EdgeCollectionCollection.
		/// </param>
		public void AddRange(EdgeCollectionCollection items)
		{
			foreach (EdgeCollection item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type EdgeCollection to the end of this EdgeCollectionCollection.
		/// </summary>
		/// <param name="value">
		/// The EdgeCollection to be added to the end of this EdgeCollectionCollection.
		/// </param>
		public  void Add(EdgeCollection value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic EdgeCollection value is in this EdgeCollectionCollection.
		/// </summary>
		/// <param name="value">
		/// The EdgeCollection value to locate in this EdgeCollectionCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this EdgeCollectionCollection;
		/// false otherwise.
		/// </returns>
		public  bool Contains(EdgeCollection value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this EdgeCollectionCollection
		/// </summary>
		/// <param name="value">
		/// The EdgeCollection value to locate in the EdgeCollectionCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public  int IndexOf(EdgeCollection value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the EdgeCollectionCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the EdgeCollection is to be inserted.
		/// </param>
		/// <param name="value">
		/// The EdgeCollection to insert.
		/// </param>
		public  void Insert(int index, EdgeCollection value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the EdgeCollection at the given index in this EdgeCollectionCollection.
		/// </summary>
		public  EdgeCollection this[int index]
		{
			get
			{
				return (EdgeCollection) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific EdgeCollection from this EdgeCollectionCollection.
		/// </summary>
		/// <param name="value">
		/// The EdgeCollection value to remove from this EdgeCollectionCollection.
		/// </param>
		public  void Remove(EdgeCollection value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by EdgeCollectionCollection.GetEnumerator.
		/// </summary>
		public sealed class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="collection"></param>
			public Enumerator(EdgeCollectionCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			/// <summary>
			/// Gets the current edge collection
			/// </summary>
			/// <value>
			/// Current edge collection
			/// </value>
			public EdgeCollection Current
			{
				get
				{
					return (EdgeCollection) (this.wrapped.Current);
				}
			}

			/// <summary>
			/// 
			/// </summary>
			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (EdgeCollection) (this.wrapped.Current);
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

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this EdgeCollectionCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new  EdgeCollectionCollection.Enumerator GetEnumerator()
		{
			return new EdgeCollectionCollection.Enumerator(this);
		}
	}
}
