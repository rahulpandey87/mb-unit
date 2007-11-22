using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A <see cref="IRule"/> that choose from a set of sub-<see cref="IRule"/>.
	/// </summary>
	public class AlternativeRule : CollectionRule
	{
		private IRuleSelector selector = new RandomRuleSelector();
		
		/// <summary>
		/// Gets or sets the <see cref="IRuleSelector"/> instance
		/// </summary>
		/// <value>
		/// <see cref="IRuleSelector"/> instance.
		/// </value>
		public virtual IRuleSelector Selector
		{
			get
			{
				return this.selector;
			}
			set
			{
				this.selector=value;
			}
		}
		
		/// <summary>
		/// Choose a <see cref="IRule"/> and launch its production.
		/// </summary>
		/// <param name="token">
		/// Authorizing token
		/// </param>
		public override void Produce(IProductionToken token)
		{
			IRule r = this.Selector.Select(this.Rules);
			r.Produce(token.Production.RequestToken(r));
			this.OnAction();
		}
	}
}
