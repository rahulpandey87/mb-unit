using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Tests.Framework
{
    /// <summary>
    /// <see cref="TestFixture"/> for the <see cref="TestSuite"/> class.
    /// </summary>
    [TestFixture]
    public class TestSuiteTest
    {
        #region Constructor tests
        [Test]
        [ExpectedArgumentNullException]
        public void NullName()
        {
            TestSuite suite = new TestSuite(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyName()
        {
            TestSuite suite = new TestSuite("");
        }
        #endregion

        #region Add/Remove Test Cases
        [Test]
        [ExpectedArgumentNullException]
        public void AddNullTestCase()
        {
            TestSuite suite = new TestSuite("Suite");
            suite.Add(null);
        }

        [Test]
        public void AddDuplicateTestCase()
        {
            TestSuite suite = new TestSuite("Suite");
            suite.Add("Test", new TestDelegate(this.Test));
            suite.Add("Test", new TestDelegate(this.Test));
        }

        [Test]
        public void AddDuplicateTestCase2()
        {
            TestSuite suite = new TestSuite("Suite");
            TestCase tc = new TestCase("Test", new TestDelegate(this.Test));
            suite.Add(tc);
            suite.Add(tc);
        }

        [Test]
        [ExpectedArgumentNullException]
        public void AddNullName()
        {
            TestSuite suite = new TestSuite("Suite");
            suite.Add(null, new TestDelegate(this.Test));
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddEmptyName()
        {
            TestSuite suite = new TestSuite("Suite");
            suite.Add("", new TestDelegate(this.Test));
        }

        [Test]
        public void AddTestCase()
        {
            TestSuite suite = new TestSuite("Suite");
            TestCase tc = suite.Add("Test", new TestDelegate(this.Test));
            Assert.IsNotNull(tc);
            Assert.AreEqual(1, suite.TestCases.Count);
        }
        #endregion

        public Object Test(Object context)
        {
            return context;
        }
    }
}

