using System.Collections;

using MbUnit.Core;
using MbUnit.Core.Framework;
using MbUnit.Core.Invokers;
using MbUnit.Core.Remoting;
using MbUnit.Framework;
using MbUnit.Core.Filters;

namespace MbUnit.Core
{
	/// <summary>
	/// Test class to illustrate how only one of the test attributes are used
	/// when discovering tests.  It would be nice to be able to mix both the
	/// <see cref="TestSuiteFixture"/> and <see cref="TestFixture"/> so that
	/// tests from both mechanisms are found
	/// 
	/// The order of the following attributes determines how the tests are
	/// found.  If <c>TestSuiteFixture</c> is first, the <see cref="TestSuite"/>
	/// is used.  If <c>TestFixture</c> is first, then the methods with the
	/// <see cref="Test"/> attribute are found instead.
	/// </summary>
	[TestSuiteFixture]
	[TestFixture]
	public class FixtureAndSuite
	{
		delegate void TestDelegate();

		[TestSuite] 
		public TestSuite GetSuite()
		{
			TestSuite suite = new TestSuite("Suite1");
			suite.Add( "SomeSuiteDefinedTest", new TestDelegate( this.SomeSuiteDefinedTest ) );
			return suite;
		}

		/// <summary>
		/// This test is called from the <see cref="TestSuite"/>
		/// </summary>
		public void SomeSuiteDefinedTest()
		{
			Explore();
		}

		/// <summary>
		/// This test is found due to the <see cref="TestFixture"/> attribute
		/// </summary>
		[Test]
		public void SomeFixtureDefinedTest()
		{
			Explore();
		}

		/// <summary>
		/// This method finds the fixture for this class, and checks that there are two
		/// tests returned (one from the <see cref="TestSuiteFixture"/> and the other
		/// from <see cref="TestFixture"/>).
		/// </summary>
		private void Explore()
		{
			FixtureExplorer explorer = new FixtureExplorer( this.GetType().Assembly );
            explorer.Filter = FixtureFilters.Type(this.GetType().FullName);

            explorer.Explore();
            int i = 0;
            foreach (object fixture in explorer.FixtureGraph.Fixtures)
                ++i;
            Assert.AreEqual(2, i);
        }
	}
}