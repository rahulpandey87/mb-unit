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
	/// Use this attribute to specify that the tagged method should return in a given number of seconds or fail otherwise.
	/// </summary>
	/// <remarks>The duration attribute allows you to specify both minimum and maximum run times for a test</remarks>
	/// <example>
	/// The following examples demonstrate the Duration attribute
	/// <code>
	/// [Test]
	/// [Duration(10)]
	/// public void SetTestMaxDurationToTenSeconds()
	/// { ... }
	/// 
	/// [Test]
	/// [Duration(5.5, 15)]
	/// public void SetTestMinAndMaxDuration()
	/// { ... }
	/// </code>
	/// </example>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
	public class DurationAttribute : DecoratorPatternAttribute 
	{
		private double minDuration = -1;
		private double maxDuration = 0;


        /// <summary>
        /// Initializes a new instance of the <see cref="DurationAttribute"/> class.
        /// </summary>
        /// <param name="maxDuration">The maximum duration of the tagged test in seconds</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="maxDuration"/> is not 0 or greater</exception>
		public DurationAttribute(
			double maxDuration
			)
		{
			if (maxDuration<0)
				throw new ArgumentException("maxDuration must be positive");
			this.maxDuration = maxDuration;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="DurationAttribute"/> class.
        /// </summary>
        /// <param name="maxDuration">The maximum duration of the tagged test in seconds</param>
        /// <param name="description">A brief description of why the duration was set for your reference</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="maxDuration"/> is not 0 or greater</exception>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DurationAttribute"/> class.
        /// </summary>
        /// <param name="minDuration">The minimum duration of the tagged test in seconds</param>
        /// <param name="maxDuration">The maximum duration of the tagged test in seconds</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="maxDuration"/> is not 0 or greater</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="maxDuration"/> is not greater than <paramref name="minDuration"/></exception>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DurationAttribute"/> class.
        /// </summary>
        /// <param name="minDuration">The minimum duration of the tagged test in seconds</param>
        /// <param name="maxDuration">The maximum duration of the tagged test in seconds</param>
        /// <param name="description">A brief description of why the duration was set for your reference</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="maxDuration"/> is not 0 or greater</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="maxDuration"/> is not greater than <paramref name="minDuration"/></exception>
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

        /// <summary>
        /// Gets the minimum duration set by the tag.
        /// </summary>
        /// <value>Minimum test duration in seconds.</value>
		public double MinDuration
		{
			get
			{
				return this.minDuration;
			}
		}

        /// <summary>
        /// Gets the maximum duration set by the tag.
        /// </summary>
        /// <value>Maximum test duration in seconds.</value>
		public double MaxDuration
		{
			get
			{
				return this.maxDuration;
			}
		}

        /// <summary>
        /// Returns the invoker class to run the test with the given duration parameters.
        /// </summary>
        /// <param name="invoker">The invoker currently set to run the test.</param>
        /// <returns>A new <see cref="DurationRunInvoker"/> object wrapping <paramref name="invoker"/></returns>
		public override IRunInvoker GetInvoker(IRunInvoker invoker)
		{
			return new DurationRunInvoker(invoker, this);
		}

	}
}
