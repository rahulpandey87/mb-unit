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
	using System.Collections;
	using System;
	using System.Reflection;
	using System.Diagnostics;
	using MbUnit.Core.Runs;
	
	public class CustomRunInvoker : IRunInvoker
	{
		private IRun generator;
		private Object tester;
		private MethodInfo mi;
		private bool feedSender;
		
		/// <summary>
		/// Default constructor - initializes all fields to default values
		/// </summary>
		public CustomRunInvoker(
		                        IRun generator,
		                        Object tester,
		                        MethodInfo mi,
								bool feedSender
		                        )
		{
			if (generator == null)
				throw new ArgumentNullException("generator");
			if (tester==null)
				throw new ArgumentNullException("tester");
			if (mi==null)
				throw new ArgumentNullException("mi");
			
			this.generator = generator;
			this.tester = tester;
			this.mi = mi;
			this.feedSender = feedSender;
		}
		
		public IRun Generator
		{
			get
			{
				return this.generator;
			}
		}
		
		public String Name
		{
			get
			{
				return this.mi.Name;
			}
		}

		public Object Execute(Object o, IList args)
		{
			if (this.feedSender)
			{
				Object[] oargs = new Object[args.Count+1];
				oargs[0]=o;
				args.CopyTo(oargs,1);
			
				return this.mi.Invoke(this.tester,oargs);		
			}
			else
			{
				Object[] oargs = new Object[args.Count];
				args.CopyTo(oargs,0);
				return this.mi.Invoke(this.tester,oargs);
			}
		}

        public virtual bool ContainsMemberInfo(MemberInfo memberInfo)
        {
            return mi == memberInfo;
        }
    }
}
