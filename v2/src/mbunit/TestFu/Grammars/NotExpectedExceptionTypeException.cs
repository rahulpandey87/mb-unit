using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// Exception throwed when an exception is catched and is
	/// not from the expected type.
	/// </summary>
	public class NotExpectedExceptionTypeException : Exception
	{
		private Type expectedType;
		
		/// <summary>
		/// Creates an instance with the expected exception type
		/// and the actual exception.
		/// </summary>
		/// <param name="expectedType">
		/// Expected exception <see cref="Type"/>
		/// </param>
		/// <param name="innerException">
		/// Actual catch <see cref="Exception"/> instance
		/// </param>
		public NotExpectedExceptionTypeException(
			Type expectedType,
			Exception innerException)
			:base("",innerException)
		{
			this.expectedType=expectedType;
		}
	}
}
