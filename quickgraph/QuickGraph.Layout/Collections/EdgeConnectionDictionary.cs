using System;
using Netron;
using QuickGraph.Concepts;

namespace QuickGraph.Layout.Collections
{
	/// <summary>
	/// A dictionary with keys of type IEdge and values of type Connection
	/// </summary>
	public class EdgeConnectionDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the EdgeConnectionDictionary class
		/// </summary>
		public EdgeConnectionDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the Connection associated with the given IEdge
		/// </summary>
		/// <param name="key">
		/// The IEdge whose value to get or set.
		/// </param>
		public virtual Connection this[IEdge key]
		{
			get
			{
				return (Connection) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this EdgeConnectionDictionary.
		/// </summary>
		/// <param name="key">
		/// The IEdge key of the element to add.
		/// </param>
		/// <param name="value">
		/// The Connection value of the element to add.
		/// </param>
		public virtual void Add(IEdge key, Connection value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this EdgeConnectionDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IEdge key to locate in this EdgeConnectionDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgeConnectionDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(IEdge key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this EdgeConnectionDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IEdge key to locate in this EdgeConnectionDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgeConnectionDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(IEdge key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this EdgeConnectionDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The Connection value to locate in this EdgeConnectionDictionary.
		/// </param>
		/// <returns>
		/// true if this EdgeConnectionDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(Connection value)
		{
			foreach (Connection item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this EdgeConnectionDictionary.
		/// </summary>
		/// <param name="key">
		/// The IEdge key of the element to remove.
		/// </param>
		public virtual void Remove(IEdge key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this EdgeConnectionDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this EdgeConnectionDictionary.
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
