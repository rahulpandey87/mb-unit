using System;
using System.Collections;
using System.Drawing;
using QuickGraph.Concepts;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts.Collections;
	/// <summary>
	/// A dictionary with keys of type IVertex and values of type PointF
	/// </summary>
	public sealed class VertexPointFDictionary: 
		DictionaryBase,
		IVertexPointFDictionary
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexPointFDictionary class
		/// </summary>
		public VertexPointFDictionary()
		{}

		/// <summary>
		/// Gets or sets the PointF associated with the given IVertex
		/// </summary>
		/// <param name="key">
		/// The IVertex whose value to get or set.
		/// </param>
		public PointF this[IVertex key]
		{
			get
			{
				return (PointF)this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this VertexPointFDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to add.
		/// </param>
		/// <param name="value">
		/// The PointF value of the element to add.
		/// </param>
		public  void Add(IVertex key, PointF value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this VertexPointFDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexPointFDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexPointFDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexPointFDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The IVertex key to locate in this VertexPointFDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexPointFDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexPointFDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The PointF value to locate in this VertexPointFDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexPointFDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(PointF value)
		{
			foreach (PointF item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this VertexPointFDictionary.
		/// </summary>
		/// <param name="key">
		/// The IVertex key of the element to remove.
		/// </param>
		public void Remove(IVertex key)
		{
			this.Dictionary.Remove(key);
		}
	}

}
