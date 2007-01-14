using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Operations
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="CollectionDomain"/> 
    /// class
    /// </summary>
    [TestFixture]
    public class CollectionDomainTest
    {
        [Test]
        public void ConstructWithArray()
        {
            int[] array = new int[] { 1, 2, 3 };
            CollectionDomain domain = new CollectionDomain(array);
            CollectionAssert.AreElementsEqual(array, domain);
        }

        [Test]
        public void BoundaryOfCollectionWithOneElement()
        {
            int[] array = new int[] { 1 };
            CollectionDomain domain = new CollectionDomain(array);
            IDomain boundary = domain.Boundary;
            CollectionAssert.AreElementsEqual(array, boundary);
        }

        [Test]
        public void BoundaryOfCollectionWithTwoElement()
        {
            int[] array = new int[] { 1,2 };
            CollectionDomain domain = new CollectionDomain(array);
            IDomain boundary = domain.Boundary;
            CollectionAssert.AreElementsEqual(array, boundary);
        }

        [Test]
        public void BoundaryOfCollectionWithMoreThanTwoElement()
        {
            int[] array = new int[] { 1, 2,3,4,5,6 };
            int[] barray = new int[] { 1, 6 };
            CollectionDomain domain = new CollectionDomain(array);
            IDomain boundary = domain.Boundary;
            CollectionAssert.AreElementsEqual(barray, boundary);
        }
    }
}