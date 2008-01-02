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
using System.Runtime.Serialization;
using System.IO;

namespace MbUnit.Core.Exceptions
{
	[Serializable]
	public class UselessTestException : Exception
	{
		private Type testType;
		private Type testedType = null;

		public UselessTestException(
			Type testType
			)
			:base()
		{
			if (testType==null)
				throw new ArgumentNullException("testType");
			this.testType = testType;
		}
		
		public UselessTestException(
			Type testType,
			String message
			)
			:base(message)
		{
			if (testType==null)
				throw new ArgumentNullException("testType");
			this.testType = testType;
		}
		
		protected UselessTestException(
			Type testType,
			SerializationInfo info, 
			StreamingContext context)
			:base(info,context)
		{
			if (testType==null)
				throw new ArgumentNullException("testType");
			this.testType = testType;
		}
		
		public UselessTestException(
			Type testType,
			string message, 
			Exception innerException
			)
			:base(message,innerException)
		{
			if (testType==null)
				throw new ArgumentNullException("testType");
			this.testType = testType;
		}

		public UselessTestException(
			Type testType,
			Type testedType
			)
			:base()
		{
			if (testType==null)
				throw new ArgumentNullException("testType");
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			this.testType = testType;
			this.testedType = testedType;
		}
		
		public UselessTestException(
			Type testType,
			Type testedType,
			String message
			)
			:base(message)
		{
			if (testType==null)
				throw new ArgumentNullException("testType");
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			this.testType = testType;
			this.testedType = testedType;
		}
		
		protected UselessTestException(
			Type testType,
			Type testedType,
			SerializationInfo info, 
			StreamingContext context)
			:base(info,context)
		{
			if (testType==null)
				throw new ArgumentNullException("testType");
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			this.testType = testType;
			this.testedType = testedType;
		}
		
		public UselessTestException(
			Type testType,
			Type testedType,
			string message, 
			Exception innerException
			)
			:base(message,innerException)
		{
			if (testType==null)
				throw new ArgumentNullException("testType");
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			this.testType = testType;
			this.testedType = testedType;
		}

		public Type TestType
		{
			get
			{
				return this.testType;
			}
		}

		public Type TestedType
		{
			get
			{
				return this.testType;
			}
		}

		public override string Message
		{	
			get
			{
				StringWriter sw =new StringWriter();
				sw.WriteLine("Useless test detected in test {0}",this.testType.FullName);
				if (this.testedType!=null)
					sw.WriteLine("Test applied to {0}",this.testedType.FullName);
				return sw.ToString();
			}
		}
	}
}
