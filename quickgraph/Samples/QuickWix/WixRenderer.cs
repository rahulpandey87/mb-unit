using System;
using System.IO;
using System.Drawing;
using System.Collections;

using QuickGraph;
using QuickGraph.Providers;
using QuickGraph.Representations;
using QuickGraph.Algorithms.Graphviz;
using QuickGraph.Concepts;

using Microsoft.Tools.WindowsInstallerXml.Serialize;

namespace QuickWix
{
    public class WixRenderer
    {
        private AdjacencyGraph graph;
        private GraphvizAlgorithm graphviz;
        private Wix wix;
        private Hashtable nodeVertices = new Hashtable();
        private GraphPopulatorWixVisitor visitor;


        public WixRenderer()
        {
            this.graph = new AdjacencyGraph(
                new CustomVertexProvider(),
                new CustomEdgeProvider(),
                true
                );
            this.graphviz = new GraphvizAlgorithm(this.graph);
            this.wix = null;

            this.graphviz.ImageType = NGraphviz.Helpers.GraphvizImageType.Svg;
            this.graphviz.CommonEdgeFormat.Font = new Font("Tahoma", 8.25f);
            this.graphviz.CommonVertexFormat.Font = new Font("Tahoma", 8.25f);
            this.graphviz.CommonVertexFormat.Style = NGraphviz.Helpers.GraphvizVertexStyle.Filled;
            this.graphviz.CommonVertexFormat.FillColor = Color.LightYellow;
            this.graphviz.CommonVertexFormat.Shape = NGraphviz.Helpers.GraphvizVertexShape.Box;
            this.graphviz.FormatVertex+=new FormatVertexEventHandler(graphviz_FormatVertex);

            this.visitor = new GraphPopulatorWixVisitor(this);
        }

        public Wix Wix
        {
            get { return this.wix; }
            set { this.wix = value; }
        }

        public GraphvizAlgorithm Graphviz
        {
            get { return this.graphviz; }
        }

        public AdjacencyGraph Graph
        {
            get { return this.graph; }
        }

        public CustomVertex AddVertex(Object node)
        {
            CustomVertex v = nodeVertices[node] as CustomVertex;
            if (v != null)
                return v;
            v = (CustomVertex)this.graph.AddVertex();
            v.Value = node;
            this.nodeVertices.Add(node, v);
            return v;
        }

        public string Render(string fileName)
        {
            if (this.Wix == null)
                return null;

            this.BuildGraph();

            string f = Path.GetFileName(fileName);
            string outputFileName = this.graphviz.Write(f+".svg");
            return outputFileName;
        }

        public void BuildGraph()
        {
            if (this.Wix == null)
                throw new InvalidOperationException("Wix is a null reference");

            this.graph.Clear();
            this.nodeVertices.Clear();
            this.visitor.VisitWix(this.Wix);
        }

        void graphviz_FormatVertex(object sender, FormatVertexEventArgs e)
        {
            CustomVertex vertex = (CustomVertex)e.Vertex;
            e.VertexFormatter.Label = String.Format("{0}", vertex.Value);
        }

        void graphviz_FormatEdge(object sender, FormatEdgeEventArgs e)
        {
        }

        private class GraphPopulatorWixVisitor : WixVisitor
        {
            private WeakReference parent;
            public GraphPopulatorWixVisitor(WixRenderer parent)
            {
                this.parent = new WeakReference(parent);
            }

            public WixRenderer Parent
            {
                get { return this.parent.Target as WixRenderer; }
            }

            public override void VisitCustom(Custom node)
            {
                IVertex action = Parent.AddVertex(node.Action);
                if (node.Before!=null)
                {
                    IVertex before = Parent.AddVertex(node.Before);
                    this.Parent.Graph.AddEdge(before, action);
                }
                if (node.After != null)
                {
                    IVertex after = Parent.AddVertex(node.After);
                    this.Parent.Graph.AddEdge(action, after);
                }
            }
        }
    }
}
