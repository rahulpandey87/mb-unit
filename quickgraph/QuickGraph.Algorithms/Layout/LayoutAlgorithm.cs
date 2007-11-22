using System;
using System.Drawing;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;
	using QuickGraph.Collections;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Algorithms;

	public abstract class LayoutAlgorithm : ILayoutAlgorithm
	{
		private IVertexAndEdgeListGraph visitedGraph;
		private float edgeLength=20;
		private Size layoutSize = new Size(800,600);
		private IVertexPointFDictionary positions = new VertexPointFDictionary();
		private bool computationAbortion=false;
		
		public LayoutAlgorithm(IVertexAndEdgeListGraph visitedGraph)
		{
			this.visitedGraph=visitedGraph;
			this.positions = new VertexPointFDictionary();
		}
		public LayoutAlgorithm(IVertexAndEdgeListGraph visitedGraph,IVertexPointFDictionary positions)
		{
			this.visitedGraph=visitedGraph;
			this.positions = positions;
		}		
		
		public virtual void UpdateEdgeLength()
		{
			this.edgeLength = 0.5F*(float)Math.Sqrt((float)this.LayoutSize.Width*this.LayoutSize.Height
				/ this.VisitedGraph.VerticesCount);
		}
		
		public IVertexAndEdgeListGraph VisitedGraph
		{
			get
			{
				return this.visitedGraph;
			}
		}
		
		Object IAlgorithm.VisitedGraph
		{
			get
			{
				return this.VisitedGraph;
			}
		}
		
		public IVertexPointFDictionary Positions
		{
			get
			{
				return this.positions;	
			}
		}
		
		public float EdgeLength
		{
			get
			{
				return this.edgeLength;				
			}
			set
			{
				this.edgeLength=value;
			}
		}
		
		public Size LayoutSize
		{
			get
			{
				return this.layoutSize;
			}
			set
			{
				this.layoutSize=value;
			}
		}

		protected bool ComputationAbortion
		{
			get
			{
				return this.computationAbortion;
			}
		}

		public virtual void RequestComputationAbortion()
		{
			this.computationAbortion=true;
		}
		
		public event EventHandler PreCompute;
		protected virtual void OnPreCompute()
		{
			if (this.PreCompute!=null)
				this.PreCompute(this,EventArgs.Empty);
			this.computationAbortion=false;
		}

		public event EventHandler PostCompute;
		protected virtual void OnPostCompute()
		{
			if (this.PostCompute!=null)
				this.PostCompute(this,EventArgs.Empty);
		}
		
		public abstract void Compute();
	}
}
