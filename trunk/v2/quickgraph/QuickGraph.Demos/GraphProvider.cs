using System;

namespace QuickGraphTest
{
	using QuickGraph;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Predicates;
	using QuickGraph.Predicates;
	using QuickGraph.Collections;
	using QuickGraph.Providers;
	using QuickGraph.Representations;
	using QuickGraph.Collections.Filtered;

	/// <summary>
	/// Summary description for GraphProvider.
	/// </summary>
	public class GraphProvider
	{

		public static BidirectionalGraph Loop()
		{
			// create a new adjacency graph
			BidirectionalGraph g = new BidirectionalGraph(
				new NamedVertexProvider(),
				new EdgeProvider(),
				false);

			NamedVertex x = (NamedVertex)g.AddVertex(); x.Name = "x";
			NamedVertex y = (NamedVertex)g.AddVertex(); y.Name = "y";
			NamedVertex z = (NamedVertex)g.AddVertex(); z.Name = "z";

			g.AddEdge(x,y);
			g.AddEdge(y,z);
			g.AddEdge(z,x);

			return g;
		}

		public static BidirectionalGraph Simple()
		{
			// create a new adjacency graph
			BidirectionalGraph g = new BidirectionalGraph(
				new NamedVertexProvider(),
				new EdgeProvider(),
				false);

			NamedVertex u = (NamedVertex)g.AddVertex(); u.Name = "u";
			NamedVertex w = (NamedVertex)g.AddVertex(); w.Name = "w";
			NamedVertex x = (NamedVertex)g.AddVertex(); x.Name = "x";
			NamedVertex y = (NamedVertex)g.AddVertex(); y.Name = "y";
			NamedVertex z = (NamedVertex)g.AddVertex(); z.Name = "z";
			NamedVertex v = (NamedVertex)g.AddVertex(); v.Name = "v";

			g.AddEdge(u,x);
			g.AddEdge(u,v);
			g.AddEdge(w,z);
			g.AddEdge(w,y);
			g.AddEdge(w,u);
			g.AddEdge(x,v);
			g.AddEdge(v,y);
			g.AddEdge(y,x);
			g.AddEdge(z,y);

			return g;
		}

		public static BidirectionalGraph FileDependency()
		{
			// create a new adjacency graph
			BidirectionalGraph g = new BidirectionalGraph(
								   new NamedVertexProvider(),
								   new EdgeProvider(),
								   false);

			// adding files and storing names
			NamedVertex zig_cpp = (NamedVertex)g.AddVertex();	
			zig_cpp.Name = "zip.cpp";
			NamedVertex boz_h = (NamedVertex)g.AddVertex();
			boz_h.Name="boz.h";
			NamedVertex zag_cpp = (NamedVertex)g.AddVertex();	
			zag_cpp.Name="zag.cpp";
			NamedVertex yow_h = (NamedVertex)g.AddVertex();		yow_h.Name="yow.h";
			NamedVertex dax_h = (NamedVertex)g.AddVertex();		dax_h.Name="dax.h";
			NamedVertex bar_cpp = (NamedVertex)g.AddVertex();	bar_cpp.Name="bar.cpp";
			NamedVertex zow_h = (NamedVertex)g.AddVertex();		zow_h.Name="zow.h";
			NamedVertex foo_cpp = (NamedVertex)g.AddVertex();	foo_cpp.Name="foo.cpp";

			NamedVertex zig_o = (NamedVertex)g.AddVertex();		zig_o.Name="zig.o";
			NamedVertex zag_o = (NamedVertex)g.AddVertex();		zag_o.Name="zago";
			NamedVertex bar_o = (NamedVertex)g.AddVertex();		bar_o.Name="bar.o";
			NamedVertex foo_o = (NamedVertex)g.AddVertex();		foo_o.Name="foo.o";
			NamedVertex libzigzag_a = (NamedVertex)g.AddVertex();libzigzag_a.Name="libzigzig.a";
			NamedVertex libfoobar_a = (NamedVertex)g.AddVertex();libfoobar_a.Name="libfoobar.a";

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

		public static BidirectionalGraph RegularLattice(int rows, int columns)
		{
			// create a new adjacency graph
			BidirectionalGraph g = new BidirectionalGraph(
				new NamedVertexProvider(),
				new EdgeProvider(),
				false);			

			NamedVertex[,] latice = new NamedVertex[rows,columns];
			// adding vertices
			for(int i=0;i<rows;++i)
			{
				for(int j=0;j<columns;++j)
				{
					latice[i,j] = (NamedVertex)g.AddVertex();
					latice[i,j].Name = String.Format("{0},{1}",i.ToString(),j.ToString());
				}
			}

			// adding edges
			for(int i =0;i<rows-1;++i)
			{
				for(int j=0;j<columns-1;++j)
				{
					g.AddEdge(latice[i,j], latice[i,j+1]);
					g.AddEdge(latice[i,j+1], latice[i,j]);

					g.AddEdge(latice[i,j], latice[i+1,j]);
					g.AddEdge(latice[i+1,j], latice[i,j]);
				}
			}

			for(int j=0;j<columns-1;++j)
			{
				g.AddEdge(latice[rows-1,j], latice[rows-1,j+1]);
				g.AddEdge(latice[rows-1,j+1], latice[rows-1,j]);
			}

			for(int i=0;i<rows-1;++i)
			{
				g.AddEdge(latice[i,columns-1], latice[i+1,columns-1]);
				g.AddEdge(latice[i+1,columns-1], latice[i,columns-1]);
			}

			return g;
		}

		public static AdjacencyGraph Fsm()
		{
			// create a new adjacency graph
			AdjacencyGraph g = new BidirectionalGraph(
				new NamedVertexProvider(),
				new NamedEdgeProvider(),
				true);
			NamedEdge e=null;

			NamedVertex s0 = (NamedVertex)g.AddVertex(); s0.Name="S0";
			NamedVertex s1 = (NamedVertex)g.AddVertex(); s1.Name="S1";
			NamedVertex s2 = (NamedVertex)g.AddVertex(); s2.Name="S2";
			NamedVertex s3 = (NamedVertex)g.AddVertex(); s3.Name="S3";
			NamedVertex s4 = (NamedVertex)g.AddVertex(); s4.Name="S4";
			NamedVertex s5 = (NamedVertex)g.AddVertex(); s5.Name="S5";

			e=(NamedEdge)g.AddEdge(s0,s1); e.Name="StartCalc";

			e=(NamedEdge)g.AddEdge(s1,s0); e.Name="StopCalc";
			e=(NamedEdge)g.AddEdge(s1,s1); e.Name="SelectStandard";
			e=(NamedEdge)g.AddEdge(s1,s1); e.Name="ClearDisplay";
			e=(NamedEdge)g.AddEdge(s1,s2); e.Name="SelectScientific";
			e=(NamedEdge)g.AddEdge(s1,s3); e.Name="EnterDecNumber";

			e=(NamedEdge)g.AddEdge(s2,s1); e.Name="SelectStandard";
			e=(NamedEdge)g.AddEdge(s2,s2); e.Name="SelectScientific";
			e=(NamedEdge)g.AddEdge(s2,s2); e.Name="ClearDisplay";
			e=(NamedEdge)g.AddEdge(s2,s4); e.Name="EnterDecNumber";
			e=(NamedEdge)g.AddEdge(s2,s5); e.Name="StopCalc";

			e=(NamedEdge)g.AddEdge(s3,s0); e.Name="StopCalc";
			e=(NamedEdge)g.AddEdge(s3,s1); e.Name="ClearDisplay";
			e=(NamedEdge)g.AddEdge(s3,s3); e.Name="SelectStandard";
			e=(NamedEdge)g.AddEdge(s3,s3); e.Name="EnterDecNumber";
			e=(NamedEdge)g.AddEdge(s3,s4); e.Name="SelectScientific";

			e=(NamedEdge)g.AddEdge(s4,s2); e.Name="ClearDisplay";
			e=(NamedEdge)g.AddEdge(s4,s3); e.Name="SelectStandard";
			e=(NamedEdge)g.AddEdge(s4,s4); e.Name="SelectScientific";
			e=(NamedEdge)g.AddEdge(s4,s4); e.Name="EnterDecNumber";
			e=(NamedEdge)g.AddEdge(s4,s5); e.Name="StopCalc";

			e=(NamedEdge)g.AddEdge(s5,s2); e.Name="StartCalc";

			return g;
		}

		public static FilteredVertexAndEdgeListGraph FilteredFsm()
		{
			AdjacencyGraph g = Fsm();

			// putting all black besides S4
			// therefore all the edges touching s4 will be filtered out.
			VertexColorDictionary vertexColors= new VertexColorDictionary();
			IVertexPredicate pred = new NameEqualPredicate("S4");
			foreach(IVertex v in g.Vertices)
			{
				if (pred.Test(v))
					vertexColors[v]=GraphColor.Black;
				else
					vertexColors[v]=GraphColor.White;
			}

			IVertexPredicate vp = new NoBlackVertexPredicate(vertexColors);
			IEdgePredicate ep = new EdgePredicate(
				new KeepAllEdgesPredicate(),
				vp
				);
			return new FilteredVertexAndEdgeListGraph(g,
				ep,
				vp
				);
		}
	}
}
