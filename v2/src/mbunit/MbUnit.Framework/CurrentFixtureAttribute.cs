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
    /// <summary>
    /// Used in conjunction, e.g. with an <see cref="MbUnit.Core.AutoRunner"/> to identify only the tests that it should run
    /// </summary>
    /// <remarks>
    /// This attribute has no specific purpose other than to identify specific tests
    /// </remarks>
    /// <example>
    /// <para>In this example, the autorunner will execute only tests tagged with the CurrentFixtureAttribute</para>
    /// <code>
    ///    namespace MbUnit.Tests {
    ///        using MbUnit.Core;
    ///        using MbUnit.Core.Filters;
    ///
    ///        static class CurrentOnly {
    ///            public static void Main(string[] args) {
    ///                using (AutoRunner auto = new AutoRunner()) {
    ///
    ///                    auto.Domain.Filter = FixtureFilters.Current;  
    ///                    auto.Run(); 
    ///                    auto.ReportToHtml();
    ///                }
    ///            }
    ///        }
    ///    }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class CurrentFixtureAttribute : Attribute {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentFixtureAttribute"/> class.
        /// </summary>
        public CurrentFixtureAttribute() { }
    }
}
