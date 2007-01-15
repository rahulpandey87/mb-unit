using System;
using System.Collections;
using QuickGraph.Collections;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Algorithms;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Concepts.Collections;
using QuickGraph.Collections.Filtered;
using QuickGraph.Predicates;

namespace QuickGraph.Algorithms.Ranking
{
	/// <summary>
	/// Algorithm that computes the PageRank ranking over a graph.
	/// </summary>
	/// <remarks>
	/// <para>
	/// <b>PageRank</b> is a method that initially designed to rank web pages
	/// objectively and mechanically. In fact, it is one of the building block 
	/// of the famous Google search engine.
	/// </para>
	/// <para>
	/// The idea behind PageRank is simple and intuitive: pages that are important are referenced
	/// by other important pages. There is an important literature on the web that explains 
	/// PageRank: http://www-db.stanford.edu/~backrub/google.html, 
	/// http://www.webworkshop.net/pagerank.html, 
	/// http://www.miswebdesign.com/resources/articles/pagerank-2.html
	/// </para>
	/// <para>
	/// The PageRank is computed by using the following iterative formula:
	/// <code>
	/// PR(A) = (1-d) + d (PR(T1)/C(T1) + ... + PR(Tn)/C(Tn)) 
	/// </code>
	/// where <c>PR</c> is the PageRank, <c>d</c> is a damping factor usually set to 0.85,
	/// <c>C(v)</c> is the number of out edgesof <c>v</c>.
	/// </para>
	/// </remarks>
	public class PageRankAlgorithm : IAlgorithm
	{
		private IBidirectionalVertexListGraph visitedGraph;
		private VertexDoubleDictionary ranks = new VertexDoubleDictionary();
		
		private int maxIterations = 60;
		private double tolerance = 2*double.Epsilon;
		private double damping = 0.85;
		
		/// <summary>
		/// Creates a PageRank algorithm around the visited graph
		/// </summary>
		/// <param name="visitedGraph">
		/// Visited <see cref="IBidirectionalVertexListGraph"/> instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <param name="visitedGraph"/> is a null reference (Nothing in Visual Basic).
		/// </exception>
		public PageRankAlgorithm(IBidirectionalVertexListGraph visitedGraph)
		{
			if (visitedGraph==null)
				throw new ArgumentNullException("visitedGraph");
			this.visitedGraph = visitedGraph;
		}

		/// <summary>
		/// Gets the visited graph
		/// </summary>
		/// <value>
		/// A <see cref="IVertexListGraph"/> instance
		/// </value>
		public IBidirectionalVertexListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}

		/// <summary>
		/// Gets the page rank dictionary
		/// </summary>
		/// <value>
		/// The <see cref="VertexDoubleDictionary"/> of <see cref="IVertex"/> - rank entries.ank entries.
		/// </value>
		public VertexDoubleDictionary Ranks
		{
			get
			{
				return this.ranks;
			}
		}
		
		/// <summary>
		/// Gets or sets the damping factor in the PageRank iteration.
		/// </summary>
		/// <value>
		/// Damping factor in the PageRank formula (<c>d</c>).
		/// </value>
		public double Damping
		{
			get
			{
				return this.damping;
			}
			set
			{
				this.damping =value;
			}
		}
		
		/// <summary>
		/// Gets or sets the tolerance to stop iteration
		/// </summary>
		/// <value>
		/// The tolerance to stop iteration.
		/// </value>
		public double Tolerance
		{
			get
			{
				return this.tolerance;
			}
			set
			{
				this.tolerance = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the maximum number of iterations
		/// </summary>
		/// <value>
		/// The maximum number of iteration.
		/// </value>
		public int MaxIteration
		{
			get
			{
				return this.maxIterations;	
			}
			set
			{
				this.maxIterations = value;
			}
		}

		#region IAlgorithm Members
		object IAlgorithm.VisitedGraph
		{
			get
			{
				return this.VisitedGraph;
			}
		}
		#endregion
		
		/// <summary>
		/// Initializes the rank map.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This method clears the rank map and populates it with rank to one for all vertices.
		/// </para>
		/// </remarks>
		public void InitializeRanks()
		{
			this.ranks.Clear();
			
			foreach(IVertex v in this.VisitedGraph.Vertices)
			{
				this.ranks.Add(v,0);	
			}			
			
			this.RemoveDanglingLinks();
		}	
		
		/// <summary>
		/// Iteratively removes the dangling links from the rank map
		/// </summary>
		public void RemoveDanglingLinks()
		{
			VertexCollection danglings = new VertexCollection();
			do
			{
				danglings.Clear();
				
				// create filtered graph
				IVertexListGraph fg = new FilteredVertexListGraph(
					this.VisitedGraph,
					new InDictionaryVertexPredicate(this.ranks)
					);
				
				// iterate over of the vertices in the rank map
				foreach(IVertex v in this.ranks.Keys)
				{
					// if v does not have out-edge in the filtered graph, remove
					if ( fg.OutDegree(v) == 0)
						danglings.Add(v);
				}
				
				// remove from ranks
				foreach(IVertex v in danglings)
					this.ranks.Remove(v);
				// iterate until no dangling was removed
			}while(danglings.Count != 0);
		}
		
		/// <summary>
		/// Computes the PageRank over the <see cref="VisitedGraph"/>.
		/// </summary>
		public void Compute()
		{
			VertexDoubleDictionary tempRanks = new VertexDoubleDictionary();
			// create filtered graph
			FilteredBidirectionalGraph fg = new FilteredBidirectionalGraph(
				this.VisitedGraph,
				Preds.KeepAllEdges(),
				new InDictionaryVertexPredicate(this.ranks)
				);			
			
			int iter = 0;
			double error = 0;
			do
			{
				// compute page ranks
				error = 0;
				foreach(DictionaryEntry de in this.Ranks)	
				{
					IVertex v = (IVertex)de.Key;
					double rank = (double)de.Value;
					// compute ARi
					double r = 0;
					foreach(IEdge e in fg.InEdges(v))
					{
						r += this.ranks[e.Source] / fg.OutDegree(e.Source);
					}
					
					// add sourceRank and store
					double newRank = (1-this.damping) + this.damping * r;
					tempRanks[v] = newRank;
					// compute deviation
					error += Math.Abs(rank - newRank);
				}				
				
				// swap ranks
				VertexDoubleDictionary temp = ranks;
				ranks = tempRanks;
				tempRanks = temp;				
				
				iter++;
			}while( error > this.tolerance && iter < this.maxIterations);
			Console.WriteLine("{0}, {1}",iter,error);
		}
		
		public double GetRanksSum()
		{
			double sum = 0;
			foreach(double rank in this.ranks.Values)
			{
				sum+=rank;
			}
			return sum;
		}
		
		public double GetRanksMean()
		{
			return GetRanksSum()/this.ranks.Count;
		}
		
		private static void swapRanks(VertexDoubleDictionary left, VertexDoubleDictionary right)
		{
			VertexDoubleDictionary temp = left;
			left = right;
			right = temp;
		}
	}
}
