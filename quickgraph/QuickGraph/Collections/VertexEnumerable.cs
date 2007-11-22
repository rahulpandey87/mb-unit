using System;
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// A wrapper class for weak collection of IVertex
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class works as a proxy for a weakly named collection of IVertex by
	/// implementing the IVertexEnumerable interface.
	/// </para>
	/// </remarks>
	/// <example>
	/// In this example, we show how to convert the key collection of
	/// a vertex dictionary to a stronly typed collection:
	/// <code>
	/// VertexStringDictionary names = new VertexStringDictionary();
	/// // adding names
	/// ...
	/// // this will fail names.Keys implements IEnumerable.
	/// IVertexEnumerable c = names.Keys;
	/// // wrapping the keys
	/// IVertexEnumrable c = new VertexEnumerable(names.Keys);
	/// </code>
	/// </example>
	public sealed class VertexEnumerable : IVertexEnumerable
	{
		private IEnumerable m_Enumerable;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="en">Wrapped enumerable</param>
		/// <exception cref="ArgumentNullException">en is null</exception>
		public VertexEnumerable(IEnumerable en)
		{
			if (en == null)
				throw new ArgumentNullException("enumerable");
			m_Enumerable = en;
		}

		/// <summary>
		/// Wraps up the weakly typed collection in a strongly typed (IVertex)
		/// collection.
		/// </summary>
		/// <param name="en">Collection to wrap</param>
		/// <returns>vertex enumerable collection</returns>
		/// <remarks>
		/// <para>
		/// The method is thread-safe.
		/// </para>
		/// </remarks>
		public static VertexEnumerable Wrap(IEnumerable en)
		{
			lock(typeof(VertexEnumerable))
			{
				return new VertexEnumerable(en);
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
		IVertexEnumerator IVertexEnumerable.GetEnumerator()
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
		public sealed class Enumerator : IVertexEnumerator
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
			/// Current vertex
			/// </summary>
			public IVertex Current
			{
				get
				{
					return (IVertex)Wrapped.Current;
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
