// MbUnit Test Framework
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
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux

using System;

namespace MbUnit.Core.Remoting
{
	/// <summary>
	/// A dictionary with keys of type Guid and values of type TestTreeNode
	/// </summary>
	public class GuidTestTreeNodeDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the GuidTestTreeNodeDictionary class
		/// </summary>
		public GuidTestTreeNodeDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the TestTreeNode associated with the given Guid
		/// </summary>
		/// <param name="key">
		/// The Guid whose value to get or set.
		/// </param>
		public virtual TestTreeNode this[Guid key]
		{
			get
			{
				return (TestTreeNode) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this GuidTestTreeNodeDictionary.
		/// </summary>
		/// <param name="value">
		/// The TestTreeNode value of the element to add.
		/// </param>
		public virtual void Add(TestTreeNode value)
		{
			this.Dictionary.Add(value.Identifier, value);
		}

		/// <summary>
		/// Determines whether this GuidTestTreeNodeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Guid key to locate in this GuidTestTreeNodeDictionary.
		/// </param>
		/// <returns>
		/// true if this GuidTestTreeNodeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(Guid key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this GuidTestTreeNodeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Guid key to locate in this GuidTestTreeNodeDictionary.
		/// </param>
		/// <returns>
		/// true if this GuidTestTreeNodeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(Guid key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Removes the element with the specified key from this GuidTestTreeNodeDictionary.
		/// </summary>
		/// <param name="key">
		/// The Guid key of the element to remove.
		/// </param>
		public virtual void Remove(Guid key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this GuidTestTreeNodeDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this GuidTestTreeNodeDictionary.
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
