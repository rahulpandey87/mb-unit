using System;
using System.Collections;

namespace TestFu.Grammars
{
	/// <summary>
	/// A collection of <see cref="IRule"/>.
	/// </summary>
	public interface IRuleCollection : ICollection
	{
		/// <summary>
		/// Gets an <see cref="IRuleEnumerator"/> instance of the rules.
		/// </summary>
		/// <returns>
		/// A valid <see cref="IRuleEnumerator"/> instance.
		/// </returns>
		new IRuleEnumerator GetEnumerator();			
	}
}
