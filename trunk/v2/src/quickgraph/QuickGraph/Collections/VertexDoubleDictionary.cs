// QuickGraph Library 
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux


using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	/// <summary>
	/// A dictionary with keys of type Vertex and values of type Double
	/// </summary>
	public sealed class VertexDoubleDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexDistanceDictionary class
		/// </summary>
		public VertexDoubleDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the Double associated with the given Vertex
		/// </summary>
		/// <param name="key">
		/// The Vertex whose value to get or set.
		/// </param>
		public  Double this[IVertex key]
		{
			get
			{
				return (Double) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this VertexDistanceDictionary.
		/// </summary>
		/// <param name="key">
		/// The Vertex key of the element to add.
		/// </param>
		/// <param name="value">
		/// The Double value of the element to add.
		/// </param>
		public  void Add(IVertex key, Double value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this VertexDistanceDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Vertex key to locate in this VertexDistanceDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexDistanceDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexDistanceDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Vertex key to locate in this VertexDistanceDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexDistanceDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexDistanceDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The Double value to locate in this VertexDistanceDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexDistanceDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(Double value)
		{
			foreach (Double item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this VertexDistanceDictionary.
		/// </summary>
		/// <param name="key">
		/// The Vertex key of the element to remove.
		/// </param>
		public  void Remove(IVertex key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this VertexDistanceDictionary.
		/// </summary>
		public  System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this VertexDistanceDictionary.
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
