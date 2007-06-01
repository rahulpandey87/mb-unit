//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by a tool.
//     Runtime Version: 1.1.4322.573
//
//     Changes to this file may cause incorrect behavior and will be lost if 
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

namespace MbUnit.Demo 
{
	using System;
	using System.Collections;
	using MbUnit.Framework;

	

	[CollectionOrderFixture(CollectionOrderTest.OrderedAscending)]
	public class CollectionOrderFixtureAttributeTest
	{
		[Provider(typeof(ArrayList))]
		public ArrayList ProvideArrayList()
		{
			return new ArrayList();
		}
		
		[Fill]
		public void FillWithRandomDataThisMethodWillFail(IList list)
		{
			Random rnd = new Random();
			for(int i = 0;i<10;++i)
				list.Add(rnd.Next());
		}

		[Fill]
		public void FillSorted(IList list)
		{
			Random rnd = new Random();
			SortedList sl = new SortedList();
			for(int i = 0;i<10;++i)
				sl.Add(rnd.Next(),null);
			foreach(DictionaryEntry de in sl)
			{
				list.Add(de.Key);
			}
		}
	}
}
