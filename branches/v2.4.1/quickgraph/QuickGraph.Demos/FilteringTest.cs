using System;

using QuickGraph.Concepts;
using QuickGraph.Concepts.Collections;
using QuickGraph.Concepts.Predicates;
using QuickGraph.Concepts.Traversals;

using QuickGraph;
using QuickGraph.Collections;
using QuickGraph.Collections.Filtered;
using QuickGraph.Representations;
using QuickGraph.Algorithms.Graphviz;
using QuickGraph.Predicates;
using NGraphviz.Helpers;

namespace QuickGraphTest
{
    public class FilteringTest
    {
        public void Run()
        {
            BasicFiltering();
            FilterFsm();
        }

        private void BasicFiltering()
        {
            BidirectionalGraph graph = GraphProvider.FileDependency();
            DrawGraph(graph,"filedependency");

            Console.WriteLine("Source vertices:");
            foreach(NamedVertex v in graph.SelectVertices(Preds.SourceVertex(graph)))
            {
                Console.WriteLine("\t{0}",v.Name);
            }
            Console.WriteLine("Sink vertices:");
            FilteredVertexEnumerable filteredVertices =
                new FilteredVertexEnumerable(
                    graph.Vertices,
                    Preds.SinkVertex(graph)
                    );
            foreach (NamedVertex v in filteredVertices)
            {
                Console.WriteLine("\t{0}",v.Name);
            }
        }

        private void FilterFsm()
        {
            AdjacencyGraph graph = GraphProvider.Fsm();
            // drawing the fsm
            DrawGraph(graph, "fsm");

            // filtering
            // putting all black besides S4
            // therefore all the edges touching s4 will be filtered out.
            VertexColorDictionary vertexColors = new VertexColorDictionary();
            IVertexPredicate pred = new NameEqualPredicate("S4");
            foreach (IVertex v in graph.Vertices)
            {
                if (pred.Test(v))
                    vertexColors[v] = GraphColor.Black;
                else
                    vertexColors[v] = GraphColor.White;
            }

            IVertexPredicate vp = new NoBlackVertexPredicate(vertexColors);
            IEdgePredicate ep = new EdgePredicate(
                Preds.KeepAllEdges(),
                vp
                );
            IVertexAndEdgeListGraph filteredGraph = new FilteredVertexAndEdgeListGraph(graph,
                ep,
                vp
                );

            DrawGraph(filteredGraph, "fsmfiltered");
        }

        public void DrawGraph(IVertexAndEdgeListGraph g, string fileName)
        {
            GraphvizAlgorithm gv = new GraphvizAlgorithm(g);
            gv.FormatVertex+=new FormatVertexEventHandler(gv_FormatVertex);
            gv.FormatEdge+=new FormatEdgeEventHandler(gv_FormatEdge);

            System.Diagnostics.Process.Start(gv.Write(fileName));
        }

        void gv_FormatVertex(object sender, FormatVertexEventArgs e)
        {
            NamedVertex v = (NamedVertex)e.Vertex;
            e.VertexFormatter.Label = v.Name;
        }

        void gv_FormatEdge(object sender, FormatEdgeEventArgs e)
        {
            NamedEdge edge = e.Edge as NamedEdge;
            if (edge == null)
                return;
            e.EdgeFormatter.Label.Value = edge.Name;
        }
    }
}
