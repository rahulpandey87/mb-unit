using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A production done by a grammar and its set of rules.
	/// </summary>
	public interface IProduction 
	{
		/// <summary>
		/// Gets the seed that created the production
		/// </summary>
		/// <value>
		/// Seed used to create the production
		/// </value>
		Object Seed {get;}

		/// <summary>
		/// Processes the request for a <see cref="IProductionToken"/>
		/// done by a rule and returns the token or throws.
		/// </summary>
		/// <param name="rule">
		/// <see cref="IRule"/> instance that requests the token.
		/// </param>
		/// <returns>
		/// A valid <see cref="IProductionToken"/> instance.
		/// </returns>
		/// <exception cref="ProductionException">
		/// The request was defined using the internal production
		/// logic.
		/// </exception>
		IProductionToken RequestToken(IRule rule);
	}
}
