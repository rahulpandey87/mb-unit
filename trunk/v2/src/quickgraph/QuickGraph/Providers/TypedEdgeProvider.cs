// QuickGraph Library 
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
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using System;
using System.Reflection;

namespace QuickGraph.Providers
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Providers;

	/// <summary>
	/// Default custom edge provider
	/// </summary>
	public class TypedEdgeProvider :
		IEdgeProvider
	{
		private int nextID = 0;
		private Type edgeType;
		private ConstructorInfo constructor = null;

		public TypedEdgeProvider(Type edgeType)
		{
			if (edgeType==null)
				throw new ArgumentNullException("edgeType");
			if (edgeType.GetInterface("QuickGraph.Concepts.IEdge",true)==null)
				throw new ArgumentException("Type does not implement IEdge");

			this.edgeType=edgeType;
			Type[] types = new Type[3];
			types[0]=typeof(int);
			types[1]=typeof(IVertex);
			types[2]=typeof(IVertex);
			this.constructor = edgeType.GetConstructor(types);
			if (this.constructor==null)
				throw new ArgumentNullException("Did not find matching constructor?");
		}


		/// <summary>
		/// Returns typeof(CustomEdge)
		/// </summary>
		public Type EdgeType
		{
			get
			{
				return this.edgeType;
			}
		}

		/// <summary>
		/// Creates a new edge
		/// </summary>
		/// <returns></returns>
		public IEdge ProvideEdge(IVertex u, IVertex v)
		{
			if (u==null)
				throw new ArgumentNullException("u");
			if (v==null)
				throw new ArgumentNullException("v");

			Object[] args  = new Object[3];
			args[0]=this.nextID++;
			args[1]=u;
			args[2]=v;
			return (IEdge)this.constructor.Invoke(args);
		}

		/// <summary>
		/// Updates a edge that has not been created with the provider
		/// </summary>
		/// <param name="e">edge to update</param>
		public void UpdateEdge(IEdge e)
		{
			e.ID = this.nextID++;
		}
	}
}
