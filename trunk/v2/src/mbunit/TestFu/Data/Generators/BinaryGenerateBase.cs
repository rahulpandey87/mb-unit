using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TestFu.Data.Generators
{
    /// <summary>
    /// A random data generator for <see cref="Byte"/> values.
    /// </summary>
    /// <remarks>
    /// <para>
	/// This <see cref="IDataGenerator"/> method generates Byte arrays with length in the range
	/// [<see cref="MinLength"/>, <see cref="MaxLength"/>].
    /// </para>
    /// </remarks>
    public abstract class BinaryGeneratorBase : DataGeneratorBase,
        IRangeDataGenerator
    {
        private int minLength = 16;
        private int maxLength = 16;

        public BinaryGeneratorBase(DataColumn column)
		:base(column)
        { }

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
                return typeof(System.Byte[]);
            }
        }

        /// <summary>
        /// Gets or sets the minimum length of the generated value
        /// </summary>
        /// <value>
		/// Minimum generated length. Default is 16. 
		/// </value>
        public int MinLength
        {
            get
            {
                return this.minLength;
            }
            set
            {
                this.minLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum generated value
        /// </summary>
        /// <value>
		/// Maximum generated length. Default is 16.
        /// </value>
        public int MaxLength
        {
            get
            {
                return this.maxLength;
            }
            set
            {
                this.maxLength = value;
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

            this.GenerateBytes(row);
        }

        protected abstract void GenerateBytes(DataRow row);
    }
}
