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

using MbUnit.Framework;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework {
    /// <summary>
    /// Tags a test method to indicate that it should throw an exception.
    /// </summary>
    /// <remarks>
    /// <para>The ExpectedExceptionAttribute is a TestDecorator that enables you to create negative tests, i.e. tests that check that your application fails the way it should.</para>
    /// <para>You can test for the type fo exception thrown, the message thrown in the exception and the inner exception</para>
    /// </remarks>
    /// <example>
    /// <para>Use a CustomAttribute to tag a test with the expected exception type:</para>
    /// <code>
    /// [Test]
    /// [ExpectedException(typeof(MyCustomException))]
    /// public void NegativeTest()
    /// { ... }
    /// </code>
    /// <para>A classical example of usage is to check that a method checks that it's arguments are non-null.</para>
    /// <code>
    ///public class Foo
    ///{
    ///   public Object Method(Object o)
    ///   {
    ///       return o.ToString();
    ///    }
    ///}
    ///
    ///[Test]
    ///[ExpectedException(typeof(ArgumentNullException))]
    ///public void MethodArgumentNull()
    ///{
    ///   Foo foo = new Foo();
    ///   foo.Method(null);
    ///}
    /// </code>
    /// <para>
    ///This test will fail because the thrown exception (NullReferenceException) is not of the right type (ArgumentNullException).</para>
    ///<para>As of version 2.4, you can also test the expected message and the inner exception:</para>
    ///<code>
    ///[Test, ExpectedException(typeof(NotImplementedException), "This should match.")]
    ///public void ExceptionAndExpectedMessage()
    ///{
    ///    throw new NotImplementedException("This should match.");
    ///}
    ///
    ///[Test, ExpectedException(typeof(NotImplementedException), typeof(ArgumentException))]
    ///public void ExceptionAndInnerException()
    ///{
    ///    throw new NotImplementedException("", new ArgumentException());
    ///}
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ExpectedExceptionAttribute : DecoratorPatternAttribute {
        Type exceptionType = null;
        string expectedMessage = null;
        Type innerExceptionType = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedExceptionAttribute"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of the exception expected to occur during test execution.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exceptionType"/> is null</exception>
        public ExpectedExceptionAttribute(Type exceptionType)
            : this(exceptionType, null, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedExceptionAttribute"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of the exception expected to occur during test execution.</param>
        /// <param name="innerExceptionType">Type of the inner exception expected to be raised during test execution.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exceptionType"/> is null</exception>
        public ExpectedExceptionAttribute(Type exceptionType, Type innerExceptionType)
            : this(exceptionType, null, innerExceptionType) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedExceptionAttribute"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of the exception expected to occur during test execution.</param>
        /// <param name="expectedMessage">The expected message to be returned with the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exceptionType"/> is null</exception>
        public ExpectedExceptionAttribute(Type exceptionType, string expectedMessage)
            : this(exceptionType, expectedMessage, null) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedExceptionAttribute"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of the exception expected to occur during test execution.</param>
        /// <param name="innerExceptionType">Type of the inner exception expected to be raised during test execution.</param>
        /// <param name="expectedMessage">The expected message to be returned with the exception.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="exceptionType"/> is null</exception>
        public ExpectedExceptionAttribute(Type exceptionType, string expectedMessage, Type innerExceptionType) {
            if (exceptionType == null)
                throw new ArgumentNullException("exceptionType");
            this.exceptionType = exceptionType;
            this.expectedMessage = expectedMessage;
            this.innerExceptionType = innerExceptionType;
        }

        /// <summary>
        /// The expected exception.
        /// </summary>
        /// <value>The type of the exception expected to be thrown.</value>
        public Type ExceptionType {
            get {
                return this.exceptionType;
            }
        }

        /// <summary>
        /// The expected message text.
        /// </summary>
        /// <value>The expected message.</value>
        public string ExpectedMessage {
            get {
                return this.expectedMessage;
            }
        }

        /// <summary>
        /// The expected inner exception.
        /// </summary>
        /// <value>The type of the expected inner exception.</value>
        public Type InnerExceptionType {
            get {
                return innerExceptionType;
            }
        }

        /// <summary>
        /// Gets the test runner class that knows how to invoke the test and expect the specified exception
        /// </summary>
        /// <param name="invoker">The invoker currently invoking the test.</param>
        /// <returns>A new <see cref="ExpectedExceptionRunInvoker"/> object wrapping <paramref name="invoker"/></returns>
        public override IRunInvoker GetInvoker(IRunInvoker invoker) {
            return new ExpectedExceptionRunInvoker(invoker, ExceptionType, ExpectedMessage, InnerExceptionType, Description);
        }
    }
}
