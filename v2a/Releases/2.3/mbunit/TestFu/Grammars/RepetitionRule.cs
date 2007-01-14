using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A <see cref="IRule"/> that executes repeatidely an inner <see cref="IRule"/>
	/// </summary>
	public class RepetitionRule : RuleBase
	{
		private IRandom random = new TestFu.Grammars.Random();
		private IRule rule;
		private int minOccurence;
		private int maxOccurence;
		
		/// <summary>
		/// Creates an instance that executes the rule between
		/// <paramref name="minOccurense"/> and <paramref name="maxOccurence"/>
		/// times.
		/// </summary>
		/// <param name="rule">
		/// <see cref="IRule"/> to repeat
		/// </param>
		/// <param name="minOccurence">
		/// Minimum number of occurence
		/// </param>
		/// <param name="maxOccurence">
		/// Maximum number of occurence
		/// </param>
		public RepetitionRule(IRule rule, int minOccurence, int maxOccurence)
			:base(false)
		{
			if (rule==null)
				throw new ArgumentNullException("rule");
			this.rule = rule;
			this.minOccurence=minOccurence;
			this.maxOccurence=maxOccurence;
			this.Name=this.ToEbnf();
		}

		/// <summary>
		/// Gets or sets the random generator used for selection repetition
		/// counts
		/// </summary>
		/// <value>
		/// The <see cref="IRandom"/> random generator.
		/// </value>
		/// <exception cref="ArgumentNullException">
		/// set property, value is a null reference
		/// </exception>
		public IRandom Random
		{
			get
			{
				return this.random;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("random");
				this.random=value;
			}
		}
		
		/// <summary>
		/// Gets the inner <see cref="IRule"/> instance
		/// </summary>
		/// <value>
		/// Repeated <see cref="IRule"/> instance.
		/// </value>
		public IRule Rule
		{
			get
			{
				return this.rule;
			}
		}
		
		/// <summary>
		/// Gets the minimum of rule execution
		/// </summary>
		/// <value>
		/// Minimum of rule execution
		/// </value>
		public int MinOccurence
		{
			get
			{
				return this.minOccurence;
			}
		}
		
		/// <summary>
		/// Gets the maximum of rule execution
		/// </summary>
		/// <value>
		/// Maximum of rule execution
		/// </value>
		public int MaxOccurence
		{
			get
			{
				return this.maxOccurence;
			}
		}
		
		/// <summary>
		/// Executes repeatidely the inner rule.
		/// </summary>
		/// <param name="token">
		/// Authorization token
		/// </param>
		public override void Produce(IProductionToken token)
		{
			int count = this.random.Next(this.minOccurence, this.maxOccurence);
			for(int i=0;i<count;++i)
			{
				this.Rule.Produce(token.Production.RequestToken(this.rule));
			}			
			this.OnAction();
		}

		/// <summary>
		/// Converts rule to EBNF like representation
		/// </summary>
		/// <returns>
		/// EBNF-like string representing the rule.
		/// </returns>
		public string ToEbnf()
		{
			if (minOccurence==0 && maxOccurence==1)
				return String.Format("{0}?",this.rule.Name);
			else if (minOccurence==1 && maxOccurence==int.MaxValue)
				return String.Format("{0}+",this.rule.Name);
			else if (minOccurence==0 && maxOccurence==int.MaxValue)
				return String.Format("{0}*",this.rule.Name);
			else if (minOccurence==1 && maxOccurence==int.MaxValue)
				return String.Format("{0}*",this.rule.Name);
			else
				return String.Format("{0}[{1},{2}]",this.rule.Name
					,this.minOccurence
					,this.maxOccurence);
		}
	}

}
