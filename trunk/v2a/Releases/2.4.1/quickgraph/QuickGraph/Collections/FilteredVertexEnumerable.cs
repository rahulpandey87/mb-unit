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
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Filtered vertex collectiohn
	/// </summary>
	/// <remarks>
	/// This colleciton is used to do filtered iteration.
	/// </remarks>
	public sealed class FilteredVertexEnumerable : IVertexEnumerable
	{
		private IVertexEnumerable m_VertexEnumerable;
		private IVertexPredicate m_VertexPredicate;

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="ec">base collection</param>
		/// <param name="ep">predicate</param>
		/// <exception cref="ArgumentNullException">ec or ep null</exception>
		public FilteredVertexEnumerable(IVertexEnumerable ec, IVertexPredicate ep)
		{
			if (ec == null)
				throw new ArgumentNullException("Vertex collection");
			if (ep == null)
				throw new ArgumentNullException("Vertex predicate");

			m_VertexEnumerable = ec;
			m_VertexPredicate = ep;
		}

		/// <summary>
		/// Base collection
		/// </summary>
		public IVertexEnumerable BaseEnumerable
		{
			get
			{
				return m_VertexEnumerable;
			}
		}

		/// <summary>
		/// Predicate
		/// </summary>
		public IVertexPredicate VertexPredicate
		{
			get
			{
				return m_VertexPredicate;
			}
		}
		
		/// <summary>
		/// Returns a filtered enumerator
		/// </summary>
		/// <returns>enumerator</returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(BaseEnumerable.GetEnumerator(), VertexPredicate);
		}

		/// <summary>
		/// IEnumerable implementation
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// IVertexEnumerable implementation
		/// </summary>
		IVertexEnumerator IVertexEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Filtered enumerator
		/// </summary>
		public sealed class Enumerator : IVertexEnumerator
		{
			private IVertexEnumerator m_Enumerator;
			private IVertexPredicate m_Predicate;

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="e">Base enumerator</param>
			/// <param name="p">predicate</param>
			public Enumerator(IVertexEnumerator e, IVertexPredicate p)
			{
				m_Enumerator = e;
				m_Predicate = p;
			}

			/// <summary>
			/// Current Vertex
			/// </summary>
			public IVertex Current
			{
				get
				{
					return m_Enumerator.Current;
				}
			}

			/// <summary>
			/// IEnumerator implementation
			/// </summary>
			Object IEnumerator.Current
			{
				get
				{
					return (Object)this.Current;	
				}
			}

			/// <summary>
			/// Positions the cursor before the first element.
			/// </summary>
			public void Reset()
			{
				m_Enumerator.Reset();
			}

			/// <summary>
			/// Moves the cursor to the next Vertex.
			/// </summary>
			/// <returns>True if successful, false if the iteration ended.</returns>
			public bool MoveNext()
			{
				bool ok;

				do 
				{
					ok = m_Enumerator.MoveNext();
					if (!ok)
						return false;
				} 
				while( !m_Predicate.Test(m_Enumerator.Current) );

				return true;
			}
		}
	}
}
