using System;

namespace MbUnit.Demo
{
    public class NakedFixture
    {
        public void TestFixtureSetUp()
        {
            Console.WriteLine("TestFixtureSetUp");
        }

        public void SetUp()
        {
            Console.WriteLine("SetUp");
        }

        public void FirstTest()
        {
            Console.WriteLine("Test1");
        }

        public void SecondTest()
        {
            Console.WriteLine("Test1");
        }

        public void TearDown()
        {
            Console.WriteLine("TearDown");
        }

        public void TestFixtureTearDown()
        {
            Console.WriteLine("TestFixtureTearDown");
        }
    }
}
