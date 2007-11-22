using System;
using System.IO;

namespace MbUnit.Tests.Samples
{
    public sealed class SampleHelper
    {
        public static string TestFixture()
        {
            using (Stream s = typeof(SampleHelper).Assembly.GetManifestResourceStream(
                "MbUnit.Tests.Samples.TestFixtureSample.cs")
                )
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
