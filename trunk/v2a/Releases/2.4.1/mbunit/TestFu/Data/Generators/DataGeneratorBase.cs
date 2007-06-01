using System;
using System.Data;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// Abstract base class from <see cref="IDataGenerator"/> instance.
	/// </summary>
	public abstract class DataGeneratorBase : IDataGenerator
	{
		private static Random rnd = new Random((int)DateTime.Now.Ticks);
		private DataColumn column;
		private double nullProbability = 0;
		
		public DataGeneratorBase(DataColumn column)
		{
			if (column==null)
				throw new ArgumentNullException("column");
			this.column=column;
			this.Update();
		}
		

		/// <summary>
		/// Gets the generated type
		/// </summary>
		/// <value>
		/// Generated type.
		/// </value>
		public abstract Type GeneratedType {get;}

		/// <summary>
		/// Resets the generator
		/// </summary>		
		public virtual void Reset()
		{
			rnd = new Random((int)DateTime.Now.Ticks);
		}
		
		/// <summary>
		/// Generates a new value
		/// </summary>
		/// <returns>
		/// New random data.
		/// </returns>		
		public abstract void GenerateData(DataRow row);
		
		/// <summary>
		/// Gets the target column
		/// </summary>
		/// <value>
		/// Target <see cref="DataColumn"/> instance.
		/// </value>		
		public DataColumn Column
		{
			get
			{
				return this.column;
			}
			set
			{
				this.column=value;
			}
		}
		
		/// <summary>
		/// Updates the internal data and verifies column information.
		/// </summary>
		protected virtual void Update()
		{
			if (this.column==null)
				throw new NotSupportedException("Column cannot be null");
			if (!this.column.DataType.IsAssignableFrom(this.GeneratedType))
				throw new NotSupportedException("Type mistmach");
			if (!this.column.AllowDBNull)
				this.nullProbability=0;
		}

		/// <summary>
		/// Gets or sets the probability to produce a NULL
		/// </summary>
		/// <remarks>
		/// This value determines the probability to produce a null value. The probability ranges from
		/// 0, never to 1 always.
		/// </remarks>
		/// <value>
		/// The probability to produce a null object.
		/// </value>
		public double NullProbability
		{
			get
			{
				return this.nullProbability;
			}
			set
			{
				if (value<0)
					throw new ArgumentOutOfRangeException("NullProbability",value,"Must be greater that 0");
				if (value>1)
					throw new ArgumentOutOfRangeException("NullProbability",value,"Must be lower that 1");
				this.nullProbability = value;
			}
		}

        public override string ToString()
        {
            return String.Format("{0}: {1}", this.Column.ColumnName, this.GeneratedType.Name);
        }

        protected static Random Rnd
		{
			get
			{
                lock (typeof(DataGeneratorBase))
                {
                    return rnd;
                }
            }
		}
		
		protected virtual void FillData(DataRow row, Object data)
		{
			row[this.Column.Ordinal] = data;
		}
		
		protected virtual bool FillNull(DataRow row)
		{
			if (this.generateNull())
			{
				FillData(row,null);
				return true;
			}
			else
				return false;
		}
		
		private bool generateNull()
		{
			return rnd.NextDouble() < this.NullProbability;
		}
	}
}
