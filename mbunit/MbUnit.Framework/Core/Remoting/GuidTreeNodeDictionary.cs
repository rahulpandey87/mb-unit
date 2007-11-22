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
using System.Windows.Forms;

namespace MbUnit.Core.Remoting
{
	/// <summary>
	/// A dictionary with keys of type Guid and values of type TreeNode
	/// </summary>
	public class GuidTreeNodeDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the GuidTreeNodeDictionary class
		/// </summary>
		public GuidTreeNodeDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the TreeNode associated with the given Guid
		/// </summary>
		/// <param name="key">
		/// The Guid whose value to get or set.
		/// </param>
		public virtual TreeNode this[Guid key]
		{
			get
			{
				return (TreeNode) this.Dictionary[key];
			}
			set
			{
				this.Dictionary[key] = value;
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this GuidTreeNodeDictionary.
		/// </summary>
		/// <param name="value">
		/// The TreeNode value of the element to add.
		/// </param>
		public virtual void Add(TreeNode value)
		{
			if (value.Tag is Guid)
				this.Dictionary.Add(value.Tag, value);
		}

		/// <summary>
		/// Determines whether this GuidTreeNodeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Guid key to locate in this GuidTreeNodeDictionary.
		/// </param>
		/// <returns>
		/// true if this GuidTreeNodeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool Contains(Guid key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Determines whether this GuidTreeNodeDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Guid key to locate in this GuidTreeNodeDictionary.
		/// </param>
		/// <returns>
		/// true if this GuidTreeNodeDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public virtual bool ContainsKey(Guid key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Removes the element with the specified key from this GuidTreeNodeDictionary.
		/// </summary>
		/// <param name="key">
		/// The Guid key of the element to remove.
		/// </param>
		public virtual void Remove(Guid key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this GuidTreeNodeDictionary.
		/// </summary>
		public virtual System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this GuidTreeNodeDictionary.
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
