using System.Collections;
using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Operations
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="UniformPairWizeDomainTupleEnumerator"/> 
    /// class
    /// </summary>
    [TestFixture]
    public class UniformPairWizeDomainTupleEnumeratorTest
    {
        [Test]
        [ExpectedArgumentNullException]
        public void ConstructWithNullDomains()
        {
            IDomainCollection domains = null;
            Products.PairWize(domains);
        }
        [Test]
        [ExpectedArgumentException]
        public void DomainCountMistmatch()
        {
            int[] array1 = new int[]{1,2};
            int[] array2 = new int[] { 1, 2, 3 };
            IDomainCollection dom = Domains.ToDomains(array1, array2);
            new UniformPairWizeProductDomainTupleEnumerable(dom);
        }

        [Test]
        [ExpectedArgumentException]
        public void DomainCountEmpty()
        {
            int[] array1 = new int[] { 1, 2 };
            int[] array2 = new int[] { };
            Products.PairWize(array1, array2);
        }

        [Test]
        public void K2L2()
        {
            this.TestProblem(2, 2);
        }

        [Test]
        public void K3L2()
        {
            this.TestProblem(3, 2);
        }

        [Test]
        public void K3L3()
        {
            this.TestProblem(3, 3);
        }

        [Test]
        public void K7L3()
        {
            Console.WriteLine(Math.Pow(3, 7));
            this.TestProblem(7, 3);
        }

        [Test]
        public void K13L3()
        {
            Console.WriteLine(Math.Pow(3, 13));
            this.TestProblem(13, 3);
        }

        [Test]
        public void K10L10()
        {
            Console.WriteLine(Math.Pow(10, 10));
            this.TestProblem(10, 10);
        }

        [Test]
        public void NonUniform()
        {
            int[] array1 = new int[] { 1, 2 };
            char[] array2 = new char[] { 'a', 'b','c' };
            string[] array3 = new string[] { "a", "combinatorial", "hello","world" };

            int i = 1;
            foreach (ITuple tuple in Products.PairWize(array1, array2, array3))
            {
                Console.WriteLine("{0}: {1}",i++, tuple);
            }
        }

        private void TestProblem(int k, int l)
        {
            // create domains
            DomainCollection domains = new DomainCollection();
            for (int i = 0; i < k; ++i)
            {
                int[] domain = new int[l];
                for (int j = 0; j < l; ++j)
                    domain[j] = j;
                domains.Add(Domains.ToDomain(domain));
            }
            // iterface
            ITupleEnumerable tuples = Products.PairWize(domains);
            int f = 1;
            foreach (ITuple tuple in tuples)
            {
                Console.WriteLine("{0}: {1}",f++,tuple);
            }
        }
    }
} 