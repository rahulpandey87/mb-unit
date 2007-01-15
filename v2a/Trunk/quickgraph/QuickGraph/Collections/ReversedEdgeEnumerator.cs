using System;
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Summary description for ReversedEdgeEnumerator.
	/// </summary>
	public sealed class ReversedEdgeEnumerator  : IEdgeEnumerator
	{
		private IEdgeEnumerator wrapped;
		public ReversedEdgeEnumerator(IEdgeEnumerator enumerator)
		{
			if (enumerator == null)
				throw new ArgumentNullException("enumerator");
			this.wrapped = enumerator;
		}
		#region IEdgeEnumerator Members

		public IEdge Current
		{
			get
			{
				return new ReversedEdge(this.wrapped.Current);
			}
		}

		#endregion

		#region IEnumerator Members

		public void Reset()
		{
			this.wrapped.Reset();
		}

		object System.Collections.IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		public bool MoveNext()
		{
			return this.wrapped.MoveNext();
		}

		#endregion
	}
}
