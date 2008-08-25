
namespace MbUnit.Framework
{
	using System;
	using System.Xml;
	using MbUnit.Core.Runs;
	using System.Reflection;
	using System.IO;

    /// <summary>
    /// Used to tag a test fixture class to indicate that it uses information contained in an XML file that should be used as 
    /// parameters for test contained in the class. To be used in conjunction with the <see cref="DataFixtureAttribute"/> and 
    /// <see cref="ForEachTestAttribute"/>.
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
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
	public class XmlDataProviderAttribute : DataProviderFixtureDecoratorAttribute
	{
		private string xpath;
		private string resourceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataProviderAttribute"/> class.
        /// </summary>
        /// <param name="resourceName">The path to the XML file</param>
        /// <param name="xpath">The XPath expression identifying the data to be used in the XMl file</param>
		public XmlDataProviderAttribute(string resourceName, string xpath)
			:base()
		{
			this.ResourceName = resourceName;
			this.XPath = xpath;
		}

        /// <summary>
        /// Gets or sets the XPath expression identifying the data to be used in the XMl file
        /// </summary>
        /// <value>The XPath expression identifying the data to be used in the XMl file</value>
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
        /// Gets or sets the path to the XML file
        /// </summary>
        /// <value>The path to the XML file</value>
		public String ResourceName
		{
			get
			{
				return this.resourceName;
			}
			set
			{
				if (value==null)
					throw new ArgumentNullException("value");
				this.resourceName = value;
			}
		}

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <returns>A <see cref="StreamReader"/> pointing to the XML file</returns>
		public virtual StreamReader LoadResource()
		{
			return new StreamReader(this.resourceName);
		}

        /// <summary>
        /// Gets the <see cref="XmlNodeList"/> to be used by the class tagged by this attribute.
        /// </summary>
        /// <returns>
        /// The <see cref="XmlNodeList"/> to be used by the class tagged by this attribute
        /// </returns>
		public override XmlNodeList GetData()
		{
			// load document
			XmlDocument doc = new XmlDocument();
			using (StreamReader stream = this.LoadResource())
			{
				doc.Load(stream);
			}
			
			// return queried xpath
			return doc.SelectNodes(this.XPath);
		}
	}
}
