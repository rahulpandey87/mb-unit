using System;
using System.Data;
using System.Data.SqlTypes;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// A random data generator for <see cref="SqlMoney"/> values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="IDataGenerator"/> method generates float values in a range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </para>
    /// </remarks>
	public class MoneyGenerator : DataGeneratorBase
	{
		private SqlMoney minValue = SqlMoney.MinValue;
		private SqlMoney maxValue = SqlMoney.MaxValue;
		
		public MoneyGenerator(DataColumn column)
			:base(column)
		{}

		/// <summary>
		/// Gets the generated type
		/// </summary>
		/// <value>
		/// Generated type.
		/// </value>
		public override Type GeneratedType 
		{
			get
			{
				return typeof(SqlMoney);
			}
		}

		/// <summary>
		/// Gets or sets the minimum generated value
		/// </summary>
		/// <value>
		/// Minimum generated value. Default is <see cref="float.MinValue"/>
		/// </value>
		public SqlMoney MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				this.minValue=value;
			}
		}
		
		/// <summary>
		/// Gets or sets the maximum generated value
		/// </summary>
		/// <value>
		/// Maximum generated value. Default is <see cref="float.MaxValue"/>
		/// </value>
		public SqlMoney MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				this.maxValue=value;
			}
		}
		/// <summary>
		/// Generates a new value
		/// </summary>
		/// <returns>
		/// New random data.
		/// </returns>		
		public override void GenerateData(DataRow row)
		{
			if (this.FillNull(row))
				return;
			
			this.FillData(
				row,
				new SqlMoney(this.MinValue.ToDouble() + Rnd.NextDouble()* (this.MaxValue.ToDouble() - this.MinValue.ToDouble()))
				);
		}		
	}
}
