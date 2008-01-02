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
using System.IO;

namespace QuickGraph.Serialization
{
	using QuickGraph.Concepts.Serialization;

	/// <summary>
	/// A data holder class
	/// </summary>
	public class GraphSerializationInfo : 
		IGraphSerializationInfo,
		ICollection
	{
		private bool isSerializing;
		private StringWriter hasher=new StringWriter();
		private Hashtable values = new Hashtable();

		/// <summary>
		/// Creates a new data holder class.
		/// </summary>
		/// <param name="isSerializing">true if it is used for serialization, 
		/// false otherwize</param>
		public GraphSerializationInfo(bool isSerializing)
		{
			this.isSerializing = isSerializing;
		}

		/// <summary>
		/// True if serializing
		/// </summary>
		public bool IsSerializing
		{
			get
			{
				return this.isSerializing;
			}
		}

		/// <summary>
		/// Number of key-value pair in the data bag.
		/// </summary>
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		/// <summary>
		/// Adds a new key-value pair
		/// </summary>
		/// <param name="key">value identifier</param>
		/// <param name="value">value</param>
		/// <exception cref="ArgumentNullException">key</exception>
		public void Add(string key, object value)
		{
			if (key==null)
				throw new ArgumentNullException("key");
			this.values.Add(key,value);
			this.hasher.WriteLine("<name=\"{0}\" value=\"{1}\"",key,value);
		}

		public string GetHashKey()
		{
			return this.hasher.ToString();
		}

		#region IDictionary Members

		public bool IsReadOnly
		{
			get
			{
				return !this.isSerializing;
			}
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return this.values.GetEnumerator();
		}

		/// <summary>
		/// Gets or sets a data entry in the graph info collection
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// set property,set a value while the graph info is deserializing
		/// </exception>
		/// <exception cref="MissingFieldException">
		/// get property, the requested key is not found
		/// </exception>
		public object this[string key]
		{
			get
			{
				if (!this.Contains(key))
					throw new MissingFieldException("key");

				return this.values[key];
			}
			set
			{
				if (!this.IsSerializing)
					throw new InvalidOperationException("graph is deserializing, cannot modify");

				this.values[key]=value;
			}
		}

		/// <summary>
		/// Gets a value indicating if the key is in the data entries.
		/// </summary>
		/// <param name="key">key to test</param>
		/// <returns>
		/// true if key is in the data collection, false otherwise
		/// </returns>
		public bool Contains(string key)
		{
			return this.values.Contains(key);
		}

		public ICollection Keys
		{
			get
			{
				return this.values.Keys;
			}
		}

		public ICollection Values
		{
			get
			{
				return this.values.Values;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return !this.isSerializing;
			}
		}

		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		public void CopyTo(Array array, int index)
		{
			this.values.CopyTo(array,index);
		}

		public object SyncRoot
		{
			get
			{
				return this.values;
			}
		}

		#endregion

		#region IEnumerable Members

		IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
