using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	/// <summary>
	/// A dictionary with keys of type IEdge and values of type Int
	/// </summary>
	public sealed class EdgeIntDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the EdgeIntDictionary class
		/// </summary>
		public EdgeIntDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the Int associated with the given IEdge
		/// </summary>
		/// <param name="key">
		/// The IEdge whose value to get or set.
		/// </param>
		public  int this[IEdge key]
		{
			get
			{
				return (int) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this EdgeIntDictionary.
		/// </summary>
		/// <param name="key">
		/// The IEdge key of the element to add.
		/// </param>
		/// <param name="value">
		/// The Int value of the element to add.
		/// </param>
		public  void Add(IEdge key, int value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this EdgeIntDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IEdge key to locate in this EdgeIntDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgeIntDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(IEdge key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this EdgeIntDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IEdge key to locate in this EdgeIntDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgeIntDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(IEdge key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this EdgeIntDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The Int value to locate in this EdgeIntDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgeIntDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(int value)
		{
			foreach (int item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this EdgeIntDictionary.
		/// </summary>
		/// <param name="key">
		/// The IEdge key of the element to remove.
		/// </param>
		public  void Remove(IEdge key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this EdgeIntDictionary.
		/// </summary>
		public  System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this EdgeIntDictionary.
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
