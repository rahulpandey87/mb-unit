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
using System.Xml.Serialization;

using MbUnit.Framework;

namespace MbUnit.Core.Filters
{
    /// <summary>
    /// Filter class for FixtureCategory attribute.
    /// </summary>
    [Serializable, XmlRoot("Category", IsNullable = false)]
	public sealed class CategoryFixtureFilter : PatternFixtureFilter
    {
        public CategoryFixtureFilter(string pattern)
            : base(pattern)
        {}

        /// <summary>
        /// Tests if a fixture has a category attribute matching a pattern.
        /// </summary>
        /// <param name="fixture">The fixture to test.</param>
        /// <returns>true if the fixture has a matching category attribute, otherwise false.</returns>
        public override bool Filter(Type fixture)
        {
			if (fixture == null)
				throw new ArgumentNullException("fixture");
			// Get category attributes
            foreach (FixtureCategoryAttribute cat in fixture.GetCustomAttributes(typeof(FixtureCategoryAttribute), true))
            {
                if (cat.Category.StartsWith(this.Pattern))
                {
                    return true;
                }
            }
            return false;
        }
    }
}