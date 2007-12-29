using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// A dictionary with keys of type VertexPair and values of type double
	/// </summary>
	public sealed class VertexPairDoubleDictionary: 
		System.Collections.DictionaryBase,
		IVertexDistanceMatrix
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexPairDoubleDictionary class
		/// </summary>
		public VertexPairDoubleDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the double associated with the given VertexPair
		/// </summary>
		/// <param name="key">
		/// The VertexPair whose value to get or set.
		/// </param>
		public  double this[VertexPair key]
		{
			get
			{
				if (this.Dictionary.Contains(key))
					return (double) this.Dictionary[key];
				else
					return double.MaxValue;
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Returns
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public double this[IVertex u, IVertex v]
		{
			get
			{
				if (u==null)
					throw new ArgumentNullException("u");
				if (v==null)
					throw new ArgumentNullException("v");

				VertexPair vp = new VertexPair(u,v);
				return this[vp];
			}
			set
			{
				if (u==null)
					throw new ArgumentNullException("u");
				if (v==null)
					throw new ArgumentNullException("v");

				VertexPair vp = new VertexPair(u,v);
				this[vp]=value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <returns></returns>
		public double Distance(IVertex u, IVertex v)
		{
			return this[u,v];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="u"></param>
		/// <param name="v"></param>
		/// <param name="d"></param>
		public void SetDistance(IVertex u, IVertex v, double d)
		{
			this[u,v]=d;
		}

		/// <summary>
		/// Adds an element with the specified key and value to this VertexPairDoubleDictionary.
		/// </summary>
		/// <param name="key">
		/// The VertexPair key of the element to add.
		/// </param>
		/// <param name="value">
		/// The double value of the element to add.
		/// </param>
		public  void Add(VertexPair key, double value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this VertexPairDoubleDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The VertexPair key to locate in this VertexPairDoubleDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexPairDoubleDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(VertexPair key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexPairDoubleDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The VertexPair key to locate in this VertexPairDoubleDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexPairDoubleDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(VertexPair key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexPairDoubleDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The double value to locate in this VertexPairDoubleDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexPairDoubleDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(double value)
		{
			foreach (double item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this VertexPairDoubleDictionary.
		/// </summary>
		/// <param name="key">
		/// The VertexPair key of the element to remove.
		/// </param>
		public  void Remove(VertexPair key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this VertexPairDoubleDictionary.
		/// </summary>
		public  System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this VertexPairDoubleDictionary.
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
