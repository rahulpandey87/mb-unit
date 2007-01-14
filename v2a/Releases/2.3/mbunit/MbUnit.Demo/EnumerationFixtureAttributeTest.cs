
namespace MbUnit.Demo 
{
	using System;

	using System.Collections;
	using System.Collections.Specialized;
	using MbUnit.Framework;
	
	
	[EnumerationFixture]
	public class EnumerationFixtureAttributeAttributeTest
	{
		private Random rnd = new Random();
		private int count = 100;
		
		[DataProvider(typeof(ArrayList))]
		public ArrayList Data()
		{
			ArrayList list = new ArrayList();
			for(int i=0;i<count;++i)
				list.Add(rnd.Next());
			return list;
		}
		
		[CopyToProvider(typeof(ArrayList))]
		public ArrayList ArrayListProvider(IList source)
		{
			ArrayList list = new ArrayList(source);
			return list;
		}
		
		[CopyToProvider(typeof(int[]))]
		public int[] IntArrayProvider(IList source)
		{
			int[] list = new int[source.Count];
			source.CopyTo(list,0);
			return list;
		}		
	}
	
	/// <summary>
	/// TODO - Add class summary
	/// </summary>
	/// <remarks>
	/// 	created by - dehalleux
	/// 	created on - 30/01/2004 18:05:06
	/// </remarks>
	[EnumerationFixture]
	public class DictionaryEnumerationFixtureAttributeAttributeTest
	{
		private Random rnd = new Random();
		private int count = 100;
		
		[DataProvider(typeof(Hashtable))]
		public Hashtable HashtableData()
		{
			Hashtable list = new Hashtable();
			for(int i=0;i<count;++i)
				list.Add(rnd.Next(),rnd.Next());
			return list;
		}
		
		[CopyToProvider(typeof(Hashtable))]
		public Hashtable HashtableProvider(IDictionary source)
		{
			Hashtable list = new Hashtable();
			foreach(DictionaryEntry de in source)
			{
				list.Add(de.Key,de.Value);
			}
			return list;
		}
		
		[CopyToProvider(typeof(SortedList))]
		public SortedList SortedListProvider(IDictionary source)
		{
			SortedList list = new SortedList();
			foreach(DictionaryEntry de in source)
			{
				list.Add(de.Key,de.Value);
			}
			return list;
		}
	}
}
