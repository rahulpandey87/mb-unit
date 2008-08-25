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
using MbUnit.Core.Invokers;

namespace MbUnit.Framework {
    /// <summary>
    /// Tags test fixture classes and individual test methods to not be run unless explicitly selected, either using the GUI or command line arguments.
    /// </summary>
    /// <example>
    /// <code>
    ///	[Test, Explicit]
    /// public void ExplicitTest()
    /// {...} 
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ExplicitAttribute : DecoratorPatternAttribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExplicitAttribute"/> class.
        /// </summary>
        public ExplicitAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExplicitAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description why the fixture or test has been marked explicit</param>
        public ExplicitAttribute(string description)
            : base(description) { }

        /// <summary>
        /// Returns the invoker class to run the test or test fixture onlyif explicitly requested.
        /// </summary>
        /// <param name="invoker">The invoker currently set to run the test.</param>
        /// <returns>A new <see cref="ExplicitRunInvoker"/> object wrapping <paramref name="invoker"/></returns>
        public override IRunInvoker GetInvoker(IRunInvoker invoker) {
            return new ExplicitRunInvoker(invoker);
        }
    }
}
