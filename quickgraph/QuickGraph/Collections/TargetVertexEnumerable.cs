using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts.Collections;

	public sealed class TargetVertexEnumerable : IVertexEnumerable
	{
		private IEdgeEnumerable edges;

		public TargetVertexEnumerable(IEdgeEnumerable edges)
		{
			if (edges==null)
				throw new ArgumentNullException("edges");
			this.edges = edges;
		}

		#region IVertexEnumerable Members

		public IVertexEnumerator GetEnumerator()
		{
			return new TargetVertexEnumerator(this.edges);
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
