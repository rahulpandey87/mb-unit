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

// created on 30/01/2004 at 18:31

namespace MbUnit.Framework
{
	using System;
	using MbUnit.Framework;
	using MbUnit.Core.Invokers;
	
	/// <summary>
	/// Tags method that should throw an exception if a predicate is true.
	/// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ConditionalExceptionAttribute : ExpectedExceptionAttribute
    {
		string predicateMethodName;
		
		public ConditionalExceptionAttribute(Type exceptionType, string predicateMethodName)
		:this(exceptionType,predicateMethodName,"")
		{
		}

		public ConditionalExceptionAttribute(Type exceptionType,string predicateMethodName, string description)
		:base(exceptionType,description)
		{
			if (predicateMethodName==null)
				throw new ArgumentNullException("predicateMethodName");
			this.predicateMethodName = predicateMethodName;
		}
		
		public String PredicateMethodName
		{
			get
			{
				return this.predicateMethodName;
			}
		}
		
		
		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new ConditionalExceptionRunInvoker(
                invoker, 
                this.ExceptionType,
                this.Description,
                this.predicateMethodName);
		}
	}
}
