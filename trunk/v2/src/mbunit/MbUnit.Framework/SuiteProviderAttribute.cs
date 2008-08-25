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
    /// Used to tag a test method within a test suite to specify the information 
    /// contained in an XML file that should be used as parameters for that test. 
	/// </summary>
	/// <seealso cref="ForEachTestAttribute"/>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public sealed class SuiteProviderAttribute : TestPatternAttribute
    {
		private string xpath;
		private Type dataType = null;
		private XmlSerializer serializer = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteProviderAttribute"/> class.
        /// </summary>
        /// <param name="xpath">The XPath to the data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="xpath"/> is null</exception>
		public SuiteProviderAttribute(string xpath)
		:this(xpath,"")
		{}

        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteProviderAttribute"/> class.
        /// </summary>
        /// <param name="xpath">The XPath to the data.</param>
        /// <param name="description">A brief description of the data being used</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="xpath"/> is null</exception>
		public SuiteProviderAttribute(string xpath, string description)
		:base(description)
		{
			this.XPath = xpath;
		}

        /// <summary>
        /// Gets or sets the XPath to the data for the test.
        /// </summary>
        /// <value>The XPath string.</value>
        /// <exception cref="ArgumentNullException">Thrown if the value is null</exception>
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

        /// <summary>
        /// Gets or sets the class <see cref="Type"/> the xml data will be deserialized into for use in your tests
        /// </summary>
        /// <value>The class <see cref="Type"/> for your tests.</value>
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

        /// <summary>
        /// Gets a value indicating whether this instance is deserialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is deserialized; otherwise, <c>false</c>.
        /// </value>
		public bool IsDeserialized
		{
			get
			{
				return this.dataType != null;
			}
		}

        /// <summary>
        /// Deserializes the specified XML node for use in a test.
        /// </summary>
        /// <param name="node">The node to deserialize.</param>
        /// <returns>An object of the type specified by <see cref="DataType"/></returns>
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
