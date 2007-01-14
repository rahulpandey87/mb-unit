using System;
using System.Configuration;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Framework.Tests.Core
{
    [TestFixture]
    public class TestAssemblyConfigTest
    {
        [Test]
        public void LoadConfig()
        {
            string config = ConfigurationSettings.AppSettings["DummyValue"];
            Assert.AreEqual("HelloWorld", config, "Configuration value not found, .config is not loaded properly");
        }
    }
}
