using System;
using System.Drawing;

namespace QuickGraph.Algorithms.Layout
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Collections;

	public class DirectedForcePotential : Potential
	{
		public DirectedForcePotential(IIteratedLayoutAlgorithm algorithm)
			:base(algorithm)
		{}
		
		public override void Compute(IVertexPointFDictionary potentials)
		{		
			// in-edges
			int i=0;
			foreach(IVertex u in this.Algorithm.VisitedGraph.Vertices)
			{
				PointF pu = this.Algorithm.Positions[u];				
					
				int j=0;
				foreach(IVertex v in this.Algorithm.VisitedGraph.Vertices)
				{
					if (j<=i)
					{
						++j;
						continue;
					}
					
					PointF pv = this.Algorithm.Positions[v];
					PointF a = PointMath.Sub(pv,pu);
					
					double duv = PointMath.Distance(pu,pv);
				
					// added repulsion
					double fr = this.RepulsionForce(duv);
					potentials[u] = PointMath.Combili(potentials[u],-checked(fr/duv),a);
					potentials[v] = PointMath.Combili(potentials[v],checked(fr/duv),a);
					
					if (this.Algorithm.VisitedGraph.ContainsEdge(u,v)
						|| this.Algorithm.VisitedGraph.ContainsEdge(v,u))
					{
						double fa = this.AttractionForce(duv);
						potentials[u] = PointMath.Combili(potentials[u],checked(fa/duv),a);
						potentials[v] = PointMath.Combili(potentials[v],-checked(fa/duv),a);
					}										
					++j;
				}
				++i;
			}
		}

		protected virtual double RepulsionForce(double distance)
		{
			return checked(this.Algorithm.EdgeLength * this.Algorithm.EdgeLength/distance);
		}

		protected virtual double AttractionForce(double distance)
		{
			return distance*distance/this.Algorithm.EdgeLength;
		}
	}
}
