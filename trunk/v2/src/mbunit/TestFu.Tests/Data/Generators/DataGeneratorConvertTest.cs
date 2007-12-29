using System;
using System.Data;
using MbUnit.Core.Framework;
using MbUnit.Framework;
using TestFu.Data;
using TestFu.Data.Generators;

namespace TestFu.Tests.Data.Generators
{

	[TestFixture]
	public class DataGeneratorConvertTest
	{
		private DataTable table =new DataTable("DummyTable");
		[Test]
		public void Binary()
		{
			this.addGenerator(typeof(byte[]), typeof(BinaryGeneratorBase));
		}
		[Test]
		public void Boolean()
		{
			this.addGenerator(typeof(bool), typeof(BooleanGenerator));
		}
		[Test]
		public void Byte()
		{
			this.addGenerator(typeof(byte), typeof(ByteGenerator));
		}
		[Test]
		public void DateTime()
		{
			this.addGenerator(typeof(DateTime), typeof(NowDateTimeGenerator));
		}
		[Test]
		public void Decimal()
		{
			this.addGenerator(typeof(decimal), typeof(DecimalGenerator));
		}
		[Test]
		public void Double()
		{
			this.addGenerator(typeof(double), typeof(DoubleGenerator));
		}
		[Test]
		public void Guid()
		{
			this.addGenerator(typeof(Guid), typeof(GuidGenerator));
		}
		[Test]
		public void Int16()
		{
			this.addGenerator(typeof(Int16), typeof(Int16Generator));
		}
		[Test]
		public void Int32()
		{
			this.addGenerator(typeof(Int32), typeof(Int32Generator));
		}
		[Test]
		public void Int64()
		{
			this.addGenerator(typeof(Int64), typeof(Int64Generator));
		}
		[Test]
		public void Single()
		{
			this.addGenerator(typeof(Single), typeof(SingleGenerator));
		}
		[Test]
		public void String()
		{
			this.addGenerator(typeof(String), typeof(RangeStringGenerator));
		}

		private void addGenerator(Type type, Type expectedGeneratorType)
		{
			DataColumn col = table.Columns.Add(type.Name,type);
			IDataGenerator gen = DataGeneratorConverter.FromColumn(col);

			Assert.AreEqual(expectedGeneratorType, gen.GetType());
			Assert.AreEqual(type, gen.GeneratedType);
		}
	}
}
