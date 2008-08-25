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
using MbUnit.Core.Framework;
using System;


namespace MbUnit.Framework {


    /// <summary>
    /// Tags method that provide new objects to be used by tests within a <see cref="TypeFixtureAttribute"/>
    /// tagged fixture class. The type-specific fixture assumes that all tests contained in the fixture that have
    /// an argument of the same type specified in the <see cref="TypeFixtureAttribute"/> will be provided by
    /// the methods tagged by <see cref="ProviderAttribute"/> or by a class specified by the <see cref="ProviderFactoryAttribute"/>
    /// tagging the same fixture class as the <see cref="TypeFixtureAttribute"/>.
    /// </summary>
    /// <remarks>
    /// <para>This fixture is particularly useful for writing fixtures of interfaces and apply it to all the types that implement the interface.</para>
    /// </remarks>
    /// <example>
    /// <para>For a complete demonstration of TypeFixtures and Providers, see <see cref="TypeFixtureAttribute"/>.</para>
    /// <para>The following example shows a method returning an ArrayList marked as a provider for IList methods</para>
    /// <code>
    /// [Provider(typeof(ArrayList))]
    /// public ArrayList ProvideFilledArrayList()
    /// {
    ///     ArrayList list = new ArrayList();
    /// 	list.Add("hello");
    /// 	list.Add("world");
    /// 	return list;
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="ProviderFactoryAttribute"/>
    /// <seealso cref="FactoryAttribute"/>
    /// <seealso cref="TypeFixtureAttribute"/>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ProviderAttribute : PatternAttribute {
        private Type providerType = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderAttribute"/> class.
        /// </summary>
        /// <param name="providerType">The <see cref="Type"/> of object returned by the tagged method</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="providerType"/> is null</exception>
        public ProviderAttribute(Type providerType) {
            if (providerType == null)
                throw new ArgumentNullException("providerType");
            this.providerType = providerType;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderAttribute"/> class.
        /// </summary>
        /// <param name="description">The description of the method.</param>
        /// <param name="providerType">The <see cref="Type"/> of object returned by the tagged method</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="providerType"/> is null</exception>
        public ProviderAttribute(Type providerType, string description)
            : base(description) {
            if (providerType == null)
                throw new ArgumentNullException("providerType");
            this.providerType = providerType;
        }


        /// <summary>
        /// Gets or sets the type returned by the provider.
        /// </summary>
        /// <value>The type returned by the provider.</value>
        /// <exception cref="ArgumentNullException">Thrown if value is null</exception>
        public Type ProviderType {
            get {
                return this.providerType;
            }
            set {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.providerType = value;
            }
        }
    }
}
