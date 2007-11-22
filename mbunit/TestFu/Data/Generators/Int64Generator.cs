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
	public class Int64Generator : DataGeneratorBase
	{
		private System.Int64 minValue = 0;
		private System.Int64 maxValue = System.Int32.MaxValue;
		
		public Int64Generator(DataColumn column)
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
				return typeof(System.Int64);
			}
		}

		/// <summary>
		/// Gets or sets the minimum generated value
		/// </summary>
		/// <value>
		/// Minimum generated value. Default is <see cref="int.MinValue"/>
		/// </value>
		public System.Int64 MinValue
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
		public System.Int64 MaxValue
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

			this.FillData(row,Rnd.Next((int)this.minValue, (int)this.maxValue));
		}		
	}
}
