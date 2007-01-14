using System;
using System.Collections.Specialized;
using System.IO;

namespace TestFu.Data.Generators
{
    public sealed class GeneratorSeeds
    {
        private static volatile StringCollection usMaleNames = null;
        private static volatile string loremIpsum = null;
        private static readonly object syncRoot = new object();

        private GeneratorSeeds()
        {}

        public static String LoremIpsum
        {
            get
            {
                if (loremIpsum == null)
                {
                    lock (syncRoot)
                    {
                        using (Stream stream = typeof(NameStringGenerator).Assembly.GetManifestResourceStream("TestFu.Data.Generators.LoremIpsum.txt"))
                        {
                            if (stream == null)
                                throw new InvalidOperationException("Could not find TestFu.Data.Generators.LoremIpsum.txt resource");
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                loremIpsum = reader.ReadToEnd();
                            }
                        }
                    }
                }
                return loremIpsum;
            }
        }

        public static StringCollection UsMaleNames
        {
            get
            {
                if (usMaleNames == null)
                {
                    lock (syncRoot)
                    {
                        usMaleNames = new StringCollection();
                        using (Stream stream = typeof(NameStringGenerator).Assembly.GetManifestResourceStream("TestFu.Data.Generators.UsMaleNames.txt"))
                        {
                            if (stream == null)
                                throw new InvalidOperationException("Could not find TestFu.Data.Generators.UsMaleNames.txt resource");
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                foreach (string name in reader.ReadToEnd().Split('\r'))
                                {
                                    if (name == null || name.Length < 1)
                                        continue;
                                    string nameCorr = name.TrimStart('\n','\r');
                                    if (nameCorr.Length < 1)
                                        continue;
                                    nameCorr = String.Format("{0}{1}", Char.ToUpper(nameCorr[0]),
                                        nameCorr.Substring(1).ToLower());
                                    usMaleNames.Add(nameCorr);
                                }
                            }
                        }
                    }
                }
                return usMaleNames;
            }
        }
    }
}
