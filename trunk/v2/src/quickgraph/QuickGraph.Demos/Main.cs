using System;
using System.Drawing;
using System.Drawing.Imaging;
using QuickGraph;
using QuickGraph.Concepts;
using QuickGraph.Concepts.Modifications;
using QuickGraph.Concepts.Traversals;
using QuickGraph.Representations;
using QuickGraph.Serialization;
using System.Xml;

namespace QuickGraphTest
{
	using NGraphviz.Helpers;

	public class MainClass
	{
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
                MaximumFlowDemo maxFlow = new MaximumFlowDemo();
                maxFlow.Run();

                //FilteringTest filterTest = new FilteringTest();
               // filterTest.Run();

                //GraphMLUpdater.Update(
				//	"../../Graphs/north",
				//	"*.graphml",
				//	"http://graphml.graphdrawing.org/xmlns/1.0rc"
				//	);
				//XmlSerializationTest serial = new XmlSerializationTest();
				//serial.ReadWriteGraphMlAdjacencyGraph();

				//SimilarityTest.BasicTest();
/*
				MazeGenerator maze = new MazeGenerator(10,10);
				maze.GenerateWalls(9,9,0,0);
				maze.Save("maze.png",ImageFormat.Png, 500,500);

				Random rnd = new Random((int)DateTime.Now.Ticks);

//				IVertexAndEdgeListGraph g = GraphProvider.FileDependency();
//				IVertexAndEdgeListGraph g = GraphProvider.RegularLattice(5,5);
//				IVertexAndEdgeListGraph g = GraphProvider.Simple();
//				IVertexAndEdgeListGraph g = GraphProvider.Loop();
				IVertexAndEdgeListGraph g = GraphProvider.Fsm();

				IVertex root = Traversal.FirstVertexIf(g.Vertices,new NameEqualPredicate("S0"));
				RandomWalkTest walk = new RandomWalkTest();
				walk.Test(g,root);

//				EdgeDfsTest edfs =new EdgeDfsTest();
//				edfs.Test();
*/
//				FileDependencyTest fd =new FileDependencyTest();
//				fd.Test();
/*				XmlSerializationTest ser = new XmlSerializationTest();
				ser.ReadWriteGraphMlAdjacencyGraph();

				ClusteredGraphTest cluster = new ClusteredGraphTest();
				cluster.Run();

				EdgeDfsTest edfs =new EdgeDfsTest();
				edfs.Test();
/*
				AssemblyGrapher ag = new AssemblyGrapher();
				ag.OutputFileName="aggraph";
				ag.ImageType = GraphvizImageType.Png;
				ag.Assemblies.Add( System.Reflection.Assembly.GetCallingAssembly());
				ag.Run();
*/
//				SerializationTest serial = new SerializationTest();
//				serial.Run();
			}
			
			catch(Exception ex)
			{
				
				Console.Out.WriteLine(ex.ToString());
			}
            Console.In.ReadLine();
        }
    }
}
