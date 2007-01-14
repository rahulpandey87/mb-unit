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


namespace MbUnit.Core.Invokers
{
	using System;
	using System.Reflection;
	using MbUnit.Core;
	using System.Collections;
	using MbUnit.Core.Runs;
	
	
	/// <summary>
	/// An invoker that wraps up the call to a fixture method.
	/// </summary>
	public class MethodRunInvoker : RunInvoker
	{
		private MethodInfo method;
		
		/// <summary>
		/// Default constructor - initializes all fields to default values
		/// </summary>
		public MethodRunInvoker(IRun generator, MethodInfo method)
			:base(generator)
		{
			if (method==null)
				throw new ArgumentNullException("method");
			
			this.method = method;
		}
		
		public MethodInfo Method
		{
			get
			{
				return this.method;
			}
		}
		
		public override String Name
		{
			get
			{
				return this.Method.Name;
			}
		}
		
		public override  Object Execute(object o, IList args)
		{
			return InvokeMethod(o,args);
		}
		
		protected Object InvokeMethod(object o, IList args)
		{
			if (this.method==null)
				return null;
			
			return TypeHelper.Invoke(method,o,args);
		}

        public override bool ContainsMemberInfo(MemberInfo memberInfo)
        {
            return memberInfo == this.method;
        }
    }
}
