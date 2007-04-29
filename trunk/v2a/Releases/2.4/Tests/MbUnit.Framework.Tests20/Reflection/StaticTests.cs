using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using MbUnit.Framework.Reflection;


namespace MbUnit.Framework.Tests20.Reflection
{
    [TestFixture]
    public class ReflectorStaticTests
    {
        TestSample _sampleObject;

        [SetUp]
        public void Setup()
        {
            _sampleObject = new TestSample();    
        }

        #region GetField Tests
        [Test]
        public void GetPublicField_DefaultAccessibility()
        {
            Assert.AreEqual("MbUnit Rocks!!!", Reflector.GetField(_sampleObject, "publicString"));
        }

        [Test]
        [ExpectedArgumentNullException()]
        public void SetPropertyWithNullObject()
        {
            Reflector.SetProperty(null, "somePropety", "value");
        }

        #endregion

    }


}
