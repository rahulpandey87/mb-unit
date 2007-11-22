using System;
using System.Data;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;

namespace TestFu.Data.Generators
{
    public class NameStringGenerator : StringGeneratorBase
    {
        private StringCollection names=new StringCollection();
        private int nameCount=2;

        public NameStringGenerator(DataColumn column)
            :base(column)
        {}
        public NameStringGenerator(DataColumn column,StringCollection names)
            :base(column)
        {
            if (names == null)
                throw new ArgumentNullException("names");
            if (names.Count == 0)
                throw new ArgumentException("Count is zero", "names");
            this.names = names;
        }

        public StringCollection Names
        {
            get
            {
                return this.names;
            }
        }

        public int NameCount
        {
            get
            {
                return this.nameCount;
            }
            set
            {
                this.nameCount = value;
            }
        }

        public static NameStringGenerator CreateFromUsNames(DataColumn column, int nameCount)
        {
            if (column == null)
                throw new ArgumentNullException("column");

            NameStringGenerator gen = new NameStringGenerator(column, GeneratorSeeds.UsMaleNames);
            gen.NameCount = nameCount;

            return gen;
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

            StringWriter sw = new StringWriter();
            int length = 0;
            for (int i = 0; i <this.NameCount && length < this.MaxLength; ++i)
            {
                int index = Rnd.Next(this.Names.Count);
                string s = this.Names[index];
                if (length + s.Length + 1 > this.MaxLength)
                    break;

                if (i == 0)
                    sw.Write(s);
                else
                    sw.Write(" {0}", s);
                length += s.Length + 1;
            }

            string name = sw.ToString();
            this.FillData(row, name);
        }
    }
}
