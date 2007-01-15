using System;
using System.Drawing;
using QuickGraph.Concepts;

namespace QuickGraph.Collections
{
	/// <summary>
	/// A dictionary with keys of type IVertex and values of type Size
	/// </summary>
	public sealed class VertexSizeFDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexSizeDictionary class
		/// </summary>
		public VertexSizeFDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the Size associated with the given IVertex
		/// </summary>
		/// <param name="key">
		/// The IVertex whose value to get or set.
		/// </param>
		public  SizeF this[IVertex key]
		{
			get
			{
				return (SizeF) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this VertexSizeDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to add.
		/// </param>
		/// <param name="value">
		/// The Size value of the element to add.
		/// </param>
		public  void Add(IVertex key, SizeF value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this VertexSizeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexSizeDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexSizeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexSizeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexSizeDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexSizeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexSizeDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The Size value to locate in this VertexSizeDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexSizeDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(SizeF value)
		{
			foreach (Size item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this VertexSizeDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to remove.
		/// </param>
		public  void Remove(IVertex key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this VertexSizeDictionary.
		/// </summary>
		public  System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this VertexSizeDictionary.
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
