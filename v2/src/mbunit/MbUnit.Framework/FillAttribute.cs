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
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{	
	/// <summary>
	/// Used to tag methods within <see cref="CollectionOrderFixtureAttribute">test fixtures implementing the collection order pattern</see>
	/// that fill collections with data. 
	/// </summary>
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=true)]
	public class FillAttribute : NonTestPatternAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="FillAttribute"/> class.
        /// </summary>
		public FillAttribute()
		{}

        /// <summary>
        /// Initializes a new instance of the <see cref="FillAttribute"/> class.
        /// </summary>
        /// <param name="description">The description of the method and what it does (for your own reference).</param>
		public FillAttribute(string description)
			:base(description)
		{}
	}
}
