namespace QuickGraphTest
{
	using System;
	using System.Drawing;

	using QuickGraph.Algorithms;
	using QuickGraph.Algorithms.Graphviz;
	using QuickGraph.Collections;

	/// <summary>
	/// Description résumée de TestGraphvizVertex.
	/// </summary>
	public class TestGraphvizVertex
	{
		private GraphvizVertex m_Vertex;
		private GraphvizEdge m_Edge;
		private VertexStringDictionary m_Names;
		public TestGraphvizVertex(VertexStringDictionary names)
		{
			m_Names = names;

			m_Vertex = new GraphvizVertex();
			m_Vertex.Shape = GraphvizVertexShape.Hexagon;
			m_Vertex.Style = GraphvizVertexStyle.Filled;
			m_Vertex.ToolTip="This is a tooltip";
			m_Vertex.Url="http://www.dotnetwiki.org";
			m_Vertex.StrokeColor = Color.Red;
			m_Vertex.FillColor = Color.LightGray;
			m_Vertex.FontColor = Color.Blue;
			m_Vertex.Orientation = 45;
			m_Vertex.Regular = true;
			m_Vertex.Sides = 2;

			m_Edge = new GraphvizEdge();
			m_Edge.HeadArrow = new GraphvizArrow(GraphvizArrowShape.Box);
			m_Edge.HeadArrow.Clipping=GraphvizArrowClipping.Left;
			m_Edge.HeadArrow.Filling = GraphvizArrowFilling.Open;

			m_Edge.TailArrow = new GraphvizArrow(GraphvizArrowShape.Dot);

			m_Edge.Label.Value = "This is an edge";
		}

		public void WriteVertex(Object sender, VertexEventArgs args)
		{
			GraphvizWriterAlgorithm algo = (GraphvizWriterAlgorithm)sender;
			m_Vertex.Label = m_Names[args.Vertex];

			algo.Output.Write(m_Vertex.ToDot());
		}

		public void WriteEdge(Object sender, EdgeEventArgs args)
		{
			GraphvizWriterAlgorithm algo = (GraphvizWriterAlgorithm)sender;

			algo.Output.Write(m_Edge.ToDot());
		}
	}
}
