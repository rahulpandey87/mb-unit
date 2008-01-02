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
using System.IO;

namespace MbUnit.Core.Monitoring
{
    public sealed class ConsoleMonitor : IMonitor
    {
        private StringWriter consoleOut = new StringWriter();
        private StringWriter consoleError = new StringWriter();
        private TextWriter originalConsoleOut = null;
        private TextWriter originalConsoleError = null;

        public ConsoleMonitor()
        { }

        public string Out
        {
            get
            {
                return this.consoleOut.ToString();
            }
        }
        public string Error
        {
            get
            {
                return this.consoleError.ToString();
            }
        }

        public void Start()
        {
            // saving streams
            this.originalConsoleError = Console.Error;
            this.originalConsoleOut = Console.Out;

            // deviate outputs
            this.consoleOut = new StringWriter();
            this.consoleError = new StringWriter();
            Console.SetOut(this.consoleOut);
            Console.SetError(this.consoleError);
        }

        public void Stop()
        {
            Console.Out.Flush();
            Console.Error.Flush();
            this.consoleOut.Flush();
            this.consoleError.Flush();
            Console.SetError(this.originalConsoleError);
            Console.SetOut(this.originalConsoleOut);
        }
    }
}
