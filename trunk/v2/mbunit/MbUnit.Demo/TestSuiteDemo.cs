using System;

using MbUnit.Framework;

namespace MbUnit.Demo 
{
    [TestSuiteFixture]
    public class TestSuiteDemo
    {
        private string helloWorld="HelloWorld";

        [TestSuite] 
        public TestSuite Suite()
        {
            TestSuite suite = new TestSuite("SimpleSuite");
            suite.Add( "Test1", new TestDelegate( this.Test ), "hello" );
            suite.Add( "Test2", new TestDelegate( this.AnotherTest), "another test" );
            return suite;
        }

        [TestSuite]
        public TestSuite VerifiedSuite()
        {
            TestSuite suite = new TestSuite("VerifiedSuite");
            TestCase tc = TestCases.Case("Verify that tests returns hello world", new TestDelegate(this.TestReturnHelloWorld),"hello");
            VerifiedTestCase vtc = TestCases.Verified(tc,this.helloWorld);
            suite.Add(vtc);
            return suite;
        }

        [TestSuite]
        public TestSuite ExpectedExceptionSuite()
        {
            TestSuite suite = new TestSuite("ExpectedExceptionSuite");

            ITestCase tc = TestCases.Case("Verfiy that tests throw", new TestDelegate(this.ThrowMe),"hello");
            ITestCase etc = TestCases.ExpectedException(tc, typeof(Exception));
            suite.Add(etc);
            return suite;
        }

        public Object Test( object testContext )
        {
            Console.WriteLine("Test");
            Assert.AreEqual("hello", testContext);
            return null;
        }
 
        public Object AnotherTest( object testContext )
        {
            Console.WriteLine("AnotherTest");
            Assert.AreEqual("another test", testContext);
            return null;
        }

        public Object TestReturnHelloWorld(Object context)
        {
            return helloWorld;
        }

        public object ThrowMe(Object context)
        {
            throw new Exception();
        }
    }
}
