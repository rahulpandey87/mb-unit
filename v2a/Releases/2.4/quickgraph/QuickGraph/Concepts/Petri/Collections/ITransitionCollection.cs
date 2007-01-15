using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface ITransitionCollection : ICollection, ITransitionEnumerable
	{
		void Add(ITransition transition);
		void Remove(ITransition transition);
		bool Contains(ITransition transition);
	}
}
