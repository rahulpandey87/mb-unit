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
using MbUnit.Core;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Core.Cons
{
	public sealed class ConsoleRunPipeListener : IRunPipeListener
	{
		private TextWriter writer = System.Console.Out;

		public TextWriter Writer
		{
			get
			{
				return this.writer;
			}
			set
			{
				this.writer=value;
			}
		}

		public void Start(RunPipe pipe)
		{}

        // MLS 12/21/05 - changing some of the messages below to give details more like the AutoRunner,
        //      although it still doesn't show all the details that the AutoRunner does.
        // TOOD: Refactor so that this is exactly like the AutoRunner, and remove duplicate code.
        
        public void Success(RunPipe pipe, ReportRun result)
        {
            Writer.WriteLine("[success] {0}", pipe.Name);
        }

        public void Ignore(RunPipe pipe, ReportRun result)
        {
			Writer.WriteLine("[ignored] {0}", pipe.Name);
		}

        public void Failure(RunPipe pipe, ReportRun result)
        {
			Writer.WriteLine("[failure] {0}: {1}", pipe.Name, result.Exception.Message);
		}

        public void Skip(RunPipe pipe, ReportRun result)
        { }
    }
}
