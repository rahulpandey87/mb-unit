using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface IPlaceCollection : ICollection, IPlaceEnumerable
	{
		void Add(IPlace place);
		void Remove(IPlace place);
		bool Contains(IPlace place);
	}
}
