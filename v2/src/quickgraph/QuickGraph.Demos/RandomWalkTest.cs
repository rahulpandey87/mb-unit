using System;
using System.Collections;
using System.Drawing;

namespace QuickGraphTest
{
	using QuickGraph;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Modifications;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Algorithms.RandomWalks;
	using QuickGraph.Algorithms.Visitors;
	using QuickGraph.Collections;
	using QuickGraph.Algorithms.Graphviz;
	using QuickGraph.Representations;
	using NGraphviz.Helpers;

	/// <summary>
	/// Summary description for RandomWalkTest.
	/// </summary>
	public class RandomWalkTest
	{
		private SuccessorRecorderVisitor vis = new SuccessorRecorderVisitor();
		private IVertex root = null;
 
		private void InitializeVertex(Object sender, VertexEventArgs e)
		{
			Console.WriteLine("InitializeVertex: {0}",((NamedVertex)e.Vertex).Name);		
		}

		private void DiscoverVertex(Object sender, VertexEventArgs e)
		{
			Console.WriteLine("DiscoverVertex: {0}",((NamedVertex)e.Vertex).Name);		
		}

		private void FinishVertex(Object sender, VertexEventArgs e)
		{
			Console.WriteLine("FinishVertex: {0}",((NamedVertex)e.Vertex).Name);		
		}

		private void TreeEdge(Object sender, EdgeEventArgs e)
		{
			Console.WriteLine("TreeEdge: {0}->{1}", 
				((NamedVertex)e.Edge.Source).Name,
				((NamedVertex)e.Edge.Target).Name
				);
		}

		private void FormatVertex(Object sender, FormatVertexEventArgs e)
		{
			e.VertexFormatter.Label = ((NamedVertex)e.Vertex).Name;
			e.VertexFormatter.Shape = GraphvizVertexShape.Box;

			if (e.Vertex==this.root)
				e.VertexFormatter.StrokeColor = Color.Blue;
			else
				e.VertexFormatter.StrokeColor = Color.Black;
		}

		private void FormatEdge(Object sender, FormatEdgeEventArgs e)
		{
			IEdge edge = e.Edge;
			if (edge is ReversedEdge)
				edge = ((ReversedEdge)e.Edge).Wrapped;
			if (vis.Successors.ContainsValue(edge))
				e.EdgeFormatter.StrokeColor = Color.Red;
			else
				e.EdgeFormatter.StrokeColor = Color.Black;

			if (edge is NamedEdge)
			{
				e.EdgeFormatter.Label.Value = ((NamedEdge)edge).Name;
			}
			else
			{
				e.EdgeFormatter.Label.Value="";
			}
		}

		public void Test(IVertexAndEdgeListGraph g, IVertex root)
		{
			this.root = root; 

			RandomWalkAlgorithm walker = new RandomWalkAlgorithm(g);
			walker.TreeEdge +=new EdgeEventHandler(walker_TreeEdge);
			walker.Generate(root,50);

			BidirectionalAdaptorGraph bg = new BidirectionalAdaptorGraph(g);
			ReversedBidirectionalGraph rg = new ReversedBidirectionalGraph(bg);
			CyclePoppingRandomTreeAlgorithm pop = new CyclePoppingRandomTreeAlgorithm(rg);

			pop.InitializeVertex +=new VertexEventHandler(this.InitializeVertex);
			pop.FinishVertex +=new VertexEventHandler(this.FinishVertex);
			pop.TreeEdge += new EdgeEventHandler(this.TreeEdge);

			pop.InitializeVertex +=new VertexEventHandler(vis.InitializeVertex);
			pop.TreeEdge += new EdgeEventHandler(vis.TreeEdge);
			pop.ClearTreeVertex += new VertexEventHandler(vis.ClearTreeVertex);

			pop.RandomTreeWithRoot(root);

			// plot tree...
			GraphvizAlgorithm gv = new GraphvizAlgorithm(g,".",GraphvizImageType.Svg);
			gv.FormatEdge +=new FormatEdgeEventHandler(this.FormatEdge);
			gv.FormatVertex +=new FormatVertexEventHandler(this.FormatVertex);

			gv.Write("randomtree");
		}

		private void walker_TreeEdge(object sender, EdgeEventArgs e)
		{
			Console.WriteLine("[walker] {0}->{1}",e.Edge.Source,e.Edge.Target);
		}
	}
}
