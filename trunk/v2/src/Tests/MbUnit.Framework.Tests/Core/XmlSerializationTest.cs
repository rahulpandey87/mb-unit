using System;
using System.Xml.Serialization;
using MbUnit.Core.Framework;
using MbUnit.Framework;
namespace MbUnit.Framework.Tests.Core
{
    [TestFixture]
    public class XmlSerializationTest
    {
        [Test]
        public void CompileSerializer()
        {
            XmlSerializer ser = new XmlSerializer(typeof(Dummy));
        }

        [XmlRoot("Dummy")]
        [Serializable]
        public class Dummy
        {
            public string Name;
        }
    }
}
