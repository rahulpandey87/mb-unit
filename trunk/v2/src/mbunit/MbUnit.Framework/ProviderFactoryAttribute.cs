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
    using MbUnit.Core.Runs;

    /// <summary>
    /// Tags a class also tagged by a <see cref="TypeFixtureAttribute"/> to indicate a class that contains properties
    /// returning objects of the type required by the <see cref="TypeFixtureAttribute"/>. 
    /// </summary>
    /// <remarks>
    /// <para>The type-specific fixture 
    /// assumes that all tests contained in the fixture that have an argument of the same type specified 
    /// in the <see cref="TypeFixtureAttribute"/> will be provided by the methods tagged by <see cref="ProviderAttribute"/> or by a class specified by the <see cref="ProviderFactoryAttribute"/>
    /// tagging the same fixture class as the <see cref="TypeFixtureAttribute"/>.</para>
    /// <para><b>This fixture is particularly useful for writing fixtures of interfaces and apply it to all the types that implement the interface.</b></para>
    /// </remarks>
    /// <example>
    /// <para>For a complete demonstration of TypeFixtures and ProviderFactories, see <see cref="TypeFixtureAttribute"/>.</para>
    /// <para>The following example shows a class called ListFactory being identified as a provider factory 
    /// for a type fixture that tests IList objects</para>
    /// <code>
    /// public class ListFactory
    /// {
    ///     [Factory]
    ///     public GetArrayList
    ///     { 
    ///         get { return new ArrayList(); 
    ///     } 
    /// 
    ///     [Factory]
    ///     public GetIntArray
    ///     {
    ///         get { return new int[] {1, 2, 3}; }
    ///     }
    /// }
    /// 
    /// [TypeFixture(typeof(IList),"IList test")]
    /// [ProviderFactory(typeof(ListFactory), typeof(IList))]
    /// public class ListTester
    /// {
    ///     ... tests using ILists ...
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="ProviderAttribute"/>
    /// <seealso cref="FactoryAttribute"/>
    /// <seealso cref="TypeFixtureAttribute"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ProviderFactoryAttribute : ProviderFixtureDecoratorPatternAttribute {
        private Type factoryType;
        private Type factoredType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFactoryAttribute"/> class.
        /// </summary>
        /// <param name="factoryType">The class providing various objects for the type fixture test class.</param>
        /// <param name="factoredType">The <see cref="Type"/> of the objects being provided to the type fixture.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="factoryType"/> or 
        /// <paramref name="factoredType"/> is null</exception>
        public ProviderFactoryAttribute(Type factoryType, Type factoredType) {
            if (factoryType == null)
                throw new ArgumentNullException("factoryType");
            if (factoredType == null)
                throw new ArgumentNullException("factoredType");

            this.factoryType = factoryType;
            this.factoredType = factoredType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFactoryAttribute"/> class.
        /// </summary>
        /// <param name="factoryType">The class providing various objects for the type fixture test class.</param>
        /// <param name="factoredType">The <see cref="Type"/> of the objects being provided to the type fixture.</param>
        /// <param name="description">A brief description of the provider factory.</param>
        /// <exception cref="ArgumentNullException">Thrown if either <paramref name="factoryType"/> or 
        /// <paramref name="factoredType"/> is null</exception>
        public ProviderFactoryAttribute(Type factoryType, Type factoredType, string description)
            : base(description) {
            if (factoryType == null)
                throw new ArgumentNullException("factoryType");
            if (factoredType == null)
                throw new ArgumentNullException("factoredType");

            this.factoryType = factoryType;
            this.factoredType = factoredType;
        }

        /// <summary>
        /// Gets or sets the provider factory <see cref="Type"/>.
        /// </summary>
        /// <value>The <see cref="Type"/> of the provider factory.</value>
        /// <exception cref="ArgumentNullException">Thrown if set to null</exception>
        public Type FactoryType {
            get {
                return this.factoryType;
            }
            set {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.factoryType = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of objects generated by the provider factory
        /// </summary>
        /// <value>The <see cref="Type"/> of objects generated by the provider factory.</value>
        /// <exception cref="ArgumentNullException">Thrown if set to null</exception>
        public Type FactoredType {
            get {
                return this.factoredType;
            }
            set {
                if (value == null)
                    throw new ArgumentNullException("value");
                this.factoredType = value;
            }
        }


        /// <summary>
        /// Gets the execution logic for the provider factory.
        /// </summary>
        /// <param name="decoratedType">The <see cref="Type"/> of the class tagged with the attribute.</param>
        /// <returns>A <see cref="ProviderFactoryRun"/> object</returns>
        public override IRun GetRun(Type decoratedType) {
            if (decoratedType == null)
                throw new ArgumentNullException("decoratedType");

            // get properties that return
            return new ProviderFactoryRun(
                this.factoryType,
                this.factoredType,
                decoratedType);
        }

    }
}
