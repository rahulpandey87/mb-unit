using System;

namespace QuickGraph.Predicates
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Predicates;

	/// <summary>
	/// Not operator to predicate
	/// </summary>
	public class NotVertexPredicate : IVertexPredicate
	{
		private IVertexPredicate predicate;

		/// <summary>
		/// Constructs a new <see cref="NotVertexPredicate"/>.
		/// </summary>
		/// <param name="predicate">predicate to invert</param>
		public NotVertexPredicate(IVertexPredicate predicate)
		{
			if (predicate==null)
				throw new ArgumentNullException("predicate");
			this.predicate = predicate;
		}

		#region IVertexPredicate Members

		/// <summary>
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		public bool Test(IVertex v)
		{
			return !this.predicate.Test(v);
		}

		#endregion
	}
}
