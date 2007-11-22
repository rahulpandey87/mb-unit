// MbUnit Library 
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
//		QuickGraph Library HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux
using System;
using System.Reflection;
using System.Collections;
using MbUnit.Core.Collections;
using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using MbUnit.Framework;

using QuickGraph.Concepts;
using QuickGraph.Concepts.Modifications;
using QuickGraph.Concepts.Serialization;
using QuickGraph.Collections.Sort;

namespace MbUnit.Core.Runs
{

	public sealed class ProcessMethodRun : IRun
	{
		private Type attributeType = null;
		
		public ProcessMethodRun(Type attributeType)
		{
			if (attributeType==null)
				throw new ArgumentNullException("attributeType");
			
			this.attributeType = attributeType;
		}
		
		public Type AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}
		
		public bool IsTest
		{
			get
			{
				return true;
			}
		}
		
		public string Name
		{
			get
			{
				return String.Format("Process[{0}]",this.AttributeType.Name);
			}
		}
		
		public override string ToString()
		{
			return this.Name;
		}

		public void Reflect(
			RunInvokerTree tree, 
			RunInvokerVertex parent, 
			Type t
			)
		{
			PopulateInvokerTree(tree,parent,t);
		}
		
		private IRunInvoker InstanceInvoker(MethodInfo mi)
		{
			IRunInvoker invoker = new MethodRunInvoker(this,mi);
						
			return DecoratorPatternAttribute.DecoreInvoker(mi,invoker);
		}
		
		private void PopulateInvokerTree(
			RunInvokerTree tree, 
			RunInvokerVertex parent, 
			Type t
			)
		{	
			// first gather all methods, with order.
			ArrayList methods = new ArrayList();
			foreach(MethodInfo mi in 
					TypeHelper.GetAttributedMethods(t,this.AttributeType))
			{
				// get sequence attribute
				TestSequenceAttribute seq = 
					(TestSequenceAttribute)TypeHelper.GetFirstCustomAttribute(mi,typeof(TestSequenceAttribute)); 
				methods.Add( new OrderedMethod(mi, seq.Order) );		
			}
			
			// sort the methods
			QuickSorter sorter = new QuickSorter();
			sorter.Sort(methods);
			
			// populate execution tree.
			RunInvokerVertex child = parent;
			foreach(OrderedMethod om in methods)
			{
				IRunInvoker invoker = InstanceInvoker(om.Method);
				child = tree.AddChild(child,invoker);
			}				
		}

		#region OrderedMethod
		private class OrderedMethod : IComparable
		{
			private MethodInfo method;
			private int order;
			
			public  OrderedMethod(MethodInfo method, int order)
			{
				this.method = method;
				this.order = order;
			}
			public MethodInfo Method
			{
				get
				{
					return this.method;
				}
			}
			public int Order
			{
				get
				{
					return this.order;	
				}
			}
			public int CompareTo(OrderedMethod obj)
			{
				return this.order.CompareTo(obj.order);
			}
			
			int IComparable.CompareTo(Object obj)
			{
				return this.CompareTo((OrderedMethod)obj);
			}
		}
		#endregion
	}
}
