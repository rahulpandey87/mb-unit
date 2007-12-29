using System;
using System.Collections;

using MbUnit.Framework;

namespace MbUnit.Demo 
{

	[TypeFixture(typeof(IEnumerable))]
	[ProviderFactory(typeof(ArrayListFactory),typeof(IEnumerable))]
	[ProviderFactory(typeof(HashtableFactory),typeof(IEnumerable))]
	[Pelikhan]
	[Importance(TestImportance.Critical)]
	public class EnumerableTest
	{
		[SetUp]
		public void SetUp()
		{
			Console.WriteLine("SetUp");
		}

		[Test]
		public void GetEnumeratorNotNull(IEnumerable en)
		{
			Assert.IsNotNull(en.GetEnumerator());
		}

		[TearDown]
		public void TearDown(IEnumerable en)
		{
			Console.WriteLine("TearDown");
		}
	}

}
