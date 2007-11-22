using System;
using System.Data;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// A random data generator for <see cref="decimal"/> values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="IDataGenerator"/> method generates decimal values in a range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
	/// </para>
    /// </remarks>
	public class DecimalGenerator : DataGeneratorBase
	{
		private System.Decimal minValue = 0;
		private System.Decimal maxValue = 1;
		
		public DecimalGenerator(DataColumn column)
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
				return typeof(System.Decimal);
			}
		}

		/// <summary>
		/// Gets or sets the minimum generated value
		/// </summary>
		/// <value>
		/// Minimum generated value. Default is <see cref="decimal.MinValue"/>
		/// </value>
		public System.Decimal MinValue
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
		/// Maximum generated value. Default is <see cref="decimal.MaxValue"/>
		/// </value>
		public System.Decimal MaxValue
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
			
			double d = (double)(checked(this.MaxValue - this.MinValue));
			
			this.FillData(
				row,
				(decimal)(this.MinValue +  (System.Decimal)(Rnd.NextDouble()*d))
				);
		}		
	}
}
