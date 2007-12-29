using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core;
using MbUnit.Core.Exceptions;
//using MbUnit.Tests.Core.Invokers;

namespace MbUnit.Tests.Core
{
	/// <summary>
    /// <see cref="TestFixture"/> for the <see cref="FixtureTest"/> class.
    /// </summary>
	[TestFixture]
    public class FixtureTest
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Console.WriteLine("TestFixtureSetUp");
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Console.WriteLine("TestFixtureTearDown");
        }

        [Test]
        [ExpectedArgumentNullException]
        public void TypeNullArgument()
        {
            Fixture fix = new Fixture(null, new MockRun(),null,null);            
        }

        [Test]
        [ExpectedArgumentNullException]
        public void RunNullArgument()
        {
            Fixture fix = new Fixture(typeof(FixtureTest), null,null,null);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void TypeNoDefaultArgument()
        {
            Fixture fix = new Fixture(typeof(NoDefaultConstructor), new MockRun(),null,null);
        }

        internal class NoDefaultConstructor
        {
            private NoDefaultConstructor() { }
        }
    }
}
