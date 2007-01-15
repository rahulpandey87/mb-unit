using System;
using System.Data;

namespace TestFu.Data.Generators
{
    public class RandomBinaryGenerator : BinaryGeneratorBase
    {
        public RandomBinaryGenerator(DataColumn column)
            :base(column)
        {}


        protected override void GenerateBytes(DataRow row)
        {
            int length = Rnd.Next(this.MinLength, this.MaxLength);
            byte[] bytes = new byte[length];

            for (int i = 0; i < length; ++i)
                bytes[i] = (byte)Rnd.Next(8);

            row[this.Column] = bytes;
        }
    }
}
