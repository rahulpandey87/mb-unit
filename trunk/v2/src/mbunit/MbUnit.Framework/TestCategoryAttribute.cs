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

    /// <summary>
    /// Associates a category name with a test method.  
    /// The category name can be used to classify tests and build test suites of related tests.
    /// </summary>
    /// <remarks>Associate multiple categories with a comma-delimited lsit of strings</remarks>
    /// <example>
    /// [TestFixture]
    /// public class CategorizedTests
    /// {
    ///     [Test]
    ///     [Category("ThisCategory")]
    ///     public void SingleCategoryTest()
    ///     {
    ///         ...
    ///     }
    /// 
    ///     [Test]
    ///     [Category("ThatCategory", "MyCategory")]
    ///     public void MultipleCategoryTest()
    ///     {
    ///         ...
    ///     }
    /// 
    /// }
    /// </example>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
	public class TestCategoryAttribute : Attribute
	{
		private ArrayList categories = new ArrayList();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCategoryAttribute"/> class.
        /// Associates a category name with the test method annotated by this attribute.
        /// </summary>
        /// <param name="category">The category name to associate</param>
		public TestCategoryAttribute(Object category)
		{
			this.categories.Add(category);
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCategoryAttribute"/> class.
        /// Associates multiple category names with the test method annotated by this attribute.
        /// </summary>
        /// <param name="category">The category name to associate</param>
        /// <param name="categories">An array of further categories to associate</param>
		public TestCategoryAttribute(Object category,params object[] categories)
		{
			this.categories.Add(category);
			this.categories.AddRange(categories);
		}

        /// <summary>
        /// Gets the categories for the test.
        /// </summary>
        /// <value>The categories for the test.</value>
		public ArrayList Categories
		{
			get
			{
				return this.categories;
			}
		}

        /// <summary>
        /// Gets the category titles.
        /// </summary>
        /// <returns>A <see cref="StringCollection"/> of category titles</returns>
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
