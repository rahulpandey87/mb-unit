using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Predicate that checks a given condition.
	/// </summary>
	public interface IPredicate
	{
		/// <summary>
		/// Checks a condition and returns result.
		/// </summary>
		/// <returns>
		/// Predicate result
		/// </returns>
		/// <param name="token">
		/// Current production token
		/// </param>
		bool Test(IProductionToken token);
	}
}
