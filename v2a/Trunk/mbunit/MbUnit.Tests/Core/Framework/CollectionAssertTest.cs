using MbUnit.Core.Exceptions;

namespace MbUnit.Tests.Core.Framework
{
    using MbUnit.Framework;

    [TestFixture]
    [FixtureCategory("Framework.Assertions")]
    public class CollectionAssertTest
    {
        [Test]
        public void Contains_UsesValueTypeComparisonForStringTypes()
        {
            string[] stringArray = { string.Format("a{0}", "c"), "b" };
            CollectionAssert.Contains(stringArray, "ac");
        }

        [Test]
        public void Contains_NullItemSearchSupported()
        {
            string[] stringArray = { "a", "b", null };
            CollectionAssert.Contains(stringArray, null);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void Contains_RaisesAssertionExceptionIfItemNotFound()
        {
            string[] stringArray = { "a", "b", "c"};
            CollectionAssert.Contains(stringArray, "d");
        }
    }
}
