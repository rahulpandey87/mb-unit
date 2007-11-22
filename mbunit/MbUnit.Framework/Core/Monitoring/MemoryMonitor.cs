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
using System.Diagnostics;

namespace MbUnit.Core.Monitoring
{
	/// <summary>
	/// Summary description for MemoryTracker.
	/// </summary>
	public sealed class MemoryMonitor : IMonitor
	{
		private MemoryStatus startStatus = null;
		private MemoryStatus endStatus = null;

		public MemoryMonitor()
		{}

		public MemoryStatus StartStatus
		{
			get
			{
				return this.startStatus;
			}
		}

		public MemoryStatus EndStatus
		{
			get
			{
				return this.endStatus;
			}
		}

		public long Usage
		{
			get
			{
                if (this.EndStatus == null)
                    throw new InvalidOperationException("Monitor not start/stopped");
                return EndStatus.WorkingSet - StartStatus.WorkingSet;
            }
		}

		public void Start()
		{
			this.startStatus = new MemoryStatus();
			this.endStatus = null;
		}

		public void Stop()
		{
            if (this.startStatus == null)
                throw new InvalidOperationException("Trying to call Stop before Start");
            this.endStatus = new MemoryStatus();
		}
		
		public long Now
		{
			get
			{
                if (this.StartStatus == null)
                    throw new InvalidOperationException("Monitor not started");
                if (this.EndStatus != null)
                    throw new InvalidOperationException("Monitor is stopped, use Usage");
                MemoryStatus now = new MemoryStatus();
                return now.WorkingSet - StartStatus.WorkingSet;
            }
		}
	}
}
