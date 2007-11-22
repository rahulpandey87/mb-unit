using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using MbUnit.Core;
using MbUnit.Core.Runs;
using MbUnit.Core.Framework;

namespace MbUnit.Framework 
{
	
	/// <summary>
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public sealed class SuiteProviderAttribute : TestPatternAttribute
    {
		private string xpath;
		private Type dataType = null;
		private XmlSerializer serializer = null;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="xpath">
		/// XPath to the desired data
		/// </param>
		/// <remarks>
		/// </remarks>
		public SuiteProviderAttribute(string xpath)
		:this(xpath,"")
		{}
		
		/// <summary>
		/// Constructor with a fixture description
		/// </summary>
		/// <param name="xpath">
		/// XPath to the desired data
		/// </param>
		/// <param name="description">fixture description</param>
		/// <remarks>
		/// </remarks>
		public SuiteProviderAttribute(string xpath, string description)
		:base(description)
		{
			this.XPath = xpath;
		}

		public String XPath
		{
			get
			{
				return this.xpath;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.xpath = value;
			}
		}
		
		public Type DataType
		{
			get
			{
				return this.dataType;
			}
			set
			{
				this.dataType = value;	
				if (this.dataType!=null)
					this.serializer = new XmlSerializer(this.dataType);
				else
					this.serializer = null;
			}
		}
		
		public bool IsDeserialized
		{
			get
			{
				return this.dataType != null;
			}
		}
		
		public Object Deserialize(XmlNode node)
		{
			// create reader on the node
			String xml = node.OuterXml;
			StringReader sreader = new StringReader(xml);
			XmlReader reader =new XmlTextReader(sreader);
			return this.serializer.Deserialize(reader);
		}
	}
}
