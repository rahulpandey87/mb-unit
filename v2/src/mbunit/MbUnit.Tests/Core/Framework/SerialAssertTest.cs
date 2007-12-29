
namespace MbUnit.Tests.Core.Framework
{
    using System;
    using System.Collections;
    using MbUnit.Core.Framework;
    using MbUnit.Framework;
    using MbUnit.Core;
    using MbUnit.Core.Exceptions;

    [TestFixture("Assertion test")]
    [FixtureCategory("Framework.Assertions")]
    public class SerialAssertTest
    {
        #region IsXmlSerializable
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void IsXmlSerilizableNullType()
        {
            SerialAssert.IsXmlSerializable(null);
        }
        [Test]
        public void IsXmlSerilizable()
        {
            SerialAssert.IsXmlSerializable(typeof(SerializableClass));
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsXmlSerilizableFail()
        {
            SerialAssert.IsXmlSerializable(typeof(NotSerializableClass));
        }
        #endregion

        #region OneWaySerialization
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void OneWaySerializationNullType()
        {
            SerialAssert.OneWaySerialization(null);      
        }
        [Test]
        [Ignore("Problem due to separate AppDomain")]
        public void OneWaySerialization()
        {
            SerialAssert.OneWaySerialization(new SerializableClass());      
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OneWaySerializationFail()
        {
            SerialAssert.OneWaySerialization(new NotSerializableClass("hello"));      
        }
        #endregion

        #region TwoWaySerialization
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void TwoWaySerializationNullType()
        {
            SerialAssert.OneWaySerialization(null);      
        }
        [Test]
        [Ignore("Problem due to separate AppDomain")]
        public void TwoWaySerialization()
        {
            SerialAssert.TwoWaySerialization(new SerializableClass());      
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TwoWaySerializationFail()
        {
            SerialAssert.TwoWaySerialization(new NotSerializableClass("hello"));      
        }
        #endregion

        #region DummyClasses
        public class NotSerializableClass
        {
            public NotSerializableClass(string s)
            { }
        }

        public class SerializableClass
        {
            public string Name = "Marc";
            public string LastName = "Paul";
        }
        #endregion
    }
}