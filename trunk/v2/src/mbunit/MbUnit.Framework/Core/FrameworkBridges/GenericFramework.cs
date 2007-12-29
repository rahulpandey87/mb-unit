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

using System.Reflection;
using System;

namespace MbUnit.Core.FrameworkBridges
{
	internal sealed class GenericFramework : IFramework
	{
        private Assembly _frameworkAssembly;
        private string _namespace;
       
        public GenericFramework(Assembly frameworkAssembly, string ns)
        {
            if (frameworkAssembly == null)
                throw new ArgumentNullException("frameworkAssembly");
            if (ns == null)
                throw new ArgumentNullException("ns");

            _frameworkAssembly = frameworkAssembly;
			_namespace = ns;
		}

		#region IFramework Members

		public Assembly FrameworkAssembly
		{
			get { return _frameworkAssembly; }
		}

		public Type TestFixtureTearDownAttributeType
		{
			get { return GetFrameworkType(_namespace + ".TestFixtureTearDownAttribute", false); }
		}

		public Type TestFixtureSetUpAttributeType
		{
			get { return GetFrameworkType(_namespace + ".TestFixtureSetUpAttribute", false); }
		}

		public Type ExpectedExceptionAttributeType
		{
			get { return GetFrameworkType(_namespace + ".ExpectedExceptionAttribute", false); }
		}

		public Type AssertionExceptionType
		{
			get { return GetFrameworkType(_namespace + ".AssertionException", false); }
		}

		public Type SetUpAttributeType
		{
			get { return GetFrameworkType(_namespace + ".SetUpAttribute", false); }
		}

		public Type TearDownAttributeType
		{
			get { return GetFrameworkType(_namespace + ".TearDownAttribute", false); }
		}

		public Type TestFixtureAttributeType
		{
			get { return GetFrameworkType(_namespace + ".TestFixtureAttribute", false); }
		}

		public Type IgnoreAttributeType
		{
			get { return GetFrameworkType(_namespace + ".IgnoreAttribute", false); }
		}

		public Type TestAttributeType
		{
			get { return GetFrameworkType(_namespace + ".TestAttribute", false); }
		}

		public Type CategoryAttributeType
		{
			get { return GetFrameworkType(_namespace + ".CategoryAttribute", false); }
		}

		public string GetCategoryAttributeName(object attribute)
		{
			return (string)findPropertyValue(attribute, "Name");
		}

		public string GetIgnoreAttributeReason(object attribute)
		{
			return (string)findPropertyValue(attribute, "Reason");
		}

		public string GetTestFixtureAttributeDescription(object attribute)
		{
			return (string)findPropertyValue(attribute, "Description");
		}

		public string GetTestAttributeDescription(object attribute)
		{
			return (string)findPropertyValue(attribute, "Description");
		}

		public Type GetExpectedExceptionAttributeExceptionType(object attribute)
		{
			return (Type)findPropertyValue(attribute, "ExceptionType");
		}

		public string GetExpectedExceptionAttributeExpectedMessage(object attribute)
		{
			return (string)findPropertyValue(attribute, "ExpectedMessage");
		}

		private object findPropertyValue(object target, string propertyName)
		{
			if(target == null) { return null; }
			Type type = target.GetType();
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			PropertyInfo property = type.GetProperty(propertyName, bindingFlags);
			if(property == null) { return null; }
			return property.GetValue(target, null);
		}

		public Type GetFrameworkType(string typeName)
		{
			return _frameworkAssembly.GetType(typeName, true);
		}

		public Type GetFrameworkType(string typeName, bool throwOnError)
		{
			return _frameworkAssembly.GetType(typeName, throwOnError);
		}

		#endregion
	}
}
