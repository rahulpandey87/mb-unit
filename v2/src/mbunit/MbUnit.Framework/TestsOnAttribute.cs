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
    /// Tags a class with the object type being tested by the methods it contains.
    /// The MbUnit GUI runner uses this information to group tests under the TestsOns tree.
    /// The default value is "Unknown"
    /// </summary>
    /// <example>
    /// <code>
    /// [TestFixture]
    /// [TestsOn("ArrayList")]
    /// public class SampleFixture
    /// {
    ///    [Test]
    ///    public void MyTest()
    ///    {
    ///        ... does something with an ArrayList 
    ///    }    
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true,Inherited =true)]
    public class TestsOnAttribute : Attribute
    {
        private Type testedType;
        /// <summary>
        /// Initializes a new instance of the <see cref="TestsOnAttribute"/> class.
        /// </summary>
        /// <param name="testedType">The <see cref="Type"/> of object being tested within the fixture.</param>
        public TestsOnAttribute(Type testedType)
        {
            if (testedType == null)
                throw new ArgumentNullException("testedType");
            this.testedType = testedType;
        }

        /// <summary>
        /// Gets the type of the tested object.
        /// </summary>
        /// <value>The type of the tested object.</value>
        public Type TestedType
        {
            get
            {
                return this.testedType;
            }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("TestsOn({0})", this.testedType.FullName);
        }
    }
}
