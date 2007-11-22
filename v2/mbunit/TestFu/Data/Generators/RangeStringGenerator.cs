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
	public class RangeStringGenerator : StringGeneratorBase
	{
        private string characters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
		/// Initializes a new instance of <see cref="RangeStringGenerator"/>.
		/// </summary>
		/// <param name="column"></param>
		public RangeStringGenerator(DataColumn column)
		:base(column)
		{}

        /// <summary>
        /// Gets or sets the string containing the generated characters
        /// </summary>
        /// <value></value>
        public string Characters
        {
            get
            {
                return this.characters;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("characters");
                if (value.Length == 0)
                    throw new ArgumentException("Length is zero", "Characters");
                this.characters = value;
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
			
			// get length
			int length = Rnd.Next(this.MinLength, this.MaxLength);
			char[] gen = new char[length];
			
			// create string with
			
			for(int i=0;i<length;++i)
			{
				// generate char
				char c = characters[Rnd.Next(characters.Length)];				
				gen[i]=c;
			}
			this.FillData(row,new String(gen));
		}		
	}
}
