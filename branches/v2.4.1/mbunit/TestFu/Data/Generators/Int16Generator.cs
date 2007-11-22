using System;
using System.Data;

namespace TestFu.Data.Generators
{
	/// <summary>
	/// A random data generator for <see cref="int"/> values.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This <see cref="IDataGenerator"/> method generates int values in a range [<see cref="MinValue"/>, <see cref="MaxValue"/>].
    /// </para>
    /// </remarks>
	public class Int16Generator : DataGeneratorBase
	{
		private System.Int16 minValue = 0;
		private System.Int16 maxValue = System.Int16.MaxValue;
		
		public Int16Generator(DataColumn column)
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
				return typeof(System.Int16);
			}
		}

		/// <summary>
		/// Gets or sets the minimum generated value
		/// </summary>
		/// <value>
		/// Minimum generated value. Default is <see cref="int.MinValue"/>
		/// </value>
		public System.Int16 MinValue
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
		/// Maximum generated value. Default is <see cref="int.MaxValue"/>
		/// </value>
		public System.Int16 MaxValue
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
			
			this.FillData(row,Rnd.Next(this.minValue, this.maxValue));
		}		
	}
}
