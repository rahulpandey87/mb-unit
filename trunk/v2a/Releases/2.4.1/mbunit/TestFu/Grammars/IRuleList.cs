using System;

namespace TestFu.Grammars
{
	/// <summary>
	/// A list of <see cref="IRule"/>
	/// </summary>
	public interface IRuleList : IRuleCollection
	{
		/// <summary>
		/// Gets or sets the <see cref="IRule"/> at position <paramref name="index"/>.
		/// </summary>
		/// <param name="index">
		/// <see cref="IRule"/> index.
		/// </param>
		IRule this[int index]{get;set;}

		/// <summary>
		/// Adds a <see cref="IRule"/> to the list.
		/// </summary>
		/// <param name="rule">
		/// <see cref="IRule"/> to add
		/// </param>
		void Add(IRule rule);

		/// <summary>
		/// Inserts a <see cref="IRule"/> instance at position <paramref name="index"/>
		/// </summary>
		/// <param name="index">
		/// position to insert the rule
		/// </param>
		/// <param name="rule">
		/// <see cref="IRule"/> to insert
		/// </param>
		void Insert(int index, IRule rule);

		/// <summary>
		/// Removes the first occurence of <paramref name="rule"/>.
		/// </summary>
		/// <param name="rule">
		/// <see cref="IRule"/> to remove
		/// </param>
		void Remove(IRule rule);

		/// <summary>
		/// Gets a value indicating if <paramref name="rule"/> is in the
		/// list.
		/// </summary>
		/// <param name="rule">
		/// <see cref="IRule"/> to test.
		/// </param>
		/// <returns>
		/// true if <paramref name="rule"/> is in the list; otherwise, false.
		/// </returns>
		bool Contains(IRule rule);

		/// <summary>
		/// Clears the list.
		/// </summary>
		void Clear();		
	}
}
