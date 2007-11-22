using System;
using System.Collections;

namespace TestFu.Grammars
{
	/// <summary>
	/// An enumerator over <see cref="IRule"/> instance.
	/// </summary>
	public interface IRuleEnumerator : IEnumerator
	{
		/// <summary>
		/// Gets the current <see cref="IRule"/> instance
		/// </summary>
		/// <value>
		/// Current <see cref="IRule"/> instance.
		/// </value>
		new IRule Current {get;}
	}
}
