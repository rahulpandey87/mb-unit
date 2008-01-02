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
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class PelikhanAttribute : AuthorAttribute
	{
		public PelikhanAttribute()
			:base("Jonathan de Halleux","dehalleux@pelikhan.com","http://www.mbunit.com")
		{}
	}

	/// <summary>
	/// This attribute identifies the author of a test fixture.
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="doc/remarkss/remarks[@name='AuthorAttribute']"/>
	[AttributeUsage(AttributeTargets.Class,AllowMultiple=true,Inherited=true)]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class AuthorAttribute : InformationAttribute
	{
		private string name;
		private string email = "";
		private string homePage = "";

        public AuthorAttribute(string name)
            : this(name, "", "unspecified")
        { }

		public AuthorAttribute(string name, string email)
            : this(name, email, "unspecified")
		{ }

		public AuthorAttribute(string name, string email, string homePage)
		{
			this.name = name;
			this.email = email;
			this.homePage = homePage;
		}

		[Category("Data")]
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		[Category("Data")]
		public string EMail
		{
			get
			{
				return this.email;
			}
		}

		[Category("Data")]
		public string HomePage			
		{
			get
			{
				return this.homePage;
			}
		}

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
