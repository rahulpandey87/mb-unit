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
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;

namespace MbUnit.Framework 
{
    /// <summary>
	/// Tag method that should return in a given time interval.
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='DurationAttribute']"/>	
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
	public class DurationAttribute : DecoratorPatternAttribute 
	{
		private double minDuration = -1;
		private double maxDuration = 0;
		
		
		public DurationAttribute(
			double maxDuration
			)
		{
			if (maxDuration<0)
				throw new ArgumentException("maxDuration must be positive");
			this.maxDuration = maxDuration;
		}

		public DurationAttribute(
			double maxDuration,
			string description
			)
			:base(description)
		{
			if (maxDuration<0)
				throw new ArgumentException("maxDuration must be positive");
			this.maxDuration = maxDuration;
		}
		
		public DurationAttribute(
			double minDuration,
			double maxDuration
			)
		{
			if (maxDuration<0)
				throw new ArgumentException("maxDuration must be positive");
			if (minDuration>maxDuration)
				throw new ArgumentException("minDuration must be smaller that maxDuration");
			
			this.minDuration = minDuration;	
			this.maxDuration = maxDuration;
		}
		
		public DurationAttribute(
			double minDuration,
			double maxDuration,
			string description
			)
			:base(description)
		{
			if (maxDuration<0)
				throw new ArgumentException("maxDuration must be positive");
			if (minDuration>maxDuration)
				throw new ArgumentException("minDuration must be smaller that maxDuration");
			
			this.minDuration = minDuration;	
			this.maxDuration = maxDuration;
		}
		
		public double MinDuration
		{
			get
			{
				return this.minDuration;
			}
		}
		
		public double MaxDuration
		{
			get
			{
				return this.maxDuration;
			}
		}
		
		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new DurationRunInvoker(invoker, this);
		}

	}
}
