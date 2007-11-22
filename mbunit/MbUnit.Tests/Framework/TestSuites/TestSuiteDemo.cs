using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Tests.Framework.TestSuites
{
    [TestSuiteFixture]
    public class TestSuiteDemo
    {
        private string helloWorld="HelloWorld";

        public delegate object MultiArgumentDelegate(Object nullable, Object notNullable, int valueType);

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
            TestCase tc = TestCases.Case("Verify that tests returns hello world", new TestDelegate(this.TestReturnHelloWorld),null);
            VerifiedTestCase vtc = TestCases.Verified(tc,this.helloWorld);
            suite.Add(vtc);
            return suite;
        }

        [TestSuite]
        public TestSuite ExpectedExceptionSuite()
        {
            TestSuite suite = new TestSuite("ExpectedExceptionSuite");

            ITestCase tc = TestCases.Case("Verfiy that tests throw", new TestDelegate(this.ThrowMe));
            ITestCase etc = TestCases.ExpectedException(tc, typeof(Exception));
            suite.Add(etc);
            return suite;
        }

        [TestSuite]
        public ITestSuite MethodSuite()
        {
            MethodTestSuite suite = new MethodTestSuite(
                "MethodSuite",
                new MultiArgumentDelegate(this.MethodTest),
                "hello",
                "world",
                1
                );

            suite.AddThrowArgumentNull(0);
            suite.AddThrowArgumentNull(1);
            return suite;
        }

        [TestSuite]
        public ITestSuite AutomaticMethodSuite()
        {
            MethodTestSuite suite = new MethodTestSuite(
                "AllMethodSuite",
                new MultiArgumentDelegate(this.MethodTest),
                "hello",
                "world",
                1
                );

            suite.AddAllThrowArgumentNull();

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

        public object MethodTest(Object nullable, Object notNullable, int valueType)
        {
            if (notNullable == null)
                throw new ArgumentNullException("notNullable");
            Console.Write("test: {0}{1}{2}",nullable,notNullable,valueType);
            return String.Format("{0}{1}{2}",nullable,notNullable,valueType);
        }
    }
}
