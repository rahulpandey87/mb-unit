using System;
using System.Drawing;
using QuickGraph;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Collections;

namespace QuickGraph.Algorithms.Layout
{
	/// <summary>
	/// This algorithm is based on the following paper:
	/// T. Fruchterman and E. Reingold. "Graph drawing by force-directed placement." Software Practice and Experience, 21(11):1129--1164, 1991.
	/// 
	/// Implemented by Arun Bhalla.
	/// </summary>
	[Obsolete("Use ForceDirectedLayoutAlgorithm instead")]
	public class FruchtermanReingoldLayoutAlgorithm
	{
		protected double k;
		private SizeF size;
		private double temperature;
		private IVertexAndEdgeListGraph graph;
		private VertexPointFDictionary pos;
		private VertexVector2DDictionary disp;
		private double c = 1.0;

		public FruchtermanReingoldLayoutAlgorithm(IVertexAndEdgeListGraph graph, SizeF size)
		{
			this.size = size;
			this.graph = graph;

			pos = new VertexPointFDictionary();
			disp = new VertexVector2DDictionary();
		}

		public IVertexAndEdgeListGraph VisitedGraph
		{
			get { return graph; }
		}

		public VertexPointFDictionary Positions
		{
			get { return pos; }
		}

		public double C
		{
			get { return c; }
			set { c = value; }
		}

		public void Compute()
		{
			Initialize();

			for (int i = 0; i < 50; i++)
			{
				Iterate();
			}
		}

		public void Iterate()
		{
			CalculateRepulsiveForces();
			CalculateAttractiveForces();
			DisplaceVertices();

			ReduceTemperature();
		}

		protected virtual double AttractiveForce(double distance)
		{
			return distance*distance/k;
		}

		protected virtual double RepulsiveForce(double distance)
		{
			return k*k/distance;
		}

		public void Initialize()
		{
			k = C * Math.Sqrt(size.Width*size.Height / graph.VerticesCount);

			Random random = new Random();
			foreach (IVertex v in graph.Vertices)
			{
				pos[v] = new PointF(
					(float)random.NextDouble() * size.Width, 
					(float)random.NextDouble() * size.Height
					);
			}
			temperature = size.Width*0.1;
		}

		protected void CalculateRepulsiveForces()
		{
			foreach (IVertex v in graph.Vertices)
			{
				disp[v] = new Vector2D();

				
				foreach (IVertex u in graph.Vertices)
				{
					if (u != v)
					{
						Vector2D delta = (Vector2D)pos[v] - (Vector2D)pos[u];
						while (delta.Norm() == 0) 
						{
							//The vertices are occupying the same location, so randomly repel
							Random random = new Random();
							delta = new Vector2D(
								random.NextDouble() * 2 - 1,
								random.NextDouble() * 2 - 1
								);
						}

						disp[v] += delta / delta.Norm() * RepulsiveForce(delta.Norm());
					}
				}
			}
		}

		protected void CalculateAttractiveForces()
		{
			foreach (IEdge e in graph.Edges)
			{
				Vector2D delta = (Vector2D)pos[e.Target] - (Vector2D)pos[e.Source];

				if (delta.Norm() > 0) 
				{
					disp[e.Target] -= delta/delta.Norm() * AttractiveForce(delta.Norm());
					disp[e.Source] += delta/delta.Norm() * AttractiveForce(delta.Norm());
				}
			}
		}

		protected void DisplaceVertices()
		{
			foreach (IVertex v in graph.Vertices)
			{
				PointF p = pos[v];
				if (disp[v].Norm() > 0) 
				{
					p = (PointF)(p + disp[v]/disp[v].Norm() * Math.Min(disp[v].Norm(), temperature));
				}
				p.X = Math.Min(size.Width, Math.Max(0, p.X));
				p.Y = Math.Min(size.Height, Math.Max(0, p.Y));
				pos[v] = p;
			}
		}

		protected void ReduceTemperature()
		{
			temperature *= 0.991;
		}
	}
}
