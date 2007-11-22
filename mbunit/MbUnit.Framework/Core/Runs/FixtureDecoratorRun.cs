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
using System.Reflection;

using MbUnit.Core.Invokers;
using MbUnit.Core.Framework;
using MbUnit.Core.Collections;

namespace MbUnit.Core.Runs
{

	/// <summary>
	/// Summary description for FixtureDecoratorRun.
	/// </summary>
	public class FixtureDecoratorRun : IRun
	{
		private Type decoratorType;

		public FixtureDecoratorRun(Type decoratorType)
		{
			if (decoratorType==null)
				throw new ArgumentNullException("decoratorType");
			this.decoratorType = decoratorType;
		}

		#region IRun Members

		public string Name
		{
			get
			{
				return String.Format("FixtureDecorator({0})",this.decoratorType.Name);
			}
		}

		public bool IsTest
		{
			get
			{
				return false;
			}
		}

		public void Reflect(RunInvokerTree tree, RunInvokerVertex parent, Type t)
		{
			if (tree==null)
				throw new ArgumentNullException("tree");
			if (parent==null)
				throw new ArgumentNullException("parent");
			if (t==null)
				throw new ArgumentNullException("t");

			// for each attribute
			foreach(FixtureDecoratorPatternAttribute ca in t.GetCustomAttributes(this.decoratorType,true))
			{
				// populate tree
				IRun carun = ca.GetRun(t);
				carun.Reflect(tree,parent,t);
			}
		}

		#endregion
	}
}
