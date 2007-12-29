using System;
using QuickGraph.Concepts;
using NGraphviz.Layout.Collections;

namespace QuickGraph.Collections
{
	/// <summary>
	/// A dictionary with keys of type IEdge and values of type PointFCollection
	/// </summary>
	public sealed class EdgePointFCollectionDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the EdgePointFCollectionDictionary class
		/// </summary>
		public EdgePointFCollectionDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the PointFCollection associated with the given IEdge
		/// </summary>
		/// <param name="key">
		/// The IEdge whose value to get or set.
		/// </param>
		public  PointFCollection this[IEdge key]
		{
			get
			{
				return (PointFCollection) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this EdgePointFCollectionDictionary.
		/// </summary>
		/// <param name="key">
		/// The IEdge key of the element to add.
		/// </param>
		/// <param name="value">
		/// The PointFCollection value of the element to add.
		/// </param>
		public  void Add(IEdge key, PointFCollection value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this EdgePointFCollectionDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IEdge key to locate in this EdgePointFCollectionDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgePointFCollectionDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(IEdge key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this EdgePointFCollectionDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IEdge key to locate in this EdgePointFCollectionDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgePointFCollectionDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(IEdge key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this EdgePointFCollectionDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The PointFCollection value to locate in this EdgePointFCollectionDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgePointFCollectionDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(PointFCollection value)
		{
			foreach (PointFCollection item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this EdgePointFCollectionDictionary.
		/// </summary>
		/// <param name="key">
		/// The IEdge key of the element to remove.
		/// </param>
		public  void Remove(IEdge key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this EdgePointFCollectionDictionary.
		/// </summary>
		public  System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this EdgePointFCollectionDictionary.
		/// </summary>
		public  System.Collections.ICollection Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}
	}
}
