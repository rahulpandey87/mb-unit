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
    /// This is the base class for attributes that can decorate fixtures identifying them as using provider classes.
    /// </summary>
	public abstract class ProviderFixtureDecoratorPatternAttribute
		: FixtureDecoratorPatternAttribute
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFixtureDecoratorPatternAttribute"/> class.
        /// </summary>
		protected ProviderFixtureDecoratorPatternAttribute()
		{}

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderFixtureDecoratorPatternAttribute"/> class.
        /// </summary>
        /// <param name="description">A brief description of the provider.</param>
		protected ProviderFixtureDecoratorPatternAttribute(string description)
			:base(description)
		{}	
	}
}
