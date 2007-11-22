using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Abstract rule containing other rules.
	/// </summary>
	public abstract class CollectionRule : RuleBase
	{
		private RuleList rules = new RuleList();
		
		/// <summary>
		/// Creates an empty instance.
		/// </summary>
		public CollectionRule()
			:base(false)
		{}
		
		/// <summary>
		/// Gets the list of rules stored in the rule.
		/// </summary>
		/// <value>
		/// <see cref="IRuleList"/> containing the child rules.
		/// </value>
		public IRuleList Rules
		{
			get
			{
				return this.rules;
			}
		}
	}
}
