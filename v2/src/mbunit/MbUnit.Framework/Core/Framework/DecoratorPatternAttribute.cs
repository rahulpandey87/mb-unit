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


namespace MbUnit.Core.Framework
{
	using System;
	using MbUnit.Core.Invokers;
	using System.Reflection;
	
	
	/// <summary>
	/// This is the base class for attributes that can decorate tests.
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='DecoratorPatternAttribute']"/>	
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public abstract class DecoratorPatternAttribute : PatternAttribute
	{
		protected DecoratorPatternAttribute()
		{}

		protected DecoratorPatternAttribute(string description)
			:base(description)
		{}
				
		public static IRunInvoker DecoreInvoker(MethodInfo mi, IRunInvoker invoker)
		{
			// looking for decoring attributes
			foreach(DecoratorPatternAttribute deco in 
				mi.GetCustomAttributes(typeof(DecoratorPatternAttribute),true))
			{
				invoker = deco.GetInvoker(invoker);				
			}			
			return invoker;
		}
		
		public abstract IRunInvoker GetInvoker(IRunInvoker wrapper);
	}
}
