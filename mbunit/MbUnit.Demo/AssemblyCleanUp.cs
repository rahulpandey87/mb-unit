using System;


using MbUnit.Framework;

[assembly: MbUnit.Framework.AssemblyCleanup(typeof(MbUnit.Demo.AssemblyCleaner))]

namespace MbUnit.Demo
{

    public class AssemblyCleaner
    {
        [SetUp]
        public static void SetUp()
        {
            Console.WriteLine("Setting up {0}", typeof(AssemblyCleaner).Assembly.FullName);
            Console.WriteLine(DateTime.Now.ToLongTimeString());
        }
        [TearDown]
        public static void TearDown()
        {
            Console.WriteLine("Cleaning up {0}", typeof(AssemblyCleaner).Assembly.FullName);
            Console.WriteLine(DateTime.Now.ToLongTimeString());
        }
    }
}
