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

namespace MbUnit.Framework {
    /// <summary>
    /// Used with Combinatorial Tests to indicate how to generate values for a test parameter.
    /// Can be tagged onto a parameter directly or a method which yields values.
    /// </summary>
    /// <example>
    /// This code demonstrates a simple factory method returning weekdays as strings and a test using it.
    /// <code>
    /// [Factory(typeof(String))]
    /// public IEnumerable Weekdays()
    /// {
    ///     yield return "Monday";
    ///     yield return "Tuesday";
    ///     yield return "Wednesday";
    ///     yield return "Thursday";
    ///     yield return "Friday";
    /// }
    /// 
    /// [CombinatorialTest]
    /// public DayLengthIsSix(
    ///     [UsingFactories("Weekdays")] string day)
    /// {
    ///     Assert.Equals(day.Length, 6);
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class FactoryAttribute : Attribute {
        private Type factoredType = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryAttribute"/> class.
        /// </summary>
        public FactoryAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryAttribute"/> class.
        /// </summary>
        /// <param name="factoredType">The <see cref="Type"/> of the objects returned by the factory method</param>
        public FactoryAttribute(Type factoredType) {
            if (factoredType == null)
                throw new ArgumentNullException("factoredType");
            this.factoredType = factoredType;
        }

        /// <summary>
        /// Returns the <see cref="Type"/> of the factory-yielded values.
        /// </summary>
        /// <value>The <see cref="Type"/> of the values.</value>
        public Type FactoredType {
            get {
                return this.factoredType;
            }
        }
    }
}
