using System;

namespace QuickGraph.Collections
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Summary description for AdjacentVertexEnumerator.
	/// </summary>
	public sealed class TargetVertexEnumerator : IVertexEnumerator
	{
		private IEdgeEnumerator enumerator;

		public TargetVertexEnumerator(IEdgeEnumerable edges)
		{
			if (edges==null)
				throw new ArgumentNullException("edges");
			this.enumerator = edges.GetEnumerator();
		}
		#region IVertexEnumerator Members

		public IVertex Current
		{
			get
			{
				return this.enumerator.Current.Target;
			}
		}

		#endregion

		#region IEnumerator Members

		public void Reset()
		{
			this.enumerator.Reset();
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
			return this.enumerator.MoveNext();
		}

		#endregion
	}
}
