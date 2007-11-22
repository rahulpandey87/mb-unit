using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// An authorization to execute a production.
	/// </summary>
	public interface IProductionToken
	{
		/// <summary>
		/// Gets a value indicating if the production is authorized
		/// </summary>
		/// <value>
		/// true if authorized, otherwise false.
		/// </value>
		bool Authorized {get;}

		/// <summary>
		/// Gets the <see cref="IProduction"/> that emited the token.
		/// </summary>
		/// <value>
		/// The <see cref="IProduction"/> instance that emited the token.
		/// </value>
		IProduction Production {get;}
	}
}
