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
using System.Runtime.Serialization;
using System.IO;
using System.Globalization;

namespace MbUnit.Core.Exceptions
{
	/// <summary>
	/// Exception throwed when not finding a vertex.
	/// </summary>
	[Serializable]
	public class MultipleCultureException : AssertionException
	{
		private CultureInfo culture;

		public MultipleCultureException(CultureInfo culture,String message)
			: base(message)
		{
			this.culture = culture;
		}
		
		/// <summary>
		/// Creates an exception with a message
		/// and an inner exception.
		/// </summary>
        /// <param name="culture">The <see cref="CultureInfo"/> to use.</param>
		/// <param name="message">Error message</param>
		/// <param name="ex">Inner exception</param>
		public MultipleCultureException(CultureInfo culture, String message, Exception ex)
			:base(message,ex)
		{
			this.culture = culture;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctx"></param>
		protected MultipleCultureException(SerializationInfo info, StreamingContext ctx)
			:base(info,ctx)
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string Message
		{
			get
			{
				return String.Format("Exceptions in different culture: {0}",this.culture.ToString());
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return this.culture;
			}
		}
	}
}
