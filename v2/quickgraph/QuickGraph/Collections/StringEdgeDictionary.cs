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
//		QuickGraph Library HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux


using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;

	/// <summary>
	/// A dictionary with keys of type String and values of type IEdge
	/// </summary>
	public  sealed class StringEdgeDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the StringEdgeDictionary class
		/// </summary>
		public StringEdgeDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the IEdge associated with the given String
		/// </summary>
		/// <param name="key">
		/// The String whose value to get or set.
		/// </param>
		public  IEdge this[String key]
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
		/// Adds an element with the specified key and value to this StringEdgeDictionary.
		/// </summary>
		/// <param name="key">
		/// The String key of the element to add.
		/// </param>
		/// <param name="value">
		/// The IEdge value of the element to add.
		/// </param>
		public  void Add(String key, IEdge value)
		{
			this.Dictionary.Add(key, value);
		}

		/// <summary>
		/// Determines whether this StringEdgeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The String key to locate in this StringEdgeDictionary.
		/// </param>
		/// <returns>
		/// true if this StringEdgeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool Contains(String key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this StringEdgeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The String key to locate in this StringEdgeDictionary.
		/// </param>
		/// <returns>
		/// true if this StringEdgeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsKey(String key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this StringEdgeDictionary contains a specific value.
		/// </summary>
		/// <param name="value">
		/// The IEdge value to locate in this StringEdgeDictionary.
		/// </param>
		/// <returns>
		/// true if this StringEdgeDictionary contains an element with the specified value;
		/// otherwise, false.
		/// </returns>
		public  bool ContainsValue(IEdge value)
		{
			foreach (IEdge item in this.Dictionary.Values)
			{
				if (item == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the element with the specified key from this StringEdgeDictionary.
		/// </summary>
		/// <param name="key">
		/// The String key of the element to remove.
		/// </param>
		public  void Remove(String key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this StringEdgeDictionary.
		/// </summary>
		public  System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this StringEdgeDictionary.
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
