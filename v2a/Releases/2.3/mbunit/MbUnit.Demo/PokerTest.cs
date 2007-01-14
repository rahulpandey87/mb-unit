using System;

using MbUnit.Framework;
using TestFu.Operations;


namespace MbUnit.Demo
{
    [TestFixture]
    public class PokerTest
    {
        public class ShiftedLinearInt32Domain : LinearInt32Domain
        {
            private int shift = 0;
            public ShiftedLinearInt32Domain(int shift, int start, int stepCount)
                :base(start,stepCount,1)
            {
                this.shift = shift;
            }

            public override int this[int index]
            {
                get 
                { 
                    return base[(index+shift)%this.Count]; 
                }
            }
        }

        public ShiftedLinearInt32Domain Card1()
        {
            return new ShiftedLinearInt32Domain(0, 0, 52);
        }
        public ShiftedLinearInt32Domain Card2()
        {
            return new ShiftedLinearInt32Domain(6, 0, 52);
        }
        public ShiftedLinearInt32Domain Card3()
        {
            return new ShiftedLinearInt32Domain(12, 0, 52);
        }
        public ShiftedLinearInt32Domain Card4()
        {
            return new ShiftedLinearInt32Domain(18, 0, 52);
        }
        public ShiftedLinearInt32Domain Card5()
        {
            return new ShiftedLinearInt32Domain(24, 0, 52);
        }

        public bool IsValid(
            [UsingFactories("Card1")] int i,
            [UsingFactories("Card2")] int j,
            [UsingFactories("Card3")] int k,
            [UsingFactories("Card4")] int l,
            [UsingFactories("Card5")] int m
            )
        {
            if (i == j || i == k || i == l || i == m || j == k || j == l || j == m || k == l || k == m || l == m)
            {
                Console.WriteLine("Skipped: {0} {1} {2} {3} {4}", i, j, k, l, m);
                return false;
            }
            return true;
        }

//        [CombinatorialTest(TupleValidatorMethod = "IsValid")]
        public void Test(
            [UsingFactories("Card1")] int i,
            [UsingFactories("Card2")] int j,
            [UsingFactories("Card3")] int k,
            [UsingFactories("Card4")] int l,
            [UsingFactories("Card5")] int m
            )
        {
            Console.WriteLine("{0} {1} {2} {3} {4}", i, j, k, l, m);
        }
    }
}