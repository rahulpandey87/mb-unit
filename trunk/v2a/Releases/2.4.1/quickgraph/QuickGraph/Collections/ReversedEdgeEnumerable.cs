using System;
using System.Collections;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Summary description for ReversedEdgeEnumerable.
	/// </summary>
	public sealed class ReversedEdgeEnumerable : IEdgeEnumerable
	{
		private IEdgeEnumerable edges;
		public ReversedEdgeEnumerable(IEdgeEnumerable edges)
		{
			this.edges = edges;
		}

		public ReversedEdgeEnumerator GetEnumerator()
		{
			return new ReversedEdgeEnumerator(this.edges.GetEnumerator());
		}

		IEdgeEnumerator IEdgeEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
