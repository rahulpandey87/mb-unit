using System;
using System.Data;
using System.Collections;
using MbUnit.Framework;
using MbUnit.Core.Framework;

namespace TestFu.Tests.Data.Generators
{
	using TestFu.Data;
	using TestFu.Data.Generators;

	public class AllDataGeneratorFactory
	{
		private Random rnd =new Random((int)System.DateTime.Now.Ticks);
		private DataTable table=new DataTable("AllTypes");
        private Hashtable generators = new Hashtable();

        public AllDataGeneratorFactory()
		{
			// filling table columns and generators
			this.addGenerator(typeof(byte[]));
			this.addGenerator(typeof(bool));
			this.addGenerator(typeof(byte));
			this.addGenerator(typeof(DateTime));
			this.addGenerator(typeof(decimal));
			this.addGenerator(typeof(double));
			this.addGenerator(typeof(Guid));
			this.addGenerator(typeof(Int16));
			this.addGenerator(typeof(Int32));
			this.addGenerator(typeof(Int64));
			this.addGenerator(typeof(Single));
			this.addGenerator(typeof(String));

            this.addGenerator("UserFirstName", typeof(string));

            DataColumn col = table.Columns.Add("LoremIpsum", typeof(string));
            IDataGenerator gen = new LoremIpsumStringGenerator(col);
            this.generators.Add(gen.GetType(), gen);

        }

        private void addGenerator(string columnName, Type type)
        {
            DataColumn col = table.Columns.Add(columnName, type);
            IDataGenerator gen = DataGeneratorConverter.FromColumn(col);
            this.generators.Add(gen.GetType(), gen);
        }

        private void addGenerator(Type type)
		{
            addGenerator(type.Name, type);
        }

		public DataTable Table
		{
			get
			{
				return this.table;
			}
		}

		public ICollection Generators
		{
			get
			{
				return this.generators.Values;
			}
		}

        [Factory]
		public BinaryGeneratorBase Binary
		{
			get
			{
                return (BinaryGeneratorBase)this.generators[typeof(BinaryGeneratorBase)];
            }
		}

        [Factory]
        public BooleanGenerator Boolean
        {
			get
			{
                return (BooleanGenerator)this.generators[typeof(BooleanGenerator)];
            }
		}

        [Factory]
        public ByteGenerator Byte
        {
			get
			{
                return (ByteGenerator)this.generators[typeof(ByteGenerator)];
            }
		}

        [Factory]
        public NowDateTimeGenerator DateTime
        {
			get
			{
                return (NowDateTimeGenerator)this.generators[typeof(NowDateTimeGenerator)];
            }
		}

        [Factory]
        public DecimalGenerator Decimal
        {
			get
			{
                return (DecimalGenerator)this.generators[typeof(DecimalGenerator)];
            }
		}

        [Factory]
        public DoubleGenerator Double
        {
			get
			{
                return (DoubleGenerator)this.generators[typeof(DoubleGenerator)];
            }
		}

        [Factory]
        public GuidGenerator Guid
        {
			get
			{
                return (GuidGenerator)this.generators[typeof(GuidGenerator)];
            }
		}

        [Factory]
        public Int16Generator Int16
        {
			get
			{
                return (Int16Generator)this.generators[typeof(Int16Generator)];
            }
		}
        [Factory]
        public Int32Generator Int32
        {
			get
			{
                return (Int32Generator)this.generators[typeof(Int32Generator)];
            }
		}
        [Factory]
        public Int64Generator Int64
        {
			get
			{
                return (Int64Generator)this.generators[typeof(Int64Generator)];
            }
		}
        [Factory]
        public SingleGenerator Single
        {
			get
			{
                return (SingleGenerator)this.generators[typeof(SingleGenerator)];
            }
		}
        [Factory]
        public RangeStringGenerator RangeString
        {
			get
			{
                return (RangeStringGenerator)this.generators[typeof(RangeStringGenerator)];
            }
		}
        [Factory]
        public NameStringGenerator NameString
        {
            get
            {
                return (NameStringGenerator)this.generators[typeof(NameStringGenerator)];
            }
        }
        [Factory]
        public LoremIpsumStringGenerator LoremIpsum
        {
            get
            {
                return (LoremIpsumStringGenerator)this.generators[typeof(LoremIpsumStringGenerator)];
            }
        }
    }
}
