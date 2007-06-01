#region using
using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
#endregion

namespace TestFu.Operations
{
    /// <summary>
    /// A <see cref="TestFixture"/> for the <see cref="Tuple"/> 
    /// class
    /// </summary>
    [TestFixture]
    public class TupleTest
    {
        private Tuple target = null;        
        /// <summary>
        /// Sets up the fixture
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.target = new Tuple();
        }

        [Test]
        public void AddAndCount()
        {
            this.target.Add(0);
            CollectionAssert.AreCountEqual(1, this.target);
        }

        [Test]
        public void Enumerate()
        {
            this.target.Add(0);
            this.target.Add("hell");
            foreach (Object o in this.target)
            { };
        }

        [Test]
        public void ConcatTwoTuples()
        {
            ITuple tuple2 = new Tuple();
            tuple2.Add("tuple2");
            this.target.Add("target");

            this.target.Concat(tuple2);
            CollectionAssert.AreCountEqual(2, target);
            Assert.AreEqual("target", target[0]);
            Assert.AreEqual("tuple2", target[1]);
        }

        [Test]
        public void CompareTupleWithNull()
        {
            target.Add(0);
            Assert.AreEqual(-1, this.target.CompareTo(null));
        }

        [Test]
        public void CompareTupleWithSelf()
        {
            target.Add(0);
            Assert.AreEqual(0, this.target.CompareTo(this.target));
        }

        [Test]
        public void CompareTupleWithLessElement()
        {
            target.Add(0);
            Tuple tuple = new Tuple();
            tuple.Add(-1);
            tuple.Add(-2);

            Assert.AreEqual(-1, this.target.CompareTo(tuple));
        }

        [Test]
        public void CompareTupleWithMoreElement()
        {
            target.Add(0);
            target.Add(1);
            Tuple tuple = new Tuple();
            tuple.Add(2);

            Assert.AreEqual(1, this.target.CompareTo(tuple));
        }

        [Test]
        public void CompareTupleWithGreedy()
        {
            target.Add(0);
            target.Add(1);
            Tuple tuple = new Tuple();
            tuple.Add(0);
            tuple.Add(0);

            Assert.AreEqual(1, this.target.CompareTo(tuple));
        }

        [Test]
        public void AcceptsNulls()
        {
            target.Add(null);
        }
    }
}