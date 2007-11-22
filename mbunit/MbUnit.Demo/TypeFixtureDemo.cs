using System;
using System.Collections;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Demo.Ayende
{
    public class ArrayListFactory
    {
        [Factory]
        public ArrayList Empty
        {
            get
            {
                return new ArrayList();
            }
        }
        [Factory]
        public ArrayList TwoElems
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(0);
                list.Add(1);
                return list;
            }
        }
    }

    [CurrentFixture]
    [TypeFixture(typeof(IEnumerable))]
    [ProviderFactory(typeof(ArrayListFactory), typeof(IEnumerable))]
    public class EnumerableFixture
    {
        [Test]
        [ExpectedException(
             typeof(InvalidOperationException),
             "Current called while cursor is before the first element"
        )]
        public void CurrentCalledBeforeMoveNext(IEnumerable en)
        {
            IEnumerator er = en.GetEnumerator();
            object p = er.Current;
        }
        [Test]
        [ExpectedException(
             typeof(InvalidOperationException),
             "Current called while cursor is past the last element"
        )]
        public void CurrentCalledAfterFinishedMoveNext(IEnumerable en)
        {
            IEnumerator er = en.GetEnumerator();
            while (er.MoveNext()) ;
            object p = er.Current;
        }
    }
}