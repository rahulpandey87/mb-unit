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
//		QuickGraph Library HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux



using System;
using System.Xml;
using System.Reflection;
using System.Collections;

namespace QuickGraph.Serialization
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Concepts.Traversals;
	using QuickGraph.Concepts.Providers;
	using QuickGraph.Collections;
	using QuickGraph.Exceptions;

	/// <summary>
	/// Base class for Graph serializers.
	/// </summary>
	/// <example>
	/// This example takes an AdjacencyGraph and serializes it to GraphML
	/// format:
	/// <code>
	/// // getting xml writer
	/// XmlTextWriter writer = new XmlTextWriter(Console.Out);
	/// writer.Formatting = Formatting.Indented;
	/// // gettin graph
	/// AdjacencyGraph g = ...;
	/// GraphSerializer ser = new GraphMlSerializer();
	/// // serialize to GraphML
	/// ser.Serialize(writer,g);
	/// </code>
	/// </example>
	public abstract class GraphSerializer
	{
		private Hashtable createdVertices = null;
		private Hashtable createdEdges = null;
		private Type graphType = null;
		private Type vertexProviderType = null;
		private Type edgeProviderType = null;

		/// <summary>
		/// Default constructor
		/// </summary>
		protected GraphSerializer()
		{}

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
					this.vertexProviderType.GetInterface("IVertexProvider",true)==null)
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
					this.edgeProviderType.GetInterface("IEdgeProvider",true)==null)
					throw new ArgumentException("type does not implement IEdgeProvider");
			}
		}

		/// <summary>
		/// Created vertices table
		/// </summary>
		public Hashtable CreatedVertices
		{
			get
			{
				return this.createdVertices;
			}
		}

		/// <summary>
		/// Created vertices table
		/// </summary>
		public Hashtable CreatedEdges
		{
			get
			{
				return this.createdEdges;
			}
		}

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
			if (writer==null)
				throw new ArgumentNullException("writer");
			if (baseGraph==null)
				throw new ArgumentNullException("baseGraph");
			if (g==null)
				throw new ArgumentNullException("g");

			// Add graph node
			WriteGraphElem(writer,baseGraph,g);

			// add vertices
			foreach(IVertex v in g.Vertices)
			{
				// get vertex data
				GraphSerializationInfo info = new GraphSerializationInfo(true);
				((IGraphSerializable)v).WriteGraphData(info);
				// write it to xml
				WriteVertexElem(writer,v,info);
			}

			// add edges
			foreach(IEdge e in g.Edges)
			{
				// get edge data
				GraphSerializationInfo info = new GraphSerializationInfo(true);
				((IGraphSerializable)e).WriteGraphData(info);
				// write to xml
				WriteEdgeElem(writer,e,info);
			}

			// finish graph node
			WriteEndGraphElem(writer);
		}


		/// <summary>
		/// Deserializes data from Xml stream.
		/// </summary>
		/// <param name="reader">xml stream</param>
		/// <returns>deserialized data</returns>
		public ISerializableVertexAndEdgeListGraph Deserialize(XmlReader reader)
		{
			if (reader==null)
				throw new ArgumentNullException("reader");

			this.createdVertices = new Hashtable();
			this.createdEdges = new Hashtable();
			ISerializableVertexAndEdgeListGraph g=ReadGraphElem(reader);

			do
			{
				if (reader.NodeType==XmlNodeType.Element)
				{
					if (!ReadVertexOrEdge(reader,g))
						break;
				}
				else if (reader.NodeType==XmlNodeType.EndElement)
					break;
				if (!reader.Read())
					break;
			}while(reader.Read());

			ReadEndGraphElem(reader);

			return g;
		}

		#region Writer Abstract methods
		/// <summary>
		/// Create the graph element and stores graph level data.
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="baseGraph">"base" graph of g</param>
		/// <param name="g">graph to serialize</param>
		protected abstract void WriteGraphElem(
			XmlWriter writer,
			ISerializableVertexAndEdgeListGraph baseGraph,
			IVertexAndEdgeListGraph g
			);

		/// <summary>
		/// Closes the graph element.
		/// </summary>
		/// <param name="writer">xml writer</param>
		protected abstract void WriteEndGraphElem(
			XmlWriter writer
			);

		/// <summary>
		/// Writes a vertex element and it's custom data stored in info.
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="v">vertex to store</param>
		/// <param name="info">vertex custom data</param>
		protected abstract void WriteVertexElem(
			XmlWriter writer,
			IVertex v, 
			GraphSerializationInfo info
			);

		/// <summary>
		/// Writes a vertex element and it's custom data stored in info.
		/// </summary>
		/// <param name="writer">xml writer</param>
		/// <param name="e">edge to store</param>
		/// <param name="info">edge custom data</param>
		protected abstract void WriteEdgeElem(
			XmlWriter writer,
			IEdge e, 
			GraphSerializationInfo info
			);
		#endregion

		#region Reader abstract methods
		/// <summary>
		/// Reads graph data and creates new graph instance
		/// </summary>
		/// <param name="reader">xml reader opened on graph data</param>
		/// <returns>created graph instance</returns>
		protected abstract ISerializableVertexAndEdgeListGraph ReadGraphElem(XmlReader reader);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		protected abstract void ReadEndGraphElem(XmlReader reader);

		/// <summary>
		/// Reads vertex or edge data
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="g"></param>
		protected abstract bool ReadVertexOrEdge(
			XmlReader reader,
			ISerializableVertexAndEdgeListGraph g
			);
		#endregion

		#region Helper Methods
		/// <summary>
		/// Formats the edge ID number
		/// </summary>
		/// <param name="e">edge</param>
		/// <returns>e.ID formatted</returns>
		protected string FormatID(IEdge e)
		{
			return String.Format("e{0}",e.ID.ToString());
		}


		/// <summary>
		/// Parses vertex id of the form 'vdd' where dd is the id number
		/// </summary>
		/// <param name="id">id identifier</param>
		/// <returns>id number</returns>
		protected string ParseVertexID(string id)
		{
			if (id==null)
				throw new ArgumentNullException("id");
			return id;
		}

		/// <summary>
		/// Parses edge id of the form 'edd' where dd is the id number
		/// </summary>
		/// <param name="id">id identifier</param>
		/// <returns>id number</returns>
		protected string ParseEdgeID(string id)
		{
			if (id==null)
				throw new ArgumentNullException("id");
			return id;
		}

		/// <summary>
		/// Formats the vertex ID number
		/// </summary>
		/// <param name="v">vertex</param>
		/// <returns>v.ID formatted</returns>
		protected string FormatID(IVertex v)
		{
			return String.Format("v{0}",v.ID.ToString());
		}

		/// <summary>
		/// Returns qualifed type name of o
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		protected string GetTypeQualifiedName(Object o)
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
		protected bool MoveToAttribute(XmlReader reader, string name, bool throwIfNotFound)
		{
			if(!reader.MoveToAttribute(name))
			{
				if (throwIfNotFound)
					throw new AttributeNotFoundException(name);
				return false;
			}
			if (!reader.ReadAttributeValue())
			{
				if (throwIfNotFound)
					throw new AttributeNotFoundException(name);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Moves reader to element with name = name
		/// </summary>
		/// <param name="reader"></param>
		protected bool MoveNextElement(XmlReader reader)
		{
			while(reader.Read())
			{
				if (reader.NodeType==XmlNodeType.Element)
					return true;

				if (reader.NodeType == XmlNodeType.EndElement)
					return false;
			}
			return false;
		}

		/// <summary>
		/// Moves reader to element with name = name
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="name"></param>
		protected bool MoveToElement(XmlReader reader, string name)
		{
			if (reader.NodeType==XmlNodeType.Element && reader.Name == name)
				return true;

			while(reader.Read())
			{
				if (reader.NodeType==XmlNodeType.Element)
					return reader.Name == name;

				if (reader.NodeType == XmlNodeType.EndElement)
					return false;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="name"></param>
		/// <param name="name2"></param>
		/// <returns></returns>
		protected bool MoveToElement(XmlReader reader, string name, string name2)
		{
			if (reader.NodeType==XmlNodeType.Element && (reader.Name == name || reader.Name==name2))
				return true;

			while(reader.Read())
			{
				if (reader.NodeType==XmlNodeType.Element)
					return (reader.Name == name|| reader.Name==name2);

				if (reader.NodeType == XmlNodeType.EndElement)
					return false;
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="name"></param>
		protected void MovePastEndElement(XmlReader reader,string name)
		{
			do {
				// <name /> case
				if (reader.Name==name && reader.NodeType==XmlNodeType.Element)
					return;

				// </name> case
				if (reader.Name==name && reader.NodeType == XmlNodeType.EndElement)
					return;
			} while(reader.Read());
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
		protected ISerializableVertexAndEdgeListGraph CreateGraph(
			Type graphType, 
			Type vertexProviderType, 
			Type edgeProviderType,
			bool directed,
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

		#endregion

	}
}
