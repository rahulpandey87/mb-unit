using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Abstract rule class
	/// </summary>
	public abstract class RuleBase : IRule
	{
		private bool terminal;
		private double weight=1;
		private string name="";
		
		/// <summary>
		/// Creates an empty rule
		/// </summary>
		/// <param name="terminal">
		/// true if the rule is terminal; otherwise, false.
		/// </param>
        public RuleBase(bool terminal)
        {
			this.terminal=terminal;			
		}

		/// <summary>
		/// Gets a value indicating if the rule is terminal
		/// </summary>
		/// <value>
		/// true if the rule is terminal; otherwise, false.
		/// </value>
		public virtual bool Terminal
		{
			get
			{
				return this.terminal;
			}
		}
		
		/// <summary>
		/// Gets or sets the rule name
		/// </summary>
		/// <value>
		/// The rule name
		/// </value>
		public virtual String Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the rule weight
		/// </summary>
		/// <value>
		/// The rule weight
		/// </value>
		/// <exception cref="ArgumentException">
		/// set property, weight is negative
		/// </exception>
		public virtual double Weight
		{
			get
			{
				return this.weight;
			}
			set
			{
				if (this.weight<0)
					throw new ArgumentException("Weight cannot be negative");
				this.weight=value;
			}
		}
		
		/// <summary>
		/// Semantic action event.
		/// </summary>
		public event EventHandler Action;

		/// <summary>
		/// Raises the <see cref="Action"/> event.
		/// </summary>
		protected virtual void OnAction()
		{
			if (this.Action!=null)
				this.Action(this, EventArgs.Empty);
		}

		/// <summary>
		/// Executes the production using the rule (abstract class).
		/// </summary>
		/// <param name="token">
		/// A production token authorizing production.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="token"/> is a null reference (Nothing in Visual Basic)
		/// </exception>
		public abstract void Produce(IProductionToken token);		
	}	

}
