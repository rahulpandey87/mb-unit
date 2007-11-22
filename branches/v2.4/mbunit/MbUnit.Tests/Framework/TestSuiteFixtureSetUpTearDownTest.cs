using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Framework
{
    /// <summary>
	/// Summary description for TestSetupTeardown.
	/// </summary>
	[TestSuiteFixture]
	public class TestSetupTeardown
	{
		public delegate void TestDelegate( int expectedSetup, int expectedTeardown );

		private int setupCalled;
		private int teardownCalled;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            setupCalled = 0;
            teardownCalled = 0;
        }

        [SetUp] 
        public void SetUp()
		{
			setupCalled++;
		}

		[TearDown] 
        public void TearDown()
		{
			teardownCalled++;
		}

		[TestSuite] 
		public TestSuite GetSuite()
		{
			TestSuite suite = new TestSuite("Suite1");
			suite.Add( "Setup 1, Teardown 0", new TestDelegate( this.Test ), 1, 0 );
			suite.Add( "Setup 2, Teardown 1", new TestDelegate( this.Test ), 2, 1 );
			suite.Add( "Setup 3, Teardown 2", new TestDelegate( this.Test ), 3, 2 );
			return suite;
		}

		/// <summary>
		/// Note that expected setup should increment BEFORE the test runs, 
		/// while expected tear down increments AFTER test runs.  So we can
		/// test setup for the current test, while we test teardown for the
		/// previous test by running multiple tests
		/// </summary>
		/// <param name="expectedSetup"></param>
		/// <param name="expectedTeardown"></param>
		public void Test( int expectedSetup, int expectedTeardown )
		{
			Assert.AreEqual( expectedSetup, setupCalled ,
                "Expected SetUp count {0} not equal to the actual count {1}",
                expectedSetup,setupCalled);
			Assert.AreEqual( expectedTeardown, teardownCalled,
                "Expected TearDown count {0} not equal to the actual count {1}",
                expectedSetup,setupCalled);
		}
	}
}