using System;
using System.Collections;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Operations
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref=" "/> 
    /// class
    /// </summary>
    [TestFixture]
    [CurrentFixture]
    public class Test
    {
        public int SingleInt()
        {
            return 0;
        }

        public int[] Ints()
        {
            return new int[] { 1, 2, 3 };
        }

        [Factory(typeof(int))]
        public IEnumerable IntsAsEnumerator()
        {
            return new int[] { 1, 2, 3 };
        }
        [Factory(typeof(char))]
        public IEnumerable Chars()
        {
            return new char[] { 'a', 'b', 'c' };
        }
        public char SingleChar()
        {
            return 'd';
        }

        [Factory(typeof(string))]
        public IEnumerable Strings()
        {
            return new string[] { "combinatorial", "test", "is cool" };
        }

        [Test]
        public void AClassicTest()
        {
            Console.WriteLine("I'm a classic test");
        }
/*
        [CombinatorialTest]
        public void TestUsingAllFactories(
            [UsingFactories] int i,
            [UsingFactories] char c,
            [UsingFactories] string s
           )
        {
            Console.WriteLine("{0} {1}", i, c, s);
        }

        [CombinatorialTest]
        public void TestUsingFactories(
            [UsingFactories("Ints")] int i,
            [UsingFactories("Chars")] char c,
            [UsingFactories("Strings")] string s
           )
        {
            Console.WriteLine("{0} {1} {2}", i, c, s);
        }

        [CombinatorialTest]
        public void TestUsing2(
            [Using("0;1;2")] int i,
            [Using("a;b;c")] char c
            )
        {
            Console.WriteLine("{0} {1}", i, c);
        }

        [CombinatorialTest]
        public void TestUsing3(
            [Using("0;1;2")] int i,
            [Using("a;b;c")] char c,
            [Using("x;y;z")] String s
            )
        {
            Console.WriteLine("{0} {1} {2}", i, c, s);
        }

        [CombinatorialTest]
        [ExpectedArgumentNullException]
        public void TestExpectedException(
            [Using("0;1;2")] int i
            )
        {
            throw new ArgumentNullException();
        }

        [CombinatorialTest]
        public void EmptyCombinatorialTest()
        {
        }
*/
        public char[] Alphabet()
        {
            string alphabet = "abcdef";
            return alphabet.ToCharArray();
        }

        public IEnumerable States()
        {
            for (int i = 0; i < 10; ++i)
                yield return i.ToString();
        }

        [CombinatorialTest]
        public void MarcsTest(
            [UsingFactories("Alphabet")] char alpha,
            [UsingLinear(0,3)] int digit,
            [UsingFactories("States")] string state
           )
        {
            Console.WriteLine("{0} {1} {2}", alpha, digit, state);
        }
    }
} 