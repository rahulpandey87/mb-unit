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
//		QuickGraph Library HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux


using System;

namespace QuickGraph.Concepts
{
	/// <summary>
	/// Event argument that contains a <seealso cref="Vertex"/>.
	/// </summary>
	public class VertexEventArgs : EventArgs
	{
		private IVertex vertex;

		/// <summary>
		/// Builds a new event argument object
		/// </summary>
		/// <param name="v">vertex to store</param>
		public VertexEventArgs(IVertex v)
		{
			if (v == null)
				throw new ArgumentNullException("vertex");
			this.vertex  = v;
		}

		/// <summary>
		/// Vertex passed to the event
		/// </summary>
		public IVertex Vertex
		{
			get
			{
				return this.vertex;
			}
		}
	}

	/// <summary>
	/// Delegate that handles an event that sends a vertex.
	/// </summary>
	public delegate void VertexEventHandler(Object sender, VertexEventArgs e);

	/// <summary>
	/// Event argument that contains an <seealso cref="IEdge"/>.
	/// </summary>
	public class EdgeEventArgs : EventArgs
	{
		private IEdge edge;

		/// <summary>
		/// Create a new event argument
		/// </summary>
		/// <param name="e">edge to store</param>
		public EdgeEventArgs(IEdge e)
		{
			if (e == null)
				throw new ArgumentNullException("edge");
			this.edge  = e;
		}

		/// <summary>
		/// Edge passed to the event
		/// </summary>
		public IEdge Edge
		{
			get
			{
				return this.edge;
			}
		}
	}

	/// <summary>
	/// Delegate that handles an edge that sends a vertex.
	/// </summary>
	public delegate void EdgeEventHandler(Object sender, EdgeEventArgs e);

	/// <summary>
	/// Event argument that contains two <seealso cref="IEdge"/>.
	/// </summary>
	public class EdgeEdgeEventArgs : EdgeEventArgs
	{
		private IEdge targetEdge;

		/// <summary>
		/// Create a new event argument
		/// </summary>
		/// <param name="e">edge to store</param>
		/// <param name="targetEdge"></param>
		public EdgeEdgeEventArgs(IEdge e, IEdge targetEdge)
			:base(e)
		{
			if (targetEdge == null)
				throw new ArgumentNullException("targetEdge");
			this.targetEdge  = targetEdge;
		}

		/// <summary>
		/// Edge passed to the event
		/// </summary>
		public IEdge TargetEdge
		{
			get
			{
				return this.targetEdge;
			}
		}
	}

	/// <summary>
	/// Delegate that handles an edge that sends a vertex.
	/// </summary>
	public delegate void EdgeEdgeEventHandler(Object sender, EdgeEdgeEventArgs e);
}
