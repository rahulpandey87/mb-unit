using System;
using System.Data;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// A random data generator for <see cref="byte"/> values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="IDataGenerator"/> method generates byte values in a range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
	/// </para>
    /// </remarks>
	public class ByteGenerator : DataGeneratorBase
	{
		private System.Byte minValue = System.Byte.MinValue;
		private System.Byte maxValue = System.Byte.MaxValue;
		
		public ByteGenerator(DataColumn column)
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
				return typeof(System.Byte);
			}
		}

		/// <summary>
		/// Gets or sets the minimum generated value
		/// </summary>
		/// <value>
		/// Minimum generated value. Default is <see cref="byte.MinValue"/>
		/// </value>
		public System.Byte MinValue
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
		/// Maximum generated value. Default is <see cref="byte.MaxValue"/>
		/// </value>
		public System.Byte MaxValue
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
				this.MinValue + (System.Byte)(Rnd.NextDouble()* (this.MaxValue - this.MinValue))
				);
		}		
	}
}
