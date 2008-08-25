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
using System.Text;
using System.Text.RegularExpressions;

using MbUnit.Core.Exceptions;

namespace MbUnit.Framework
{
    /// <summary>
    /// Performance Assertion class
    /// </summary>
	public sealed class PerfAssert
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="PerfAssert"/> class.
        /// </summary>
		private PerfAssert(){}

        /// <summary>
        /// Creates a countdown timer that will assert if execution time exceeds maximum duration.
        /// </summary>
        /// <param name="maxDuration">Maximum duration for the test in seconds</param>
        /// <returns>A <see cref="CountDownTimer"/> that knows the <paramref name="maxDuration"/> of the test</returns>
        /// <exception cref="AssertionException">Thrown if <paramref name="maxDuration"/> is not greater than 0</exception>
		public CountDownTimer Duration(double maxDuration)
		{
			Assert.IsTrue(maxDuration>0,"Maximum duration is not strictly positive");
			
			return new CountDownTimer(maxDuration);
		}
		
		
	}
}
