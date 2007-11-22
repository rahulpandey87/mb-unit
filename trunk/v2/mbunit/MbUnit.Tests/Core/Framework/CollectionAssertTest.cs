using MbUnit.Core.Exceptions;

namespace MbUnit.Tests.Core.Framework
{
    using MbUnit.Framework;

    [TestFixture]
    [FixtureCategory("Framework.Assertions")]
    public class CollectionAssertTest
    {
        #region Contains
        [Test]
        public void Contains_UsesValueTypeEqualityForStringTypes()
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
            string[] stringArray = { "a", "b", "c" };
            CollectionAssert.Contains(stringArray, "d");
        }
        #endregion

        #region AreEquivalent

        [Test]
        public void AreEquivalent_UsesValueEqualityForStrings()
        {
            string[] stringArray1 = {"ab", "bc", "cd"};
            string[] stringArray2 = {"cd", string.Format("b{0}", "c"), "ab"};
            CollectionAssert.AreEquivalent(stringArray1, stringArray2);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AreEquivalent_DifferentSizeAreNotEquivalent()
        {
            string[] stringArray1 = { "a", "b"};
            string[] stringArray2 = { "a", "b", "c" };
            CollectionAssert.AreEquivalent(stringArray1, stringArray2);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void AreEquivalent_DifferentItemsAreNotEquivalent()
        {
            string[] stringArray1 = { "a", "b", "c" };
            string[] stringArray2 = { "a", "b", "d" };
            CollectionAssert.AreEquivalent(stringArray1, stringArray2);
        }

        #endregion

        #region IsSubsetOf

        [Test]
        public void IsSubsetOf_UsesValueEqualityForStrings()
        {
            string[] superset = { "ab", "bc", "cd" };
            string[] subset = { "cd", string.Format("b{0}", "c")};
            CollectionAssert.IsSubsetOf(subset, superset);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void IsSubsetOf_AssertExceptionWhenSupersetIsSmallerThanSubset()
        {
            string[] superset = { "a", "b" };
            string[] subset = { "a", "b", "c" };
            CollectionAssert.IsSubsetOf(subset, superset);
        }

        [Test, ExpectedException(typeof(AssertionException))]
        public void IsSubsetOf_DifferentItemInSubsetRaisesAssertException()
        {
            string[] superset = { "a", "b", "c" };
            string[] subset = { "a", "d" };
            CollectionAssert.IsSubsetOf(subset, superset);
        }

        #endregion
    }
}
