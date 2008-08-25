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
    /// Use this attribute to identify the parent text fixture class or classes whose tests must execute successfully
    /// before the tests in this class are executed
    /// </summary>
    /// <example>
    /// The following demonstrates the identification of two parent fixture classes that must execute
    /// successfully before the tests in ChildTestClass will execute
    /// <code>
    /// [DependsOn(ParentTestClass1)]
    /// [DependsOn(ParentTestClass2)]
    /// public class ChildTestClass
    /// {
    ///    ...
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true, Inherited =true)]
    public sealed class DependsOnAttribute : Attribute
    {
        private Type parentFixtureType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class.
        /// </summary>
        /// <param name="parentFixtureType">Type of the parent fixture.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="parentFixtureType"/> is null</exception>
        public DependsOnAttribute(Type parentFixtureType)
        {
            if (parentFixtureType == null)
                throw new ArgumentNullException("parentFixtureType");
            this.parentFixtureType = parentFixtureType;
        }

        /// <summary>
        /// Gets or sets the type of the parent fixture.
        /// </summary>
        /// <value>The type of the parent fixture class.</value>
        public Type ParentFixtureType
        {
            get
            {
                return this.parentFixtureType;
            }
            set
            {
                this.parentFixtureType = value;
            }
        }
    }
}
