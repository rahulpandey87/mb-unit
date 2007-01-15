using System;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using MbUnit.Core.Filters;

namespace MbUnit.Framework.Tests.Core.Filters
{
    
	[TestFixture]
	[FixtureCategory("FixtureFilters.Tests")]
	[Importance(TestImportance.Critical)]
	internal class DummyFixture
	{}


    [TypeFixture(typeof(IFixtureFilter))]
    [ProviderFactory(typeof(FixtureFilterFactory), typeof(IFixtureFilter))]
    public class FixtureFilterTest
    {
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void FilterWithNullType(IFixtureFilter filter)
		{
			filter.Filter(null);
		}

		[Test]
        public void Filter(IFixtureFilter filter)
        {
			filter.Filter(typeof(DummyFixture));
		}

		[Test]
        public void IsXmlSerializable(IFixtureFilter filter)
        {
			SerialAssert.IsXmlSerializable(filter.GetType());
		}

		[Test]
        public void Serialize(IFixtureFilter filter)
        {
			Console.WriteLine(SerialAssert.OneWaySerialization(filter));
		}
	}
}
