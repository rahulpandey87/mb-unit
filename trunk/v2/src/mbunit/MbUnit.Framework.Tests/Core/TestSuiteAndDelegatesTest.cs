using System;
using MbUnit.Framework;
using MbUnit.Core.Framework;

namespace MbUnit.Framework.Tests.Core.Delegates
{
	public delegate void VoidTestDelegate();

	public class ClassA
	{
		public void SomeMethodA()
		{
		}
	}

	/// <summary>
	/// This fixture creates a suite, with delegates that point to methods in some
	/// related 'helper' classes.  Notice how the current implementation tries
	/// to call the methods on the fixture, and gets a System.Reflection.TargetException
	/// saying "Object does not match target type.".
	/// 
	/// This test should pass once the code is changed to call the method
	/// specified by the delegate on the object specified when the delegate
	/// is created (instead of the fixture itself).  This should be available
	/// as <see cref="Delegate.Target"/> on the delegate that is passed in.
	/// </summary>
	[TestSuiteFixture]
	public class TestCaseDelegateFixture
	{
		ClassA classA = new ClassA();

		/// <summary>
		/// Suite of all tests to run
		/// </summary>
		/// <returns></returns>
		[TestSuite] 
		public TestSuite GetSuite()
		{
			TestSuite suite = new TestSuite("SeparateClass");
			suite.Add( "TestClassA", new VoidTestDelegate(classA.SomeMethodA ) );
			return suite;
		}
	}
}