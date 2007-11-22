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
using System.Collections;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Xml.Schema;

namespace QuickGraph.Serialization
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Serialization.GraphML;
	using QuickGraph.Concepts.Providers;
	

	/// <summary>
	/// Graph serializer to the GraphML format.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This object serializes outputs to the GraphML 
	/// (http://graphml.graphdrawing.org/) format.
	/// </para>
	/// </remarks>
	/// <include file='QuickGraph.Serialization.Doc.xml' path='doc/examples/example[@name="SaveGraphML"]'/>
	public class GraphMLGraphSerializer
	{
		const string graphTypeKeyName = @"graph-type";
		const string vertexProviderTypeKeyName = @"vertex-provider-type";
		const string edgeProviderTypeKeyName = @"edge-provider-type";
		const string allowParallelEdgesKeyName = @"allow-parallel-edges";

		private string dtdPath = "http://graphml.graphdrawing.org/dtds/1.0rc/graphml.dtd";
		private bool typeFromXml = false;

		private Hashtable createdVertices = null;
		private Hashtable createdEdges = null;
		private Type graphType = null;
		private Type vertexProviderType = typeof(SerializableVertexProvider);
		private Type edgeProviderType = typeof(SerializableEdgeProvider);
		private bool allowParallelEdges = true;

		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphMLGraphSerializer()
		{}

		public GraphMLGraphSerializer(string dtdPath)
		{
			this.dtdPath = dtdPath;
		}

		public bool TypeFromXml
		{
			get
			{
				return this.typeFromXml;
			}
			set
			{
				this.typeFromXml = value;
			}
		}

		public Type GraphType
		{
			get
			{
				return this.graphType;
			}
			set
			{
				this.graphType  = value;
				if (this.graphType!=null &&
					this.graphType.GetInterface("ISerializableVertexAndEdgeListGraph",true)==null)
					throw new ArgumentException("type does not implement ISerializableVertexAndEdgeListGraph");
			}
		}

		public Type VertexProviderType
		{
			get
			{
				return this.vertexProviderType;
			}
			set
			{
				this.vertexProviderType = value;
				if (this.vertexProviderType!=null && 
					this.vertexProviderType.GetInterface("IVertexProvider",true)==null
					)
					throw new ArgumentException("type does not implement IVertexProvider");
			}
		}

		public Type EdgeProviderType
		{
			get
			{
				return this.edgeProviderType;
			}
			set
			{
				this.edgeProviderType = value;
				if (this.edgeProviderType!=null && 
					this.edgeProviderType.GetInterface("IEdgeProvider",true)==null
					)
					throw new ArgumentException("type does not implement IEdgeProvider");
			}
		}

		public bool AllowParallelEdges
		{
			get
			{
				return this.allowParallelEdges;
			}
			set
			{
				this.allowParallelEdges = value;
			}
		}

		public string DtdPath
		{
			get
			{
				return this.dtdPath;
			}
			set
			{
				this.dtdPath = value;
			}
		}

		/// <summary>
		/// Created vertices table
		/// </summary>
		private Hashtable CreatedVertices
		{
			get
			{
				return this.createdVertices;
			}
		}

		/// <summary>
		/// Created vertices table
		/// </summary>
		private Hashtable CreatedEdges
		{
			get
			{
				return this.createdEdges;
			}
		}

		public static void Validate(XmlReader reader)
		{
			XmlValidatingReader vr = new XmlValidatingReader(reader);

			vr.ValidationType = ValidationType.Auto;
			vr.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

			while (vr.Read()){};
		}

		private static void ValidationHandler(object sender, ValidationEventArgs args)
		{
			Debug.WriteLine(args.ToString());
		}

		#region Serialization
		/// <summary>
		/// Serializes g to xml
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="g">graph to serialize</param>
		/// <exception cref="ArgumentNullException">writer or g are null</exception>
		/// <exception cref="ArgumentException">g vertex or edge does not
		/// implement <see cref="IGraphSerializable"/>.
		/// </exception>
		public void Serialize(
			XmlWriter writer, 
			ISerializableVertexAndEdgeListGraph g
			)
		{
			Serialize(writer,g,g);
		}

		/// <summary>
		/// Serializes the filtered graph g to xml
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="baseGraph">"base" graph of g</param>
		/// <param name="g">graph to serialize</param>
		/// <exception cref="ArgumentNullException">writer or g are null</exception>
		/// <exception cref="ArgumentException">g vertex or edge does not
		/// implement <see cref="QuickGraph.Concepts.Serialization.IGraphSerializable"/>.
		/// </exception>
		public void Serialize(
			XmlWriter writer, 
			ISerializableVertexAndEdgeListGraph baseGraph,
			IVertexAndEdgeListGraph g
			)
		{
			GraphMltype graphml = new GraphMltype();

			KeyType graphTypeKey = new KeyType();
			graphTypeKey.ID = graphTypeKeyName;
			graphml.Key.AddKeyType(graphTypeKey);

			KeyType vertexProviderTypeKey = new KeyType();
			vertexProviderTypeKey.ID = vertexProviderTypeKeyName;
			graphml.Key.AddKeyType(vertexProviderTypeKey);

			KeyType edgeProviderTypeKey = new KeyType();
			edgeProviderTypeKey.ID = edgeProviderTypeKeyName;
			graphml.Key.AddKeyType(edgeProviderTypeKey);

			KeyType allowParralelEdgeKey = new KeyType();
			allowParralelEdgeKey.ID = allowParallelEdgesKeyName;
			graphml.Key.AddKeyType(allowParralelEdgeKey);

			KeyType nameKey = new KeyType();
			nameKey.ID = @"name";
			graphml.Key.AddKeyType(nameKey);

			GraphType graph = SerializeGraph(baseGraph,g);
			graphml.Items.AddGraph(graph);

			// add dtd
			// <!DOCTYPE graphml PUBLIC "-GraphML DTD" "http://graphml.graphdrawing.org/dtds/1.0rc/graphml.dtd">
			//writer.WriteDocType("graphml","-GraphML DTD",this.dtdPath,null);

			// serialize
			XmlSerializer ser = new XmlSerializer(typeof(GraphMltype));
			ser.Serialize(writer,graphml);
		}

		private GraphType SerializeGraph(
			ISerializableVertexAndEdgeListGraph baseGraph,
			IVertexAndEdgeListGraph g)
		{
			// create graph node
			GraphType graph = new GraphType();			

			// fill up graph
			if (g.IsDirected)
				graph.EdgeDefault = GraphEdgeDefaultType.Directed;
			else
				graph.EdgeDefault= GraphEdgeDefaultType.Undirected;

			// adding type information
			IGraphSerializationInfo info = new GraphSerializationInfo(true);
			info.Add(graphTypeKeyName,GetTypeQualifiedName(baseGraph));
			info.Add(vertexProviderTypeKeyName,GetTypeQualifiedName(baseGraph.VertexProvider));
			info.Add(edgeProviderTypeKeyName,GetTypeQualifiedName(baseGraph.EdgeProvider));
			info.Add(allowParallelEdgesKeyName,g.AllowParallelEdges);
			
			// add data...
			foreach(DataType dt in ToDatas(info))
			{
				graph.Items.AddData(dt);
			}

			// add vertices
			foreach(IVertex v in g.Vertices)
				graph.Items.AddNode( SerializeVertex(v) );

			// add edges
			foreach(IEdge e in g.Edges)
				graph.Items.AddEdge( SerializeEdge(e) );

			return graph;
		}

		private NodeType SerializeVertex(IVertex v)
		{
			NodeType node = new NodeType();
			node.ID = FormatID(v);
			GraphSerializationInfo info = new GraphSerializationInfo(true);
			((IGraphSerializable)v).WriteGraphData(info);

			foreach(DataType dt in ToDatas(info))
			{
				node.Items.AddData(dt);
			}

			return node;
		}

		private EdgeType SerializeEdge(IEdge e)
		{
			EdgeType edge = new EdgeType();
			edge.ID = FormatID(e);
			edge.Source = FormatID(e.Source);
			edge.Target = FormatID(e.Target);
			edge.Directed = true;

			GraphSerializationInfo info = new GraphSerializationInfo(true);
			((IGraphSerializable)e).WriteGraphData(info);

			foreach(DataType dt in ToDatas(info))
			{
				edge.Data.AddDataType(dt);
			}

			return edge;
		}
		#endregion

		#region Deserialization
		public ISerializableVertexAndEdgeListGraph Deserialize(XmlReader reader)
		{
			this.createdEdges = new Hashtable();
			this.createdVertices = new Hashtable();

			// rebuild graphml object
			XmlSerializer ser = new XmlSerializer(typeof(GraphMltype));
			GraphMltype graphml = (GraphMltype)ser.Deserialize(reader);

			// get graph
			GraphType gt = null;
			foreach(Object item in graphml.Items)
			{
				gt = item as GraphType;
				if (gt!=null)
					break;
			}


			if (gt==null)
				throw new ArgumentException("no graph information found");


			// retreive data for reflection
			if (this.typeFromXml)
			{
				foreach(Object item in gt.Items)
				{
					DataType dt  = item as DataType;
					if (dt==null)
						continue;
					switch(dt.Key)
					{
						case graphTypeKeyName:
							this.graphType = ToType(TextToString(dt.Text));
							break;
						case vertexProviderTypeKeyName:
							this.vertexProviderType = ToType(TextToString(dt.Text));
							break;
						case edgeProviderTypeKeyName:
							this.edgeProviderType = ToType(TextToString(dt.Text));
							break;
						case allowParallelEdgesKeyName:
							this.allowParallelEdges= ToBool(TextToString(dt.Text));
							break;
					}
				}
			}

			if (this.GraphType==null)
				throw new InvalidOperationException("GraphType is null");
			if (this.VertexProviderType==null)
				throw new InvalidOperationException("VertexProviderType is null");
			if (this.EdgeProviderType==null)
				throw new InvalidOperationException("EdgeProviderType is null");

			// create graph
			ISerializableVertexAndEdgeListGraph g = CreateGraph(
				this.GraphType,
				this.VertexProviderType,
				this.EdgeProviderType,
				gt.EdgeDefault,
				this.allowParallelEdges
				);

			// populate graph vertices
			bool isVertexDeserialiable = 
				typeof(IGraphDeSerializable).IsAssignableFrom(g.VertexProvider.VertexType);
			foreach(Object item in gt.Items)
			{
				NodeType node = item as NodeType;
				if (node==null)
					continue;

				IVertex v = g.AddVertex();
				this.CreatedVertices[node.ID]=v;
				if (isVertexDeserialiable)
				{
					IGraphSerializationInfo info = InfoFromNode(node);
					((IGraphDeSerializable)v).ReadGraphData(info);
				}
			}

			bool isEdgeDeserialiable = 
				typeof(IGraphDeSerializable).IsAssignableFrom(g.EdgeProvider.EdgeType);
			foreach(Object item in gt.Items)
			{
				EdgeType edge = item as EdgeType;
				if (edge==null)
					continue;

				IEdge e = g.AddEdge(
					(IVertex)this.CreatedVertices[edge.Source],
					(IVertex)this.CreatedVertices[edge.Target]
					);
				this.CreatedEdges[edge.ID]=e.ID;

				if (isEdgeDeserialiable)
				{
					IGraphSerializationInfo info = InfoFromEdge(edge);
					((IGraphDeSerializable)e).ReadGraphData(info);
				}
			}

			return g;
		}
		#endregion

		#region Helpers

		private string TextToString(DataType.Textcollection text)
		{
			StringWriter sw = new StringWriter();
			foreach(string s in text)
				sw.Write(s);
			return sw.ToString();
		}

		/// <summary>
		/// Formats the vertex ID number
		/// </summary>
		/// <param name="v">vertex</param>
		/// <returns>v.ID formatted</returns>
		private string FormatID(IVertex v)
		{
			return String.Format("v{0}",v.ID.ToString());
		}

		/// <summary>
		/// Formats the edge ID number
		/// </summary>
		/// <param name="e">edge</param>
		/// <returns>e.ID formatted</returns>
		private string FormatID(IEdge e)
		{
			return String.Format("e{0}",e.ID.ToString());
		}

		private DataType[] ToDatas(IGraphSerializationInfo info)
		{
			DataType[] dts = new DataType[info.Count];

			int i = 0;
			foreach(DictionaryEntry de in info)
			{
				dts[i] = new DataType();
				dts[i].Key = de.Key.ToString();
				if (de.Value!=null)
					dts[i].Text.AddString(de.Value.ToString());
				++i;
			}
			return dts;
		}

		/// <summary>
		/// Returns qualifed type name of o
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		private string GetTypeQualifiedName(Object o)
		{
			if (o==null)
				throw new ArgumentNullException("o");
			return this.GetTypeQualifiedName(o.GetType());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private string GetTypeQualifiedName(Type t)
		{
			return Assembly.CreateQualifiedName(
				t.Assembly.FullName,
				t.FullName
				);
		}

		private Type ToType(string text)
		{
			return Type.GetType(text,true);
		}

		private bool ToBool(string text)
		{
			return bool.Parse(text);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="graphType"></param>
		/// <param name="vertexProviderType"></param>
		/// <param name="edgeProviderType"></param>
		/// <param name="directed"></param>
		/// <param name="allowParallelEdges"></param>
		/// <returns></returns>
		private ISerializableVertexAndEdgeListGraph CreateGraph(
			Type graphType, 
			Type vertexProviderType, 
			Type edgeProviderType,
			GraphEdgeDefaultType directed,
			bool allowParallelEdges
			)
		{
			// create providers
			IVertexProvider vp = 
				(IVertexProvider)vertexProviderType.GetConstructor(Type.EmptyTypes).Invoke(null);
			IEdgeProvider ep = 
				(IEdgeProvider)edgeProviderType.GetConstructor(Type.EmptyTypes).Invoke(null);

			// create graph
			Type[] gts = new Type[3];
			gts[0]=typeof(IVertexProvider);
			gts[1]=typeof(IEdgeProvider);
			gts[2]=typeof(bool);

			Object[] gps = new Object[3];
			gps[0]=vp;
			gps[1]=ep;
			gps[2]=allowParallelEdges;

			return (ISerializableVertexAndEdgeListGraph)graphType.GetConstructor(gts).Invoke(gps); 
		}

		private IGraphSerializationInfo InfoFromNode(NodeType node)
		{
			GraphSerializationInfo info = new GraphSerializationInfo(false);

			foreach(Object o in node.Items)
			{
				DataType dt = o as DataType;
				if (dt==null)
					continue;
				info.Add(dt.Key, dt.Text.ToString());
			}

			return info;
		}

		private IGraphSerializationInfo InfoFromEdge(EdgeType edge)
		{
			GraphSerializationInfo info = new GraphSerializationInfo(false);

			foreach(DataType dt in edge.Data)
			{
				info.Add(dt.Key, dt.Text);
			}

			return info;
		}

		#endregion
	}
}
