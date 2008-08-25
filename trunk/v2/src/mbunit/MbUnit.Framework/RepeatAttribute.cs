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
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework 
{
    /// <summary>
    /// <para>
    /// This attribute decorates a test method or fixture and causes it to be invoked repeatedly within the same thread.
    /// </para>
    /// </summary>
    /// <seealso cref="ThreadedRepeatAttribute"/>
    [Obsolete("This attribute has been replaced by the RepeatTestAttribute.")]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class RepeatAttribute : DecoratorPatternAttribute
    {
		private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatAttribute"/> class.
        /// </summary>
        /// <param name="count">The number of times to repeat the test</param>
        /// <example>
        /// <para>In this example the test will be repeated ten times</para>
        /// <code>
        /// [Test]
        /// [Repeat(10)]
        /// public void Test()
        /// {
        ///     // This test will be executed 10 times.
        /// }
        /// </code>
        /// </example>
		public RepeatAttribute(int count)
		{
			this.count = count;	
		}

        /// <summary>
        /// Returns the invoker class to run the test the given number of times.
        /// </summary>
        /// <param name="wrapper">The invoker currently set to run the test.</param>
        /// <returns>A new <see cref="RepeatRunInvoker"/> object wrapping <paramref name="wrapper"/></returns>
		public override IRunInvoker GetInvoker(MbUnit.Core.Invokers.IRunInvoker wrapper) 
		{
			return new RepeatRunInvoker(wrapper,count);			
		}
	}
}
