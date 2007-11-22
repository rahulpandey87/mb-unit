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

namespace QuickGraph.Algorithms.Clone
{
	using QuickGraph.Concepts;
	/// <summary>
	/// Edge cloning event handler
	/// </summary>
	public delegate void CloneEdgeEventHandler(object sender, CloneEdgeEventArgs e);

	/// <summary>
	/// Edge cloning event argument
	/// </summary>
	public class CloneEdgeEventArgs : EventArgs
	{
		private IEdge original;
		private IEdge clone;

		/// <summary>
		/// Create a new Vertex cloning event argument
		/// </summary>
		/// <param name="original">original vertex</param>
		/// <param name="clone">clone vertex</param>
		/// <exception cref="ArgumentNullReference">
		/// <paramref name="original"/> or <paramref name="clone"/>
		/// is a null reference.
		/// </exception>
		public CloneEdgeEventArgs(IEdge original, IEdge clone)
		{
			if (original==null)
				throw new ArgumentNullException("original");
			if (clone==null)
				throw new ArgumentNullException("clone");

			this.original = original;
			this.clone = clone;
		}

		/// <summary>
		/// Original vertex
		/// </summary>
		public IEdge Original
		{
			get
			{
				return this.original;
			}
		}

		/// <summary>
		/// Clone vertex
		/// </summary>
		public IEdge Clone
		{
			get
			{
				return this.clone;
			}
		}
	}
}
