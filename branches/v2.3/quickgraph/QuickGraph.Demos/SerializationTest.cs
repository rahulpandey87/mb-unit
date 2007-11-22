using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Schema;
using System.IO;

namespace QuickGraphTest
{
	using QuickGraph;
	using QuickGraph.Providers;
	using QuickGraph.Concepts;
	using QuickGraph.Representations;
	using QuickGraph.Collections;
	using QuickGraph.Serialization;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Serialization;

	/// <summary>
	/// Summary description for SerializationTest.
	/// </summary>
	public class XmlSerializationTest
	{
		public AdjacencyGraph m_Graph;
		public NamedVertex u,v,w;
		public NamedEdge uv,uw;
		public XmlSerializationTest()
		{
			m_Graph = new AdjacencyGraph(
				new NamedVertexProvider(),
				new NamedEdgeProvider(),
				true
				);
			
			u = (NamedVertex)Graph.AddVertex(); u.Name = "u";
			v = (NamedVertex)Graph.AddVertex(); v.Name = "v";
			w = (NamedVertex)Graph.AddVertex(); w.Name = "w";

			uv = (NamedEdge)Graph.AddEdge(u,v); uv.Name = "uv";
			uw = (NamedEdge)Graph.AddEdge(u,w); uw.Name = "uw";
		}

		public AdjacencyGraph Graph
		{
			get
			{
				return m_Graph;
			}
		}

		public void ReadWriteGraphMlAdjacencyGraph()
		{
			StringWriter sw = new StringWriter();
			XmlTextWriter writer = new XmlTextWriter(sw);
			writer.Formatting = Formatting.Indented;

			GraphMLGraphSerializer ser = new GraphMLGraphSerializer(@"../../graphml.dtd");
			ser.Serialize(writer,Graph);

			Console.WriteLine(sw.ToString());
			Validate(sw.ToString());
			Console.WriteLine("Validated");

			StringReader sr = new StringReader(sw.ToString());
			XmlTextReader reader = new XmlTextReader(sr);
			ISerializableVertexAndEdgeListGraph g = ser.Deserialize(reader);

			writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;
			Console.WriteLine("Outputting reloaded");
			ser.Serialize(writer,g);
		}

		internal void Validate(string xmlString)
		{
			XmlTextReader tr = new XmlTextReader(new StringReader(xmlString));
			XmlValidatingReader vr = new XmlValidatingReader(tr);

			vr.ValidationType = ValidationType.Auto;
			vr.ValidationEventHandler += new ValidationEventHandler (ValidationHandler);

			while(vr.Read())
			{};
		}

		protected void ValidationHandler(object sender, ValidationEventArgs args)
		{
			Console.WriteLine("{0} {1}",args.Severity,args.Message);
		}
	}
}
