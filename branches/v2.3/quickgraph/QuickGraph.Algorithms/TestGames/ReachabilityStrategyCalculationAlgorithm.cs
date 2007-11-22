using System;

namespace QuickGraph.Algorithms.TestGames
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Predicates;
	using QuickGraph.Collections;


	/// <summary>
	/// Summary description for ReachabilityStrategyCalculationAlgorithm.
	/// </summary>
	public class ReachabilityStrategyCalculationAlgorithm
	{
		private VertexCollection front = null;
		private VertexCollection newFront = null;
		private VertexDoublesDictionary probs = null;
		private VertexDoublesDictionary costs = null;

		private ITestGraph testGraph;
		private VertexCollection goals = new VertexCollection();
		private IStrategy strategy;
		private IPerformanceComparer performanceComparer;

		public ReachabilityStrategyCalculationAlgorithm(
			ITestGraph testGraph,
			IStrategy strategy
			)
		{
			if (this.testGraph==null)
				throw new ArgumentNullException("testGraph");

			this.testGraph = testGraph;
			this.strategy = new Strategy();
			this.performanceComparer = new PairPreOrderPerformanceComparer();
		}

		public ITestGraph TestGraph
		{
			get
			{
				return this.testGraph;
			}
		}

		public VertexCollection Goals
		{
			get
			{
				return this.goals;
			}
		}

		public IPerformanceComparer PerformanceComparer
		{
			get
			{
				return this.performanceComparer;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("performanceComparer");
				this.performanceComparer=value;
			}
		}

		public IVertexEnumerable NotGoals
		{
			get
			{
				return new FilteredVertexEnumerable(
					this.testGraph.Graph.Vertices,
					Preds.Not(Preds.InCollection(this.goals))
					);
			}
		}

		public IStrategy Strategy
		{
			get
			{
				return this.strategy;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("strategy");
				this.strategy = value;
			}
		}

		private IEdgeEnumerable EdgesWidthTargetInFront
		{
			get
			{
				
				return new FilteredEdgeEnumerable(
					this.TestGraph.Graph.Edges,
					Preds.OutEdge(Preds.KeepAllEdges(),Preds.InCollection(this.front))
					);
			}
		}

		public void Calculate(int moveCount)
		{
			Initialize();

			for(int i = 1;i<moveCount;++i)
			{
				PropagateChanges(i);

				foreach(IEdge e in EdgesWidthTargetInFront )
				{
					TraverseEdge(e,i);
				}

				front = newFront;
				newFront = new VertexCollection();
			}
		}

		protected void Initialize()
		{
			this.front = new VertexCollection();
			this.front.AddRange(this.goals);
			this.newFront = new VertexCollection();
			this.probs = new VertexDoublesDictionary();
			this.costs = new VertexDoublesDictionary();

			foreach(IVertex v in this.TestGraph.Graph.Vertices)
			{
				// setting probs
				DoubleCollection col =new DoubleCollection();
				if (this.Goals.Contains(v))
				{
					col.Add(0);
				}
				else
				{
					col.Add(1);
				}

				this.probs.Add(v,col);

				// setting costs
				col = new DoubleCollection();
				col.Add(0);
				this.costs.Add(v,col);
			}

			foreach(IVertex v in this.TestGraph.States)
			{
				this.Strategy.SetChooseEdge(v,0,null);
			}
		}

		protected void PropagateChanges(int i)
		{
			foreach(IVertex v in this.TestGraph.Graph.Vertices)
			{
				PropagateLast(this.probs[v]);
				PropagateLast(this.costs[v]);
			}
			foreach(IVertex v in this.TestGraph.States)
			{
				this.Strategy.SetChooseEdge(
					v,i,
					this.Strategy.ChooseEdge(v,i-1)
					);
			}
		}

		private void PropagateLast(DoubleCollection col)
		{
			col.Add(col[col.Count-1]);
		}

		protected void TraverseEdge(IEdge e, int i)
		{
			IVertex u = e.Source;
			IVertex v = e.Target;

			if (this.TestGraph.ContainsChoicePoint(u))
			{
				if (this.newFront.Contains(u))
					return;

				double pr = 0;
				double cr = 0;
				foreach(IEdge d in this.TestGraph.Graph.OutEdges(u))
				{
					pr += this.TestGraph.Prob(d)*this.probs[d.Target][i-1];
					cr += this.TestGraph.Cost(d)+this.costs[d.Target][i-1];
				}
				this.probs[u].Add(pr);
				this.costs[u].Add(cr);

				this.newFront.Add(u);
			}
			else if (Improving(e,i))
			{
				this.Strategy.SetChooseEdge(u,i,e);
				this.probs[u].Add(this.probs[v][i-1]);
				this.costs[u].Add(
					this.TestGraph.Cost(e) + this.costs[v][i-1]
					);
				this.newFront.Add(u);
			}
		}

		protected bool Improving(IEdge e, int i)
		{
			// e = (u,v)
			return this.PerformanceComparer.Compare(
				this.probs[e.Target][i-1], // Pr(v,i-1)
				this.TestGraph.Cost(e) + this.costs[e.Target][i-1], //cost(e)+C(v,i-1)
				this.probs[e.Source][i], //Pr(u,i)
				this.costs[e.Source][i] // C(u,i)
				);
		}
	}
}
