using System;
using System.Text.RegularExpressions;

namespace TestFu.Grammars
{
	/// <summary>
	/// A <see cref="IRule"/> that guard an inner <see cref="IRule"/> instance
	/// execution from a specific exceptionType.
	/// </summary>
	public class GuardedRule : IRule
	{
		private string name;
		private IRule rule;
		private Type exceptionType;
		private Regex messageRegex;

		/// <summary>
		/// Creates an instance with the guarded rule and the expected
		/// exception type.
		/// </summary>
		/// <param name="rule">
		/// Guarded <see cref="IRule"/> instance
		/// </param>
		/// <param name="exceptionType">
		/// Expected <see cref="Exception"/> type.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="rule"/> or <paramref name="exceptionType"/>
		/// is a null reference.
		/// </exception>
		public GuardedRule(IRule rule, Type exceptionType)
			:this(rule,exceptionType,null)
		{
		}

		/// <summary>
		/// Creates an instance with the guarded rule, the expected
		/// exception type and the regular expression to match the message.
		/// </summary>
		/// <param name="rule">
		/// Guarded <see cref="IRule"/> instance
		/// </param>
		/// <param name="exceptionType">
		/// Expected <see cref="Exception"/> type.
		/// </param>
		/// <param name="messageRegex">
		/// Regular expression used to match the exception message
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="rule"/> or <paramref name="exceptionType"/>
		/// is a null reference.
		/// </exception>
		public GuardedRule(IRule rule, Type exceptionType, Regex messageRegex)
		{
			if (rule==null)
				throw new ArgumentNullException("rule");
			if (exceptionType==null)
				throw new ArgumentNullException("exceptionType");

			this.rule=rule;
			this.exceptionType=exceptionType;
			this.messageRegex=messageRegex;

			this.name=String.Format("G({0})",rule.Name);
		}
		
		/// <summary>
		/// Semantic actions event
		/// </summary>		
		public event EventHandler Action;
		
		/// <summary>
		/// Raises the <see cref="Action"/> event.
		/// </summary>
		protected virtual void OnAction()
		{
			if (this.Action!=null)
				this.Action(this,EventArgs.Empty);
		}
		
		/// <summary>
		/// Gets or sets the regular expression to match the message.
		/// </summary>
		/// <value>
		/// The <see cref="Regex"/> instance used to mach the message.
		/// </value>
		/// <remarks>
		/// <para>
		/// If this property is set to null, no message matching is performed.
		/// </para>
		/// </remarks>
		public Regex MessageRegex
		{
			get
			{
				return this.messageRegex;
			}
			set
			{
				this.messageRegex=value;
			}
		}

		/// <summary>
		/// Gets or sets the rule name (for debugging purpose)
		/// </summary>
		/// <value>
		/// The rule name.
		/// </value>
		public string Name
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
		/// Gets or sets a value indicating the rule importance
		/// </summary>
		/// <value>
		/// Value indicating the rule importance
		/// </value>
		public double Weight
		{
			get
			{
				return this.rule.Weight;
			}
			set
			{
				this.rule.Weight=value;
			}
		}
		
		/// <summary>
		/// Gets a value indicating if the rule is terminal. 
		/// </summary>
		/// <value>
		/// Always returns true.
		/// </value>
		public bool Terminal
		{
			get
			{
				return false;
			}
		}
		
		/// <summary>
		/// Executes the inner <see cref="IRule"/> and guards for
		/// a particular exception type.
		/// </summary>
		/// <param name="token">
		/// Authorization token
		/// </param>
		public void Produce(IProductionToken token)
		{
			try
			{
				this.rule.Produce(token.Production.RequestToken(this.rule));
			}
			catch(ProductionException)
			{
				return;
			}
			catch(Exception ex)
			{
				// check type
				if (!this.exceptionType.IsAssignableFrom(ex.GetType()))
					throw new NotExpectedExceptionTypeException(exceptionType,ex);
				// check message
				if (this.messageRegex!=null)
				{
					if (!this.messageRegex.IsMatch(ex.Message))
						throw new NotExpectedMessageException(messageRegex,ex);
				}
			}
			this.OnAction();
		}
	}	
}
