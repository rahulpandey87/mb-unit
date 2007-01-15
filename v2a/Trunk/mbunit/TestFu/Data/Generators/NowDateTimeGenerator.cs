using System;
using System.Data;

namespace TestFu.Data.Generators
{
    public class NowDateTimeGenerator : DataGeneratorBase
    {
        public NowDateTimeGenerator(DataColumn column)
            :base(column)
        {}

        public override Type GeneratedType
        {
            get { return typeof(DateTime); }
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
                DateTime.Now
                );
        }
    }
}
