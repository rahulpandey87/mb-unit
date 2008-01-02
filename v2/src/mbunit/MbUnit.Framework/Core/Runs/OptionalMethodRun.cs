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


namespace MbUnit.Core.Runs
{
	using System;
	using System.Reflection;
	using MbUnit.Core;
	using MbUnit.Core.Collections;
	using System.Diagnostics;
	using MbUnit.Core.Invokers;
	using MbUnit.Core.Runs;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Serialization;
	
	public class OptionalMethodRun : MethodRun
	{
		public OptionalMethodRun(Type attributeType, 
		                         bool isTest)
			:base(attributeType,isTest,false)
		{
			
		}
		
		protected override void PopulateInvokerTree(
			RunInvokerTree tree, 
			RunInvokerVertex parent, 
			Type t
			)
		{			
			if (TypeHelper.HasMethodCustomAttribute(t,this.AttributeType))
			{				
				MethodInfo mi = TypeHelper.GetAttributedMethod(t,this.AttributeType);
				tree.AddChild(parent, new MethodRunInvoker(this,mi));
			}			
		}
	}
}
