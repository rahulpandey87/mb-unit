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

namespace MbUnit.Core.Exceptions 
{
	using MbUnit.Core.Invokers;
	using System;
	using System.Runtime.Serialization;
	using System.Collections;
	using System.IO;
		
	/// <summary>
	/// Base class for MbUnit exceptions
	/// </summary>
	[Serializable]
	public class AssertionException : ApplicationException 
	{
		/// <summary>
		/// Initializes an empty <see cref="AssertionException"/> instance.
		/// </summary>
		public AssertionException()
		   :base()
		{}
		
      /// <summary>
      /// Initializes an <see cref="AssertionException"/> with the given assertion failure <paramref name="message"/>
      /// </summary>
      /// <param name="message">The message detailing the assertion failure</param>
		public AssertionException(
			String message)
		:base(message)
		{}
		
      /// <summary>
      /// Initializes an <see cref="AssertionException"/> with the given serialization <paramref name="info"/> and <paramref name="context"/>
      /// </summary>
      /// <param name="info">Details of the serialization issue</param>
      /// <param name="context">The streaming context within which it occured</param>
		protected AssertionException(
			SerializationInfo info, 
			StreamingContext context)
		:base(info,context)
		{}
		
      /// <summary>
      /// Initializes an <see cref="AssertionException"/> with the given assertion failure <paramref name="message"/> and <paramref name="innerException"/>
      /// </summary>
      /// <param name="message">The message detailing the assertion failure</param>
      /// <param name="innerException">The exception to pass on</param>
		public AssertionException(
		    string message, 
		    Exception innerException
		    )
		:base(message,innerException)
		{}
	}
}
