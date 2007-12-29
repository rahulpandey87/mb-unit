
namespace MbUnit.Framework.Testers
{
	using System;
	using System.Collections;
	using MbUnit.Core;
	using MbUnit.Framework;
		
	/// <summary>
	/// Collection order tester class.
	/// </summary>
	public class CollectionOrderTester
	{
		private CollectionOrderTest order;		
		private IComparer comparer;

		public CollectionOrderTester(CollectionOrderTest order, IComparer comparer)
		{
			if (comparer==null)
				throw new ArgumentNullException("comparer");

			this.order = order;
			this.comparer = comparer;
		}
		
		[Test("Sort the provided collection and applies order test")]
		public Object Sort(Object o, IEnumerable list)
		{			
			IEnumerator en = ((IEnumerable)list).GetEnumerator();
			
			switch(order)
			{
				case CollectionOrderTest.OrderedAscending:
					TestAscending(en);
				break;
				case CollectionOrderTest.OrderedDescending:
					TestDescending(en);
					break;
			}
			
			return null;
		}
		
		internal void TestAscending(IEnumerator en)
		{			
			if (!en.MoveNext())
				return;
			
			Object previous = en.Current;
			while(en.MoveNext())
			{
				Assert.IsTrue(this.comparer.Compare(previous, en.Current)<=0);
				previous = en.Current;
			}
		}
		
		internal void TestDescending(IEnumerator en)
		{
			if (!en.MoveNext())
				return;
			
			Object previous = en.Current;
			while(en.MoveNext())
			{
				Assert.IsTrue(this.comparer.Compare(previous, en.Current)>=0);
				previous = en.Current;
			}
		}
	}

}
