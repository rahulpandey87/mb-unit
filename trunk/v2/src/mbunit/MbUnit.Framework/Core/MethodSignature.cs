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


namespace MbUnit.Core 
{
	using System;
	using System.Reflection;
	
	
	/// <summary>
	/// TODO - Add class summary
	/// </summary>
	/// <remarks>
	/// 	created by - dehalleux
	/// 	created on - 30/01/2004 11:35:56
	/// </remarks>
	public class MethodSignature
	{
		private Type returnType;
		private ParameterInfo[] parameters;
		
		public MethodSignature(MethodInfo mi)
		{
			if (mi==null)
				throw new ArgumentNullException("mi");
			this.returnType = mi.ReturnType;
			this.parameters = mi.GetParameters();
		}
		
		public Type ReturnType
		{
			get
			{
				return this.returnType;
			}			
		}
		
		public ParameterInfo[] GetParameters()
		{
			return this.parameters;
		}
				
		public Object[] GetParameterInstances()
		{
			if (this.parameters.Length==0)
				return null;
			
			Object[] pas = new Object[this.parameters.Length];
				
			int i = 0;
			foreach(ParameterInfo pi in this.parameters)
			{
				ConstructorInfo ci = pi.ParameterType.GetConstructor(Type.EmptyTypes);
				if (ci==null)
					throw new Exception("No default constructor");
				
				pas[i++] = ci.Invoke(null);
			}
				
			return pas;
		}
		
		public void CheckSignature(MethodInfo mi)
		{
			if (mi==null)
				throw new ArgumentNullException("mi");
			if (returnType==null)
				throw new ArgumentNullException("returnType");
			
			if (mi.ReturnType!=returnType)
				throw new ArgumentException("return type do not match");

			CheckArguments(mi);		
		}
		
		public void CheckArguments(MethodInfo mi)
		{
			if (mi==null)
				throw new ArgumentNullException("mi");
						
			ParameterInfo[] pis = mi.GetParameters();
			if (pis.Length != this.parameters.Length)
				throw new ArgumentException("number of arguments do not match");
			
			for(int i = 0; i< pis.Length;++i)
			{
				if (pis[i] != this.parameters[i])
					throw new ArgumentException("argument type do not match");
			}			
		}
	}
}
