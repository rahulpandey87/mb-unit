using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface IPlaceEnumerable : IEnumerable
	{
		new IPlaceEnumerator GetEnumerator();
	}
}
