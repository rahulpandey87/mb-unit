using System;
using System.Text;
using MbUnit.Framework;

namespace MbUnit.Tests.Core.Framework
{
    [TestFixture]
    public class MultipleRowTest
    {
        #region Private Members

        private int numberOfRows;

        #endregion

        #region SetUp and TearDown

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            numberOfRows = 0;
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Assert.AreEqual(numberOfRows, 5);
        }

        #endregion

        /// <summary>
        /// Makes sure that all the rows are passed to the test (it's verified in the fixture teardown).
        /// </summary>
        /// <param name="row"></param>
        [RowTest]
        [Row("row1")]
        [Row("row3")]
        [Row("row3")]
        [Row("row4")]
        [Row("row5")]
        public void RowWithTreeValue(string row)
        {
            numberOfRows++;
        }

    }
}
