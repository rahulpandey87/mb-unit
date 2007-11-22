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
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace MbUnit.Core.Monitoring
{
	/// <summary>
	/// A high performance timer
	/// </summary>
	/// <remarks>
	/// High Precision Timer based on Win32 methods.
	/// </remarks>
	/// <example>
	/// This example times the execution of a method:
	/// <code>
	/// TimeMonitor timer = new TimeMonitor();
	/// timer.Start();
	///    ... // execute code
	/// timer.Stop();
	/// 
	/// Console.WriteLine("Duration: {0}",timer.Duration);
    /// </code>
    /// </example>
	public sealed class TimeMonitor : IMonitor
	{
        private long startTicks;
        private long stopTicks;
		private bool running=false;
		
		/// <summary>Default constructor</summary>
		/// <remarks>Initializes the timer.</remarks>
		public TimeMonitor()
		{
            startTicks = 0;
            stopTicks  = 0;
		}
		
		/// <summary>Starts the timer</summary>
		/// <remarks>Resets the duration and starts the timer</remarks>
		public void Start()
		{
			// lets do the waiting threads there work
			Thread.Sleep(0);

            this.startTicks = this.stopTicks = DateTime.Now.Ticks;
            this.running=true;
		}
		
		/// <summary>Stops the timer</summary>
		/// <remarks>Stops the timer</remarks>
		public void Stop()
		{
            if (!running)
                throw new InvalidOperationException("Stop called before start");
            this.stopTicks = DateTime.Now.Ticks;
            this.running=false;
		}
		
		/// <summary>Gets the current duration value without stopping the timer</summary>
		/// <value>Current duration value</value>
		public double Now
		{
			get
			{
                if (this.running)
                {
                    long nowTicks = DateTime.Now.Ticks;
                    return GetDuration(startTicks, nowTicks);
                }
				else
					return this.Duration;
			}		
		}

		/// <summary>Gets the timed duration value in seconds</summary>
		/// <value>Timer duration</value>
		public double Duration
		{
			get
			{
                return GetDuration(startTicks, stopTicks);
            }
        }

        private static double GetDuration(long startTicks, long endTicks)
        {
            long spanTicks = endTicks - startTicks;
            return spanTicks * 1e-7;
        }
    }
}
