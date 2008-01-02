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


namespace MbUnit.Core.Exceptions 
{
	using MbUnit.Core.Invokers;
	using System;
	using System.Runtime.Serialization;
	using System.IO;
	using System.Collections;
	
	[Serializable]
	public class InvokerException : System.Exception 
	{
		private Object tested = null;
		private IList args = null; 
		private IRunInvoker invoker = null;
		
		public InvokerException(
		    IRunInvoker invoker, 
		    Object tested,
		    IList args
		    )
		{
			this.invoker = invoker;
			this.tested = tested;
			this.args = args;
		}
		
		public InvokerException(
		    IRunInvoker invoker, 
		    Object tested,
		    IList args,
		    string message
		    )
		:base(message)
		{
			this.invoker = invoker;
			this.tested = tested;
			this.args = args;
		}
		
		protected InvokerException(
			IRunInvoker invoker, 
		    Object tested,
		    IList args,
			SerializationInfo info, StreamingContext context)
		:base(info,context)
		{
			this.invoker = invoker;
			this.tested = tested;
			this.args = args;
		}
		
		public InvokerException(
			IRunInvoker invoker, 
		    Object tested,
		    IList args,
		    string message, 
		    Exception innerException
		    )
		:base(message,innerException)
		{
			this.invoker = invoker;
			this.tested = tested;
			this.args = args;
		}
		
		public Object Tested
		{
			get
			{
				return this.tested;
			}
		}
		
		public IList Args
		{
			get
			{
				return this.args;
			}			
		}
		
		public IRunInvoker Invoker
		{
			get
			{
				return this.invoker;
			}
		}
		
		public override string Message
		{
			get
			{
				StringWriter sw =new StringWriter();
				
				sw.WriteLine("Exception raised while invoquing test.");
				sw.WriteLine("\tInvoker: {0}",this.Invoker);
				if (this.tested!=null)
				{
					sw.WriteLine("\tTested Type: {0}", Tested.GetType().Name);
					sw.WriteLine(Tested.ToString());
				}
				sw.WriteLine("\tArguments:");
				foreach(Object o in this.Args)
				{
					sw.WriteLine("\t\t{0} {1}",o.GetType().Name,o.ToString());
				}
				return sw.ToString();	
			}
		}
	}
}
