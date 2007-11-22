
namespace MbUnit.Demo 
{
	using System;
	using MbUnit.Framework;

	using System.Collections;
	
	
	[CollectionIndexingFixture]
	public class CollectionIndexingFixtureAttributeTest
	{		 
		[IntIndexerProvider(typeof(ArrayList),100)]
		 public ArrayList ProvideArrayList100()
		 {
		 	ArrayList list = new ArrayList();
		 	for(int i =0;i<100;++i)
		 		list.Add(i);	 	
		 	
		 	return list;
		 }
		 
		[IntIndexerProvider(typeof(ArrayList),10)]
		 public ArrayList ProvideArrayList()
		 {
		 	ArrayList list = new ArrayList();
		 	for(int i=0;i<10;++i)
		 		list.Add(i);
		 	
		 	return list;
		 }
		 
	}
}
