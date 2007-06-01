using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A factory for <see cref="IProduction"/> instances.
	/// </summary>
	public interface IProductionFactory
	{
		/// <summary>
		/// Creates a new <see cref="IProduction"/> instance.
		/// </summary>
		/// <returns>
		/// A valid <see cref="IProduction"/> instance.
		/// </returns>
		IProduction CreateProduction(Object seed);
	}
}
