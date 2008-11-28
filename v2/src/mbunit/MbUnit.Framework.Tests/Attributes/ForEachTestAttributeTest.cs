using System;
using System.Xml;
using System.Xml.Serialization;
using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Attributes
{
    /// <summary>
    /// Example of how to use ForEachTest.
    /// </summary>
    [DataFixture]
    [ResourceXmlDataProvider(typeof(DataDrivenTests), "MbUnit.Framework.Tests.Attributes.sample.xml", "//User")]
    [ResourceXmlDataProvider(typeof(DataDrivenTests), "MbUnit.Framework.Tests.Attributes.sample.xml", "DataFixture/Customers/User")]
    public class DataDrivenTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [ForEachTest("//User")]
        public void ForEachTest(XmlNode node)
        {
            Assert.IsNotNull(node);
            Assert.AreEqual("User", node.Name);
            Console.WriteLine(node.OuterXml);
        }

        [ForEachTest("//User", DataType = typeof(User))]
        public void ForEachTestWithSerialization(User user)
        {
            Assert.IsNotNull(user);
            Console.WriteLine(user.ToString());
        }

        [TearDown]
        public void TearDown()
        {
        }
    }

    [XmlRoot("User")]
    public class User
    {
        private string name;
        private string lastName;

        [XmlAttribute("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute("LastName")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public override string ToString()
        {
            return String.Format("{0}.{1}", name, lastName);
        }
    }
}
