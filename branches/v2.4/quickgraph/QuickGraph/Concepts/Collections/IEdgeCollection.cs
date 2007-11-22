using System;

namespace QuickGraph.Concepts.Collections
{
	using System.Collections;
	/// <summary>
	/// An edge enumerable collection
	/// </summary>
	public interface IEdgeCollection : 
		ICollection,
		IEdgeEnumerable
	{
		bool Contains(IEdge e);
		IEdge this[int index]
		{
			get;
			set;
		}
	}
}
