using System;

namespace QuickGraph.Algorithms.TestGames
{
	using QuickGraph.Concepts;
	using QuickGraph.Collections;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Predicates;

	/// <summary>
	/// Optimal winning strategy calculation algorithm.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This algorithm is an implementation of the 
	/// http://research.microsoft.com/users/schulte/Papers/OptimalStrategiesForTestingNondeterminsticSystems(ISSTA2004).pdf
	/// </para>
	/// </remarks>
	public class OptimalWinningStrategyCalculationAlgorithm
	{
		private ITestGraph testGraph;
		private VertexCollection goals = new VertexCollection();

		private VertexIntDictionary unvisitedSuccessorCounts = null;
		private VertexDoubleDictionary costs = null;
		private PriorithizedVertexBuffer priorityQueue = null;
		private VertexEdgeDictionary states = new VertexEdgeDictionary();

		public OptimalWinningStrategyCalculationAlgorithm(
			ITestGraph testGraph)
		{
			if (testGraph==null)
				throw new ArgumentNullException("testGraph");
			this.testGraph = testGraph;
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

		public VertexEdgeDictionary States
		{
			get
			{
				return this.states;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("states");
				this.states = value;
			}
		}

		protected IVertexEnumerable NotGoals
		{
			get
			{
				return new FilteredVertexEnumerable(
					this.testGraph.Graph.Vertices,
					Preds.Not(Preds.InCollection(this.goals))
					);
			}
		}

		public void Calculate()
		{
			this.Initialize();
			this.CalculateWinningStrategy();
		}

		protected void Initialize()
		{
			this.costs =new VertexDoubleDictionary();
			this.priorityQueue = new PriorithizedVertexBuffer(costs);
			this.unvisitedSuccessorCounts = new VertexIntDictionary();
			this.states.Clear();

			foreach(IVertex v in this.goals)
			{
				this.costs.Add(v,0);
				this.priorityQueue.Push(v);
			}

			foreach(IVertex v in this.NotGoals)
			{
				this.costs.Add(v,double.PositiveInfinity);
			}				

			foreach(IVertex v in this.TestGraph.ChoicePoints)
			{
				this.unvisitedSuccessorCounts.Add(v, this.testGraph.Graph.OutDegree(v));
			}

			foreach(IVertex v in this.TestGraph.Graph.Vertices)
			{
				this.states.Add(v,null);
			}
		}

		protected void CalculateWinningStrategy()
		{
			while(this.priorityQueue.Count != 0)
			{
				IVertex v = this.priorityQueue.Pop();
				Relax(v);
			}
		}

		protected void Relax(IVertex v)
		{
			foreach(IEdge e in this.testGraph.Graph.InEdges(v))
			{
				IVertex u = e.Source;
				if (this.testGraph.ContainsChoicePoint(u))
				{
					this.unvisitedSuccessorCounts[u]--;
					if (this.unvisitedSuccessorCounts.Count!=0)
					{
						double cu = 0;
						foreach(IEdge d in this.testGraph.Graph.OutEdges(u))
						{
							cu = Math.Max(
								cu,
								this.TestGraph.Cost(d) + this.costs[d.Target]
								);
						}
						this.costs[u]=cu;

						this.priorityQueue.Push(u);
					}
				}
				else 
				{
					double cev = this.TestGraph.Cost(e) + this.costs[v];
					if (this.costs[u] > cev)
					{
						this.costs[u] = cev;
						this.priorityQueue.Push(u);
						this.states[u]=e;
					}
				}
			}

		}
	}
}
