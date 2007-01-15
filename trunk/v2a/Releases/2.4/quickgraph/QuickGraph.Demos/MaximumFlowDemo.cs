using System;
using QuickGraph;
using QuickGraph.Concepts;
using QuickGraph.Providers;
using QuickGraph.Representations;
using QuickGraph.Algorithms.MaximumFlow;
using QuickGraph.Collections;
using QuickGraph.Algorithms.Graphviz;

namespace QuickGraphTest
{
    public class MaximumFlowDemo
    {
        private AdjacencyGraph graph;
        private GraphvizAlgorithm graphviz;

        private NamedVertex s, x, v, w, t;
        private IEdge sx, sv, xv, xw, wv, wt, vt;
        private EdgeDoubleDictionary capacities = new EdgeDoubleDictionary();

        private ReversedEdgeAugmentorAlgorithm reversedEdgeAugmentor;
        private MaximumFlowAlgorithm maxFlow = null;

        public MaximumFlowDemo()
        {
            graph = new AdjacencyGraph(
                new NamedVertexProvider(),
                new EdgeProvider(),
                true);

            s = (NamedVertex)graph.AddVertex(); s.Name = "s";
            x = (NamedVertex)graph.AddVertex(); x.Name = "x";
            v = (NamedVertex)graph.AddVertex(); v.Name = "v";
            w = (NamedVertex)graph.AddVertex(); w.Name = "w";
            t = (NamedVertex)graph.AddVertex(); t.Name = "t";

            sx = graph.AddEdge(s, x); capacities[sx] = 5;
            sv = graph.AddEdge(s, v); capacities[sv] = 7;
            xv = graph.AddEdge(x, v); capacities[xv] = 3;
            xw = graph.AddEdge(x, w); capacities[xw] = 7;
            wv = graph.AddEdge(w, v); capacities[wv] = 5;
            wt = graph.AddEdge(w, t); capacities[wt] = 4;
            vt = graph.AddEdge(v, t); capacities[vt] = 6;

            this.graphviz = new GraphvizAlgorithm(this.graph);
            this.graphviz.ImageType = NGraphviz.Helpers.GraphvizImageType.Svg;
            this.graphviz.GraphFormat.RankDirection = NGraphviz.Helpers.GraphvizRankDirection.LR;
            this.graphviz.FormatVertex+=new FormatVertexEventHandler(graphviz_FormatVertex);
            this.graphviz.FormatEdge+=new FormatEdgeEventHandler(graphviz_FormatEdge);

            this.reversedEdgeAugmentor = new ReversedEdgeAugmentorAlgorithm(this.graph);
            this.reversedEdgeAugmentor.ReversedEdgeAdded += new EdgeEventHandler(reversedEdgeAugmentor_ReversedEdgeAdded);
        }

        public void Run()
        {
            Render("flowGraph", true);

            // add reversed edges
            reversedEdgeAugmentor.AddReversedEdges();
            // render with reversed edges
            Render("flowGraphWithReversedEdges", true);

            // create max flow algorithm
            this.maxFlow = new PushRelabelMaximumFlowAlgorithm(
                this.graph,
                this.capacities,
                this.reversedEdgeAugmentor.ReversedEdges
                );

            // compute max flow
            double f = this.maxFlow.Compute(s, t);
            Console.WriteLine("Maximum flow: {0}", f);

            // clean up
            reversedEdgeAugmentor.RemoveReversedEdges();

            // render without reversed edges
            Render("flowWithOutReversedEdges", true);
        }

        public string Render(string fileName,bool show)
        {
            string output = this.graphviz.Write(fileName);
            if (show)
                System.Diagnostics.Process.Start(output);
            return output;
        }

        void graphviz_FormatVertex(object sender, FormatVertexEventArgs e)
        {
            NamedVertex v = (NamedVertex)e.Vertex;
            e.VertexFormatter.Label = v.Name;
        }

        void graphviz_FormatEdge(object sender, FormatEdgeEventArgs e)
        {
            double cap = this.capacities[e.Edge];
            string label = cap.ToString();
            if (this.maxFlow != null)
            {
                double r = this.maxFlow.ResidualCapacities[e.Edge];
                double f = this.capacities[e.Edge] - r;
                label = string.Format("{0} - {1}", label, f);

            }
            if (this.reversedEdgeAugmentor.AugmentedEdges.Contains(e.Edge))
                e.EdgeFormatter.Style = NGraphviz.Helpers.GraphvizEdgeStyle.Dashed;
            else
                e.EdgeFormatter.Style = NGraphviz.Helpers.GraphvizEdgeStyle.Solid;

            e.EdgeFormatter.Label.Value = label;
        }

        void reversedEdgeAugmentor_ReversedEdgeAdded(Object sender, EdgeEventArgs e)
        {
            this.capacities[e.Edge] = 0;
        }
    }
}
