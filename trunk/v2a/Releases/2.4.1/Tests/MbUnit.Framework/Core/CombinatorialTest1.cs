using System;
using System.Collections;

using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnitTest
{
    public interface ICountX
    {
        int Count(string s);
    }

    class SickCountX : ICountX
    {
        public int Count(string s)
        {
            return 2;
        }
    }

    class CountX : ICountX
    {
        public int Count(string s)
        {
            int count = 0;
            foreach (char c in s)
                if (c == 'x')
                    count++;
            return count;
        }
    }

    [TestFixture]
    public class CombinatorialTest
    {
        public class StringX
        {
            public StringX(string value, int xcount) { Value = value; XCount = xcount; }
            public string Value;
            public int XCount;
            public override string ToString() { return String.Format("{0},{1}", this.Value, this.XCount); }
        }

        [CombinatorialTest, Ignore("Too expensive")]
        public void CountIsExact(
           [UsingFactories("Counters")] ICountX counter,
           [UsingFactories("Strings")] StringX s)
        {
            Assert.AreEqual(s.XCount, counter.Count(s.Value));
        }

        [Factory(typeof(StringX))]
        public IEnumerable Strings()
        {
            yield return new StringX("", 0);
            yield return new StringX("x", 1);
            yield return new StringX("xa", 1);
            yield return new StringX("xax", 2);
            yield return new StringX("aaa", 0);
        }

        [Factory(typeof(ICountX))]
        public IEnumerable Counters()
        {
            yield return new SickCountX();
            yield return new CountX();
        }
    }

}