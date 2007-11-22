using System;

namespace QuickGraph.Concepts.Petri
{
	public interface IMutablePetriNet : IPetriNet
	{
		IPlace AddPlace(string name);
		ITransition AddTransition(string name);
		IArc AddArc(IPlace place, ITransition transition);
		IArc AddArc(ITransition transition,IPlace place);
	}
}
