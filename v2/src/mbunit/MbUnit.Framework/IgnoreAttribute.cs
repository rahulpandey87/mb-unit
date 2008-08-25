// Copyright (c) 2007 mbunit.com
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

using System;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;

namespace MbUnit.Framework 
{
    /// <summary>
    /// <para>
    /// Indicates that a test is to be ignored by the framework and will not be run.
    /// The test will still appear in test reports along with the reason that it
    /// was ignored, if provided.
    /// </para>
    /// <para>
    /// This attribute can be used to disable tests that are broken or expensive
    /// without commenting them out or removing them from the source code.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class IgnoreAttribute : DecoratorPatternAttribute
    {
        /// <summary>
        /// Indicates that this test is to be ignored and provides a reason.
        /// </summary>
        /// <param name="description">The reason for which the test is to be ignored</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="description"/>
        /// is null</exception>
		public IgnoreAttribute(string description)
			:base(description)
		{}

        /// <summary>
        /// Indicates that this test is to be ignored without providing a reason.
        /// </summary>
		public IgnoreAttribute()
			:base("")
		{}

        /// <summary>
        /// Returns the invoker class to run the test with the given duration parameters.
        /// </summary>
        /// <param name="wrapper">The invoker currently set to run the test.</param>
        /// <returns>A new <see cref="IgnoreRunInvoker"/> object wrapping <paramref name="wrapper"/></returns>
		public override IRunInvoker GetInvoker(MbUnit.Core.Invokers.IRunInvoker wrapper) 
		{
			return new IgnoreRunInvoker(wrapper);			
		}
	}
}
