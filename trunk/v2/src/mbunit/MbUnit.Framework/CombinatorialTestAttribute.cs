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
using MbUnit.Core.Framework;
using TestFu.Operations;

namespace MbUnit.Framework {
    /// <summary>
    /// Defines how sets of values will be combined in a <see cref="CombinatorialTestAttribute">Combinatorial Test</see>
    /// </summary>
    [Serializable]
    public enum CombinationType {
        AllPairs,
        Cartesian
    }

    /// <summary>
    /// Tag use to mark a mark a combinatorial unit test method. This tells MbUnit to do a pairwise enumeration over the different data sample of each parameter for the test
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each parameter in the test method should be of a type that yields various values for inclusion in the pairwase evaluation. They
    /// can be tagged with either <see cref="FactoryAttribute"/> at their class definition or one of the <see cref="UsingEnumAttribute"/>, 
    /// <see cref="UsingFactoriesAttribute"/>, <see cref="UsingImplementationsAttribute"/>, <see cref="UsingLinearAttribute"/>, or
    /// <see cref="UsingLiteralsAttribute"/>
    /// </para>
    /// 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class CombinatorialTestAttribute : TestPatternAttribute {
        private CombinationType combinationType = CombinationType.AllPairs;

        private string tupleValidatorMethod = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinatorialTestAttribute" /> class.
        /// </summary>
        /// <remarks>
        /// The <see cref="CombinationType"/> for this test will be set to AllPairs. 
        /// </remarks>
        public CombinatorialTestAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinatorialTestAttribute" /> class with the named CombinationType
        /// </summary>
        /// <param name="combinationType">The type of combination to be applied to the values of each parameter in testing</param>
        public CombinatorialTestAttribute(CombinationType combinationType) {
            this.combinationType = combinationType;
        }

        /// <summary>
        /// Returns the <see cref="CombinationType"/> for this test.
        /// </summary>
        public CombinationType CombinationType {
            get { return this.combinationType; }
        }

        /// <summary>
        /// Gets or sets the method used by MbUnit to determine whether or not a combination of values in valid or not
        /// and should or should not be included in the set of tests being carried out
        /// </summary>
        /// <example>
        /// <code>
        /// [CombinatorialTest(TupleValidatorMethod = "IsValid")]
        /// public void TestWithValidator(string a, string b)
        /// {
        ///    ...
        /// }
        /// 
        /// public bool IsValid(string s1, string s2)
        /// {
        ///  if (s1 == null && s2 == null)
        ///  return false;
        ///  if (s1 != null && s2 != null)
        ///  return false;
        ///  return true;
        ///}
        /// </code>
        /// </example>
        public string TupleValidatorMethod {
            get {
                return tupleValidatorMethod;
            }

            set {
                tupleValidatorMethod = value;
            }
        }

        /// <summary>
        /// Returns the object representing the set of all tuples to be tested.
        /// </summary>
        /// <param name="domains">The collection of sets of parameter values to be combined</param>
        /// <returns>An <see cref="ITupleEnumerable"/> object representing the set of tuples to be tested</returns>
        public ITupleEnumerable GetProduct(IDomainCollection domains) {
            switch (this.CombinationType) {
                case CombinationType.AllPairs:
                    return Products.PairWize(domains);
                case CombinationType.Cartesian:
                    return Products.Cartesian(domains);
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
