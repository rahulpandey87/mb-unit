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

namespace MbUnit.Core.Monitoring
{
    public sealed class ReportMonitor : IMonitor
    {
        private ConsoleMonitor consoler = new ConsoleMonitor();
        private TimeMonitor timer = new TimeMonitor();
        private MemoryMonitor memorizer = new MemoryMonitor();
        private CurrentDirectoryMonitor currentDirectorizer = new CurrentDirectoryMonitor();
        private DebugMonitor debugMonitor = new DebugMonitor();

        public void Start()
        {
            this.currentDirectorizer.Start();
            this.memorizer.Start();
            this.timer.Start();
            this.consoler.Start();
            this.debugMonitor.Start();
        }

        public void Stop()
        {
            this.currentDirectorizer.Stop();
            this.memorizer.Stop();
            this.timer.Stop();
            this.consoler.Stop();
            this.debugMonitor.Stop();
        }

        public TimeMonitor Timer
        {
            get
            {
                return this.timer;
            }
        }

        public MemoryMonitor Memorizer
        {
            get
            {
                return this.memorizer;
            }
        }

        public ConsoleMonitor Consoler
        {
            get
            {
                return this.consoler;
            }
        }
    }
}
