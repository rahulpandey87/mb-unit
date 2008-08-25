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

// created on 30/01/2004 at 18:31

namespace MbUnit.Framework {
    using System;
    using MbUnit.Framework;
    using MbUnit.Core.Invokers;

    /// <summary>
    /// Tags method that should throw an exception if a predicate is true.
    /// </summary>
    /// <example>
    /// In the following example, the test expects an exception to be thrown if the list is is read only
    /// <code>
    ///    [TypeFixture(typeof(IList))]
    ///    public class ListTest {
    ///        public bool IsReadOnly(IList list) {
    ///            return list.ReadOnly;
    ///        }
    ///
    ///        [Test("List accepts null values")]
    ///        [ConditionalException(typeof(NotSupportedException), "IsReadOnly")]
    ///        public void AddNull(IList list) {
    ///            list.Add(null);
    ///            ...
    ///        }
    ///    }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ConditionalExceptionAttribute : ExpectedExceptionAttribute {
        string predicateMethodName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalExceptionAttribute" /> class with the expected type of exception and the name fo the predicate to check
        /// </summary>
        /// <param name="exceptionType">The <see cref="Type"/> of excpetion to expect</param>
        /// <param name="predicateMethodName">The name of the predicate method to check</param>
        /// <exception cref="ArgumentNullException">Thrown in <paramref name="predicateMethodName"/> is null</exception>
        public ConditionalExceptionAttribute(Type exceptionType, string predicateMethodName)
            : this(exceptionType, predicateMethodName, "") {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalExceptionAttribute" /> class with the expected type of exception and the name fo the predicate to check
        /// </summary>
        /// <param name="exceptionType">The <see cref="Type"/> of excpetion to expect</param>
        /// <param name="predicateMethodName">The name of the predicate method to check</param>
        /// <param name="description">A string to display if the exception is not thrown when expected</param>
        /// <exception cref="ArgumentNullException">Thrown in <paramref name="predicateMethodName"/> is null</exception>
        public ConditionalExceptionAttribute(Type exceptionType, string predicateMethodName, string description)
            : base(exceptionType, description) {
            if (predicateMethodName == null)
                throw new ArgumentNullException("predicateMethodName");
            this.predicateMethodName = predicateMethodName;
        }

        /// <summary>
        /// Gets the name of the predicate being cecked during test execution
        /// </summary>
        public String PredicateMethodName {
            get {
                return this.predicateMethodName;
            }
        }

        /// <summary>
        /// Returns a <see cref="ConditionalExceptionRunInvoker"/> object to invoke the test if the predicate is true
        /// </summary>
        /// <param name="invoker">The standard test invoker derived from <see cref="IRunInvoker"/></param>
        /// <returns>A <see cref="ConditionalExceptionRunInvoker"/> object to invoke the test if the predicate is true</returns>
        public override IRunInvoker GetInvoker(IRunInvoker invoker) {
            return new ConditionalExceptionRunInvoker(
                invoker,
                this.ExceptionType,
                this.Description,
                this.predicateMethodName);
        }
    }
}
