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
	using MbUnit.Core.Invokers;
	using System.Collections;
	
	[Serializable]
	public class ExceptionTypeMistmachException : AssertionException 
	{
		private Type expected;
		private Exception found;

        public ExceptionTypeMistmachException(Type expected,
                                              string description,
		                                      Exception found
		                                      )
			:base(description,found)
        {
			this.expected = expected;
			this.found = found;
		}
		
		public override string Message
		{
			get
			{
				return String.Format("{0} Expected exception of type {1}, got {2} ({3})",
                    base.Message,
					this.expected.FullName,
					this.found.GetType(),
					this.found.Message
					);
			}
		}
	}
}
