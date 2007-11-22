using System;

namespace QuickGraph.Collections
{
	/// <summary>
	/// A collection of elements of type Double
	/// </summary>
	public class DoubleCollection: System.Collections.CollectionBase
	{
		/// <summary>
		/// Initializes a new empty instance of the DoubleCollection class.
		/// </summary>
		public DoubleCollection()
		{
			// empty
		}

		/// <summary>
		/// Initializes a new instance of the DoubleCollection class, containing elements
		/// copied from an array.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the new DoubleCollection.
		/// </param>
		public DoubleCollection(Double[] items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Initializes a new instance of the DoubleCollection class, containing elements
		/// copied from another instance of DoubleCollection
		/// </summary>
		/// <param name="items">
		/// The DoubleCollection whose elements are to be added to the new DoubleCollection.
		/// </param>
		public DoubleCollection(DoubleCollection items)
		{
			this.AddRange(items);
		}

		/// <summary>
		/// Adds the elements of an array to the end of this DoubleCollection.
		/// </summary>
		/// <param name="items">
		/// The array whose elements are to be added to the end of this DoubleCollection.
		/// </param>
		public virtual void AddRange(Double[] items)
		{
			foreach (Double item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds the elements of another DoubleCollection to the end of this DoubleCollection.
		/// </summary>
		/// <param name="items">
		/// The DoubleCollection whose elements are to be added to the end of this DoubleCollection.
		/// </param>
		public virtual void AddRange(DoubleCollection items)
		{
			foreach (Double item in items)
			{
				this.List.Add(item);
			}
		}

		/// <summary>
		/// Adds an instance of type Double to the end of this DoubleCollection.
		/// </summary>
		/// <param name="value">
		/// The Double to be added to the end of this DoubleCollection.
		/// </param>
		public virtual void Add(Double value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic Double value is in this DoubleCollection.
		/// </summary>
		/// <param name="value">
		/// The Double value to locate in this DoubleCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this DoubleCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(Double value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this DoubleCollection
		/// </summary>
		/// <param name="value">
		/// The Double value to locate in the DoubleCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(Double value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the DoubleCollection at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the Double is to be inserted.
		/// </param>
		/// <param name="value">
		/// The Double to insert.
		/// </param>
		public virtual void Insert(int index, Double value)
		{
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the Double at the given index in this DoubleCollection.
		/// </summary>
		public virtual Double this[int index]
		{
			get
			{
				return (Double) this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific Double from this DoubleCollection.
		/// </summary>
		/// <param name="value">
		/// The Double value to remove from this DoubleCollection.
		/// </param>
		public virtual void Remove(Double value)
		{
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by DoubleCollection.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator
		{
			private System.Collections.IEnumerator wrapped;

			public Enumerator(DoubleCollection collection)
			{
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public Double Current
			{
				get
				{
					return (Double) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return (Double) (this.wrapped.Current);
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
		/// Returns an enumerator that can iterate through the elements of this DoubleCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual DoubleCollection.Enumerator GetEnumerator()
		{
			return new DoubleCollection.Enumerator(this);
		}
	}

}
