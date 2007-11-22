using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A production rule
	/// </summary>
	/// <remarks>
	/// <para>
	/// A <see cref="IRule"/> instance is used to execute a production.
	/// </para>
	/// </remarks>
	public interface IRule
	{	
		/// <summary>
		/// Gets or sets a value indicating the rule importance
		/// </summary>
		/// <value>
		/// Value indicating the rule importance
		/// </value>
		/// <exception cref="ArgumentException">
		/// set property, value is negative.
		/// </exception>
		double Weight {get;set;}

		/// <summary>
		/// Semantic actions event.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Semantic action are triggered upon each successful rule execution.
		/// </para>
		/// </remarks>
		event EventHandler Action;

		/// <summary>
		/// Gets a value indicating if the rule is terminal
		/// </summary>
		/// <value>
		/// true if the rule is terminal; otherwise, false.
		/// </value>
		bool Terminal {get;}

		/// <summary>
		/// Gets or sets the rule name (for debugging purpose)
		/// </summary>
		/// <value>
		/// The rule name.
		/// </value>
		string Name {get;set;}

		/// <summary>
		/// Executes the production using the rule.
		/// </summary>
		/// <param name="token">
		/// A production token authorizing production.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="token"/> is a null reference (Nothing in Visual Basic)
		/// </exception>
		void Produce(IProductionToken token);
	}
}
