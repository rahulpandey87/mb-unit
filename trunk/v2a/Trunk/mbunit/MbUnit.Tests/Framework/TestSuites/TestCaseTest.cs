using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Tests.Framework.TestSuites
{
    /// <summary>
    /// <see cref="TestFixture"/> for the <see cref="TestCase"/> class.
    /// </summary>
    [TestFixture]
    public class TestCaseTest
    {
        public delegate void TestDelegate(TextWriter writer);

        #region Constructor Tests
        [Test]
        [ExpectedArgumentNullException]
        public void NullName()
        {
            StringWriter sw = new StringWriter();
            TestCase tc = new TestCase(null, new TestDelegate(this.Hello), sw);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyName()
        {
            StringWriter sw = new StringWriter();
            TestCase tc = new TestCase("", new TestDelegate(this.Hello), sw);
        }

        [Test]
        [ExpectedArgumentNullException]
        public void NullTest()
        {
            StringWriter sw = new StringWriter();
            TestCase tc = new TestCase("test", null, sw);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void ParameterIncompatibleWithTest()
        {
            StringWriter sw = new StringWriter();
            TestCase tc = new TestCase("test", null, 1);
        }
        #endregion

        #region Properties
        [Test]
        public void GetName()
        {
            StringWriter sw = new StringWriter();
            string name = "MyName";
            TestCase tc = new TestCase(name, new TestDelegate(this.Hello), sw);
            Assert.AreEqual(name, tc.Name);
        }

        [Test]
        public void GetTest()
        {
            StringWriter sw = new StringWriter();
            string name = "MyName";
            Delegate hello = new TestDelegate(this.Hello);
            TestCase tc = new TestCase(name, hello, sw);
            Assert.AreEqual(hello.Method, tc.TestDelegate.Method);
        }

        [Test]
        public void GetParameters()
        {
            StringWriter sw = new StringWriter();
            string name = "MyName";
            Delegate hello = new TestDelegate(this.Hello);
            TestCase tc = new TestCase(name, hello, sw);
            ArrayAssert.AreEqual(new Object[] { sw }, tc.GetParameters());
        }
        #endregion

        #region Invokation
        [Test]
        public void Invoke()
        {
            StringWriter sw = new StringWriter();
            string name = "MyName";
            Delegate hello = new TestDelegate(this.Hello);
            TestCase tc = new TestCase(name, hello, sw);
            tc.Invoke(this,new ArrayList());
            Assert.AreEqual("hello", sw.ToString());
        }
        #endregion

        public void Hello(TextWriter writer)
        {
            writer.Write("hello");
        }
    }
}

