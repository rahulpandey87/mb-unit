using System;
using System.Collections;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Tests.Data.Adapters
{
	[TestFixture]
	public class AdaptTest
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FromNull()
		{
			Adapt.From(null);	
		}

		[Test]
		public void FromIList()
		{
			IList list = new ArrayList();
			Binding b = Adapt.From(list);	
			
			Assert.IsNotNull(b);
			Assert.AreEqual(typeof(ListAdapter),b.GetType());
		}


		[Test]
		public void FromITypedList()
		{
			Assert.Fail();
		}
		
	}
}
