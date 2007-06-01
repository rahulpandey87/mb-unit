using System;
using System.Data;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="IDataGenerator"/> method generates float values in a range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </para>
    /// </remarks>
	public class SingleGenerator : DataGeneratorBase
	{
		private System.Single minValue = 0;
		private System.Single maxValue = 1;
		
		public SingleGenerator(DataColumn column)
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
				return typeof(System.Single);
			}
		}

		/// <summary>
		/// Gets or sets the minimum generated value
		/// </summary>
		/// <value>
		/// Minimum generated value. Default is 0
		/// </value>
		public System.Single MinValue
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
		/// Maximum generated value. Default is 0
		/// </value>
		public System.Single MaxValue
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
				(System.Single)(this.MinValue + Rnd.NextDouble()* (this.MaxValue - this.MinValue))
				);
		}		
	}
}
