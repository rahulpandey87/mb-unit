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
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace MbUnit.Core.Monitoring
{
	/// <summary>
	/// Describes the status of the memory.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The code to retreive the total physical and available physical memory
	/// was takened from the AUT project (http://aut.tigris.org).
	/// </para>
	/// </remarks>
    [Obsolete("Uses GC information")]
    public sealed class MemoryStatus
    {
		private long workingSet;

        [Obsolete("Uses GC information")]
        public MemoryStatus()
        {
            this.workingSet = GC.GetTotalMemory(false);
        }

		public long WorkingSet
		{
			get
			{
				return this.workingSet;
			}
		}
	}
}
