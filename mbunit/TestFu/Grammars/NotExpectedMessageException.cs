using System;
using System.Text.RegularExpressions;

namespace TestFu.Grammars
{
	/// <summary>
	/// Exception throwed when an exception message does not match
	/// with the message regular expression
	/// </summary>
	public class NotExpectedMessageException : Exception
	{
		private Regex messageRegex;

		/// <summary>
		/// Creates an instance with the message regular expression and
		/// the actual catched exception.
		/// </summary>
		/// <param name="messageRegex">
		/// The <see cref="Regex"/> instance used to match the message
		/// </param>
		/// <param name="innerException">
		/// The actual <see cref="Exception"/> instance.
		/// </param>
		public NotExpectedMessageException(Regex messageRegex, Exception innerException)
			:base("",innerException)
		{
			this.messageRegex=messageRegex;
		}

		/// <summary>
		/// Gets the <see cref="Regex"/> instance used to match the exception message
		/// </summary>
		/// <value>
		/// <see cref="Regex"/> message matcher.
		/// </value>
		public Regex MessageRegex
		{
			get
			{
				return this.messageRegex;
			}
		}
	}
}
