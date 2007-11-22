using System;
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// A wrapper class for weak collection of IEdge
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class works as a proxy for a weakly named collection of IEdge by
	/// implementing the IEdgeEnumerable interface.
	/// </para>
	/// </remarks>
	/// <example>
	/// In this example, we show how to convert the value collection of
	/// a vertex dictionary to a stronly typed collection:
	/// <code>
	/// VertexEdgeDictionary names = new VertexEdgeDictionary();
	/// ...
	/// // this will fail names.Values implements IEnumerable.
	/// IEdgeEnumerable c = names.Values;
	/// // wrapping the values
	/// IEdgeEnumrable c = new EdgeEnumerable(names.Values);
	/// </code>
	/// </example>
	public sealed class EdgeEnumerable : IEdgeEnumerable
	{
		private IEnumerable m_Enumerable;

		/// <summary>
		/// Method
		/// </summary>
		/// <param name="en">Wrapped enumerable</param>
		/// <exception cref="ArgumentNullException">en is null</exception>
		public EdgeEnumerable(IEnumerable en)
		{
			if (en == null)
				throw new ArgumentNullException("enumerable");
			m_Enumerable = en;
		}

		/// <summary>
		/// Wraps up the weakly typed collection in a strongly typed (IEdge)
		/// collection.
		/// </summary>
		/// <param name="en">Collection to wrap</param>
		/// <returns>Edge enumerable collection</returns>
		/// <remarks>
		/// <para>
		/// The method is thread-safe.
		/// </para>
		/// </remarks>
		public static EdgeEnumerable Wrap(IEnumerable en)
		{
			lock(typeof(EdgeEnumerable))
			{
				return new EdgeEnumerable(en);
			}
		}

		/// <summary>
		/// Wrapped enumerable
		/// </summary>
		public IEnumerable Enumerable
		{
			get
			{
				return m_Enumerable;
			}
		}

		/// <summary>
		/// Return a strongly typed enumerator
		/// </summary>
		/// <returns>strongly typed enumerator</returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator(Enumerable.GetEnumerator());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IEdgeEnumerator IEdgeEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Strongly typed enumerator
		/// </summary>
		public class Enumerator : IEdgeEnumerator
		{
			private IEnumerator m_Enumerator;

			/// <summary>
			/// Builds a new enumrator
			/// </summary>
			/// <param name="e">wrapped enumerator</param>
			/// <exception cref="ArgumentNullException">e is null</exception>
			public Enumerator(IEnumerator e)
			{
				if (e==null)
					throw new ArgumentNullException("enumerator");
				m_Enumerator = e;
			}

			/// <summary>
			/// Wrapped enumerator
			/// </summary>
			public IEnumerator Wrapped
			{
				get
				{
					return m_Enumerator;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public void Reset()
			{
				Wrapped.Reset();
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public bool MoveNext()
			{
				return Wrapped.MoveNext();
			}

			/// <summary>
			/// Current Edge
			/// </summary>
			public IEdge Current
			{
				get
				{
					return (IEdge)Wrapped.Current;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			Object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}
		}
	}
}
