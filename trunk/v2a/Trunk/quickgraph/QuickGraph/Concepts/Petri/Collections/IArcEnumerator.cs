using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface IArcEnumerator  : IEnumerator
	{
		new IArc Current {get;}
	}
}
