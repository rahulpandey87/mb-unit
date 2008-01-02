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

namespace MbUnit.Core.Runs
{
	using MbUnit.Core.Invokers;

	using QuickGraph.Concepts.Serialization;

	/// <summary>
	/// Summary description for ProviderFactoryRun.
	/// </summary>
	public class AssemblyProviderRun : IRun
	{
		private Type typeToAssembly;
		private Type factoredType;
		private string _namespace;
		private bool recurse;
		private Type decoratedType;

		public AssemblyProviderRun(
			Type typeToAssembly, 
			Type factoredType, 
			string _namespace,
			bool recurse,
			Type decoratedType)
		{
			if (typeToAssembly==null)
				throw new ArgumentNullException("typeToAssembly");
			if (factoredType == null)
				throw new ArgumentNullException("factoredType");
			if (_namespace==null)
				throw new ArgumentNullException("_namespace");
			if (decoratedType ==null)
				throw new ArgumentNullException("decoratedType");

			this.typeToAssembly = typeToAssembly;
			this.factoredType = factoredType;
			this._namespace = _namespace;
			this.recurse = recurse;
			this.decoratedType = decoratedType;
		}

		#region IRun Members

		public string Namespace
		{
			get
			{
				return this._namespace;
			}
		}

		public string Name
		{
			get
			{
				return String.Format("AssemblyProvider({0},{1}->{2})",
					this.typeToAssembly.Assembly.FullName,
					this.Namespace,
					this.factoredType.Name
					);
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
			throw new Exception("not implemented");
		}

		#endregion

	}
}
