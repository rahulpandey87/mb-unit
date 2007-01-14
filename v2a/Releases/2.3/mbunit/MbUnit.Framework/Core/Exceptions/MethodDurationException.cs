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


namespace MbUnit.Core.Exceptions 
{
	using MbUnit.Core.Invokers;
	using System;
	using System.Runtime.Serialization;
	using System.Collections;
		
	[Serializable]
	public class MethodDurationException : ExecuteException 
	{
		private double minDuration;
		private double maxDuration;
		private double actualDuration;
		
		public MethodDurationException(
		    double minDuration,
		    double maxDuration,
		    double actualDuration,
		    IRunInvoker invoker, 
		    Object tested,
		    IList args
		    )
		   :base(invoker,tested,args)
		{
			this.minDuration = minDuration;
			this.maxDuration = maxDuration;
			this.actualDuration = actualDuration;
		}
		
		public MethodDurationException(
		    double minDuration,
		    double maxDuration,
		    double actualDuration,
		    IRunInvoker invoker, 
		    Object tested,
		    IList args,
		    string message
		    )
		:base(invoker,tested,args,message)
		{
			this.minDuration = minDuration;
			this.maxDuration = maxDuration;
			this.actualDuration = actualDuration;
		}
		
		protected MethodDurationException(
		    double minDuration,
		    double maxDuration,
		    double actualDuration,
			IRunInvoker invoker, 
		    Object tested,
		    IList args,
			SerializationInfo info, StreamingContext context)
		:base(invoker,tested,args,info,context)
		{
			this.minDuration = minDuration;
			this.maxDuration = maxDuration;
			this.actualDuration = actualDuration;
		}
		
		public MethodDurationException(
		    double minDuration,
		    double maxDuration,
		    double actualDuration,
			IRunInvoker invoker, 
		    Object tested,
		    IList args,
		    string message, 
		    Exception innerException
		    )
		:base(invoker,tested,args,message,innerException)
		{
			this.minDuration = minDuration;
			this.maxDuration = maxDuration;
			this.actualDuration = actualDuration;
		}
		
		public override string Message
		{
			get
			{
				return String.Format("Method duration too long. Expected [{0},{1]], got {2}",
				                     this.minDuration,
				                     this.maxDuration,
				                     this.actualDuration);
			}
		}
	}
}
