using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface ITransitionEnumerable : IEnumerable
	{
		new ITransitionEnumerator GetEnumerator();
	}
}
