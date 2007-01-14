using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A object that select a rule between a collection of rules.
	/// </summary>
	public interface IRuleSelector
	{
		/// <summary>
		/// Select a <see cref="IRule"/> from <paramref name="rules"/>
		/// </summary>
		/// <param name="rules">
		/// <see cref="IRule"/> array to select from
		/// </param>
		/// <returns>
		/// Select <see cref="IRule"/> instance
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="rules"/> is a null reference
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="rules"/> is empty
		/// </exception>
		IRule Select(params IRule[] rules);	

		/// <summary>
		/// Select a <see cref="IRule"/> from <paramref name="rules"/>
		/// </summary>
		/// <param name="rules">
		/// <see cref="IRule"/> collection to select from
		/// </param>
		/// <returns>
		/// Select <see cref="IRule"/> instance
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="rules"/> is a null reference
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="rules"/> is empty
		/// </exception>
		IRule Select(IRuleCollection rules);	
	}
}
