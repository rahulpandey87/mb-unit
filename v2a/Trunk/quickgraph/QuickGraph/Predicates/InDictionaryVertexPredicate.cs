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
	public class InDictionaryVertexPredicate : IVertexPredicate
	{
		private IDictionary dictionary;

		/// <summary>
		/// Creates a predicate that checks if vertices are in
		/// <paramref name="list"/>
		/// </summary>
		/// <param name="dictionary">dictionary of vertices</param>
		/// <exception cref="ArgumentNullException">dictionary is a null reference</exception>
		public InDictionaryVertexPredicate(IDictionary dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException("list");
			this.dictionary = dictionary;
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
			return this.dictionary.Contains(v);
		}
		#endregion
	}
}
