using System;
using MbUnit.Core.Framework;

namespace MbUnit.Framework
{
	/// <summary>
	/// Sample unit test to expose a bug in TestSuite, where an  ExpectedException is
	/// not checked for if an exception occurs, failing a test that should  otherwise
	/// pass.
	/// </summary>
	[TestSuiteFixture]
	public class TestExpectedException
	{
		delegate void TestDelegate();

		[TestSuite] 
		public TestSuite GetSuite()
		{
			TestSuite suite = new TestSuite("Suite1");
			suite.Add( "TestExpectedException", new TestDelegate(this.TestThrowsException ) );
			return suite;
		}

		/// <summary>
		/// This test will pass once TestSuite tests for expected  exception attributes
		/// on test run
		/// </summary>
		[ExpectedException( typeof(ApplicationException) )]
		public void TestThrowsException()
		{
			throw new ApplicationException( "This exception should be caught, due to the ExpectedException attribute" );
		}
	}
}