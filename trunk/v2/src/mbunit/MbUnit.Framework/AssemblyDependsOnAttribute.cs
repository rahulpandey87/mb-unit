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
    /// Use this attribute to identify an assembly whose tests must execute successfully
    /// before the tests in this assembly are executed
    /// </summary>
    /// <example>
    /// The following demonstrates the identification of the parent assembly that must execute
    /// successfully
    /// <code>
    /// [assembly: AssemblyDependsOn("Tests.ParentAssembly")]
    /// ...
    /// public class ChildAssembly.TestClass
    /// {
    ///    ...
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class AssemblyDependsOnAttribute : Attribute {
        private string assemblyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyDependsOnAttribute" /> class. 
        /// </summary>
        /// <param name="assemblyName">Identifies the parent assembly that must execute successfully</param>
        ///     /// <example>
        /// The following demonstrates the identification of the parent assembly that must execute
        /// successfully
        /// <code>
        /// [assembly: AssemblyDependsOn("Tests.ParentAssembly")]
        /// ...
        /// public class ChildAssembly.TestClass
        /// {
        ///    ...
        /// }
        /// </code>
        /// </example>
        public AssemblyDependsOnAttribute(string assemblyName) {
            if (assemblyName == null)
                throw new ArgumentNullException("assemblyName");
            this.assemblyName = assemblyName;
        }

        /// <summary>
        /// The name of the parent assembly
        /// </summary>
        public string AssemblyName {
            get {
                return this.assemblyName;
            }
        }
    }
}
