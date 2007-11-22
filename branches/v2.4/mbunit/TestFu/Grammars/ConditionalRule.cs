using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// If then else rule fashion.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This rule uses the results of a <see cref="IPredicate"/>
	/// instance to select which <see cref="IRule"/> to execute:
	/// <code>
	/// if (predicate.Test(...))
	///    rule.Produce(...);
	/// else
	///    elseRule.Produce(...);
	/// </code>
	/// </para>
	/// </remarks>
	public class ConditionalRule : RuleBase
	{
		private IPredicate predicate;
		private IRule rule;
		private IRule elseRule;
		
		/// <summary>
		/// Creates a <see cref="ConditionalRule"/> with a 
		/// <see cref="IPredicate"/> instance and 
		/// <see cref="IRule"/> instance.
		/// </summary>
		/// <param name="predicate">
		/// <see cref="IPredicate"/> instance used for testing
		/// </param>
		/// <param name="rule">
		/// rule to execute.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="predicate"/> or <paramref name="rule"/>
		/// is a null reference.
		/// </exception>
		public ConditionalRule(IPredicate predicate, IRule rule)
			:this(predicate,rule,null)
		{
			
		}

		/// <summary>
		/// Creates a <see cref="ConditionalRule"/> with a 
		/// <see cref="IPredicate"/> instance and 
		/// <see cref="IRule"/> instance. If the predicate returns
		/// false, <paramref name="elseRule"/> is executed.
		/// </summary>
		/// <param name="predicate">
		/// <see cref="IPredicate"/> instance used for testing
		/// </param>
		/// <param name="rule">
		/// rule to execute.
		/// </param>
		/// <param name="elseRule">
		/// rule to execute if predicate is false.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="predicate"/> or <paramref name="rule"/>
		/// is a null reference.
		/// </exception>
		public ConditionalRule(IPredicate predicate, IRule rule, IRule elseRule)
			:base(false)
		{
			if (predicate==null)
				throw new ArgumentNullException("predicate");
			this.predicate=predicate;
			if (rule==null)
				throw new ArgumentNullException("rule");
			this.rule=rule;
			this.elseRule=elseRule;
		}
		

		/// <summary>
		/// Gets or sets the predicate for the condition.
		/// </summary>
		/// <value>
		/// <see cref="IPredicate"/> instance used for testing the condition.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference
		/// </exception>
		public IPredicate Predicate
		{
			get
			{
				return this.predicate;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("Predicate");
				this.predicate=value;
			}
		}

		/// <summary>
		/// Gets or sets the rule executed when the predicate is true
		/// </summary>
		/// <value>
		/// <see cref="IRule"/> instance executed when <see cref="Predicate"/>
		/// is true.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference
		/// </exception>
		public IRule Rule
		{
			get
			{
				return this.rule;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("Rule");
				this.rule=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the rule executed when the predicate is false
		/// </summary>
		/// <value>
		/// <see cref="IRule"/> instance executed when <see cref="Predicate"/>
		/// is false.
		/// </value>
		public IRule ElseRule
		{
			get
			{
				return this.elseRule;
			}
			set
			{
				this.elseRule=value;
			}
		}
		
		/// <summary>
		/// Executes one of the rules depending on the predicate result.
		/// </summary>
		/// <param name="token">
		/// A production token authorizing production.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="token"/> is a null reference (Nothing in Visual Basic)
		/// </exception>
		public override void Produce(IProductionToken token)
		{
			if (this.predicate.Test(token))
				this.Rule.Produce(token.Production.RequestToken(this.Rule));
			else
			{
				if (this.ElseRule!=null)
					this.ElseRule.Produce(token.Production.RequestToken(this.ElseRule));
			}
		}
	}	

}
