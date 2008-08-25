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
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.IO;

using MbUnit.Core.Exceptions;

namespace MbUnit.Framework
{
	/// <summary>
    /// Class containing generic assert methods for the verification of <see cref="Type"/> information by reflection.
	/// </summary>
	public sealed class ReflectionAssert
	{
		private ReflectionAssert(){}

        /// <summary>
        /// Asserts whether an instance of the <paramref name="parent"/>
        /// can be assigned from an instance of <paramref name="child"/>.
        /// </summary>
        /// <param name="parent">Parent <see cref="Type"/> instance.</param>
        /// <param name="child">Child <see cref="Type"/> instance.</param>
		public static void IsAssignableFrom(Type parent, Type child)
		{
			Assert.IsNotNull(parent);
			Assert.IsNotNull(child);
			Assert.IsTrue(parent.IsAssignableFrom(child),
			              "{0} is not assignable from {1}",
			              parent,
			              child
			              );
		}

        /// <summary>
        /// Asserts whether <paramref name="child"/> is an instance of the
        /// <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that <paramref name="child"/> should be an instance of.</param>
        /// <param name="child">The <see cref="Object"/> to test</param>
		public static void IsInstanceOf(Type type, Object child)
		{
			Assert.IsNotNull(type);
			Assert.IsNotNull(child);
			Assert.IsTrue(type.IsInstanceOfType(child),
			              "{0} is not an instance of {1}",
			              type,
			              child
			              );
		}

        /// <summary>
        /// Asserts that <paramref name="type"/> has a default public constructor
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to test.</param>
		public static void HasDefaultConstructor(Type type)
		{
			HasConstructor(type, Type.EmptyTypes);
		}

        /// <summary>
        /// Asserts that <paramref name="type"/> has a public instance constructor with a signature defined by <paramref name="parameters"/>.
        /// </summary>
        /// <param name="type">The type to test</param>
        /// <param name="parameters">The list of parameters for the constructor</param>
		public static void HasConstructor(Type type, params Type[] parameters)
		{
			HasConstructor(type,BindingFlags.Public | BindingFlags.Instance,parameters);
		}

        /// <summary>
        /// Asserts that the <paramref name="type"/> has a constructor, with the specified binding <paramref name="flags"/>, 
        /// with a signature defined by <paramref name="parameters"/>.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="flags">The <see cref="BindingFlags"/> the type should have.</param>
        /// <param name="parameters">The list of parameters for the constructor.</param>
		public static void HasConstructor(Type type, BindingFlags flags, params Type[] parameters)
		{
			Assert.IsNotNull(type);
			Assert.IsNotNull(type.GetConstructor(flags,null,parameters,null),
			                 "{0} does not have matching constructor",
			                 type.FullName
			                 );
		}

        /// <summary>
        /// Asserts that the <paramref name="type"/> has a public instance method with the given <paramref name="name"/> 
        /// and signature defined by <paramref name="parameters"/>.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="name">The name of the method to look for.</param>
        /// <param name="parameters">The parameters for the method.</param>
		public static void HasMethod(Type type, string name, params Type[] parameters)
		{
			HasMethod(type,BindingFlags.Public | BindingFlags.Instance,name,parameters); 
		}


        /// <summary>
        /// Asserts that the <paramref name="type"/> has a method with the given <paramref name="name"/>,
        /// <paramref name="flags"/> and signature defined by <paramref name="parameters"/>.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="flags">The <see cref="BindingFlags"/> the method should have.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="parameters">The parameters for the method.</param>
		public static void HasMethod(Type type, BindingFlags flags, string name, params Type[] parameters)
		{
			Assert.IsNotNull(type, "Type is null");
			StringAssert.IsNonEmpty(name);
			
			Assert.IsNotNull(type.GetMethod(name,parameters),
			                 "Method {0} of type {1} not found with matching arguments",
			                 name,
			                 type
			                 );
		}

        /// <summary>
        /// Asserts that the <paramref name="type"/> has a public field with the given <paramref name="name"/>.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="name">The name of the field to find.</param>
		public static void HasField(Type type, string name)
		{
			HasField(type, BindingFlags.Public | BindingFlags.Instance,name );
		}

        /// <summary>
        /// Asserts that the <paramref name="type"/> has a field with the given 
        /// <paramref name="flags"/> and <paramref name="name"/>.
        /// </summary>
        /// <param name="type">The type to test.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <param name="flags">The <see cref="BindingFlags"/> for the field.</param>
		public static void HasField(Type type, BindingFlags flags,string name)
		{
			Assert.IsNotNull(type, "Type is null");
			StringAssert.IsNonEmpty(name);
			
			Assert.IsNotNull(type.GetField(name),
			                 "Field {0} of type {1} not found with binding flags {2}",
			                 name,
			                 type,
			                 flags
			                 );
		}

        /// <summary>
        /// Verifies that <see cref="Type"/> <paramref name="t"/> has a read-only property called <paramref name="propertyName"/>
        /// </summary>
        /// <param name="t">The type to test.</param>
        /// <param name="propertyName">Name of the read-only property to find</param>
		public static void ReadOnlyProperty(Type t, string propertyName)
		{
			Assert.IsNotNull(t);
			Assert.IsNotNull(propertyName);
			PropertyInfo pi = t.GetProperty(propertyName);
			Assert.IsNotNull(pi,
				"Type {0} does not contain property {1}",
				t.FullName,
				propertyName);
			ReadOnlyProperty(pi);
		}

        /// <summary>
        /// Verifies the property described by <paramref name="pi"/> is read-only
        /// </summary>
        /// <param name="pi">A <see cref="PropertyInfo"/> object describing a property</param>
		public static void ReadOnlyProperty(PropertyInfo pi)
		{
			Assert.IsNotNull(pi);
			Assert.IsFalse(pi.CanWrite,
				"Property {0}.{1} is not read-only",
				pi.DeclaringType.Name,
				pi.Name
				);
		}

        /// <summary>
        /// Verifies that <see cref="Type"/> <paramref name="t"/> has a write-only property called <paramref name="propertyName"/>
        /// </summary>
        /// <param name="t">The type to test.</param>
        /// <param name="propertyName">Name of the write-only property to find</param>
		public static void WriteOnlyProperty(Type t, string propertyName)
		{
			Assert.IsNotNull(t);
			Assert.IsNotNull(propertyName);
			PropertyInfo pi = t.GetProperty(propertyName);
			Assert.IsNotNull(pi,
				"Type {0} does not contain property {1}",
				t.FullName,
				propertyName);
			WriteOnlyProperty(pi);
		}

        /// <summary>
        /// Verifies the property described by <paramref name="pi"/> is write-only
        /// </summary>
        /// <param name="pi">A <see cref="PropertyInfo"/> object describing a property</param>
		public static void WriteOnlyProperty(PropertyInfo pi)
		{
			Assert.IsNotNull(pi);
			Assert.IsFalse(pi.CanRead,
				"Property {0}.{1} is not read-only",
				pi.DeclaringType.FullName,
				pi.Name
				);
		}


        /// <summary>
        /// Verifies whether the specified <see cref="Type"/> <paramref name="t"/> is sealed.
        /// </summary>
        /// <param name="t">The type to test</param>
		public static void IsSealed(Type t)
		{
			Assert.IsNotNull(t);
			Assert.IsTrue(t.IsSealed,
				"Type {0} is not sealed",
				t.FullName);
		}


        /// <summary>
        /// Verifies whether the specified <see cref="Type"/> <paramref name="t"/> has only private constructors
        /// and is thus not creatable
        /// </summary>
        /// <param name="t">The type to test</param>
		public static void NotCreatable(Type t)
		{
			Assert.IsNotNull(t);
			foreach(ConstructorInfo ci in t.GetConstructors())
			{
				Assert.Fail(
					"Non-private constructor found in class {0}  that must not be creatable",
					t.FullName);
			}
		}
	}
}
