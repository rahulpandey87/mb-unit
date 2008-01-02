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
	using System;
	using System.IO;
	using System.Runtime.Serialization;
	
	[Serializable]
	public class InterfaceNotImplementedException : System.Exception 
	{
		private Type type;
		private Type interfaceType;
		
		public InterfaceNotImplementedException(Type t, Type interfaceType) 
		{
			this.type = t;
			this.interfaceType = interfaceType;
		}
		
		public InterfaceNotImplementedException(
		    Type t,
		    Type interfaceType,
		    string message
		    )
		:base(message)
		{
			this.type = t;
			this.interfaceType = interfaceType;
		}
		
		protected InterfaceNotImplementedException(
		    Type t,
		    Type interfaceType,
			SerializationInfo info, 
			StreamingContext context)
		:base(info,context)
		{
			this.type = t;
			this.interfaceType = interfaceType;
		}
		
		public InterfaceNotImplementedException(
		    Type t,
		    Type interfaceType,
		    string message, 
		    Exception innerException
		    )
		:base(message,innerException)
		{
			this.type = t;
			this.interfaceType = interfaceType;
		}	
	
		public override string Message		
		{
			get
			{
				return String.Format("Type {0} does not implement the interface {1}.",
			             this.type.FullName,
			             this.interfaceType.FullName
			             );
			}
		}
	}
}
