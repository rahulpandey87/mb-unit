using System;
using System.Collections;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnitTest
{
    [TestFixture, Ignore("Too expensive")]
    public class ValidatorTest
    {
        [Factory]
        public string[] Strings()
        {
            return new string[] { null, "hello" };
        }

        public bool IsValid(string s1, string s2)
        {
            if (s1 == null && s2 == null)
                return false;
            if (s1 != null && s2 != null)
                return false;
            return true;
        }

        [CombinatorialTest(TupleValidatorMethod = "IsValid"), ExpectedArgumentNullException]
        public void TestWithValidator([UsingFactories("Strings")] string s1, [UsingFactories("Strings")] string s2)
        {
            if (s1 == null)
                throw new ArgumentNullException();
            if (s2 == null)
                throw new ArgumentNullException();
        }

        [CombinatorialTest, ExpectedArgumentNullException]
        public void TestWithNoParams()
        {
        }
    }
}