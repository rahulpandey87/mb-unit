using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface IPlaceEnumerator  : IEnumerator
	{
		new IPlace Current {get;}
	}
}
