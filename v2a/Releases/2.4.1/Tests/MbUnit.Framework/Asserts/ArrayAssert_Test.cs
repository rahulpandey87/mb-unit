using System;
using System.Text;

namespace MbUnit.Framework.Tests.Asserts
{
    [TestFixture]
    public class ArrayAssert_Test
    {
        [Test]
        public void AreEqualBool()
        {
            bool[] arr1 = new bool[5];
            bool[] arr2 = new bool[5];

            arr1[0] = true;
            arr1[1] = false;
            arr1[2] = true;
            arr1[3] = false;
            
            arr2[0] = true;
            arr2[1] = false;
            arr2[2] = true;
            arr2[3] = false;

            ArrayAssert.AreEqual(arr1, arr2);
        }

        [Test]
        public void AreEqualLong()
        {
            long[] arr1 = new long[5];
            long[] arr2 = new long[5];

            arr1[0] = long.MaxValue;
            arr1[1] = long.MinValue;
            arr1[2] = long.MaxValue;
            arr1[3] = long.MinValue;

            arr2[0] = long.MaxValue;
            arr2[1] = long.MinValue;
            arr2[2] = long.MaxValue;
            arr2[3] = long.MinValue;

            ArrayAssert.AreEqual(arr1, arr2);
        }

        [Test]
        public void AreEqualChar()
        {
            Char[] arr1 = new Char[5];
            Char[] arr2 = new Char[5];

            arr1[0] = char.MaxValue;
            arr1[1] = char.MinValue;
            arr1[2] = char.MaxValue;
            arr1[3] = char.MinValue;

            arr2[0] = char.MaxValue;
            arr2[1] = char.MinValue;
            arr2[2] = char.MaxValue;
            arr2[3] = char.MinValue;

            ArrayAssert.AreEqual(arr1, arr2);
        }

        [Test]
        public void AreEqualInt()
        {
            int[] arr1 = new int[5];
            int[] arr2 = new int[5];

            arr1[0] = int.MaxValue;
            arr1[1] = int.MinValue;
            arr1[2] = int.MaxValue;
            arr1[3] = int.MinValue;

            arr2[0] = int.MaxValue;
            arr2[1] = int.MinValue;
            arr2[2] = int.MaxValue;
            arr2[3] = int.MinValue;

            ArrayAssert.AreEqual(arr1, arr2);
        }

        [Test]
        public void AreEqualByte()
        {
            byte[] arr1 = new byte[5];
            byte[] arr2 = new byte[5];

            arr1[0] = byte.MaxValue;
            arr1[1] = byte.MinValue;
            arr1[2] = byte.MaxValue;
            arr1[3] = byte.MinValue;

            arr2[0] = byte.MaxValue;
            arr2[1] = byte.MinValue;
            arr2[2] = byte.MaxValue;
            arr2[3] = byte.MinValue;

            ArrayAssert.AreEqual(arr1, arr2);
        }
    }
}
