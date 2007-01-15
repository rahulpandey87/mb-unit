#region Includes
using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;
#endregion

using MbUnit.Core.Reports;

namespace MbUnit.Framework.Tests.Core.Reports
{
	/// <summary>
	/// <see cref="TestFixture"/> for the <see cref="NamePretifier"/> class.
	/// </summary>
	[TestFixture]
    public class NamePretifierTest
    {
		private NamePretifier np;

        [SetUp]
		public void SetUp()
		{
			this.np=new NamePretifier();
		}

        #region Property checking
        [Test]
        public void FixtureSuffixGetSet()
        {
            Guid guid = Guid.NewGuid();
            np.FixtureSuffix = guid.ToString();
            Assert.AreEqual(guid.ToString(), np.FixtureSuffix);
        }

        [Test]
        public void TestSuffixGetSet()
        {
            Guid guid = Guid.NewGuid();
            np.TestSuffix = guid.ToString();
            Assert.AreEqual(guid.ToString(), np.TestSuffix);
        } 
        #endregion

        #region Argument checking
        [Test]
        [ExpectedArgumentNullException]
        public void PretifyNullFixtureName()
        {
            np.PretifyFixture(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void PretifyEmptyFixtureName()
        {
            np.PretifyFixture("");
        }
        [Test]
        public void PretifySuffixOnlyFixtureName()
        {
            np.PretifyFixture("Test");
        }
        [Test]
        [ExpectedArgumentNullException]
        public void PretifyNullTestName()
        {
            np.PretifyTest(null);
        }
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void PretifyEmptyTestName()
        {
            np.PretifyTest(null);
        }
        [Test]
        public void PretifySuffixTestName()
        {
            np.PretifyTest("Test");
        }
        #endregion

        #region Capitalize
        [Test]
        public void PrefityFixtureName()
        {
            string pretty = np.PretifyFixture("Abcd");
            Assert.AreEqual("Abcd", pretty);
        }
        [Test]
        public void PrefityFixtureNameWithSuffix()
        {
            string pretty = np.PretifyFixture("AbcdTest");
            Assert.AreEqual("Abcd", pretty);
        }
        [Test]
        public void PrefityFixtureNameWithSuffixInside()
        {
            string pretty = np.PretifyFixture("AbcdTestHello");
            Assert.AreEqual("AbcdTestHello", pretty);
        }
        [Test]
        public void PrefityFixtureNameOfSize1()
        {
            string pretty = np.PretifyFixture("A");
            Assert.AreEqual("A", pretty);
        }

        [Test]
        public void PrefityFixtureNameOfSize2()
        {
            string pretty = np.PretifyFixture("Ab");
            Assert.AreEqual("Ab", pretty);
        } 
        [Test]
		public void PretifyTestTest()
		{
			string value = "AaBbbbbCcccDdd";
			string pretty = np.PretifyTest(value);
			Assert.AreEqual("aa bbbbb cccc ddd",pretty);
        }
        [Test]
        public void PretifyTestWithSuffix()
        {
            string value = "AaBbbbbCcccDddTest";
            string pretty = np.PretifyTest(value);
            Assert.AreEqual("aa bbbbb cccc ddd", pretty);
        }
        [Test]
        public void PretifyTestWithSuffixInside()
        {
            string value = "AaBbbbbCcccDddTestHello";
            string pretty = np.PretifyTest(value);
            Assert.AreEqual("aa bbbbb cccc ddd test hello", pretty);
        }
        [Test]
        public void PretifyTestWithSetUp()
        {
            string value = "SetUp.AaBbbbbCcccDddTest";
            string pretty = np.PretifyTest(value);
            Assert.AreEqual("aa bbbbb cccc ddd", pretty);
        }
        [Test]
        public void PretifyTestWithTearDown()
        {
            string value = "SetUp.AaBbbbbCcccDddTest.TearDown";
            string pretty = np.PretifyTest(value);
            Assert.AreEqual("aa bbbbb cccc ddd", pretty);
        }
        #endregion
    }
}

