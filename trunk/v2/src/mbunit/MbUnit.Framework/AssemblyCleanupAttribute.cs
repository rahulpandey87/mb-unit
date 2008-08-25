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
namespace MbUnit.Framework
{
    /// <summary>
    /// Use this attribute to identify the class containing setup and teardown methods for an assembly of tests.
    /// The setup method will be run before any other tests or setup methods in the assembly and the teardown method
    /// will be run after any other teardown methods in the assembly.
    /// </summary>
    /// <remarks>
    /// There are some rules associated with this attribute
    /// <list type="bullet">
    /// <item>Only one class with AssemblyCleanUp per assembly is authorized</item>
    /// <item>Both methods are optional</item>
    /// <item>If SetUp fails, no tests are run and they are all marked as failure</item>
    /// <item>TearDown is always executed</item>
    /// <item>If TearDown fails, all the tests are marked as failure</item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following demonstrates the identification of the Cleaner class containing setup and teardown methods
    /// for the assembly that contains the class
    /// <code>
    /// [assembly: AssemblyCleanUp(typeof(Cleaner))]
    /// ...
    /// public class Cleaner
    /// {
    ///     [SetUp]
    ///     public static void SetUp()
     ///    {
    ///         Console.WriteLine("Setting up {0}", typeof(AssemblyCleanUp).Assembly.FullName);
    ///     }
    /// 
    ///     [TearDown]
    ///     public static void TearDown()
    ///     {
    ///         Console.WriteLine("Cleaning up {0}", typeof(AssemblyCleanUp).Assembly.FullName);
    ///     }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple =false,Inherited =false)]
    public sealed class AssemblyCleanupAttribute : Attribute
    {
        private Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyCleanupAttribute" /> class. />
        /// </summary>
        /// <param name="targetType">Identifies the class containing the setup and teardown method for the assembly</param>
        /// <remarks>
        /// There are some rules associated with this attribute
        /// <list type="bullet">
        /// <item>Only one class with AssemblyCleanUp per assembly is authorized</item>
        /// <item>Both methods are optional</item>
        /// <item>If SetUp fails, no tests are run and they are all marked as failure</item>
        /// <item>TearDown is always executed</item>
        /// <item>If TearDown fails, all the tests are marked as failure</item>
        /// </list>
        /// </remarks>
        /// <example>
        /// The following demonstrates the identification of the Cleaner class containing setup and teardown methods
        /// for the assembly that contains the class
        /// <code>
        /// [assembly: AssemblyCleanUp(typeof(Cleaner))]
        /// ...
        /// public class Cleaner
        /// {
        ///     [SetUp]
        ///     public static void SetUp()
        ///    {
        ///         Console.WriteLine("Setting up {0}", typeof(AssemblyCleanUp).Assembly.FullName);
        ///     }
        /// 
        ///     [TearDown]
        ///     public static void TearDown()
        ///     {
        ///         Console.WriteLine("Cleaning up {0}", typeof(AssemblyCleanUp).Assembly.FullName);
        ///     }
        /// }
        /// </code>
        /// </example>
        public AssemblyCleanupAttribute(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType");
            this.targetType = targetType;
        }

        /// <summary>
        /// Identifies the class containing the setup and teardown methods for the test assembly
        /// </summary>
        public Type TargetType
        {
            get
            {
                return this.targetType;
            }
        }
    }
}
