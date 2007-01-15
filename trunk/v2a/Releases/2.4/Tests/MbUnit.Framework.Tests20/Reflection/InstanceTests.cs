using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using MbUnit.Framework.Reflection;

namespace MbUnit.Framework.Tests20.Reflection
{
    [TestFixture]
    public class InstanceTests
    {
        TestSample sampleObject;
        Reflector reflect;

        [SetUp]
        public void Setup()
        {
            sampleObject = new TestSample();
            reflect = new Reflector(sampleObject);
        }

        [Test]
        public void RunPrivateMethodOnObjectUsingInstance()
        {
            reflect.RunPrivateMethod("IncCounter");
        }

        [Test]
        public void CheckPrivateValueOnObjectUsingInstance()
        {
            object result = reflect.GetPrivateVariable("counter");
            Assert.AreEqual(0, (int)result);
        }

        [Test]
        public void RunPrivateMethodAndCheckPrivateValueUsingInstance()
        {
            int execute = 3;
            for (int i = 0; i < execute; i++)
                reflect.RunPrivateMethod("IncCounter");

            object result = reflect.GetPrivateVariable("counter");
            Assert.AreEqual(execute, (int)result);
        }
    }
}
