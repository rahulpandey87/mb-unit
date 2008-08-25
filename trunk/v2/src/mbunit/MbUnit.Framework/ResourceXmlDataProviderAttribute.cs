
namespace MbUnit.Framework
{
	using System;
	using System.Xml;
	using MbUnit.Core.Runs;
	using System.Reflection;
	using System.IO;


    /// <summary>
    /// Tags a class also tagged with a <see cref="DataFixtureAttribute"/> to associate an XML file (which is an
    /// embedded resource in your project) with the tests in your class.
    /// </summary>
    /// <remarks>
    /// <para>If your XML data file is a standalone file rather than an embedded resource, use <see cref="XmlDataProviderAttribute"/>
    /// rather than <see cref="ResourceXmlDataProviderAttribute"/>.</para>
    /// <para>For a full demonstration of this test pattern, please visit <see cref="DataFixtureAttribute"/>.</para>
    /// </remarks>
    /// <para>The test class adorned with the DataFixture attribute looks like this</para>
    /// <code>
    ///    [DataFixture]
    ///    [ResourceXmlDataProvider(typeof(EmbeddedXmlFile), "sample.xml" "//User")]
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
    public sealed class ResourceXmlDataProviderAttribute : XmlDataProviderAttribute
    {
		private Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceXmlDataProviderAttribute"/> class.
        /// </summary>
        /// <param name="t">The type associated with the embedded resource</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="xpath">The xpath to the data you wish to use within the XML file</param>
		public ResourceXmlDataProviderAttribute(Type t, string resourceName, string xpath)
			:base(resourceName,xpath)
		{
			this.type=t;
		}

        /// <summary>
        /// Loads the resource.
        /// </summary>
        /// <returns>A <see cref="StreamReader"/> for the XML file</returns>
		public override StreamReader LoadResource()
		{
			return new StreamReader(this.type.Assembly.GetManifestResourceStream(this.ResourceName));
		}
	}
}
