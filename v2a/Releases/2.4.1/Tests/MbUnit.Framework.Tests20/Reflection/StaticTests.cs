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
        public static readonly string MSCorLibAssembly = Environment.GetEnvironmentVariable("SystemRoot")
            + @"\Microsoft.NET\Framework\v2.0.50727\mscorlib.dll";
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

        [Test]
        public void CreateInstanceByAssemblyNameAndClassWithDefaultConstructo()
        {
            string className = "System.Number";
            object obj = Reflector.CreateInstance(MSCorLibAssembly, className);
            Assert.IsNotNull(obj);
            Assert.AreEqual(true, Reflector.InvokeMethod(AccessModifier.Default, obj, "IsWhite", ' '));
            Assert.AreEqual(false, Reflector.InvokeMethod(AccessModifier.Default, obj, "IsWhite", 'V'));
        }

        [Test]
        public void CreateInstanceByAssemblyNameAndClassWithParametizedConstructor()
        {
            string className = "System.Collections.KeyValuePairs";
            object obj = Reflector.CreateInstance(MSCorLibAssembly, className, 1, 'A');
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, Reflector.GetProperty(obj, "Key"));
            Assert.AreEqual('A', Reflector.GetProperty(obj, "Value"));
        }
    }


}
