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


namespace MbUnit.Core.Exceptions 
{
	using System;
	using System.IO;
	using System.Runtime.Serialization;
	
	[Serializable]
	public class IndexerNotFoundException : System.Exception 
	{
		private Type type;
		private Type[] parameters;
		
		public IndexerNotFoundException(Type t, Type[] parameters) 
		{
			this.type = t;
			this.parameters = parameters;
		}
		
		public IndexerNotFoundException(
		    Type t,
		    Type[] parameters,
		    string message
		    )
		:base(message)
		{
			this.type = t;
			this.parameters = parameters;
		}
		
		protected IndexerNotFoundException(
		    Type t,
		    Type[] parameters,
			SerializationInfo info, 
			StreamingContext context)
		:base(info,context)
		{
			this.type = t;
			this.parameters = parameters;
		}
		
		public IndexerNotFoundException(
		    Type t,
		    Type[] parameters,
		    string message, 
		    Exception innerException
		    )
		:base(message,innerException)
		{
			this.type = t;
			this.parameters = parameters;
		}	
	
		public override string Message
		{
			get
			{
				StringWriter sw = new StringWriter();
				sw.WriteLine("Could not find an indexer matching the desired signature");
				sw.WriteLine("Type: {0}",this.type.FullName);
				sw.WriteLine("Parameter types:");
				for(int i=0;i<this.parameters.Length;++i)
					sw.WriteLine("\t{0}",this.parameters[i].FullName);
			
				return sw.ToString();
			}
		}
	}
}
