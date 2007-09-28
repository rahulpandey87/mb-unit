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
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux

using System;
namespace MbUnit.Framework
{
   /// <summary>
   /// Can be used to define guard methods that will act at the assembly level. 
   /// Typically, highly time consuming resources can be initialized and cleaned up using those methods.
   /// <list type="bullet">
   /// <listheader><term>Rationale</term></listheader>
   /// <item>The user provides a class containing static methods tagged with <see cref="SetUpAttribute" /> and <see cref="TearDownAttribute" /> (both are optional).</item>
   /// <item>A assembly-level attribute, <see cref="AssemblyCleanUpAttribute" />, is used to specify which class contains the method.</item>
   /// <item>If the setup method fails, no tests are executed.</item>
   /// <item>If the teardown method fails, all tests are failed.</item>
   /// </list>
   /// </summary>
   /// <example>
   /// <code>
   /// // in AssemblyInfo.cs 
   /// [assembly: MbUnit.Framework.AssemblyCleanup(typeof(MbUnit.Demo.AssemblyCleaner))]
   ///
   /// // in AssemblyCleaner.cs
   /// using System;
   /// using MbUnit.Framework;
   /// 
   /// namespace MbUnit.Demo
   /// {
   ///     public static class AssemblyCleaner
   ///     {
   ///         [SetUp]
   ///         public static void SetUp()
   ///         {
   ///             Console.WriteLine("Setting up {0}", typeof(AssemblyCleaner).Assembly.FullName);
   ///             Console.WriteLine(DateTime.Now.ToLongTimeString());
   ///         }
   ///         [TearDown]
   ///         public static void TearDown()
   ///         {
   ///             Console.WriteLine("Cleaning up {0}", typeof(AssemblyCleaner).Assembly.FullName);
   ///             Console.WriteLine(DateTime.Now.ToLongTimeString());
   ///         }
   ///     }
   /// }
   /// </code>
   /// </example>

    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple =false,Inherited =false)]
    public sealed class AssemblyCleanupAttribute : Attribute
    {
        private Type targetType;

        public AssemblyCleanupAttribute(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType");
            this.targetType = targetType;
        }

        public Type TargetType
        {
            get
            {
                return this.targetType;
            }
        }
    }
}
