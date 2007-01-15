#region using
using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
#endregion

namespace TestFu.Operations
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="Domains"/> 
    /// class
    /// </summary>
    [TestFixture]
    public class DomainsTest
    {
        [Test]
        [ExpectedArgumentNullException]
        public void UniformizeWithNull()
        {
            IDomainCollection domains = null;
            Domains.Uniformize(domains);
        }

        [Test]
        public void UniformizeUniform()
        {
            int[] array1 = new int[] { 1, 2 };
            int[] array2 = new int[] { 1, 2 };

            IDomainCollection domains = Domains.Uniformize(array1, array2);
            foreach (IDomain domain in domains)
                Assert.AreEqual(domain.Count, 2);
        }

        [Test]
        public void UniformizeNonUniform()
        {
            int[] array1 = new int[] { 1, 2 };
            int[] array2 = new int[] { 1, 2,3,4};
            int[] array3 = new int[] { 1, 2, 3, 4,5,6,7 };

            IDomainCollection domains = Domains.Uniformize(array1, array2,array3);
            foreach (IDomain domain in domains)
                Assert.AreEqual(domain.Count, 7);

        }
    }
}