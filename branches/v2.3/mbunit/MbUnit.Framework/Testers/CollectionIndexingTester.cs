using System;
using System.Collections;
using System.Reflection;

namespace MbUnit.Framework.Testers
{
	using MbUnit.Core;
	using MbUnit.Framework;
	using MbUnit.Core.Exceptions;	

	/// <summary>
	/// Collection indexing test class
	/// </summary>
	/// <include file="MbUnit.Framework.Doc.xml" path="dot/remarkss/remarks[@name='CollectionIndexingTester']"/>
	public class CollectionIndexingTester
	{
		private IndexerProviderAttribute attribute;
		private PropertyInfo indexer;
		
		public CollectionIndexingTester(IndexerProviderAttribute attribute)
		{
			if (attribute==null)
				throw new ArgumentNullException("attribute");
			
			this.attribute = attribute;
			this.indexer = TypeHelper.GetIndexer(
				attribute.ProviderType,
				attribute.IndexType
				);
		}
		
		public IndexerProviderAttribute Attribute
		{
			get
			{
				return this.attribute;
			}
		}
		
		protected Object InvokeIndexer(Object list, Object index)
		{
			return TypeHelper.GetValue(this.indexer,list, index);
		}
		
		[Test("Iterates over the elements of the list")]
		public void Iterate(Object fixture, Object list)
		{			
			Object item = this.Attribute.First;
			while (!item.Equals(this.Attribute.Last))
			{
				InvokeIndexer(list,item);					
				item=this.Attribute.Iterator.Increment(item);
			}
		}	
			
		[Test("Accessing element before the first index should throw")]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void BeforeFirst(Object fixture, Object list)
		{			
			InvokeIndexer(list,this.Attribute.Iterator.Decrement(this.Attribute.First));
		}			
		
		[Test("Accessing element after the last index should throw")]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void AfterLast(Object fixture, Object list)
		{			
			InvokeIndexer(list,this.Attribute.Iterator.Increment(this.Attribute.Last));
		}			
		
	}
}
