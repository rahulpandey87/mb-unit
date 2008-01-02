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
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using System;
using System.Collections;
using System.Reflection;

namespace MbUnit.Core.Collections
{
	/// <summary>
	/// Summary description for AttributedMethodCollection.
	/// </summary>
	public sealed class AttributedPropertyCollection : ICollection
	{
		private Type testedType;
		private Type customAttributeType;

		public AttributedPropertyCollection(Type testedType, Type customAttributeType)
		{
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");
			this.testedType = testedType;
			this.customAttributeType = customAttributeType;
		}
		
		public Object SyncRoot
		{
			get
			{
				return this;
			}
		}
		
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}
		
		public void CopyTo(Array array, int index)
		{
			int i = index;
			foreach(PropertyInfo mi in this)
			{
				array.SetValue(mi,i++);	
			}
		}
		
		public int Count
		{
			get
			{
				AttributedPropertyEnumerator en = GetEnumerator();
				int n = 0;
				while(en.MoveNext())
					++n;
				return n;
			}
		}

		public AttributedPropertyEnumerator GetEnumerator()
		{
			return new AttributedPropertyEnumerator(
				this.testedType,
				this.customAttributeType
				);
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
