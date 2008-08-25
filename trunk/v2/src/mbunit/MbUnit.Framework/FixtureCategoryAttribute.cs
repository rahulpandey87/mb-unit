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

namespace MbUnit.Framework
{
    /// <summary>
    /// Used to tag a test fixture class with a category for use in the MbUnit GUi or console runner
    /// </summary>
    /// <example>
    /// <code>
    /// [TestFixture]
    /// [FixtureCategory("My Category")]
    /// public class SomeTests
    /// { ... }
    /// </code>
    /// </example>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class FixtureCategoryAttribute : Attribute
	{
		private string category;
        /// <summary>
        /// Initializes a new instance of the <see cref="FixtureCategoryAttribute"/> class.
        /// </summary>
        /// <param name="category">The name of the category to assing to the test fixture</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="category"/> is null</exception>
		public FixtureCategoryAttribute(string category)
		{
			if(category==null)
				throw new ArgumentNullException("category");
			this.category = category;
		}

        /// <summary>
        /// Gets or sets the test fixture category.
        /// </summary>
        /// <value>The test fixture category.</value>
		public string Category
		{
			get
			{
				return this.category;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.category = value;
			}
		}
	}
}
