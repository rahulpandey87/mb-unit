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


namespace QuickGraph.Collections
{
	using System;
	using System.Collections;
	using QuickGraph.Concepts;

	/// <summary>
	/// A vertex buffer that acts like a stack.
	/// </summary>
	public class VertexBuffer : IEnumerable
	{
		private ArrayList buffer;

		/// <summary>
		/// Constructs an empty buffer
		/// </summary>
		public VertexBuffer()
		{
			this.buffer = new ArrayList();
		}

		/// <summary>
		/// The number of vertices in the buffer
		/// </summary>
		public int Count
		{
			get
			{
				return this.buffer.Count;
			}
		}

		/// <summary>
		/// Returns the latest vertex in the buffer. Leaves it in the buffer.
		/// </summary>
		/// <returns>Latest vertex</returns>
		public virtual IVertex Peek()
		{
			if (this.buffer.Count==0)
				throw new ArgumentOutOfRangeException();

			return (IVertex)this.buffer[0];
		}

		/// <summary>
		/// Pushes a new vertex at the end of the buffer:
		/// </summary>
		/// <param name="v">Vertex to push</param>
		public virtual void Push(IVertex v)
		{
			this.buffer.Add(v);
		}

		/// <summary>
		/// Removes the latest vertex.
		/// </summary>
		public virtual IVertex Pop()
		{
			IVertex v = (IVertex)this.buffer[0];
			this.buffer.RemoveAt(0);
			return v;
		}

		/// <summary>
		/// Sorts the buffer using the comparer
		/// </summary>
		/// <param name="comparer">Comparer used to sort the buffer</param>
		public virtual void Sort(IComparer comparer)
		{
			this.buffer.Sort(0,this.buffer.Count,comparer);
		}

		/// <summary>
		/// Returns an enumerator over the buffer
		/// </summary>
		/// <returns>Buffer enumerator</returns>
		public IEnumerator GetEnumerator()
		{
			return this.buffer.GetEnumerator();
		}
	}
}
