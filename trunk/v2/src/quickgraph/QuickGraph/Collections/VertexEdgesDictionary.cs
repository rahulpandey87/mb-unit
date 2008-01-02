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
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;

	/// <summary>
	/// A dictionary with keys of type Vertex and values of type EdgeCollection
	/// </summary>
	public sealed class VertexEdgesDictionary: DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the VertexEdgesDictionary class
		/// </summary>
		public VertexEdgesDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the EdgeCollection associated with the given Vertex
		/// </summary>
		/// <param name="key">
		/// The Vertex whose value to get or set.
		/// </param>
		public  EdgeCollection this[IVertex key]
		{
			get
			{
				return (EdgeCollection) this.Dictionary[key];
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this VertexEdgesDictionary.
		/// </summary>
		/// <param name="key">
		/// The Vertex key of the element to add.
		/// </param>
		/// <param name="value">
		/// The EdgeCollection value of the element to add.
		/// </param>
		public  void Add(IVertex key, EdgeCollection value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this VertexEdgesDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Vertex key to locate in this VertexEdgesDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexEdgesDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexEdgesDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Vertex key to locate in this VertexEdgesDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexEdgesDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(IVertex key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this VertexEdgesDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The EdgeCollection value to locate in this VertexEdgesDictionary.
		/// </param>
		/// <returns>
		/// true if this VertexEdgesDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(EdgeCollection value)
		{
			foreach (EdgeCollection item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this VertexEdgesDictionary.
		/// </summary>
		/// <param name="key">
		/// The Vertex key of the element to remove.
		/// </param>
		public  void Remove(IVertex key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this VertexEdgesDictionary.
		/// </summary>
		public  System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this VertexEdgesDictionary.
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
