using System;
using QuickGraph.Concepts;
using Netron;

namespace QuickGraph.Layout.Collections
{
	/// <summary>
	/// A dictionary with keys of type Connection and values of type IEdge
	/// </summary>
	public class ConnectionEdgeDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the ConnectionEdgeDictionary class
		/// </summary>
		public ConnectionEdgeDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the IEdge associated with the given Connection
		/// </summary>
		/// <param name="key">
		/// The Connection whose value to get or set.
		/// </param>
		public virtual IEdge this[Connection key]
		{
			get
			{
				return (IEdge) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this ConnectionEdgeDictionary.
		/// </summary>
		/// <param name="key">
		/// The Connection key of the element to add.
		/// </param>
		/// <param name="value">
		/// The IEdge value of the element to add.
		/// </param>
		public virtual void Add(Connection key, IEdge value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this ConnectionEdgeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Connection key to locate in this ConnectionEdgeDictionary.
		/// </param>
		/// <returns>
		/// true if this ConnectionEdgeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(Connection key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this ConnectionEdgeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Connection key to locate in this ConnectionEdgeDictionary.
		/// </param>
		/// <returns>
		/// true if this ConnectionEdgeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(Connection key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this ConnectionEdgeDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The IEdge value to locate in this ConnectionEdgeDictionary.
		/// </param>
		/// <returns>
		/// true if this ConnectionEdgeDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(IEdge value)
		{
			foreach (IEdge item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this ConnectionEdgeDictionary.
		/// </summary>
		/// <param name="key">
		/// The Connection key of the element to remove.
		/// </param>
		public virtual void Remove(Connection key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this ConnectionEdgeDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this ConnectionEdgeDictionary.
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
