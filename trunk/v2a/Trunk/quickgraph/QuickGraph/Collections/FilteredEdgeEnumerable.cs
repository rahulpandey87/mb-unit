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
	/// Description résumée de FilteredEdgeCollection.
	/// </summary>
	public sealed class FilteredEdgeEnumerable: IEdgeEnumerable
	{
		private IEdgeEnumerable m_EdgeCollection;
		private IEdgePredicate m_EdgePredicate;

		/// <summary>
		/// Filtered edge collection
		/// </summary>
		/// <param name="ec">base collection</param>
		/// <param name="ep">filtering predicate</param>
		public FilteredEdgeEnumerable(IEdgeEnumerable ec, IEdgePredicate ep)
		{
			if (ec == null)
				throw new ArgumentNullException("edge collection");
			if (ep == null)
				throw new ArgumentNullException("edge predicate");

			m_EdgeCollection = ec;
			m_EdgePredicate = ep;
		}

		/// <summary>
		/// Base collection
		/// </summary>
		public IEdgeEnumerable BaseCollection
		{
			get
			{
				return m_EdgeCollection;
			}
		}

		/// <summary>
		/// Edge predicate
		/// </summary>
		public IEdgePredicate EdgePredicate
		{
			get
			{
				return m_EdgePredicate;
			}
		}

		/// <summary>
		/// Returns the enumerator
		/// </summary>
		/// <returns></returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(BaseCollection.GetEnumerator(), EdgePredicate);
		}

		/// <summary>
		/// IEnumerable implementation
		/// </summary>
		IEdgeEnumerator IEdgeEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Filetred enumerator class
		/// </summary>
		public sealed class  Enumerator : IEdgeEnumerator
		{
			private IEdgeEnumerator m_Enumerator;
			private IEdgePredicate m_Predicate;

			/// <summary>
			/// 
			/// </summary>
			/// <param name="e"></param>
			/// <param name="p"></param>
			public Enumerator(IEdgeEnumerator e, IEdgePredicate p)
			{
				m_Enumerator = e;
				m_Predicate = p;
			}

			/// <summary>
			/// Current edge
			/// </summary>
			public IEdge Current
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
			/// Moves the cursor to the next in-edge.
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
