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

namespace MbUnit.Framework {
    /// <summary>
    /// <para>
    /// The test attribute is applied to a method that represents a single test
    /// case within a fixture.  By default, if the method throws an unexpected exception,
    /// the test will be deemed to have failed.  Otherwise, the test will pass.
    /// </para>
    /// <para>
    /// The default behavior may be modified by test decorator attributes that
    /// may alter the execution environment of the test, catch and reinterpret
    /// any exceptions it throws, or impose additional constraints upon its execution.
    /// </para>
    /// <para>
    /// Output from the test, such as text written to the console, is captured
    /// by the framework and will be included in the test report.  
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The method to which this attribute is applied must be declared by the
    /// fixture class and must not have any parameters.  The method may be static.
    /// </para>
    /// <para><em>Implements:</em> Type Test Pattern</para>
    /// <para><em>Logic:</em>
    /// <code>
    /// {Provider}
    /// [SetUp]
    /// {Test}
    /// [TearDown]
    /// </code>
    /// </para>
    /// <para>
    /// This fixture is quite similar to the <b>Simple Test</b> pattern, but it applies to
    /// any instance of a particular type provided by the user.
    /// </para>
    /// <para>
    /// The test fixture first looks for methods tagged with the <see cref="ProviderAttribute"/>
    /// method. These method must return an object assignable with the tested type. This instance will
    /// be feeded to the other methods of the fixture.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// [TestFixture]
    /// public class Fixture 
    /// {
    ///    [Test]
    ///    public void Test1()    
    ///    {...}
    /// 
    ///    [Test]
    ///    public void Test2()
    ///    {...}
    /// }
    /// </code>
    /// </example>	
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class TestAttribute : TestPatternAttribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttribute"/> class.
        /// </summary>
        public TestAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description of the test.</param>
        public TestAttribute(string description)
            : base(description) { }
    }
}
