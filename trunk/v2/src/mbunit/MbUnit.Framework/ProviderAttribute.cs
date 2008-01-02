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
using MbUnit.Core.Framework;
using System;


namespace MbUnit.Framework 
{
	
	
	/// <summary>
	/// Tags method that provide new object to be used in the following tests.
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='ProviderAttribute']"/>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
	public class ProviderAttribute : PatternAttribute
	{
		private Type providerType =null;

		/// <summary>
		/// Constructs a provider attribute for the <paramref name="providerType"/>
		/// type.
		/// </summary>
		/// <param name="providerType">provider type</param>
		public ProviderAttribute(Type providerType) 
		{
			if (providerType==null)
				throw new ArgumentNullException("providerType");
			this.providerType=providerType;
		}
		
		/// <summary>
		/// Constructs a provider attribute for the <paramref name="providerType"/>
		/// type.
		/// </summary>
		/// <param name="providerType">provider type</param>
		/// <param name="description">description of the provider</param>
		public ProviderAttribute(Type providerType, string description)
		:base(description)
		{
			if (providerType==null)
				throw new ArgumentNullException("providerType");
			this.providerType=providerType;
		}
		
		/// <summary>
		/// Gets or sets the provided type
		/// </summary>
		/// <value>
		/// Provided type.
		/// </value>
		public Type ProviderType
		{
			get
			{
				return this.providerType;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.providerType = value;
			}
		}
	}
}
