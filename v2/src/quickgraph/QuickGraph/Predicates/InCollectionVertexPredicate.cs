using System;
using System.Collections;

namespace QuickGraph.Predicates
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Concepts.Collections;

	/// <summary>
	/// Predicate for checking that a vertex is in a collection
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class InCollectionVertexPredicate : IVertexPredicate
	{
		private IVertexCollection list;

		/// <summary>
		/// Creates a predicate that checks if vertices are in
		/// <paramref name="list"/>
		/// </summary>
		/// <param name="list">list of vertices</param>
		/// <exception cref="ArgumentNullException">list is a null reference</exception>
		public InCollectionVertexPredicate(IVertexCollection list)
		{
			if (list == null)
				throw new ArgumentNullException("list");
			this.list = list;
		}

		#region IVertexPredicate Members
		/// <summary>
		/// Gets a value indicating if <paramref name="v"/>
		/// is in the collection.
		/// </summary>
		/// <param name="v">vertex to test</param>
		/// <returns>true if <paramref name="v"/> is in the collection,
		/// false otherwize
		/// </returns>
		public bool Test(IVertex v)
		{
			return this.list.Contains(v);
		}
		#endregion
	}
}
