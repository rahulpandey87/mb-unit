using System;
using Netron;
using QuickGraph.Concepts;

namespace QuickGraph.Layout.Collections
{
	/// <summary>
	/// A dictionary with keys of type IVertex and values of type Shape
	/// </summary>
	public class VertexShapeDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexShapeDictionary class
		/// </summary>
		public VertexShapeDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the Shape associated with the given IVertex
		/// </summary>
		/// <param name="key">
		/// The IVertex whose value to get or set.
		/// </param>
		public virtual Shape this[IVertex key]
		{
			get
			{
				return (Shape) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this VertexShapeDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to add.
		/// </param>
		/// <param name="value">
		/// The Shape value of the element to add.
		/// </param>
		public virtual void Add(IVertex key, Shape value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this VertexShapeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexShapeDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexShapeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexShapeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexShapeDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexShapeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexShapeDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The Shape value to locate in this VertexShapeDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexShapeDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(Shape value)
		{
			foreach (Shape item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this VertexShapeDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to remove.
		/// </param>
		public virtual void Remove(IVertex key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this VertexShapeDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this VertexShapeDictionary.
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
