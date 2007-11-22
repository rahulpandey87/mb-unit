// QuickGraph Library 
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		QuickGraph Library HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;
using System.Collections;

namespace QuickGraph.Serialization
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Providers;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Serialization;

	/// <summary>
	/// A wrapper for serializings graphs
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class lets you serialize to xml your graphs, even if you are using
	/// custom vertex and edges.
	/// </para>
	/// <para>
	/// </para>
	/// </remarks>
	/// <example>
	/// In this example, a AdjacencyGraph is created with
	/// custom providers. It is serialized and deserialized to xml:
	/// <code>
	/// AdjacencyGraph g = new AdjacencyGraph(
	///     new NamedVertexProvider(),
	///     new NamedEdgeProvider(),
	///     true
	///     );
	///		
	/// NamedEdge u = (NamedVertex)Graph.AddVertex(); u.Name = "u";
	/// NamedEdge v = (NamedVertex)Graph.AddVertex(); v.Name = "v";
	/// NamedEdge w = (NamedVertex)Graph.AddVertex(); w.Name = "w";
    ///
	/// NamedEdge uv = (NamedEdge)Graph.AddEdge(u,v); uv.Name = "uv";
	/// NamedEdge uw = (NamedEdge)Graph.AddEdge(u,w); uw.Name = "uw";
	/// 
	/// StringWriter sw = new StringWriter();
	/// XmlTextWriter writer = new XmlTextWriter(sw);
	/// writer.Formatting = Formatting.Indented;
	/// 
	/// GraphSerializer ser = new GraphSerializer(Graph);
	/// ser.WriteXml(writer);
	///
	/// Console.WriteLine(sw.ToString());
	///
	/// StringReader sr = new StringReader(sw.ToString());
	/// XmlTextReader reader = new XmlTextReader(sr);
	/// ser.ReadXml(reader);
	///
	/// sw = new StringWriter();
	/// writer = new XmlTextWriter(sw);
	/// writer.Formatting = Formatting.Indented;
	/// ser.WriteXml(writer);
	/// Console.WriteLine(sw.ToString());
	/// </code>
	/// </example>
	public class XmlGraphSerializer
	{
		private ISerializableVertexAndEdgeListGraph m_Graph;

		/// <summary>
		/// Empty constructor
		/// </summary>
		public XmlGraphSerializer() 
		{
			m_Graph = null;
		}

		/// <summary>
		/// Constructs a serializer around g
		/// </summary>
		/// <param name="g">graph to serialize</param>
		/// <exception cref="ArgumentNullException">g is null</exception>
		public XmlGraphSerializer(ISerializableVertexAndEdgeListGraph g) 
		{
			if (g==null)
				throw new ArgumentNullException("graph");
			m_Graph = g;
		}

		/// <summary>
		/// Serialized graph
		/// </summary>
		public ISerializableVertexAndEdgeListGraph Graph
		{
			get
			{
				return m_Graph;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("graph");
				m_Graph = value;
			}
		}

		/// <summary>
		/// Serializes graph to xml. <see cref="WriteXml"/>
		/// </summary>
		/// <param name="writer"></param>
		public void Serialize(XmlWriter writer)
		{	
			this.WriteXml(writer);
		}

		/// <summary>
		/// Serializes the graph to xml
		/// </summary>
		/// <param name="writer">opened xml writer</param>
		/// <remarks>
		/// <para>
		/// The edge and vertex class must be serializable.
		/// </para>
		/// </remarks>
		public void WriteXml(XmlWriter writer)
		{
			// create serializers
			XmlSerializer vertexSer = new XmlSerializer(Graph.VertexProvider.VertexType);
			XmlSerializer edgeSer = new XmlSerializer(Graph.EdgeProvider.EdgeType);

			writer.WriteStartElement("graph");

			writer.WriteStartAttribute("type","");
			writer.WriteString(GetTypeQualifiedName(Graph.GetType()));
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("vertex-provider-type","");
			writer.WriteString(GetTypeQualifiedName(Graph.VertexProvider.GetType()));
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("edge-provider-type","");
			writer.WriteString(GetTypeQualifiedName(Graph.EdgeProvider.GetType()));
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("directed","");
			writer.WriteString(Graph.IsDirected.ToString().ToLower());
			writer.WriteEndAttribute();

			writer.WriteStartAttribute("allow-parallel-edges","");
			writer.WriteString(Graph.AllowParallelEdges.ToString().ToLower());
			writer.WriteEndAttribute();

			// vertices
			writer.WriteStartElement("vertices");
			foreach(IVertex v in Graph.Vertices)
				vertexSer.Serialize(writer,(Object)v);
			writer.WriteEndElement(); // vertices

			// edges
			writer.WriteStartElement("edges");
			foreach(IEdge e in Graph.Edges)
				edgeSer.Serialize(writer,(Object)e);
			writer.WriteEndElement(); // edges	

			writer.WriteEndElement(); // graph
		}

		/// <summary>
		/// Reads graph data from Xml and create the graph object.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Deserializes a graph from Xml.
		/// </para>
		/// </remarks>
		/// <param name="reader">opened xml reader</param>
		/// <returns>deserialized graph</returns>
		public Object ReadXml(XmlReader reader)
		{
			ReadGraphXml(reader);

			XmlSerializer vertexSer = new XmlSerializer(Graph.VertexProvider.VertexType);
			XmlSerializer edgeSer = new XmlSerializer(Graph.EdgeProvider.EdgeType);
			Hashtable vertexMap = new Hashtable();

			// reading vertices
			do 
			{
				if(!reader.Read())
					break;
			} while(reader.Name != "vertices");

			if (reader.Name != "vertices")
				throw new Exception("could not find vertices node");

//			reader.Read();
			do 
			{
				if(!reader.Read())
					break;
				if (reader.Name == "vertices" && reader.NodeType==XmlNodeType.EndElement)
					break;
				if (reader.NodeType != XmlNodeType.Element)
					continue;

				// creating new vertex
				IVertex v=(IVertex)vertexSer.Deserialize(reader);

				// storing ID in table
				vertexMap[v.ID]=v;	

				// adding vertex to graph
				Graph.AddVertex(v);
			} while(true);

			// reading edges
			do 
			{
				if(!reader.Read())
					break;
			} while(reader.Name != "edges");

			if (reader.Name != "edges")
				throw new Exception("could not find edges node");

//			reader.Read();
			do 
			{
//				if(!reader.Read())
//					break;
				if (reader.Name == "edges" && reader.NodeType==XmlNodeType.EndElement)
					break;
				if (reader.NodeType != XmlNodeType.Element)
					continue;

				// creating new vertex
				IEdge es=(IEdge)edgeSer.Deserialize(reader);

				// get vertices
				es.Source = (IVertex)vertexMap[es.Source.ID];
				es.Target = (IVertex)vertexMap[es.Target.ID];

				Graph.AddEdge(es);
			} while(true);

			return Graph;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		protected string GetTypeQualifiedName(Type t)
		{
			return Assembly.CreateQualifiedName(
				t.Assembly.FullName,
				t.FullName
				);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="name"></param>
		protected void MoveToAttribute(XmlReader reader, string name)
		{
			if(!reader.MoveToAttribute(name))
				throw new Exception("could not find attribute");
			if (!reader.ReadAttributeValue())
				throw new Exception("attribute empty");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		protected void ReadGraphXml(XmlReader reader)
		{
			if (!reader.Read() || reader.Name != "graph")
				throw new Exception("could not find graph node");

			// getting types
			MoveToAttribute(reader,"type");
			Type graphType = Type.GetType(reader.Value,true);

			MoveToAttribute(reader,"vertex-provider-type");
			Type vertexProviderType = Type.GetType(reader.Value,true);

			MoveToAttribute(reader,"edge-provider-type");
			Type edgeProviderType = Type.GetType(reader.Value,true);

			MoveToAttribute(reader,"directed");
			bool directed = bool.Parse(reader.Value);

			MoveToAttribute(reader,"allow-parallel-edges");
			bool allowParallelEdges = bool.Parse(reader.Value);

			// create providers
			IVertexProvider vp = (IVertexProvider)vertexProviderType.GetConstructor(Type.EmptyTypes).Invoke(null);
			IEdgeProvider ep = (IEdgeProvider)edgeProviderType.GetConstructor(Type.EmptyTypes).Invoke(null);

			// create graph
			Type[] gts = new Type[3];
			gts[0]=typeof(IVertexProvider);
			gts[1]=typeof(IEdgeProvider);
			gts[2]=typeof(bool);

			Object[] gps = new Object[3];
			gps[0]=vp;
			gps[1]=ep;
			gps[2]=allowParallelEdges;
			Graph = (ISerializableVertexAndEdgeListGraph)graphType.GetConstructor(gts).Invoke(gps); 
		}
	}
}
