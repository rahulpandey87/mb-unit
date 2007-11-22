using System;

namespace QuickGraph.Algorithms.AllShortestPath.Reducers
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	public class FloydWarshallPredecessorRecorderReducer
	{
		private IVertexPredecessorMatrix predecessors;
	
		public FloydWarshallPredecessorRecorderReducer(IVertexPredecessorMatrix predecessors)
		{
			if (predecessors==null)
				throw new ArgumentNullException(@"predecessors");
		
			this.predecessors=predecessors;
		}
	
		public IVertexPredecessorMatrix Predecessors
		{
			get
			{
				return this.predecessors;
			}
		}
	
		public void ReducePath(Object sender, FloydWarshallEventArgs args)
		{
			Predecessors.SetPredecessor(
				args.Source,
				args.Target,
				Predecessors.Predecessor(args.Intermediate,args.Target)
				);
		}
	}
}
