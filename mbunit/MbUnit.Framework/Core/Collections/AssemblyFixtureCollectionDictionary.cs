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
using System.Reflection;

namespace MbUnit.Core.Collections
{
	/// <summary>
	/// A dictionary with keys of type Assembly and values of type TypeCollection
	/// </summary>
	public sealed class AssemblyFixtureCollectionDictionary: System.Collections.DictionaryBase
	{
		/// <summary>
		/// Initializes a new empty instance of the AssemblyTypeCollectionDictionary class
		/// </summary>
		public AssemblyFixtureCollectionDictionary()
		{
			// empty
		}

		/// <summary>
		/// Gets or sets the TypeCollection associated with the given Assembly
		/// </summary>
		/// <param name="key">
		/// The Assembly whose value to get or set.
		/// </param>
		public FixtureCollection this[Assembly key]
		{
			get
			{
				return (FixtureCollection) this.Dictionary[key];
			}
		}

		/// <summary>
		/// Adds an element with the specified key and value to this AssemblyTypeCollectionDictionary.
		/// </summary>
		/// <param name="key">
		/// The Assembly key of the element to add.
		/// </param>
		public void Add(Assembly key)
		{
            if (key == null)
                throw new ArgumentNullException("key");
            this.Dictionary.Add(key, new FixtureCollection());
		}

		/// <summary>
		/// Determines whether this AssemblyTypeCollectionDictionary contains a specific key.
		/// </summary>
		/// <param name="key">
		/// The Assembly key to locate in this AssemblyTypeCollectionDictionary.
		/// </param>
		/// <returns>
		/// true if this AssemblyTypeCollectionDictionary contains an element with the specified key;
		/// otherwise, false.
		/// </returns>
		public bool Contains(Assembly key)
		{
			return this.Dictionary.Contains(key);
		}

		/// <summary>
		/// Removes the element with the specified key from this AssemblyTypeCollectionDictionary.
		/// </summary>
		/// <param name="key">
		/// The Assembly key of the element to remove.
		/// </param>
		public void Remove(Assembly key)
		{
			this.Dictionary.Remove(key);
		}

		/// <summary>
		/// Gets a collection containing the keys in this AssemblyTypeCollectionDictionary.
		/// </summary>
		public System.Collections.ICollection Keys
		{
			get
			{
				return this.Dictionary.Keys;
			}
		}

		/// <summary>
		/// Gets a collection containing the values in this AssemblyTypeCollectionDictionary.
		/// </summary>
		public System.Collections.ICollection Values
		{
			get
			{
				return this.Dictionary.Values;
			}
		}
	}

}
