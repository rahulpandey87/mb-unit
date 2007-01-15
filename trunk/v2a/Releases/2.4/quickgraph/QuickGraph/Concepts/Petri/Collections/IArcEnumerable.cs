using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface IArcEnumerable : IEnumerable
	{
		new IArcEnumerator GetEnumerator();
	}
}
