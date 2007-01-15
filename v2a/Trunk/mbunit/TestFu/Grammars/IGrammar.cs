using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A grammar containing a set of rules, a <see cref="StartRule"/>.
	/// </summary>
	public interface IGrammar : IRule
	{
		/// <summary>
		/// Gets or sets the starting rule.
		/// </summary>
		/// <value>
		/// The start <see cref="IRule"/>.
		/// </value>
		IRule StartRule {get;set;}		

		/// <summary>
		/// Launches a production.
		/// </summary>
		void Produce(Object seed);

		/// <summary>
		/// Raised when production is finished.
		/// </summary>
		event EventHandler ProductionFinished;
	}
	
}
