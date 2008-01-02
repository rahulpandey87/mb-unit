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
	/// Default custom vertex provider
	/// </summary>
	public class TypedVertexProvider :
		IVertexProvider
	{
		private int nextID = 0;
		private Type vertexType;
		private ConstructorInfo constructor = null;

		public TypedVertexProvider(Type vertexType)
		{
			if (vertexType==null)
				throw new ArgumentNullException("vertexType");
			if (vertexType.GetInterface("QuickGraph.Concepts.IVertex",true)==null)
				throw new ArgumentException("Type does not implement IVertex");

			this.vertexType=vertexType;
			Type[] types = new Type[1];
			types[0]=typeof(int);
			this.constructor = vertexType.GetConstructor(types);
			if (this.constructor==null)
				throw new ArgumentException("Did not find matching constructor?");
		}


		/// <summary>
		/// Returns typeof(CustomVertex)
		/// </summary>
		public Type VertexType
		{
			get
			{
				return this.vertexType;
			}
		}

		/// <summary>
		/// Creates a new vertex
		/// </summary>
		/// <returns></returns>
		public IVertex ProvideVertex()
		{
			Object[] args  = new Object[1];
			args[0]=this.nextID++;
			return (IVertex)this.constructor.Invoke(args);
		}

		/// <summary>
		/// Updates a vertex that has not been created with the provider
		/// </summary>
		/// <param name="v">vertex to update</param>
		public void UpdateVertex(IVertex v)
		{
			v.ID = this.nextID++;
		}
	}
}
