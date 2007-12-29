using System;

namespace QuickGraph.Concepts.Collections
{
	using System.Collections;
	/// <summary>
	/// A collection of <see cref="IPort"/> instance
	/// </summary>
	public interface IPortCollection : 
		ICollection,
		IPortEnumerable
	{
		bool Contains(IPort p);
	}
}
