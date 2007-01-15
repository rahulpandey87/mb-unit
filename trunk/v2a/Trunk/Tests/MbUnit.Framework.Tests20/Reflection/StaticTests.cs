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
        TestSample sampleObject;

        [SetUp]
        public void Setup()
        {
            sampleObject = new TestSample();    
        }

        [Test]
        public void RunPrivateMethodOnObjectUsingStatic()
        {
            Reflector.RunNonPublicMethod(sampleObject, "IncCounter");
        }

        [Test]
        public void CheckPrivateValueOnObjectUsingStatic()
        {
            object result = Reflector.GetNonPublicVariable(sampleObject, "counter");
            Assert.AreEqual(0, (int)result);
        }

        [Test]
        public void RunPrivateMethodAndCheckPrivateValueUsingStatic()
        {
            int execute = 3;
            for (int i = 0; i < execute;  i++)
                Reflector.RunNonPublicMethod(sampleObject, "IncCounter");

            object result = Reflector.GetNonPublicVariable(sampleObject, "counter");
            Assert.AreEqual(execute, (int)result);
        }
    }


}
