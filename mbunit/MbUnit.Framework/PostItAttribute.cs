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
using System.ComponentModel;
using System.Reflection;
using MbUnit.Core;

namespace MbUnit.Framework
{
	/// <summary>
	/// Summary description for PostItAttribute.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[AttributeUsage(AttributeTargets.All,AllowMultiple=false,Inherited=true)]
	public class PostItAttribute : InformationAttribute
	{
		private string message;
		private AuthorAttribute author;

		public PostItAttribute(string message,Type author)
		{
			if (message==null)
				throw new ArgumentNullException("message");
			if (author==null)
				throw new ArgumentNullException("author");
			if (!typeof(AuthorAttribute).IsAssignableFrom(author.GetType()))
				throw new ArgumentException("author must derive from AuthorAttribute");
			this.message = message;

			ConstructorInfo ci = TypeHelper.GetConstructor(author,Type.EmptyTypes);
			this.author = (AuthorAttribute)ci.Invoke(null);
		}

		[Category("Data")]
		public String Message
		{
			get
			{
				return this.message;
			}
		}

		[Category("Data")]
		public AuthorAttribute Author
		{
			get
			{
				return this.author;
			}
		}

		[Browsable(false)]
		public Type AuthorType
		{
			get
			{
				return this.author.GetType();
			}
		}

	}
}
