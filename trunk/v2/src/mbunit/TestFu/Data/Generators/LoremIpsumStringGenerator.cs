using System;
using System.IO;
using System.Collections;
using System.Data;

namespace TestFu.Data.Generators
{
    public class LoremIpsumStringGenerator : StringGeneratorBase
    {
        public LoremIpsumStringGenerator(DataColumn column)
            :base(column)
        {}

        public override void GenerateData(DataRow row)
        {
            if (this.FillNull(row))
                return;

            int length = Rnd.Next(this.MinLength, this.MaxLength);
            char[] buffer = new char[length];

            string loremIpsum = GeneratorSeeds.LoremIpsum;

            for (int i = 0; i < length; ++i)
                buffer[i] = loremIpsum[i % loremIpsum.Length];

            this.FillData(row, new String(buffer));
        }
    }
}
