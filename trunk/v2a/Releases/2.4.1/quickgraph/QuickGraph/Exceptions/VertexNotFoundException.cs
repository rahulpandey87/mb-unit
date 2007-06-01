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
	public class VertexNotFoundException : Exception
	{
		/// <summary>
		/// Build a new exception
		/// </summary>
		/// <param name="name">vertex name</param>
		public VertexNotFoundException(String name)
			: base(name)
		{}

		/// <summary>
		/// Constructs an empty exception
		/// </summary>
		public VertexNotFoundException()
			:base()
		{}
		
		/// <summary>
		/// Creates an exception with a vertex name
		/// and an inner exception.
		/// </summary>
		/// <param name="name">Vertex name</param>
		/// <param name="ex">Inner exception</param>
		public VertexNotFoundException(String name, Exception ex)
			:base(name,ex)
		{}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="ctx"></param>
		protected VertexNotFoundException(SerializationInfo info, StreamingContext ctx)
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
				return String.Format("Could not find {0} vertex",base.Message);
			}
		}

	}
}
