using System;
using System.Data;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// A random data generator for <see cref="DateTime"/> values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="IDataGenerator"/> method generates DateTime values in a range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
	/// </para>
    /// </remarks>
	public class DateTimeGenerator : DataGeneratorBase
	{
		private DateTime minValue = DateTime.MinValue;
		private DateTime maxValue = DateTime.MaxValue;
		
		public DateTimeGenerator(DataColumn column)
		:base(column)
		{}

		/// <summary>
		/// Gets the generated type
		/// </summary>
		/// <value>
		/// Generated type.
		/// </value>
		public override  Type GeneratedType 
		{
			get
			{
				return typeof(System.DateTime);
			}
		}

		/// <summary>
		/// Gets or sets the minimum generated value
		/// </summary>
		/// <value>
		/// Minimum generated value. Default is <see cref="DateTime.MinValue"/>
		/// </value>
		public DateTime MinValue
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
		/// Maximum generated value. Default is <see cref="DateTime.MaxValue"/>
		/// </value>
		public DateTime MaxValue
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
			
			TimeSpan ts = this.maxValue-this.minValue;
			long interval = (long)(ts.Ticks * Rnd.NextDouble());
			
			this.FillData(
				row,
				this.MinValue + new TimeSpan(interval)
				);
		}		
	}
}
