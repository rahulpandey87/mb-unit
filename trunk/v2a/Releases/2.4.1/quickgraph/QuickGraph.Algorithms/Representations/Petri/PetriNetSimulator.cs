using System;
using System.Collections;

namespace QuickGraph.Representations.Petri
{
	using QuickGraph.Concepts.Petri;
	using QuickGraph.Concepts.Petri.Collections;
	using QuickGraph.Representations.Petri.Collections;

	public class PetriNetSimulator
	{
		private IPetriNet net;
		private Hashtable transitionBuffers = new Hashtable();
		private Hashtable transitionEnabled = new Hashtable();

		public PetriNetSimulator(IPetriNet net)
		{
			this.net = net;
		}

		public IPetriNet Net
		{
			get
			{
				return this.net;
			}
		}

		public void Initialize()
		{
			this.transitionBuffers.Clear();
			this.transitionEnabled.Clear();
			foreach(ITransition tr in this.Net.Transitions)
			{
				this.transitionBuffers.Add(tr, new TokenCollection());
				this.transitionEnabled.Add(tr,false);
			}
		}
		public void SimulateStep()
		{
			// first step, iterate over arc and gather tokens in transitions
			foreach(IArc arc in this.net.Arcs)
			{
				if(!arc.IsInputArc)
					continue;
				ITokenCollection tokens = (ITokenCollection)this.transitionBuffers[arc.Transition];
				// get annotated tokens
				ITokenCollection annotatedTokens = arc.Annotation.Eval(arc.Place.Marking);
				//add annontated tokens
				tokens.AddRange(annotatedTokens);
			}

			// second step, see which transition was enabled
			foreach(ITransition tr in this.Net.Transitions)
			{
				// get buffered tokens
				ITokenCollection tokens = (ITokenCollection)this.transitionBuffers[tr];
				// check if enabled, store value
				this.transitionEnabled[tr]=tr.Condition.IsEnabled(tokens);        
			}

			// third step, iterate over the arcs
			foreach(IArc arc in this.Net.Arcs)
			{
				if (!(bool)this.transitionEnabled[arc.Transition])
					continue;

				if(arc.IsInputArc)
				{
					// get annotated tokens
					ITokenCollection annotatedTokens = arc.Annotation.Eval(arc.Place.Marking);
					// remove annotated comments from source place
					arc.Place.Marking.RemoveRange(annotatedTokens);
				}
				else
				{
					ITokenCollection tokens = (ITokenCollection)this.transitionBuffers[arc.Transition];
					// get annotated tokens
					ITokenCollection annotatedTokens = arc.Annotation.Eval(tokens);
					// adding annotated comments to target place
					arc.Place.Marking.AddRange(annotatedTokens);
				}
			}
			// step four, clear buffers
			foreach(ITransition tr in this.Net.Transitions)
			{
				((ITokenCollection)this.transitionBuffers[tr]).Clear();
				this.transitionEnabled[tr]=false;
			}
		}
	}
}
