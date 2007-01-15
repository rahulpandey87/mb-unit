using System;

namespace QuickGraph.Algorithms.Graphviz
{

	using QuickGraph.Concepts.Petri;
	using QuickGraph.Representations.Petri;

	using NGraphviz.Helpers;
	using System.Diagnostics;

	public class PetriNetRenderer
	{
		private GraphvizAlgorithm algo;
		public PetriNetRenderer(PetriGraph net)
		{
			this.algo=new GraphvizAlgorithm(net);
			this.algo.ImageType = GraphvizImageType.Png;
			this.algo.FormatVertex+=new FormatVertexEventHandler(algo_FormatVertex);
		}

		public GraphvizAlgorithm Algo
		{
			get
			{
				return this.algo;
			}
		}

		public void Render(string fileName)
		{
			Process.Start( this.algo.Write(fileName));
		}

		private void algo_FormatVertex(object sender, FormatVertexEventArgs e)
		{
			e.VertexFormatter.Label=e.Vertex.ToString();
			if (e.Vertex is IPlace)
			{
				e.VertexFormatter.Shape = GraphvizVertexShape.Circle;
			}
			else if (e.Vertex is Transition)
			{
				e.VertexFormatter.Shape = GraphvizVertexShape.Box;
			}
		}
	}
}
