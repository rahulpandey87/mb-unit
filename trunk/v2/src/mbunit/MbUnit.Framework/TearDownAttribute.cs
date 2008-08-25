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

namespace MbUnit.Framework
{
    /// <summary>
    /// <para>
    /// The tear down attribute is applied to a method that is to be invoked after
    /// each test in a fixture executes.  The method will run once for each test.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// The attribute may be applied to multiple methods within a fixture, however
    /// the order in which they are processed is undefined.
    /// </para>
    /// <para>
    /// The method to which this attribute is applied must be declared by the
    /// fixture class and must not have any parameters.  The method may be static.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>In this example MySetup will be run before each test in the SampleFixture class is run 
    /// and MyTearDown will be run after each test finished execution</para>
    /// <code>
    /// [TestFixture]
    /// public class SampleFixture
    /// {
    ///    private Foo foo;
    ///    [SetUp]
    ///    public void MySetUp()
    ///    {
    ///        foo = new Foo(); 
    ///    }    
    ///     ...
    /// 
    ///    [TearDown]
    ///    public void MyTearDown()
    ///    {
    ///        if (foo!=null)
    ///        {
    ///           foo.Dispose();
    ///           foo=null;
    ///        }
    ///    }
    /// }
    /// </code>
    /// </example>
    /// <see cref="SetUpAttribute"/>
    /// <see cref="TestFixtureAttribute"/>
    /// <see cref="TestAttribute"/>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
	public class TearDownAttribute : PatternAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="TearDownAttribute"/> class.
        /// </summary>
		public TearDownAttribute()
		{}

        /// <summary>
        /// Initializes a new instance of the <see cref="TearDownAttribute"/> class.
        /// </summary>
        /// <param name="description">A description of the teardown method.</param>
		public TearDownAttribute(string description)
			:base(description)
		{}
	}
}
