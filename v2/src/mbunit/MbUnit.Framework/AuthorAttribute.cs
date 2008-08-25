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
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

namespace MbUnit.Framework
{
    [Obsolete("This demonstration of subclassing the Author attribute will be removed in MbUnit v3")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class PelikhanAttribute : AuthorAttribute
	{
		public PelikhanAttribute()
			:base("Jonathan de Halleux","dehalleux@pelikhan.com","http://www.mbunit.com")
		{}
	}

    /// <summary>
    /// Associates the author's name and email address with a test fixture, test method,
    /// test parameter or other test component.
    /// </summary>
	[AttributeUsage(AttributeTargets.Class,AllowMultiple=true,Inherited=true)]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class AuthorAttribute : InformationAttribute
	{
		private string name;
		private string email = "";
		private string homePage = "";

        /// <summary>
        /// Associates the author's name with the test component annotated
        /// by this attribute.
        /// </summary>
        /// <param name="name">The author's name</param>
        public AuthorAttribute(string name)
            : this(name, "", "unspecified")
        { }

        /// <summary>
        /// Associates the author's name and email address with the test component annotated
        /// by this attribute.
        /// </summary>
        /// <param name="name">The author's name</param>
        /// <param name="email">The author's email address</param>
		public AuthorAttribute(string name, string email)
            : this(name, email, "unspecified")
		{ }

        /// <summary>
        /// Associates the author's name, email address and homepage with the test component annotated
        /// by this attribute.
        /// </summary>
        /// <param name="name">The author's name</param>
        /// <param name="email">The author's email address</param>
        /// <param name="homePage">The author's home page</param>
		public AuthorAttribute(string name, string email, string homePage)
		{
			this.name = name;
			this.email = email;
			this.homePage = homePage;
		}

        /// <summary>
        /// Gets or sets the author's name.
        /// </summary>
		[Category("Data")]
		public string Name
		{
			get
			{
				return this.name;
			}
		}

        /// <summary>
        /// Gets or sets the author's email address or empty string if none.
        /// </summary>
		[Category("Data")]
		public string EMail
		{
			get
			{
				return this.email;
			}
		}

        /// <summary>
        /// Gets or sets the author's homepage or empty string if none.
        /// </summary>
		[Category("Data")]
		public string HomePage			
		{
			get
			{
				return this.homePage;
			}
		}

        /// <inheritdoc />
		public override string ToString()
		{
			return String.Format("{0},  {1}, {2}",
				this.Name,
				this.EMail,
				this.HomePage
				);
		}
	}
}
