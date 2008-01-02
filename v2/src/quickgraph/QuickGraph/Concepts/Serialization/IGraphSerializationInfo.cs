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

namespace QuickGraph.Concepts.Serialization
{
	/// <summary>
	/// A class for adding and retreiving atomic data.
	/// </summary>
	public interface IGraphSerializationInfo : ICollection
	{
		/// <summary>
		/// Get a value indicating if the object is serializing
		/// </summary>
		/// <value>
		/// true if serializing, false if deserializing
		/// </value>
		bool IsSerializing{get;}

		/// <summary>
		/// Adds a new key-value pair
		/// </summary>
		/// <param name="key">value identifier</param>
		/// <param name="value">value</param>
		/// <exception cref="ArgumentNullException">key is a null reference</exception>
		void Add(string key, object value);

		/// <summary>
		/// Gets a value indicating if the key is in the entry collection
		/// </summary>
		/// <param name="key">key to test</param>
		/// <returns>true if key is in the dictionary, false otherwise</returns>
		/// <exception cref="ArgumentNullException">key is a null reference</exception>
		bool Contains(string key);

		/// <summary>
		/// Gets or sets a value from a key
		/// </summary>
		/// <param name="key">value identifier</param>
		/// <returns>
		/// value associated with the key. If the key is not present
		/// in the data, null value is returned
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// get,set property, key is a null reference
		/// </exception>
		object this[string key] {get;set;}
	}
}
