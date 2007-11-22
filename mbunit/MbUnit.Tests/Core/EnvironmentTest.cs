using System;
using System.Collections;
using System.IO;
using MbUnit.Core.Framework;
using MbUnit.Framework;

namespace MbUnit.Tests.Core.Reports.Serialization
{
    [TestFixture]
    public class EnvironmentTest
    {
        [Test]
        public void AssemblyLocation()
        {
            Console.WriteLine("Assembly.Location: {0}", this.GetType().Assembly.Location);
            Console.WriteLine("Assembly.CodeBase: {0}", this.GetType().Assembly.CodeBase);
        }
        [Test]
        public void ShowVariables()
        {
            Console.WriteLine("Environment.CurrentDirectory: {0}", Environment.CurrentDirectory);
            foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
            {
                Console.WriteLine("{0}: {1}", de.Key, de.Value);
            }
        }
    }
}

