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


namespace QuickGraph
{
	using System;
	using System.Xml.Serialization;
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Serialization;

	/// <summary>
	/// A Graph Vertex.
	/// </summary>
	[Serializable]
	[XmlRoot("vertex")]
	public class Vertex : IVertex, IGraphBiSerializable
	{	
		private int m_ID;

		/// <summary>
		/// Default constructor. Used for serialization.
		/// </summary>
		public Vertex()
		{
			m_ID  = -1;
		}

		/// <summary>
		/// Builds a new vertex
		/// </summary>
		public Vertex(int id)
		{
			m_ID = id;
		}

		/// <summary>
		/// Unique identification number
		/// </summary>
		[XmlAttribute("vertex-id")]
		public int ID
		{
			get
			{
				return m_ID;
			}
			set
			{
				m_ID = value;
			}
		}

		#region IComparable implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static bool operator < (Vertex v1, Vertex v2)
		{
			return v1.CompareTo(v2)<0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="v1"></param>
		/// <param name="v2"></param>
		/// <returns></returns>
		public static bool operator > (Vertex v1, Vertex v2)
		{
			return v1.CompareTo(v2)>0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return this.CompareTo((Vertex)obj)==0;
		}


		/// <summary>
		/// Compares two vertices
		/// </summary>
		/// <param name="obj">vertex to compare</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">obj is not of type Vertex</exception>
		public int CompareTo(Vertex obj)
		{
			if(obj==null)
				return -1;
			return ID.CompareTo(obj.ID);
		}

		int IComparable.CompareTo(Object obj)
		{
			return this.CompareTo((Vertex)obj);
		}
		#endregion

		/// <summary>
		/// Converts to string by returning the ID.
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
		/// Hash code. ID used as identification number.
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
