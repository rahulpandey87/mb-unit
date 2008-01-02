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

namespace MbUnit.Core
{
	using System;
	using MbUnit.Core.Invokers;
    using MbUnit.Core.Reports.Serialization;
	
	public class RunPipeResultEventArgs : RunPipeEventArgs
	{
        private ReportRun result;
        /// <summary>
		/// Default constructor - initializes all fields to default values
		/// </summary>
        public RunPipeResultEventArgs(RunPipe pipe, ReportRun result)
		:base(pipe)
		{
			if (result==null)
				throw new ArgumentNullException("result");
			this.result = result;
		}

        public ReportRun Result
        {
			get
			{
				return this.result;
			}
		}
	}
	
	public delegate void RunPipeSuccessEventHandler(Object sender,
	                                           		RunPipeResultEventArgs e);
}
