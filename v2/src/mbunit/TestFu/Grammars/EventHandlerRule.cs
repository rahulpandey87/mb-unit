using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A <see cref="IRule"/> that wraps a <see cref="EventHandler"/>
	/// call.
	/// </summary>
	public class EventHandlerRule : RuleBase
	{
		private EventHandler handler;

		/// <summary>
		/// Creates an instance with a <see cref="EventHandler"/>
		/// attached.
		/// </summary>
		/// <param name="handler">
		/// Handler to attach
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="handler"/> is a null reference
		/// </exception>
		public EventHandlerRule(EventHandler handler)
			:base(true)
		{
			if (handler==null)
				throw new ArgumentNullException("handler");
			this.handler=handler;
		}

		/// <summary>
		/// Invokes handler.
		/// </summary>
		/// <param name="token"></param>
		public override void Produce(IProductionToken token)
		{
			this.handler(this, EventArgs.Empty);
			this.OnAction();
		}
	}
}
