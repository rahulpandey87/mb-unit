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
using System.Collections;
using System.Reflection;

namespace MbUnit.Core.Collections
{
	/// <summary>
	/// Summary description for AttributedMethodEnumerator.
	/// </summary>
	public sealed class AttributedMethodEnumerator : IEnumerator
	{
		private MethodInfo[] methods;
		private IEnumerator methodEnumerator;
		private Type customAttributeType;

		public AttributedMethodEnumerator(Type testedType, Type customAttributeType)
		{
			if (testedType==null)
				throw new ArgumentNullException("testedType");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");

			this.methods = testedType.GetMethods();
			this.customAttributeType = customAttributeType;
			this.methodEnumerator = methods.GetEnumerator();
		}

		public void Reset()
		{
			this.methodEnumerator.Reset();
		}

		public MethodInfo Current
		{
			get
			{
				return (MethodInfo)this.methodEnumerator.Current;
			}
		}

		Object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		public bool MoveNext()
		{
			bool success=false;
			while(true)
			{
				success=this.methodEnumerator.MoveNext();
				if (!success)
					break;
				
				if (TypeHelper.HasCustomAttribute(
					(MethodInfo)this.methodEnumerator.Current,
					this.customAttributeType
					))
				{
					success=true;
					break;
				}
				else
					success = false;
			}

			return success;
		}
	}
}
