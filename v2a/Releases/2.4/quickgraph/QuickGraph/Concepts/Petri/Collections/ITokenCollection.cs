using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface ITokenCollection : ICollection
	{
		void Add(Object token);
		void AddRange(ITokenCollection tokens);
		void Remove(Object token);
		void RemoveAll(Object token);
		void RemoveRange(ITokenCollection tokens);
		bool Contains(Object token);
		void Clear();
	}
}
