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
using System.Reflection;
using System.Collections;

namespace MbUnit.Core.Invokers 
{	
	using MbUnit.Core.Monitoring;
	using MbUnit.Core.Exceptions;
	using MbUnit.Framework;
	
	///
	public class DurationRunInvoker : DecoratorRunInvoker
	{
		private DurationAttribute attribute;
		private TimeMonitor timer = new TimeMonitor();
		
		public DurationRunInvoker(
			IRunInvoker invoker, 
			DurationAttribute attribute
			)
		:base(invoker)
		{
			if (attribute==null)
				throw new ArgumentNullException("attribute");
			this.attribute = attribute;
		}
		
		public double MinDuration
		{
			get
			{
				return this.attribute.MinDuration;
			}
		}
		
		public double MaxDuration
		{
			get
			{
				return this.attribute.MaxDuration;
			}
		}
		
		public override Object Execute(Object o, IList args)
		{
			timer.Start();
			// run test
			Object result = this.Invoker.Execute(o,args);
			
			timer.Stop();
			// checkl time is in bounds
			if (timer.Duration<=this.MinDuration || timer.Duration>=this.MaxDuration)
			{
				throw new MethodDurationException(
					this.MinDuration,
					this.MaxDuration,
					timer.Duration,
					this,
					o,
					args
					);
			}
			    
			return result;
		}
	}
}
