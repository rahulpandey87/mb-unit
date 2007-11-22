using System;
using System.Collections;

using MbUnit.Framework;

namespace MbUnit.Demo
{
	[CompositeFixture(typeof(EnumerableTest))]
	[ProviderFactory(typeof(ArrayListFactory),typeof(IEnumerable))]
	[ProviderFactory(typeof(HashtableFactory),typeof(IEnumerable))]
	[Pelikhan]
	[FixtureCategory("Important.Tests.Should.Be.Here")]
	[FixtureCategory("A.Test.Can.Be.In.Multiple.Categories")]
	[FixtureCategory("A.Test.Can.Be.In.Multiple.Categories2")]
	[Importance(TestImportance.Critical)]
	public class CompositeTest
	{
	}
}
