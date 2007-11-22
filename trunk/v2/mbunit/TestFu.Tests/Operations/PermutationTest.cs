using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Operations
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="Permutation"/> 
    /// class
    /// </summary>
    [TestFixture]
    public class PermutationTest
    {
        [Test]
        public void Identity2()
        {
            Permutation p = new Permutation(2);
            ShowPermutation(p);
        }

        [Test]
        public void K2N5()
        {
            Permutation p = new Permutation(5);
            ShowPermutation(p);
        }


        private static void ShowPermutation(Permutation p)
        {
            int i = 0;
            foreach (Permutation s in p.GetSuccessors())
                Console.WriteLine("{0}: {1}", i++, s);
        }
    }
}