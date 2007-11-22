using System;
using System.Collections;
using System.IO;

namespace QuickGraph.Representations.Petri
{
  
	using QuickGraph.Concepts.Petri;
	using QuickGraph.Concepts;

	public class PetriNet : IMutablePetriNet
	{
		private int version = 0;
		private ArrayList places = new ArrayList();
		private ArrayList transitions = new ArrayList();
		private ArrayList arcs = new ArrayList();
		private PetriGraph graph = new PetriGraph(true);      

		//    private Hashtable inputArcs = new Hashtable();
		//    private Hashtable outputArcs = new Hashtable();

		public PetriNet()
		{}

		public PetriGraph Graph
		{
			get
			{
				return this.graph;
			}
		}

		#region IMutablePetriNet
		public IPlace AddPlace(string name)
		{
			IPlace p = new Place(version++,name);
			this.places.Add(p);
			this.graph.AddVertex(p);
			return p;
		}
		public ITransition AddTransition(string name)
		{
			ITransition tr = new Transition(version++,name);
			this.transitions.Add(tr);
			this.graph.AddVertex(tr);
			return tr;
		}
		public IArc AddArc(IPlace place, ITransition transition)
		{
			IArc arc=new Arc(version++,place,transition);
			this.arcs.Add(arc);
			this.graph.AddEdge(arc);
			return arc;
		}
		public IArc AddArc(ITransition transition,IPlace place)
		{
			IArc arc=new Arc(version++,transition,place);
			this.arcs.Add(arc);
			this.graph.AddEdge(transition,place);
			return arc;
		}
		#endregion

		#region IPetriNet Members

		public IList Places
		{
			get
			{
				// TODO:  Add PetriNet.Places getter implementation
				return this.places;
			}
		}

		public IList Transitions
		{
			get
			{
				// TODO:  Add PetriNet.Transitions getter implementation
				return this.transitions;
			}
		}

		public IList Arcs
		{
			get
			{
				// TODO:  Add PetriNet.Arcs getter implementation
				return this.arcs;
			}
		}

		#endregion

		public override string ToString()
		{
			StringWriter sw = new StringWriter();
			sw.WriteLine("-----------------------------------------------");
			sw.WriteLine("Places ({0})",this.places.Count);
			foreach(Place place in this.places)
			{
				sw.WriteLine("\t{0}",place.ToStringWithMarking());
			}

			sw.WriteLine("Transitions ({0})",this.transitions.Count);
			foreach(ITransition transition in this.transitions)
			{
				sw.WriteLine("\t{0}",transition);
			}

			sw.WriteLine("Arcs");
			foreach(IArc arc in this.arcs)
			{
				sw.WriteLine("\t{0}",arc);
			}
			return sw.ToString();
		}

	}
}
