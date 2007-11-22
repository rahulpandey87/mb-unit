using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Text;

namespace TestFu.Data.Generators
{
    /// <summary>
    /// A random generator of <see cref="System.String"/> instances.
    /// </summary>
    public abstract class StringGeneratorBase : DataGeneratorBase,
        IRangeDataGenerator
    {
        private int minLength = 1;
        private int maxLength = 255;

        /// <summary>
        /// Initializes a new instance of <see cref="StringGeneratorBase"/>.
        /// </summary>
        /// <param name="column"></param>
        public StringGeneratorBase(DataColumn column)
		:base(column)
        {
            if (column.MaxLength > 0)
                this.maxLength = Math.Min(255, column.MaxLength);
        }

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
                return typeof(System.String);
            }
        }

        /// <summary>
        /// Gets or sets the minimum length of the string
        /// </summary>
        /// <value>
        /// Minimum length of the string.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// set proprety, the value is negative.
        /// </exception>
        public int MinLength
        {
            get
            {
                return this.minLength;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");
                this.minLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum length of the string
        /// </summary>
        /// <value>
        /// Maximum length of the string.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// set proprety, the value is less than 1.
        /// </exception>		
        public int MaxLength
        {
            get
            {
                return this.maxLength;
            }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");
                this.maxLength = Math.Min(value, 255); ;
            }
        }
    }
}
