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
    /// The DataProvider method attribute allows you to mark a method as your collection populator for tests.
    /// </summary>
    /// <remarks>This tag should be used within classes marked with an <see cref="EnumerationFixtureAttribute"/>.</remarks>
    /// <example>
    /// <code>
    ///    [EnumerationFixture]
    ///    public class EnumerationFixtureAttributeAttributeTest {
    ///        private Random rnd = new Random();
    ///        private int count = 100;
    /// 
    ///        [DataProvider(typeof(ArrayList))]
    ///        public ArrayList Data() {
    ///            ArrayList list = new ArrayList();
    ///            for (int i = 0; i &lt; count; ++i)
    ///                list.Add(rnd.Next());
    ///            return list;
    ///        }
    ///        [CopyToProvider(typeof(ArrayList))]
    ///        public ArrayList ArrayListProvider(IList source) {
    ///            ArrayList list = new ArrayList(source);
    ///            return list;
    ///        }
    ///        [CopyToProvider(typeof(int[]))]
    ///        public int[] IntArrayProvider(IList source) {
    ///            int[] list = new int[source.Count];
    ///            source.CopyTo(list, 0);
    ///            return list;
    ///        }
    ///    }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DataProviderAttribute : NonTestPatternAttribute {
        private Type providerType = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderAttribute"/> class.
        /// </summary>
        /// <param name="providerType">The <see cref="Type"/> of object to return to tests expecting data</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="providerType"/> is null</exception>
        public DataProviderAttribute(Type providerType) {
            if (providerType == null)
                throw new ArgumentNullException("providerType");
            this.providerType = providerType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderAttribute"/> class.
        /// </summary>
        /// <param name="description">The description of the provider for reference</param>
        /// <param name="providerType">The <see cref="Type"/> of object to return to tests expecting data</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="providerType"/> is null</exception>
        public DataProviderAttribute(Type providerType, string description)
            : base(description) {
            if (providerType == null)
                throw new ArgumentNullException("providerType");
            this.providerType = providerType;
        }

        /// <summary>
        /// Gets or sets <see cref="Type"/> of object to return to tests expecting data
        /// </summary>
        /// <value>The <see cref="Type"/> of object to return to tests expecting data</value>
        public Type ProviderType {
            get {
                return this.providerType;
            }
            set {
                this.providerType = value;
            }
        }
    }
}
