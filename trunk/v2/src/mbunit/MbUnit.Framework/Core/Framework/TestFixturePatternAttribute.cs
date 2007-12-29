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
using System.Threading;
using MbUnit.Core;
using MbUnit.Core.Runs;

namespace MbUnit.Core.Framework
{
	/// <summary>
	/// Base class for attributes that define test fixtures.
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='TestFixturePatternAttribute']"/>		
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
	public abstract class TestFixturePatternAttribute : PatternAttribute
	{
        private int timeOut = 10;
        private ApartmentState apartmentState = ApartmentState.Unknown;

		protected TestFixturePatternAttribute()
		{}

		protected TestFixturePatternAttribute(string description)
		:base(description)
		{}

        public ApartmentState ApartmentState
        {
            get { return this.apartmentState; }
            set { this.apartmentState = value; }
        }

        /// <summary>
        /// Gets or sets the fixture timeout in minutes.
        /// </summary>
        /// <remarks>
        /// Default value is 5 minutes.
        /// </remarks>
        /// <value>
        /// Time out minutes.
        /// </value>
        public int TimeOut
        {
            get { return this.timeOut; }
            set { this.timeOut = value; }
        }

        public TimeSpan GetTimeOut()
        {
            return new TimeSpan(0, this.timeOut, 0);
        }

        public abstract IRun GetRun();		

		public static TestFixturePatternAttribute GetFixturePattern(Type fixtureType)
		{
			if (fixtureType==null)
				throw new ArgumentNullException("fixtureType");

			return (TestFixturePatternAttribute)TypeHelper.GetFirstCustomAttribute(
				fixtureType,
				typeof(TestFixturePatternAttribute)
				);
		}
	}
}
