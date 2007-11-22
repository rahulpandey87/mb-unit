using System;
using QuickGraph.Concepts;
using Netron;

namespace QuickGraph.Layout.Collections
{
	/// <summary>
	/// A dictionary with keys of type Shape and values of type IVertex
	/// </summary>
	public class ShapeVertexDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the ShapeVertexDictionary class
		/// </summary>
		public ShapeVertexDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the IVertex associated with the given Shape
		/// </summary>
		/// <param name="key">
		/// The Shape whose value to get or set.
		/// </param>
		public virtual IVertex this[Shape key]
		{
			get
			{
				return (IVertex) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this ShapeVertexDictionary.
		/// </summary>
		/// <param name="key">
		/// The Shape key of the element to add.
		/// </param>
		/// <param name="value">
		/// The IVertex value of the element to add.
		/// </param>
		public virtual void Add(Shape key, IVertex value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this ShapeVertexDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Shape key to locate in this ShapeVertexDictionary.
		/// </param>
		/// <returns>
		/// true if this ShapeVertexDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(Shape key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this ShapeVertexDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Shape key to locate in this ShapeVertexDictionary.
		/// </param>
		/// <returns>
		/// true if this ShapeVertexDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(Shape key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this ShapeVertexDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The IVertex value to locate in this ShapeVertexDictionary.
		/// </param>
		/// <returns>
		/// true if this ShapeVertexDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsValue(IVertex value)
		{
			foreach (IVertex item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this ShapeVertexDictionary.
		/// </summary>
		/// <param name="key">
		/// The Shape key of the element to remove.
		/// </param>
		public virtual void Remove(Shape key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this ShapeVertexDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this ShapeVertexDictionary.
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
