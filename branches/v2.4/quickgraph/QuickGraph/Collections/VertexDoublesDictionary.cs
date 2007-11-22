using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	/// <summary>
	/// A dictionary with keys of type IVertex and values of type DoubleCollection
	/// </summary>
	public class VertexDoublesDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexDoublesDictionary class
		/// </summary>
		public VertexDoublesDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the DoubleCollection associated with the given IVertex
		/// </summary>
		/// <param name="key">
		/// The IVertex whose value to get or set.
		/// </param>
		public virtual DoubleCollection this[IVertex key]
		{
			get
			{
				return (DoubleCollection) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this VertexDoublesDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to add.
		/// </param>
		/// <param name="value">
		/// The DoubleCollection value of the element to add.
		/// </param>
		public virtual void Add(IVertex key, DoubleCollection value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this VertexDoublesDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexDoublesDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexDoublesDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexDoublesDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexDoublesDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexDoublesDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexDoublesDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The DoubleCollection value to locate in this VertexDoublesDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexDoublesDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(DoubleCollection value)
		{
			foreach (DoubleCollection item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this VertexDoublesDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to remove.
		/// </param>
		public virtual void Remove(IVertex key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this VertexDoublesDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this VertexDoublesDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}
	}
}
