using System;

namespace QuickGraph.Algorithms.TestGames
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;

	/// <summary>
	/// Summary description for BoundedReachabilityGame.
	/// </summary>
	public class BoundedReachabilityGame
	{
		private ITestGraph testGraph = null;
		private IVertex root = null;
		private int moveCount = 0;
		private VertexCollection goals  = new VertexCollection();
		private IBoundedReachabilityGamePlayer tester = null;
		private IBoundedReachabilityGamePlayer implementationUnderTest = null;

		public ITestGraph TestGraph 
		{
			get
			{
				return this.testGraph;
			}
		}

		public IVertex Root 
		{
			get
			{
				return this.root;
			}
		}

		public int MoveCount 
		{
			get
			{
				return this.moveCount;
			}
		}

		public IVertexEnumerable Goals 
		{
			get
			{
				return this.goals;
			}
		}

		public IBoundedReachabilityGamePlayer Tester 
		{
			get
			{
				return this.tester;
			}
		}


		public IBoundedReachabilityGamePlayer ImplementationUnderTest 
		{
			get
			{
				return this.implementationUnderTest;
			}
		}

		public event EventHandler GameLost;

		protected void OnGameLost()
		{
			if(this.GameLost!=null)
				GameLost(this,new EventArgs());
		}
		public event EventHandler GameWon;
		protected void OnGameWon()
		{
			if(this.GameWon!=null)
				GameWon(this,new EventArgs());
		}

		public event EdgeEventHandler ChosenEdge;

		protected void OnChosenEdge(IEdge e)
		{
			if (this.ChosenEdge!=null)
				ChosenEdge(this,new EdgeEventArgs(e));
		}


		public void Play()
		{
			IVertex v = this.root;
			int k = 0;

			for(;k < this.moveCount;++k)
			{
				v = PlayOnce(v,k);
				if (v==null)
					break;
			}
		}

		public IVertex PlayOnce(IVertex v, int k)
		{
			if (this.testGraph.ContainsChoicePoint(v))
			{
				if (k>=this.moveCount)
				{
					OnGameLost();
					return null;
				}
				else
				{
					IEdge e = this.ImplementationUnderTest.ChooseEdge(v,k);
					if (e==null)
					{
						OnGameLost();
						return null;
					}
					OnChosenEdge(e);
					v = e.Target;
					++k;

					return v;
				}
			}
			else if (this.goals.Contains(v))
			{
				OnGameWon();
				return null;
			}
			else if(k >= this.moveCount)
			{
				OnGameLost();
				return null;
			}
			else
			{
				IEdge e = this.tester.ChooseEdge(v,k);
				if (e==null)
				{
					OnGameLost();
					return null;
				}

				OnChosenEdge(e);
				v=e.Target;
				k++;

				return v;
			}
		}
	}
}
