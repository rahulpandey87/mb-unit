using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Static helper class for creating rules.
	/// </summary>
	public sealed class Rules
	{
		#region Constructor
		private Rules()
		{}
		#endregion
		
		/// <summary>
		/// Creates an alternative of rules.
		/// </summary>
		/// <param name="rules">
		/// Set of rule to choose from alternatively.
		/// </param>
		/// <remarks>
		/// <code>
		/// [EBNF]
		/// rule := A | B | C
		/// 
		/// [C#]
		/// IRule rule = Rules.Alt(A,B,C);
		/// </code>
		/// </remarks>
		/// <returns>
		/// An <see cref="AlternativeRule"/> instance implementing
		/// the alternative rule choosing.
		/// </returns>
		public static AlternativeRule Alt(params IRule[] rules)
		{
			AlternativeRule alt = new AlternativeRule();
			foreach(IRule rule in rules)
			{
				alt.Rules.Add(rule);
			}
			return alt;
		}

		/// <summary>
		/// Creates a weighted alternative of rules.
		/// </summary>
		/// <param name="rules">
		/// Set of rule to choose from alternatively.
		/// </param>
		/// <remarks>
		/// <para>
		/// The <see cref="IRule.Weight"/> property of each rule is used to
		/// weight the probability to choose the rule.
		/// </para>
		/// <code>
		/// [EBNF]
		/// rule := A | B | C where A is chosen with P(A)=A.Weight / ABC.Weight
		/// and ABC.Weight = A.Weight + B.Weight + C.Weight
		/// 
		/// [C#]
		/// IRule rule = Rules.WeightedAlt(A,B,C);
		/// </code>
		/// </remarks>
		/// <returns>
		/// An <see cref="AlternativeRule"/> instance implementing
		/// the alternative rule choosing.
		/// </returns>
		public static AlternativeRule WeightedAlt(params IRule[] rules)
		{
			AlternativeRule alt = Alt(rules);
			alt.Selector = new WeightedRandomRuleSelector();
			return alt;
		}
		
		/// <summary>
		/// Creates a sequence of rules.
		/// </summary>
		/// <param name="rules">
		/// Set of rule to execute in sequence.
		/// </param>
		/// <remarks>
		/// <code>
		/// [EBNF]
		/// rule := A B C
		/// 
		/// [C#]
		/// IRule rule = Rules.Seq(A,B,C);
		/// </code>
		/// </remarks>
		/// <returns>
		/// An <see cref="SequenceRule"/> instance implementing
		/// the sequence of rules.
		/// </returns>
		public static SequenceRule Seq(params IRule[] rules)
		{
			SequenceRule seq = new SequenceRule();
			foreach(IRule rule in rules)
			{
				seq.Rules.Add(rule);
			}
			return seq;			
		}   
		
		/// <summary>
		/// Creates an optional rule.
		/// </summary>
		/// <param name="rule">
		/// Rule to execute optionaly.
		/// </param>
		/// <remarks>
		/// <code>
		/// [EBNF]
		/// rule := A?
		/// 
		/// [C#]
		/// IRule rule = Rules.Opt(A);
		/// </code>
		/// </remarks>
		/// <returns>
		/// An <see cref="RepetitionRule"/> instance implementing
		/// the ? operator.
		/// </returns>
		public static RepetitionRule Opt(IRule rule)
		{
			return Repetition(rule, 0, 1);
		}

		/// <summary>
		/// Creates a rule to be execute one or more times.
		/// </summary>
		/// <param name="rule">
		/// Rule to be executed.
		/// </param>
		/// <remarks>
		/// <code>
		/// [EBNF]
		/// rule := A+
		/// 
		/// [C#]
		/// IRule rule = Rules.Pos(A);
		/// </code>
		/// </remarks>
		/// <returns>
		/// An <see cref="RepetitionRule"/> instance implementing
		/// the + operator.
		/// </returns>
		public static RepetitionRule Pos(IRule rule)
		{
			return Repetition(rule, 1, int.MaxValue);
		}
		
		/// <summary>
		/// Creates a rule to be execute zero or more times.
		/// </summary>
		/// <param name="rule">
		/// Rule to be executed.
		/// </param>
		/// <remarks>
		/// <code>
		/// [EBNF]
		/// rule := A*
		/// 
		/// [C#]
		/// IRule rule = Rules.Kleene(A);
		/// </code>
		/// </remarks>
		/// <returns>
		/// An <see cref="RepetitionRule"/> instance implementing
		/// the * operator.
		/// </returns>
		public static RepetitionRule Kleene(IRule rule)
		{
			return Repetition(rule, 0, int.MaxValue);
		}
		
		/// <summary>
		/// Creates a rule to be execute between <paramref name="minOccurence"/>
		/// and <paramref name="maxOccurence"/> times.
		/// </summary>
		/// <param name="rule">
		/// Rule to be executed.
		/// </param>
		/// <remarks>
		/// <code>
		/// [EBNF]
		/// rule := A{m,n}
		/// 
		/// [C#]
		/// IRule rule = Rules.Repetition(A,m,n);
		/// </code>
		/// </remarks>
		/// <param name="minOccurence">
		/// minimum number of execution of <paramref name="rule"/>
		/// </param>
		/// <param name="maxOccurence">
		/// maximum number of execution of <paramref name="rule"/>
		/// </param>
		/// <returns>
		/// An <see cref="RepetitionRule"/> instance implementing
		/// the {m,n} operator.
		/// </returns>
		public static RepetitionRule Repetition(IRule rule, int minOccurence, int maxOccurence)
		{
			return new RepetitionRule(rule, minOccurence, maxOccurence);
		}
		
		/// <summary>
		/// Creates a <see cref="IRule"/> that executes an <see cref="EventHandler"/>.
		/// </summary>
		/// <param name="handler">
		/// <see cref="EventHandler"/> to execute
		/// </param>
		/// <returns>
		/// <see cref="EventHandlerRule"/> instance that contains <paramref name="handler"/>
		/// </returns>
		public static EventHandlerRule EventHandler(EventHandler handler)
		{
			return new EventHandlerRule(handler);
		}
		
		/// <summary>
        /// Creates a <see cref="IRule"/> that executes an <see cref="MethodInvoker"/>.
		/// </summary>
		/// <param name="del">
        /// <see cref="MethodInvoker"/> to execute
		/// </param>
		/// <returns>
        /// <see cref="MethodInvokerRule"/> instance that contains 
		/// <paramref name="del"/>
		/// </returns>
		public static MethodInvokerRule Method(MethodInvoker del)
		{
			return new MethodInvokerRule(del);
		}

		
		/// <summary>
		/// Creates a <see cref="IRule"/> that executes an <see cref="ProductionTokenDelegate"/>.
		/// </summary>
		/// <param name="del">
		/// <see cref="ProductionTokenDelegate"/> to execute
		/// </param>
		/// <returns>
		/// <see cref="ProductionTokenDelegateRule"/> instance that contains 
		/// <paramref name="del"/>
		/// </returns>
		public static ProductionTokenDelegateRule Method(ProductionTokenDelegate del)
		{
			return new ProductionTokenDelegateRule(del);
		}

		/// <summary>
		/// Guards the execution of a <see cref="IRule"/> from an expected
		/// <see cref="Exception"/> type.
		/// </summary>
		/// <param name="rule">
		/// <see cref="IRule"/> instance to guard.
		/// </param>
		/// <param name="exceptionType">
		/// Expected throwed exception when <paramref name="rule"/> is executed
		/// </param>
		/// <returns>
		/// A <see cref="GuardedRule"/> instance guarding <paramref name="rule"/>
		/// </returns>
		public static GuardedRule Guard(IRule rule, Type exceptionType)
		{
			return new GuardedRule(rule, exceptionType);
		}
		
		/// <summary>
		/// Creates a conditional rule with "if" rule.
		/// </summary>
		/// <param name="cond">
		/// Condition expression
		/// </param>
		/// <param name="rule">
		/// <see cref="IRule"/> to execute if condition is true.
		/// </param>
		/// <returns>
		/// A <see cref="ConditionalRule"/> implementing condition rule execution.
		/// </returns>
		public static ConditionalRule If(IPredicate cond, IRule rule)
		{
			return new ConditionalRule(cond,rule);
		}

		/// <summary>
		/// Creates a conditional rule with "if" rule and "else" rule.
		/// </summary>
		/// <param name="cond">
		/// Condition expression
		/// </param>
		/// <param name="rule">
		/// <see cref="IRule"/> to execute if condition is true.
		/// </param>
		/// <param name="elseRule">
		/// <see cref="IRule"/> to execute if condition is false.
		/// </param>
		/// <returns>
		/// A <see cref="ConditionalRule"/> implementing condition rule execution.
		/// </returns>
		public static ConditionalRule If(IPredicate cond, IRule rule, IRule elseRule)
		{
			return new ConditionalRule(cond,rule,elseRule);			
		}

		/// <summary>
		/// Creates a conditional rule with "if" rule.
		/// </summary>
		/// <param name="cond">
		/// Condition expression
		/// </param>
		/// <param name="rule">
		/// <see cref="IRule"/> to execute if condition is true.
		/// </param>
		/// <param name="elseRule">
		/// <see cref="IRule"/> to execute if condition is false.
		/// </param>
		/// <returns>
		/// A <see cref="ConditionalRule"/> implementing condition rule execution.
		/// </returns>
		public static ConditionalRule If(ConditionDelegate cond, IRule rule, IRule elseRule)
		{
			return new ConditionalRule(Predicates.If(cond),rule,elseRule);			
		}
		
		/// <summary>
		/// Creates a conditional rule with "if" rule and "else" rule.
		/// </summary>
		/// <param name="cond">
		/// Condition expression
		/// </param>
		/// <param name="rule">
		/// <see cref="IRule"/> to execute if condition is true.
		/// </param>
		/// <returns>
		/// A <see cref="ConditionalRule"/> implementing condition rule execution.
		/// </returns>
		public static ConditionalRule If(ConditionDelegate cond, IRule rule)
		{
			return new ConditionalRule(Predicates.If(cond),rule);			
		}
	}
}
