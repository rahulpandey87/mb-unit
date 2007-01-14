using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A sequence of rules.
	/// </summary>
	public class SequenceRule : CollectionRule
	{
		/// <summary>
		/// Executes sub-rule production in sequence.
		/// </summary>
		/// <param name="token">
		/// <see cref="IProductionToken"/> to authorize production.
		/// </param>
		public override void Produce(IProductionToken token)
		{
			foreach(IRule r in this.Rules)
			{
				r.Produce(token.Production.RequestToken(r));
			}			
			this.OnAction();
		}
	}
}
