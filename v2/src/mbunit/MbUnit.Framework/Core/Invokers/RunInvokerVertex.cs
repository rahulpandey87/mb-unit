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

namespace MbUnit.Core.Invokers
{
	using System;
	
	using QuickGraph.Concepts;
	using QuickGraph;
	using QuickGraph.Concepts.Providers;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Serialization;


	/// <summary>
	/// A <see cref="IVertex"/> implementation, containing a
	/// <see cref="IRunInvoker"/>.
	/// </summary>
	public class RunInvokerVertex : Vertex, IGraphSerializable
	{
		private IRunInvoker invoker = null;

		/// <summary>
		/// Builds a new unitialized vertex. Internal use only.
		/// </summary>		
		/// <remarks>You should not call this method directly.</remarks>
		public RunInvokerVertex(int id)
			:base(id)
		{}
		
		/// <summary>
		/// Gets a value indicating if the vertex has a <see cref="IRunInvoker"/>
		/// instance attached to it.
		/// </summary>
		/// <value>
		/// true if the vertex has a <see cref="IRunInvoker"/> instance attached.
		/// </value>
		public bool HasInvoker
		{
			get
			{
				return this.invoker != null;
			}
		}
		
		/// <summary>
		/// Gets the <see cref="IRunInvoker"/> attached to the vertex.
		/// </summary>
		/// <value>
		/// The <see cref="IRunInvoker"/> instance attached to the vertex
		/// </value>
		/// <exception cref="InvalidOperationException">
		/// the <see cref="IRunInvoker"/> is a null reference
		/// </exception>
		public IRunInvoker Invoker
		{
			get
			{
				if (this.invoker==null)
					throw new InvalidOperationException("invoker is not initialized");
				return this.invoker;
			}
			set
			{
				this.invoker = value;
			}
		}

		/// <summary>
		/// Not implemented.
		/// </summary>
		/// <remarks/>
		/// <exception cref="InvalidOperationException">always thrown</exception>
		public override void ReadGraphData (IGraphSerializationInfo info)
		{
			throw new InvalidOperationException("Not implemented");
		}

		/// <summary>
		/// Serializes informations to the <see cref="IGraphSerializationInfo"/> instance.
		/// </summary>		
		/// <param name="info">serialization device</param>
		/// <exception cref="ArgumentNullException">info is a null reference</exception>
		public override void WriteGraphData(IGraphSerializationInfo info)
		{
			if (info==null)
				throw new ArgumentNullException("info");
			base.WriteGraphData(info);
			if (!HasInvoker)
				return;
			
			info.Add("name",this.Invoker.Name);
			info.Add("test",this.Invoker.Generator.IsTest);
		}
		
		/// <summary>
		/// Converts the object to string
		/// </summary>
		/// <remarks>
		/// This class outputs the vertex ID and <c>Invoker.ToString()</c>.
		/// </remarks>
		/// <returns>
		/// String representation of the vertex
		/// </returns>
		public override string ToString()
		{
			return String.Format("{0},  {1}",
				this.ID,
				(HasInvoker) ? Invoker.ToString() : "null"
				);
		}
	}

	/// <summary>
	/// Internal use
	/// </summary>
	public sealed class RunInvokerVertexProvider : IVertexProvider
	{
		private int nextID = 0;
		
		public Type VertexType
		{
			get
			{	
				return typeof(RunInvokerVertexProvider);				
			}
		}
		
		public IVertex ProvideVertex()
		{
			return new RunInvokerVertex(nextID++);
		}
		
		public void UpdateVertex(IVertex v)
		{
			nextID = v.ID+1;
		}
	}
}
