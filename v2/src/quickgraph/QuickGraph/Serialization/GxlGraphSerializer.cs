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
using System.Collections;

namespace QuickGraph.Serialization
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Concepts.Traversals;
	
	/// <summary>
	/// Graph serializer to the GXL format.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This object serializes outputs to the GXL (http://www.gupro.de/GXL/) 
	/// format.
	/// </para>
	/// </remarks>
	public class GxlGraphSerializer : GraphSerializer
	{
		/// <summary>
		/// Create the graph element and stores graph level data.
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="baseGraph">"base" graph of g</param>
		/// <param name="g">graph to serialize</param>		
		protected override void WriteGraphElem(
			XmlWriter writer,
			ISerializableVertexAndEdgeListGraph baseGraph,
			IVertexAndEdgeListGraph g
			)
		{
			writer.WriteStartElement("gxl");

			// adding xsd ref
			writer.WriteAttributeString("xmlns","http://www.gupro.de/GXL/xmlschema/gxl-1.0.xsd");

			// adding graph node
			writer.WriteStartElement("graph");
			writer.WriteAttributeString("edgeids","true");
			if (g.IsDirected)
				writer.WriteAttributeString("edgemode","directed");
			else
				writer.WriteAttributeString("edgemode","undirected");

			// adding type information
			IGraphSerializationInfo info = new GraphSerializationInfo(true);
			info.Add("graph-type",GetTypeQualifiedName(baseGraph));
			info.Add("vertex-provider-type",GetTypeQualifiedName(baseGraph.VertexProvider));
			info.Add("edge-provider-type",GetTypeQualifiedName(baseGraph.EdgeProvider));
			info.Add("allow-parallel-edges",g.AllowParallelEdges);
			WriteInfo(writer,info);
		}

		/// <summary>
		/// Closes the graph element.
		/// </summary>
		/// <param name="writer">xml writer</param>
		protected override void WriteEndGraphElem(XmlWriter writer)
		{
			// graph
			writer.WriteEndElement();
			// graphml
			writer.WriteEndElement();
		}

		/// <summary>
		/// Writes a vertex element and it's custom data stored in info.
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="v">vertex to store</param>
		/// <param name="info">vertex custom data</param>
		protected override void WriteVertexElem(
			XmlWriter writer,
			IVertex v, 
			GraphSerializationInfo info
			)
		{
			writer.WriteStartElement("node");
			writer.WriteAttributeString("id",FormatID(v));
			WriteInfo(writer,info);
			writer.WriteEndElement(); // node
		}

		/// <summary>
		/// Writes a vertex element and it's custom data stored in info.
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="e">edge to store</param>
		/// <param name="info">edge custom data</param>
		protected override void WriteEdgeElem(
			XmlWriter writer,
			IEdge e, 
			GraphSerializationInfo info
			)
		{
			writer.WriteStartElement("edge");
			writer.WriteAttributeString("id",FormatID(e));
			writer.WriteAttributeString("from",FormatID(e.Source));
			writer.WriteAttributeString("to",FormatID(e.Target));
			WriteInfo(writer,info);
			writer.WriteEndElement(); // node
		}

		/// <summary>
		/// Reads graph data and creates new graph instance
		/// </summary>
		/// <param name="reader">xml reader opened on graph data</param>
		/// <returns>created graph instance</returns>
		protected override ISerializableVertexAndEdgeListGraph ReadGraphElem(XmlReader reader)
		{
			MoveToElement(reader,"gxl");
			MoveToElement(reader,"graph");

			// get directed state
			bool directed = true;
			if (MoveToAttribute(reader,"edgemode",false))
			{
				if (reader.Value!="directed")
					directed = false;
			}

			IGraphSerializationInfo info = ReadInfo(reader);

			// getting types...
			if (this.GraphType==null)
				this.GraphType = Type.GetType(info["graph-type"].ToString(),true);
			if (this.VertexProviderType==null)
				this.VertexProviderType = Type.GetType(info["vertex-provider-type"].ToString(),true);
			if (this.EdgeProviderType==null)
				this.EdgeProviderType = Type.GetType(info["edge-provider-type"].ToString(),true);
			bool allowParallelEdges = bool.Parse(info["allow-parallel-edges"].ToString());


			ISerializableVertexAndEdgeListGraph g = CreateGraph(
				this.GraphType,
				this.VertexProviderType,
				this.EdgeProviderType,
				directed,
				allowParallelEdges
				);
			
			return g;
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		protected override void ReadEndGraphElem(XmlReader reader)
		{
			MovePastEndElement(reader,"graph"); 
			MovePastEndElement(reader,"gxl"); 
		}


		/// <summary>
		/// Reads vertex or edge data
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="g"></param>
		protected override bool ReadVertexOrEdge(
			XmlReader reader,
			ISerializableVertexAndEdgeListGraph g
			)
		{
			if (reader.Name == "node")
			{
				ReadVertex(reader,g);
				return true;
			}
			else if (reader.Name == "edge")
			{
				ReadEdge(reader,g);
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="g"></param>
		protected void ReadVertex(
			XmlReader reader,
			ISerializableVertexAndEdgeListGraph g
			)
		{
			MoveToAttribute(reader,"id",true);
			string id = this.ParseVertexID(reader.Value);

			// add vertex.
			IVertex v=g.AddVertex();

			// add to table
			CreatedVertices[id]=v;

			GraphSerializationInfo info = ReadInfo(reader);
			if (info != null)
				((IGraphDeSerializable)v).ReadGraphData(info);

			this.MovePastEndElement(reader,"node");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="g"></param>
		protected void ReadEdge(
			XmlReader reader,
			ISerializableVertexAndEdgeListGraph g
			)
		{
			MoveToAttribute(reader,"id",true);
			string id = this.ParseEdgeID(reader.Value);

			MoveToAttribute(reader,"from",true);
			string sourceid = ParseVertexID(reader.Value);
			MoveToAttribute(reader,"to",true);
			string targetid = ParseVertexID(reader.Value);

			// add edge.
			IEdge e=g.AddEdge(
				(IVertex)CreatedVertices[sourceid],
				(IVertex)CreatedVertices[targetid]
				);

			// add to table
			CreatedEdges[id]=e;

			GraphSerializationInfo info = ReadInfo(reader);
			if (info != null)
				((IGraphDeSerializable)e).ReadGraphData(info);

			this.MovePastEndElement(reader,"edge");		
		}

		/// <summary>
		/// Writes custom info to GraphMl
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="info">custom data</param>
		protected void WriteInfo(
			XmlWriter writer,
			IGraphSerializationInfo info)
		{
			if (info.Count == 0)
				return;

			foreach(DictionaryEntry de in info)
			{
				writer.WriteStartElement("attr");
					writer.WriteAttributeString("name",(string)de.Key);
					writer.WriteStartElement(de.Value.GetType().ToString());
						writer.WriteString(de.Value.ToString());
					writer.WriteEndElement();
				writer.WriteEndElement();        
			}
		}


		/// <summary>
		/// Reads custom info from GraphMl
		/// </summary>
		/// <param name="reader">xml reader</param>
		/// <returns>custom data</returns>
		protected GraphSerializationInfo ReadInfo(
			XmlReader reader)
		{
			GraphSerializationInfo info = new GraphSerializationInfo(false);

			while (MoveToElement(reader,"attr"))
			{
				MoveToAttribute(reader,"name",true);
				string key = reader.Value;
				if (!MoveNextElement(reader))
					throw new Exception("expected data, not found");
				string t = reader.Name;
				if (!reader.Read())
					throw new Exception("expected data, not found");
				string value = reader.Value;
				info.Add(key,value);
				MovePastEndElement(reader,t);
				MovePastEndElement(reader,"attr");
			}

			if (info.Count>0)
				return info;
			else
				return null;
		}
	}
}
