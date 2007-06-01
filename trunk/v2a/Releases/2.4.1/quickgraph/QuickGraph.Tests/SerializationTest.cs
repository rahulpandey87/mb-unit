using System;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Schema;
using System.IO;

namespace QuickGraph.UnitTests
{
	using QuickGraph;
	using QuickGraph.Providers;
	using QuickGraph.Concepts;
	using QuickGraph.Representations;
	using QuickGraph.Collections;
	using QuickGraph.Serialization;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Serialization;

	using MbUnit.Core.Framework;
	using MbUnit.Framework;

	/// <summary>
	/// Summary description for SerializationTest.
	/// </summary>
	[TestFixture]
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

		[Test]
		public void WriteXmlVertex()
		{
			XmlSerializer ser = new XmlSerializer(Graph.VertexProvider.VertexType);
			XmlTextWriter writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;

			ser.Serialize(writer, (Object)v);
			Console.WriteLine();
		}

		[Test]
		public void WriteXmlEdge()
		{
			XmlSerializer ser = new XmlSerializer(Graph.EdgeProvider.EdgeType);
			XmlTextWriter writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;

			ser.Serialize(writer, (Object)uv);
		}

		[Test]
		public void WriteXmlAdjacencyGraph()
		{
			XmlTextWriter writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;

			XmlGraphSerializer ser = new XmlGraphSerializer(Graph);
			ser.WriteXml(writer);
		}

		[Test]
		public void WriteReadXmlAdjacencyGraph()
		{
			StringWriter sw = new StringWriter();
			XmlTextWriter writer = new XmlTextWriter(sw);
			writer.Formatting = Formatting.Indented;

			XmlGraphSerializer ser = new XmlGraphSerializer(Graph);
			ser.WriteXml(writer);

			StringReader sr = new StringReader(sw.ToString());
			XmlTextReader reader = new XmlTextReader(sr);
			AdjacencyGraph g = (AdjacencyGraph)ser.ReadXml(reader);

			ser = new XmlGraphSerializer(g);
			StringWriter swResult = new StringWriter();
			writer = new XmlTextWriter(swResult);
			writer.Formatting = Formatting.Indented;
			ser.WriteXml(writer);

			Console.WriteLine("------------ original xml ---------------");
			Console.WriteLine(sw.ToString());

			Console.WriteLine("------------ output xml ---------------");
			Console.WriteLine(swResult.ToString());

			XmlAssert.XmlEquals(sw.ToString(), swResult.ToString());
		}

		[Test]
		public void WriteGraphMlAdjacencyGraph()
		{
			XmlTextWriter writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;

			GraphMLGraphSerializer ser = new GraphMLGraphSerializer();
			ser.Serialize(writer,Graph);
		}

		[Test]
		public void WriteReadGraphMlAdjacencyGraph()
		{
			StringWriter sw = new StringWriter();
			XmlTextWriter writer = new XmlTextWriter(sw);
			writer.Formatting = Formatting.Indented;

			GraphMLGraphSerializer ser = new GraphMLGraphSerializer();
			ser.Serialize(writer,Graph);
			Console.WriteLine("------------- serialized graph -----------------");
			Console.WriteLine(sw.ToString());

			XmlAssert.XmlValid(sw.ToString());

			StringReader sr = new StringReader(sw.ToString());
			XmlTextReader reader = new XmlTextReader(sr);
			ser.TypeFromXml = true;
			ISerializableVertexAndEdgeListGraph g = ser.Deserialize(reader);

			ser = new GraphMLGraphSerializer();
			StringWriter swResult = new StringWriter();
			writer = new XmlTextWriter(swResult);
			writer.Formatting = Formatting.Indented;
			ser.Serialize(writer,g);
			Console.WriteLine("------------- deserialized graph -----------------");
			Console.WriteLine(swResult.ToString());

			XmlAssert.XmlEquals(sw.ToString(), swResult.ToString());
			CheckGraphEqual(Graph,g);
		}

		[Test]
		public void WriteGxlAdjacencyGraph()
		{
			XmlTextWriter writer = new XmlTextWriter(Console.Out);
			writer.Formatting = Formatting.Indented;

			GxlGraphSerializer ser = new GxlGraphSerializer();
			ser.Serialize(writer,Graph);
		}

		[Test]
		public void ReadWriteGxlAdjacencyGraph()
		{
			StringWriter sw = new StringWriter();
			XmlTextWriter writer = new XmlTextWriter(sw);
			writer.Formatting = Formatting.Indented;

			GxlGraphSerializer ser = new GxlGraphSerializer();
			ser.Serialize(writer,Graph);

			XmlAssert.XmlValid(sw.ToString());

			StringReader sr = new StringReader(sw.ToString());
			XmlTextReader reader = new XmlTextReader(sr);
			ISerializableVertexAndEdgeListGraph g = ser.Deserialize(reader);

			CheckGraphEqual(Graph,g);
		}

		internal void CheckGraphEqual(IVertexAndEdgeListGraph left, IVertexAndEdgeListGraph right)
		{
			Assert.AreEqual(left.GetType(),right.GetType());
			Assert.AreEqual(left.VerticesCount,right.VerticesCount);
			Assert.AreEqual(left.EdgesCount,right.EdgesCount);
		}
	}
}
