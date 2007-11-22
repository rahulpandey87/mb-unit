using System;
using System.Data;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace TestFu.Tests.Data.Generators
{
	using TestFu.Data;

	[TypeFixture(typeof(IDataGenerator))]
	[ProviderFactory(typeof(AllDataGeneratorFactory),typeof(IDataGenerator))]
    [CurrentFixture]
	public class DataGeneratorTest
	{
		public bool AllowDBNull(IDataGenerator dg)
		{
			if (dg==null)
				throw new ArgumentNullException("dg");
			return dg.Column.AllowDBNull;
		}

		[Test]
		public void ColumnIsNotNull(IDataGenerator dg)
		{
			Assert.IsNotNull(dg.Column);
		}

		[Test]
		public void SetNullProbability(IDataGenerator dg)
		{
			dg.NullProbability=0;
			Assert.AreEqual(0,dg.NullProbability);
		}

		[Test]
		public void GeneratedTypeIsNotNull(IDataGenerator dg)
		{
			Assert.IsNotNull(dg.GeneratedType);
		}

		[Test]
		[ConditionalException(typeof(Exception),"AllowDBNull")]
		public void NullProbabilityIsOneIfNonNull(IDataGenerator dg)
		{
			Assert.AreEqual(1,dg.NullProbability);
		}

		[Test]
		public void GeneratesTheRightType(IDataGenerator dg)
		{
			dg.NullProbability=0;
			// generate row
			DataRow row = dg.Column.Table.NewRow();
			dg.GenerateData(row);
			Assert.IsFalse(row.HasErrors);
			// check that value has the right type
			ReflectionAssert.IsAssignableFrom(dg.GeneratedType,row[dg.Column].GetType());
		}
	}
}
