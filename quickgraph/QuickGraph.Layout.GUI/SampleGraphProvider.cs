using System;
using QuickGraph.Concepts;
using QuickGraph;
using QuickGraph.Providers;
using QuickGraph.Representations;

namespace QuickGraph.Layout.GUI
{
	/// <summary>
	/// Summary description for SampleGraphProvider.
	/// </summary>
	public sealed class SampleGraphProvider
	{
		private SampleGraphProvider()
		{}

		public static AdjacencyGraph FileDependency()
		{
			// create a new adjacency graph
			AdjacencyGraph g = new AdjacencyGraph(
				new NamedVertexProvider(),
				new NamedEdgeProvider(),
				true
				);

			// adding files and storing names
			NamedVertex zig_cpp = (NamedVertex)g.AddVertex();	zig_cpp.Name="zip.cpp";
			NamedVertex boz_h = (NamedVertex)g.AddVertex();		boz_h.Name="boz.h";
			NamedVertex zag_cpp = (NamedVertex)g.AddVertex();	zag_cpp.Name="zag.cpp";
			NamedVertex yow_h = (NamedVertex)g.AddVertex();		yow_h.Name="yow.h";
			NamedVertex dax_h = (NamedVertex)g.AddVertex();		dax_h.Name="dax.h";
			NamedVertex bar_cpp = (NamedVertex)g.AddVertex();	bar_cpp.Name="bar.cpp";
			NamedVertex zow_h = (NamedVertex)g.AddVertex();		zow_h.Name="zow.h";
			NamedVertex foo_cpp = (NamedVertex)g.AddVertex();	foo_cpp.Name="foo.cpp";

			NamedVertex zig_o = (NamedVertex)g.AddVertex();		zig_o.Name="zi.o";
			NamedVertex zag_o = (NamedVertex)g.AddVertex();		zag_o.Name="zag.o";
			NamedVertex bar_o = (NamedVertex)g.AddVertex();		bar_o.Name="bar.o";
			NamedVertex foo_o = (NamedVertex)g.AddVertex();		foo_o.Name="foo.o";
			NamedVertex libzigzag_a = (NamedVertex)g.AddVertex(); libzigzag_a.Name="libzigzag.a";
			NamedVertex libfoobar_a = (NamedVertex)g.AddVertex(); libfoobar_a.Name="libfoobar.a";

			NamedVertex killerapp = (NamedVertex)g.AddVertex();	killerapp.Name="killerapp";

			// adding dependencies
			g.AddEdge(dax_h, foo_cpp); 
			g.AddEdge(dax_h, bar_cpp); 
			g.AddEdge(dax_h, yow_h);
			g.AddEdge(yow_h, bar_cpp); 
			g.AddEdge(yow_h, zag_cpp);
			g.AddEdge(boz_h, bar_cpp); 
			g.AddEdge(boz_h, zig_cpp); 
			g.AddEdge(boz_h, zag_cpp);
			g.AddEdge(zow_h, foo_cpp); 
			g.AddEdge(foo_cpp, foo_o);
			g.AddEdge(foo_o, libfoobar_a);
			g.AddEdge(bar_cpp, bar_o);
			g.AddEdge(bar_o, libfoobar_a);
			g.AddEdge(libfoobar_a, libzigzag_a);
			g.AddEdge(zig_cpp, zig_o);
			g.AddEdge(zig_o, libzigzag_a);
			g.AddEdge(zag_cpp, zag_o);
			g.AddEdge(zag_o, libzigzag_a);
			g.AddEdge(libzigzag_a, killerapp);

			return g;
		}
	}
}
