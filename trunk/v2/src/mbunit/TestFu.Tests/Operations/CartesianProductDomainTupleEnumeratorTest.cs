#region using
using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
#endregion

namespace TestFu.Operations
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="CartesianProductDomainTupleEnumerator"/> 
    /// class
    /// </summary>
    [TestFixture]
    public class CartesianProductDomainTupleEnumeratorTest
    {
        [Test]
        public void SingleDomain()
        {
            int[] array = new int[] { 1, 2, 3 };
            IDomain domain = Domains.ToDomain(array);

            int i = 0;
            foreach (ITuple tuple in Products.Cartesian(domain))
            {
                Assert.AreEqual(array[i], tuple[0]);
                ++i;
            }
            Assert.AreEqual(i, array.Length);
        }

        [Test]
        public void TwoDomains()
        {
            int[] array = new int[] { 1, 2, 3 };
            IDomain domain = Domains.ToDomain(array);
            string[] array2 = new string[] { "hello", "world" };
            IDomain domain2 = Domains.ToDomain(array2);

            object[] results = new object[array.Length*array2.Length];
            for (int i = 0; i < array.Length; ++i)
            {
                for (int j = 0; j < array2.Length; ++j)
                {
                    results[i*array2.Length+j] = new object[] { array[i], array2[j] };
                }
            }

            int k=0;
            foreach (ITuple tuple in Products.Cartesian(domain,domain2))
            {
                object[] result = (object[])results[k];
                Assert.AreEqual(result[0], tuple[0]);
                Assert.AreEqual(result[1], tuple[1]);
                ++k;
            }
            Assert.AreEqual(k, array.Length*array2.Length);
        }
    }
}