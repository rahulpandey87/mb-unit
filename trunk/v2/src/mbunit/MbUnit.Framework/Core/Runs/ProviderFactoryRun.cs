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

using System;
using System.Reflection;
using MbUnit.Framework;

namespace MbUnit.Core.Runs
{
	using MbUnit.Core.Invokers;

	using QuickGraph.Concepts.Serialization;

	/// <summary>
	/// Summary description for ProviderFactoryRun.
	/// </summary>
	public sealed class ProviderFactoryRun : IRun
	{
		private Type factoryType;
		private Type factoredType;
		private Type decoratedType;

		public ProviderFactoryRun(
			Type factoryType, 
			Type factoredType, 
			Type decoratedType)
		{
			if (factoryType==null)
				throw new ArgumentNullException("factoryType");
			if (factoredType == null)
				throw new ArgumentNullException("factoredType");
			if (decoratedType ==null)
				throw new ArgumentNullException("decoratedType");

			this.factoryType = factoryType;
			this.factoredType = factoredType;
			this.decoratedType = decoratedType;
		}

		#region IRun Members

		public string Name
		{
			get
			{
				return String.Format("ProviderFactory({0}->{1})",
					this.factoryType.Name,
					this.factoredType.Name
					);
			}
		}

		public bool IsTest
		{
			get { return true; }
		}

		public void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
		{
			// getting properties from the factory that returns a
			// type assignable to factored type
			foreach(PropertyInfo pi in TypeHelper.GetAttributedProperties(this.factoryType,typeof(FactoryAttribute)))
			{
				// make sure readable
				if (!pi.CanRead)
					continue;
				// check return type
				if (!this.factoredType.IsAssignableFrom(pi.PropertyType))
					continue;
				// check it is not an index
				if (pi.GetGetMethod().GetParameters().Length!=0)
					continue;

				// ok, we can add this one to the tree
				PropertyGetRunInvoker pget = new PropertyGetRunInvoker(
					this,
					pi
					);
				tree.AddChild(parent,pget);
			}

/*
			// getting methods
            foreach (MethodInfo mi in TypeHelper.GetAttributedProperties(this.factoredType, typeof(FactoryAttribute)))
            {
				if (mi.GetParameters().Length!=0)
					continue;
				if (!this.factoredType.IsAssignableFrom(mi.ReturnType))
					continue;
				if (mi.Name.StartsWith("get_"))
					continue;

				ArgumentFeederRunInvoker mrun = new ArgumentFeederRunInvoker(
					this,
					mi
					);
				tree.AddChild(parent,mrun);
			}
*/
		}

		#endregion
	}
}
