using System;
using System.Collections;

namespace QuickGraph.Concepts.Petri.Collections
{
	public interface ITransitionEnumerator  : IEnumerator
	{
		new ITransition Current {get;}
	}
}
