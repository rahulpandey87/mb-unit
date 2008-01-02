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
using System.Collections.Specialized;

namespace MbUnit.Framework
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
	public class TestCategoryAttribute : Attribute
	{
		private ArrayList categories = new ArrayList();
	
		public TestCategoryAttribute(Object category)
		{
			this.categories.Add(category);
		}
	
		public TestCategoryAttribute(Object category,params object[] categories)
		{
			this.categories.Add(category);
			this.categories.AddRange(categories);
		}
		
		public ArrayList Categories
		{
			get
			{
				return this.categories;
			}
		}
		
		public StringCollection GetCategoryTitles()
		{
			SortedList sl =new SortedList();
			foreach(Object o in this.categories)
			{
				sl[o.ToString()]=null;
			}
			
			StringCollection sc =new StringCollection();
			foreach(Object key in sl.Keys)
				sc.Add(key.ToString());
			return sc;
		}
	}
}
