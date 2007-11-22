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

namespace MbUnit.Core.FrameworkBridges
{
	using System;
	using System.Reflection;
	using System.Collections;

	internal sealed class FrameworkFactory
	{
		private static IDictionary __frameworkByAssembly = new Hashtable();
        private static IList __frameworkFactories;

        static FrameworkFactory()
		{
			__frameworkFactories = new ArrayList();
			__frameworkFactories.Add(new GenericFrameworkFactory("nunit.framework", "NUnit.Framework"));
			__frameworkFactories.Add(new GenericFrameworkFactory("csUnit", "csUnit"));
		}

		public static IList FrameworkFactories
		{
			get { return __frameworkFactories; }
		}

		public static IFramework FromFixture(object fixture)
		{
			return FromType(fixture.GetType());
		}

		public static IFramework FromType(Type type)
		{
			return FromAssembly(type.Assembly);
		}

		public static IFramework FromMethod(MethodBase method)
		{
			return FromType(method.DeclaringType);
		}

		public static IFramework FromAssembly(Assembly assembly)
		{
			IFramework framework = (IFramework)__frameworkByAssembly[assembly];
			if(framework == null)
			{
				foreach(IFrameworkFactory frameworkFactory in __frameworkFactories)
				{
					framework = frameworkFactory.FromAssembly(assembly);
					if(framework != null)
					{
						__frameworkByAssembly[assembly] = framework;
						break;
					}
				}
			}
			return framework;
		}
	}
}

