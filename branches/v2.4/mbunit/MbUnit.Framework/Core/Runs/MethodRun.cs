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
	using System.Reflection;
	using MbUnit.Core.Collections;	
	using MbUnit.Core.Invokers;
	using MbUnit.Core.Framework;
	using MbUnit.Core.Reflection;
	
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Serialization;

	public class MethodRun : IRun
	{
		private Type attributeType = null;
		private Type invokerType = null;
		private ConstructorInfo invokerConstructor = null;
		private bool isTest = false;
		private bool allowMultiple = false;
		private SignatureChecker checker = null;
		
		public MethodRun(Type attributeType, bool isTest, bool allowMultiple)
		{
			if (attributeType==null)
				throw new ArgumentNullException("attributeType");
			
			this.invokerType = typeof(MethodRunInvoker);
			this.attributeType = attributeType;
			this.isTest = isTest;
			this.allowMultiple = allowMultiple;
			
			GetInvokerConstructor();
		}
		
		public MethodRun(Type attributeType, Type invokerType, bool isTest, bool allowMultiple)
		{
			if (attributeType==null)
				throw new ArgumentNullException("attributeType");
			if (invokerType ==null)
				throw new ArgumentNullException("invokerType");
			if (invokerType.GetInterface("IRunInvoker",false)==null)
				throw new ArgumentException("invokerType "+invokerType.Name+" does not implement IRunInvoker");
			
			this.invokerType = invokerType;
			this.attributeType = attributeType;
			this.isTest = isTest;
			this.allowMultiple = allowMultiple;
			
			GetInvokerConstructor();
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
				return this.isTest;
			}
		}
		
		public bool AllowMultiple
		{
			get
			{
				return this.allowMultiple;
			}
		}
		
		public string Name
		{
			get
			{
				string name = this.AttributeType.Name;
				if (this.AllowMultiple)
					name = String.Format("[{0}]",name);
				
				if(this.isTest)
					name += "*";

				return name;
			}
		}

		public SignatureChecker Checker
		{
			get
			{
				return this.checker;
			}
			set
			{
				this.checker=value;
			}
		}
		
		public override string ToString()
		{
			return String.Format("{0}, {1}, {2}",
			                this.AttributeType.Name,
			                (this.IsTest) ? "test": "method",
			                (this.AllowMultiple) ? "multiple" : "single"
			                );
		}

		public virtual void Reflect(
			RunInvokerTree tree, 
			RunInvokerVertex parent, 
			Type t
			)
		{
			CheckType(t);
			PopulateInvokerTree(tree,parent,t);
		}
			
		protected void GetInvokerConstructor()
		{
			this.invokerConstructor =		
				TypeHelper.GetConstructor(this.invokerType,typeof(IRun),typeof(MethodInfo));
		}
		
		protected IRunInvoker InstanceInvoker(MethodInfo mi)
		{
			Object[] args=new Object[2];
			args[0]=this;
			args[1]=mi;
			IRunInvoker invoker = (IRunInvoker)this.invokerConstructor.Invoke(args);
						
			return DecoratorPatternAttribute.DecoreInvoker(mi,invoker);
		}
		
		protected virtual void PopulateInvokerTree(
			RunInvokerTree tree, 
			RunInvokerVertex parent, 
			Type t
			)
		{	
			if (this.AllowMultiple)
			{
				foreach(MethodInfo mi in 
					TypeHelper.GetAttributedMethods(t,this.AttributeType))
				{
					try
					{
						if (this.Checker!=null)
							this.Checker.Check(mi);

						IRunInvoker invoker = InstanceInvoker(mi);
						RunInvokerVertex child = 
							tree.AddChild(parent,invoker);
					}
					catch(Exception ex)
					{
						FailedLoadingRunInvoker invoker = new FailedLoadingRunInvoker(
							this,ex);
						tree.AddChild(parent,invoker);
					}
				}				
			}
			else
			{
				try
				{
					MethodInfo mi = TypeHelper.GetAttributedMethod(t,this.AttributeType);
					if (this.Checker!=null)
						this.Checker.Check(mi);

					tree.AddChild(parent, new MethodRunInvoker(this,mi)); 
				}
				catch(Exception ex)
				{
					FailedLoadingRunInvoker invoker = new FailedLoadingRunInvoker(
						this,ex);
					tree.AddChild(parent,invoker);
				}
			}
		}
		
		protected virtual void CheckType(Type t)
		{
			// check multiplicity
			if (!this.AllowMultiple)
			{
				AttributedMethodCollection methods = 
					TypeHelper.GetAttributedMethods(t,this.AttributeType);
				if (methods.Count>1)
					throw new Exception("custom attribute must be unique in class");
			}			
		}
	}
}
