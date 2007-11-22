using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using NUnit.Framework;

namespace mbunittest
{
    [Serializable]
    public class SimpleObject
    {
        string m_content;
        public string Content
        {
            get { return m_content; }
            set { m_content = value; }
        }
    }

    /// <summary>
    /// Summary description for SerializationTest.
    /// </summary>
    [TestFixture]
    public class SerializationTest
    {
        /// <summary>
        /// Currenty serialization tests are failing in MbUnit.  This 
        /// test should help debug why...
        /// </summary>
        [Test]
        public void TestSerializeMbUnit()
        {
            string content = "abc";

            SimpleObject obj = new SimpleObject();
            obj.Content = content;

            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            StringWriter writer = new StringWriter();
            XmlTextWriter textWriter = new XmlTextWriter(writer);
            serializer.Serialize(textWriter, obj);

            Assert.IsTrue(writer.ToString().Length > 3);
        }
    }
}