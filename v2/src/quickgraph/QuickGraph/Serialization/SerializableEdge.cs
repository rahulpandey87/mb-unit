using System;
using System.Collections;
using System.Collections.Specialized;

namespace QuickGraph.Serialization
{
	using QuickGraph.Concepts;
	using QuickGraph.Concepts.Serialization;
	using QuickGraph.Serialization;
	using QuickGraph;

	/// <summary>
	/// Summary description for SerializableEdge.
	/// </summary>
	public class SerializableEdge : Edge
	{
		private StringDictionary entries = new StringDictionary();	
	
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="id"></param>
		public SerializableEdge(int id, IVertex source, IVertex target)
			:base(id,source,target)
		{}

		/// <summary>
		/// Gets the dictionary of key-and-value pairs
		/// </summary>
		/// <value>
		/// Data entries
		/// </value>
		public StringDictionary Entries
		{
			get
			{
				return this.entries;
			}
		}

		/// <summary>
		/// Adds nothing to serialization info
		/// </summary>
		/// <param name="info">data holder</param>
		/// <exception cref="ArgumentNullException">info is null</exception>
		/// <exception cref="ArgumentException">info is not serializing</exception>
		public override void WriteGraphData(IGraphSerializationInfo info)
		{
			if (info==null)
				throw new ArgumentNullException("info");
			if (!info.IsSerializing)
				throw new ArgumentException("not serializing");
			base.WriteGraphData(info);

			foreach(DictionaryEntry de in this.entries)
			{
				info.Add(de.Key.ToString(), de.Value.ToString());
			}
		}

		/// <summary>
		/// Reads no data from serialization info
		/// </summary>
		/// <param name="info">data holder</param>
		/// <exception cref="ArgumentNullException">info is null</exception>
		/// <exception cref="ArgumentException">info is serializing</exception>
		public override void ReadGraphData(IGraphSerializationInfo info)
		{
			if (info==null)
				throw new ArgumentNullException("info");
			if (info.IsSerializing)
				throw new ArgumentException("serializing");
			base.ReadGraphData(info);

			foreach(DictionaryEntry de in info)
			{
				this.entries[de.Key.ToString()]=de.Value.ToString();
			}
		}
	}
}
