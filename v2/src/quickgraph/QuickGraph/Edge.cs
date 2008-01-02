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


namespace QuickGraph
{
	using System;
	using System.Xml.Serialization;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Serialization;

	/// <summary>
	/// A graph edge
	/// </summary>
	/// <remarks>
	/// This class represents a directed edge. It links
	/// a source <seealso cref="Vertex"/> to a target <seealso cref="Vertex"/>.
	/// 
	/// The source and target vertices can be accessed as properties.
	/// </remarks>
	/// <example>
	/// This sample shows a basic usage of an edge:
	/// <code>
	/// Vertex v;   // vertex
	/// foreach(Edge e in v.InEdges)
	/// {
	///     Console.WriteLine("{0} -> {1}",
	///			e.Source.GetHashCode(),
	///			e.Target.GetHashCode()
	///			);
	/// }
	/// </code>
	/// </example>
	[Serializable]
	[XmlRoot("edge")]
	public class Edge : IEdge, IGraphBiSerializable
	{
		private int id;
		private IVertex source;
		private IVertex target;

		/// <summary>
		/// Empty Method. Used for serialization.
		/// </summary>
		public Edge()
		{
			this.id = -1;
			this.source = null;
			this.target = null;
		}

		/// <summary>
		/// Builds an edge from source to target
		/// </summary>
		/// <param name="id">unique identification number</param>
		/// <param name="source">Source vertex</param>
		/// <param name="target">Target vertex</param>
		/// <exception cref="ArgumentNullException">Source or Target is null</exception>
		public Edge(int id, IVertex source, IVertex target)
		{
			if (source == null)
				throw new ArgumentNullException("Source");
			if (target == null)
				throw new ArgumentNullException("Target");
			this.id = id;
			this.source = source;
			this.target = target;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e1"></param>
		/// <param name="e2"></param>
		/// <returns></returns>
		public static bool operator < (Edge e1, Edge e2)
		{
			return e1.CompareTo(e2)<0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e1"></param>
		/// <param name="e2"></param>
		/// <returns></returns>
		public static bool operator > (Edge e1, Edge e2)
		{
			return e1.CompareTo(e2)>0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return this.CompareTo((Edge)obj)==0;
		}

		/// <summary>
		/// Compares two edges
		/// </summary>
		/// <param name="obj">Edge to compare</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">obj is not of type Edge.</exception>
		public int CompareTo(Edge obj)
		{
			if(obj==null)
				return -1;
			return ID.CompareTo(obj.ID);
		}

		int IComparable.CompareTo(Object obj)
		{
			return this.CompareTo((Edge)obj);
		}

		/// <summary>
		/// Converts to string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return ID.ToString();
		}

		/// <summary>
		/// Converts to string by returning the formatted ID
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		public string ToString(IFormatProvider provider)
		{
			return ID.ToString(provider);
		}

		/// <summary>
		/// Edge unique identification number
		/// </summary>
		[XmlAttribute("edge-id")]
		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		/// <summary>
		/// Source vertex
		/// </summary>
		[XmlIgnore]
		public IVertex Source
		{
			get
			{
				return this.source;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("source vertex");
				this.source = value;
			}
		}

		/// <summary>
		/// Source vertex id, for serialization
		/// </summary>
		[XmlAttribute("source")]
		public int SourceID
		{
			get
			{
				return this.source.ID;
			}
			set
			{
				this.source = new Vertex(value);
			}
		}

		/// <summary>
		/// Target Vertex
		/// </summary>
		[XmlIgnore]
		public IVertex Target
		{
			get
			{
				return target;
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("target vertex");
				target = value;
			}
		}

		/// <summary>
		/// Source vertex id, for serialization
		/// </summary>
		[XmlAttribute("target")]
		public int TargetID
		{
			get
			{
				return this.target.ID;
			}
			set
			{
				this.target = new Vertex(value);
			}
		}

		/// <summary>
		/// Hash code, using ID
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Adds nothing to serialization info
		/// </summary>
		/// <param name="info">data holder</param>
		/// <exception cref="ArgumentNullException">info is null</exception>
		/// <exception cref="ArgumentException">info is not serializing</exception>
		public virtual void WriteGraphData(IGraphSerializationInfo info)
		{
			if (info==null)
				throw new ArgumentNullException("info");
			if (!info.IsSerializing)
				throw new ArgumentException("not serializing");
		}

		/// <summary>
		/// Reads no data from serialization info
		/// </summary>
		/// <param name="info">data holder</param>
		/// <exception cref="ArgumentNullException">info is null</exception>
		/// <exception cref="ArgumentException">info is serializing</exception>
		public virtual void ReadGraphData(IGraphSerializationInfo info)
		{
			if (info==null)
				throw new ArgumentNullException("info");
			if (info.IsSerializing)
				throw new ArgumentException("serializing");
		}
	}
}
