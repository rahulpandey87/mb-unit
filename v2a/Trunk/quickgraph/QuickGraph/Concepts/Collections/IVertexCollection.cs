using System;

namespace QuickGraph.Concepts.Collections
{
	using System.Collections;
	/// <summary>
	/// A vertex enumerable collection
	/// </summary>
	public interface IVertexCollection : 
		ICollection,
		IVertexEnumerable
	{
		bool Contains(IVertex v);
	}
}
