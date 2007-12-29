using System;
using System.Drawing;
using System.Collections;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Concepts.Traversals;

	public class ForceDirectedLayoutAlgorithm : 
		LayoutAlgorithm, 
		IForceDirectedLayoutAlgorithm
	{
		private ILayoutAlgorithm preLayoutAlgorithm=null;
		private IPotential potential=null;
		private int currentIteration = 0;
		private int maxIteration = 500;
		private VertexPointFDictionary potentials=new VertexPointFDictionary();
		
		private float maxMovement =50;
		private float heat = 1;
		private float heatDecayRate=0.99F;

		private Object syncRoot = null;
		
		public ForceDirectedLayoutAlgorithm(
			IVertexAndEdgeListGraph visitedGraph,
			IPotential potential,
			ILayoutAlgorithm preLayoutAlgorithm
			)
			:base(visitedGraph)
		{
			this.preLayoutAlgorithm=preLayoutAlgorithm;
			this.potential=potential;
			this.potentials=new VertexPointFDictionary();
		}
		
		public ForceDirectedLayoutAlgorithm(
			IVertexAndEdgeListGraph visitedGraph
			)
			:base(visitedGraph)
		{
			this.preLayoutAlgorithm=new RandomLayoutAlgorithm(visitedGraph,this.Positions);
			this.potential=new DirectedForcePotential(this);
			this.potentials=new VertexPointFDictionary();
		}
		
		public Object SyncRoot
		{
			get
			{
				return this.syncRoot;
			}
			set
			{
				this.syncRoot=value;
			}
		}

		public ILayoutAlgorithm PreLayoutAlgorithm 
		{
			get
			{
				return this.preLayoutAlgorithm;
			}
			set
			{
				this.preLayoutAlgorithm=value;
			}
		}
		
		public IPotential Potential
		{
			get
			{
				return this.potential;
			}
			set
			{
				this.potential=value;
			}
		}
		
		public override void UpdateEdgeLength()
		{
			base.UpdateEdgeLength();
			this.PreLayoutAlgorithm.UpdateEdgeLength();
			this.maxMovement=this.EdgeLength/2;
		}
		
		public int MaxIteration
		{
			get
			{
				return this.maxIteration;
			}
			set
			{
				this.maxIteration=value;
			}
		}
		
		public int CurrentIteration
		{
			get
			{
				return this.currentIteration;
			}
		}

		public event EventHandler PreIteration;
		protected virtual void OnPreIteration()
		{
			if (this.PreIteration!=null)
				this.PreIteration(this, EventArgs.Empty);
		}

		public event EventHandler PostIteration;
		protected virtual void OnPostIteration()
		{
			if (this.PostIteration!=null)
				this.PostIteration(this, EventArgs.Empty);
		}

		public override void Compute()
		{
			this.OnPreCompute();
			// prelayout
			this.PreLayoutAlgorithm.Compute();
			
			this.heat=1;
			// iterate layout
			for(	this.currentIteration=0;
					currentIteration<this.MaxIteration
					&& !this.ComputationAbortion;
					++currentIteration)
			{
				lock(this.SyncRoot)
				{
					this.Iterate();	
				}
				this.heat*=this.heatDecayRate;
			}			
			this.OnPostCompute();
		}
		
		public virtual void Iterate()
		{						
			this.OnPreIteration();
			
			//clear potentials
			foreach(IVertex v in this.VisitedGraph.Vertices)
				this.potentials[v]=new PointF();

			// compute potentials	
			this.Potential.Compute(this.potentials);
			
			// update position
			foreach(DictionaryEntry de in this.potentials)
			{
				IVertex v = (IVertex)de.Key;
				PointF p = (PointF)de.Value;
				this.Positions[v]=PointMath.ScaleSaturate(this.Positions[v],
					this.potentials[v],
					this.heat,
					this.maxMovement);
			}
			
			this.OnPostIteration();
		}
	}
}
