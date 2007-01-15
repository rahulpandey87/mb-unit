using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A class that creates random values.
	/// </summary>
	public interface IRandom
	{
		/// <summary>
		/// Returns a nonnegative random number.
		/// </summary>
		/// <returns>
		/// A 32-bit signed integer greater than or equal to zero and less than 
		/// <see cref="int.MaxValue"/>.
		/// </returns>
		int Next();

		/// <summary>
		/// Returns a nonnegative random number less than the specified maximum.
		/// </summary>
		/// <param name="max"></param>
		/// <returns>
		/// A 32-bit signed integer greater than or equal to zero and less than 
		/// <paramref name="max"/>.
		/// </returns>
		int Next(int max);

		/// <summary>
		/// Returns a random number within a specified range.
		/// </summary>
		/// <param name="minValue">
		/// The lower bound of the random number returned. 
		/// </param>
		/// <param name="maxValue">
		/// The upper bound of the random number returned. 
		/// maxValue must be greater than or equal to minValue.
		/// </param>
		/// <returns>
		/// A 32-bit signed integer greater than or equal to minValue and less 
		/// than maxValue; that is, the range of return values includes 
		/// minValue but not MaxValue. If minValue equals maxValue, minValue 
		/// is returned.
		/// </returns>
		int Next(int minValue, int maxValue);

		/// <summary>
		/// Returns a random number between 0.0 and 1.0.
		/// </summary>
		/// <returns>
		/// A double-precision floating point number greater than or equal 
		/// to 0.0, and less than 1.0.
		/// </returns>
		double NextDouble();
	}
}
