using System;
using System.Data;

namespace TestFu.Data.Generators
{
	public sealed class DataGeneratorConverter
	{
		private DataGeneratorConverter(){}
		
		public static IDataGenerator FromColumn(DataColumn column)
		{
			if (column.DataType.IsAssignableFrom(typeof(System.Byte[])))
				return new RandomBinaryGenerator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Boolean)))
				return new BooleanGenerator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Byte)))
				return new ByteGenerator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.DateTime)))
				return new NowDateTimeGenerator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Decimal)))
				return new DecimalGenerator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Double)))
				return new DoubleGenerator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Guid)))
				return new GuidGenerator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Int16)))
				return new Int16Generator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Int32)))
				return new Int32Generator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Int64)))
				return new Int64Generator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Data.SqlTypes.SqlMoney)))
				return new MoneyGenerator(column);
			if (column.DataType.IsAssignableFrom(typeof(System.Single)))
				return new SingleGenerator(column);
            if (column.DataType.IsAssignableFrom(typeof(System.String)))
            {                
                if (column.ColumnName.EndsWith("FirstName"))
                    return NameStringGenerator.CreateFromUsNames(column,2);
                if (column.ColumnName.EndsWith("MiddleName"))
                    return NameStringGenerator.CreateFromUsNames(column,2);
                if (column.ColumnName.EndsWith("LastName"))
                    return NameStringGenerator.CreateFromUsNames(column,2);
                if (column.ColumnName.EndsWith("Name"))
                    return NameStringGenerator.CreateFromUsNames(column, 3);

                return new RangeStringGenerator(column);
            }

            throw new NotImplementedException("column type not implemented");
		}
	}
}
