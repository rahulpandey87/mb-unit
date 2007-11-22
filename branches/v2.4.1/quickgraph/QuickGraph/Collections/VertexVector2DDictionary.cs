using System;
using QuickGraph.Concepts;

namespace QuickGraph.Collections
{
	/// <summary>
	/// A dictionary with keys of type IVertex and values of type Vector2D
	/// </summary>
	public sealed class VertexVector2DDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexVector2DDictionary class
		/// </summary>
		public VertexVector2DDictionary()
		{
		}

		/// <summary>
		/// Gets or sets the Vector2D associated with the given IVertex
		/// </summary>
		/// <param name="key">
		/// The IVertex whose value to get or set.
		/// </param>
		public  Vector2D this[IVertex key]
		{
			get
			{
				return (Vector2D) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this VertexVector2DDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to add.
		/// </param>
		/// <param name="value">
		/// The Vector2D value of the element to add.
		/// </param>
		public  void Add(IVertex key, Vector2D value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this VertexVector2DDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexVector2DDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexVector2DDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexVector2DDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexVector2DDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexVector2DDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexVector2DDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The Vector2D value to locate in this VertexVector2DDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexVector2DDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(Vector2D value)
		{
			foreach (Vector2D item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this VertexVector2DDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to remove.
		/// </param>
		public  void Remove(IVertex key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this VertexVector2DDictionary.
		/// </summary>
		public  System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this VertexVector2DDictionary.
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
