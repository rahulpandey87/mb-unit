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

namespace MbUnit.Core.Runs
{
	using System;
	using System.Collections;
	using System.Reflection;
	using MbUnit.Core.Invokers;
	using MbUnit.Core.Exceptions;
	using MbUnit.Core.Framework;
	using System.Diagnostics;
	
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts;
	
	using QuickGraph.Concepts.Serialization;
	
	
	
	/// <summary>
	/// TODO - Add class summary
	/// </summary>
	/// <remarks>
	/// 	created by - dehalleux
	/// 	created on - 30/01/2004 15:26:18
	/// </remarks>
	public class CustomRun : IRun 
	{
		private bool isTest;
		private bool feedSender = true;
		private Type testerType = null;
		private Type targetAttributeType = null;
		private Object[] parameters = null;
		private ConstructorInfo constructor = null;
		
		public CustomRun(
		                 Type testerType,
		                 Type targetAttributeType,
		                 bool isTest,
		                 params object[] parameters
		                 )
		{
			if (testerType==null)
				throw new ArgumentNullException("testerType");
			if (targetAttributeType==null)	
				throw new ArgumentNullException("targetAttributeType");
			
			this.testerType = testerType;
			this.targetAttributeType = targetAttributeType;
			this.isTest = isTest;
			this.parameters=parameters;
			this.constructor = TypeHelper.GetConstructor(testerType,parameters);
		}

		public bool FeedSender
		{
			get
			{
				return this.feedSender;
			}
			set
			{
				this.feedSender = value;
			}
		}
		
		public bool IsTest
		{
			get
			{
				return this.isTest;
			}
		}
		
		public string Name
		{
			get
			{
				string name = testerType.Name;
				if (isTest)
					name+="*";
				return name;
			}
		}
		
		public Type TesterType
		{
			get
			{
				return this.testerType;
			}
		}
		
		public Type TargetAttributeType
		{
			get
			{
				return this.targetAttributeType;
			}
		}
		
		public void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
		{			
			// add tests methods
			foreach(MethodInfo mi in TypeHelper.GetAttributedMethods(
				this.testerType,
				this.targetAttributeType)
				)
			{
				CustomRunInvoker cr = new CustomRunInvoker(
				                                           this,
				                                           InstanceTester(),
				                                           mi,
														   this.feedSender
				                                           );
				// adding decoration and to tree
				tree.AddChild(parent,
				              DecoratorPatternAttribute.DecoreInvoker(mi,cr)
				              );
			}
		}
		
		protected Object InstanceTester()
		{
			return this.constructor.Invoke(this.parameters);
		}
	}
}
