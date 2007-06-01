using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface IArcCollection : ICollection, IArcEnumerable
	{
		void Add(IArc arc);
		void Remove(IArc arc);
		bool Contains(IArc arc);
	}
}
