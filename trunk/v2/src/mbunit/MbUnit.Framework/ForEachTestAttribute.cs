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
    /// Used to tag a test method to specify the information contained in an XML file that should be used as 
    /// parameters for that test. To be used in conjunction with the <see cref="DataFixtureAttribute"/> and 
    /// <see cref="XmlDataProviderAttribute"/>.
    /// </summary>
    /// <example>
    /// <para>In this example a test class uses an XML file as its source of data. The file looks like this</para>
    /// <code>
    /// &lt;DataFixture&gt;
    ///   &lt;Employees&gt;
    ///     &lt;User Name="Mickey" LastName="Mouse" /&gt;
    ///   &lt;/Employees&gt;
    ///   &lt;Customers&gt;
    ///     &lt;User Name="Jonathan" LastName="de Halleux" /&gt;
    ///     &lt;User Name="Voldo" LastName="Unkown" /&gt;
    ///   &lt;/Customers&gt;
    /// &lt;/DataFixture&gt;
    /// </code>
    /// <para>The class which maps the data in the XML file to a .NET class looks like this</para>
    /// <code>
    ///    [XmlRoot("User")]
    ///    public class User {
    /// 
    ///        private string name;
    ///        private string lastName;
    /// 
    ///        public User() { }
    ///        [XmlAttribute("Name")]
    ///        public String Name {
    ///            get { return this.name; }
    ///            set { this.name = value; }
    ///        }
    /// 
    ///        [XmlAttribute("LastName")]
    ///        public String LastName {
    ///            get { return this.lastName; }
    ///            set { this.lastName = value; }
    ///        }
    ///    }
    /// </code>
    /// <para>The test class contains test methods with the ForEachTest attribute like this</para>
    /// <code>
    ///    [DataFixture]
    ///    [XmlDataProvider("sample.xml", "//User")]
    ///    [XmlDataProvider("sample.xml", "DataFixture/Customers")]
    ///    public class DataDrivenTests {
    /// 
    ///        [ForEachTest("User")]
    ///        public void ForEachTest(XmlNode node) {
    ///            Assert.IsNotNull(node);
    ///            Assert.AreEqual("User", node.Name);
    ///            Console.WriteLine(node.OuterXml);
    ///        }
    ///
    ///        [ForEachTest("User", DataType = typeof(User))]
    ///        public void ForEachTestWithSerialization(User user) {
    ///            Assert.IsNotNull(user);
    ///            Console.WriteLine(user.ToString());
    ///        }
    ///    }
    /// </code>
    /// </example>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=true)]
    public sealed class ForEachTestAttribute : TestPatternAttribute
    {
		private string xpath;
		private Type dataType = null;
		private XmlSerializer serializer = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="ForEachTestAttribute"/> class.
        /// </summary>
        /// <param name="xpath">The XPath to the data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="xpath"/> is null</exception>
		public ForEachTestAttribute(string xpath)
		:this(xpath,"")
		{}


        /// <summary>
        /// Initializes a new instance of the <see cref="ForEachTestAttribute"/> class.
        /// </summary>
        /// <param name="xpath">The XPath to the data.</param>
        /// <param name="description">A brief description of the test fixture for your reference.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="xpath"/> is null</exception>
        public ForEachTestAttribute(string xpath, string description)
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
