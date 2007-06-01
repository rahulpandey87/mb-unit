// created on 29/01/2004 at 17:15
using System;
using System.Diagnostics;

namespace MbUnit.Demo 
{

	using MbUnit.Framework;

	[TestFixture("TestFixtureAttribute test")]
	public class TestFixtureAttributeTest
	{
		private static void ping()
		{
			Debug.WriteLine("Hello, World!");
		}

 		[SetUp]
		public void SetUpMethod()
		{
			Console.WriteLine("TestFixtureAttributeTest.SetUp called");
		}
		
		[Test]
		public void TestMethod()
		{
			Console.WriteLine("TestFixtureAttributeTest.Test called");	
		}		

		[Test]
		public void FailedTest()
		{
			Assert.AreEqual(0,1,"This test should fail");
		}		
		
		[TearDown]
		public void TearDownMethod()
		{
			Console.WriteLine("TestFixtureAttributeTest.TearDown called");			
		}
		
	}
}
