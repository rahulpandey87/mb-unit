using System;

namespace QuickGraphTest
{
	using NGraphviz.Helpers;
	using QuickGraph;
	using QuickGraph.Providers;
	using QuickGraph.Representations;
	using QuickGraph.Concepts;
	using QuickGraph.Algorithms.Graphviz;

	/// <summary>
	/// Summary description for ClusteredGraphTest.
	/// </summary>
	public class ClusteredGraphTest
	{
		private void FormatVertex(Object sender, FormatVertexEventArgs args)
		{
			args.VertexFormatter.Label = (args.Vertex as NamedVertex).Name;
		}
		private void FormatEdge(Object sender, FormatEdgeEventArgs args)
		{
			args.EdgeFormatter.Label.Value = (args.Edge as NamedEdge).Name;
		}

		public void Run()
		{
			// create an adjacency graph
			AdjacencyGraph g = new AdjacencyGraph(
				new NamedVertexProvider(),
				new NamedEdgeProvider(),
				true
				);

			// create a clustered graph
			ClusteredAdjacencyGraph cg = new ClusteredAdjacencyGraph(g);

			// adding some vertices to the main graph
			NamedVertex a = cg.AddVertex() as NamedVertex; a.Name="a";
			NamedVertex b = cg.AddVertex() as NamedVertex; b.Name="b";
			NamedVertex c = cg.AddVertex() as NamedVertex; c.Name="c";

			NamedEdge ab = cg.AddEdge(a,b) as NamedEdge; ab.Name="ab";
			NamedEdge ac = cg.AddEdge(a,c) as NamedEdge; ac.Name="ac";

			// adding a cluster
			ClusteredAdjacencyGraph cg1 = cg.AddCluster();
			// adding vertices and edges to the cluster
			NamedVertex d = cg1.AddVertex() as NamedVertex; d.Name="d";
			NamedVertex e = cg1.AddVertex() as NamedVertex; e.Name="e";
			NamedVertex f = cg1.AddVertex() as NamedVertex; f.Name="f";
			NamedEdge de = cg1.AddEdge(d,e) as NamedEdge; de.Name="de";
			NamedEdge df = cg1.AddEdge(d,f) as NamedEdge; df.Name="df";

			// adding a second cluster
			ClusteredAdjacencyGraph cg2 = cg.AddCluster();
			// adding vertices
			NamedVertex h = cg2.AddVertex() as NamedVertex; h.Name="h";
			NamedVertex i = cg2.AddVertex() as NamedVertex; i.Name="i";
			NamedEdge hi = cg2.AddEdge(h,i) as NamedEdge; hi.Name="hi";

			// adding a sub-sub-cluster
			ClusteredAdjacencyGraph cg21 = cg2.AddCluster();
			// adding vertices
			NamedVertex k = cg21.AddVertex() as NamedVertex; k.Name="k";
			NamedVertex l = cg21.AddVertex() as NamedVertex; l.Name="l";
			NamedEdge ak = cg.AddEdge(a,k) as NamedEdge; ak.Name="ak";
			NamedEdge kl = cg21.AddEdge(k,l) as NamedEdge; kl.Name="kl";


			// interconnecting
			NamedEdge cd = cg.AddEdge(c,d) as NamedEdge; cd.Name="cd";
			NamedEdge bh = cg.AddEdge(b,h) as NamedEdge; bh.Name="bh";
			NamedEdge ei = cg.AddEdge(e,i) as NamedEdge; ei.Name="ei";

			GraphvizAlgorithm gw = new GraphvizAlgorithm(
				cg,
				"../cluster",
				GraphvizImageType.Svgz
				);
			gw.FormatVertex += new FormatVertexEventHandler(this.FormatVertex);
			gw.FormatEdge += new FormatEdgeEventHandler(this.FormatEdge);
			gw.Write("cluster");

			cg2.Colapsed = true;
			gw.Write("cluster_collapsed");

		}
	}
}
