// QuickGraph Library 
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
//		QuickGraph Library HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux


using System;
using System.Runtime.Serialization;
using System.IO;

namespace QuickGraph.Exceptions
{
	/// <summary>
	/// Exception throwed when not finding a vertex.
	/// </summary>
	[Serializable]
	public class EdgeNotFoundException : Exception
	{
		/// <summary>
		/// Construct an <see cref="EdgeNotFoundException"/> instance.
		/// </summary>
		/// <param name="message"></param>
		public EdgeNotFoundException(String message)
			: base(message)
		{}

		/// <summary>
		/// Constructs an empty exception
		/// </summary>
		public EdgeNotFoundException()
			:base()
		{}
		
		/// <summary>
		/// Creates an exception with a message
		/// and an inner exception.
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="ex">Inner exception</param>
		public EdgeNotFoundException(String message, Exception ex)
			:base(message,ex)
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctx"></param>
		protected EdgeNotFoundException(SerializationInfo info, StreamingContext ctx)
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
				return String.Format("Edge {0} was not found",base.Message);
			}
		}
	}
}
