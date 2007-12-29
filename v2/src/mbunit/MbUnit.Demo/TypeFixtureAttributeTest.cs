
namespace MbUnit.Demo 
{
	using System;

	using MbUnit.Framework;
	using System.Collections;
	using System.Diagnostics;
	
	public class IListProviderFactory
	{
        [Factory]
		public ArrayList SomeArray
		{
			get
			{
				return new ArrayList();
			}
		}

        [Factory]
        public ArrayList AnotherArray
        {
            get
            {
                return new ArrayList();
            }
        }
	}
	
	/// <summary>
	/// TODO - Add class summary
	/// </summary>
	/// <remarks>
	/// 	created by - dehalleux
	/// 	created on - 30/01/2004 11:56:44
	/// </remarks>
	[TypeFixture(typeof(IList),"IList test")]
	[ProviderFactory(typeof(IListProviderFactory), typeof(IList))]
	public class TypeFixtureAttributeTest
	{
		[Provider(typeof(ArrayList))]
		public ArrayList ProvideArrayList()
		{
			return new ArrayList();
		}

		[Provider(typeof(ArrayList))]
		public ArrayList ProvideFilledArrayList()
		{
			ArrayList list = new ArrayList();
			list.Add("hello");
			list.Add("world");
			
			return list;
		}
				
				
		[SetUp]
		public void SetUp(IList list)
		{
			Console.WriteLine("Starting test");
		}
				
		[Test]
		public void AddElement(IList list)
		{
			int count = list.Count;
			list.Add(null);
			Assert.AreEqual(list.Count,count+1);
		}
	}
}
