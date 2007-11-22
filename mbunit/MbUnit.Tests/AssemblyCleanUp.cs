using System;
using System.Collections;
using MbUnit.Core.Framework;
using MbUnit.Framework;

[assembly: MbUnit.Framework.AssemblyCleanup(typeof(MbUnit.Tests.AssemblyCleaner))]

namespace MbUnit.Tests
{

    public class AssemblyCleaner
    {
        private static Stack callStack = new Stack();
        [SetUp]
        public static void SetUp()
        {
            callStack.Push(null);
            if (callStack.Count != 1)
                return;
            Console.WriteLine("Setting up {0}", typeof(AssemblyCleaner).Assembly.FullName);
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            MbUnit.Tests.Core.Graph.AssemblyCompiler.CreateAssemblies();
        }
        [TearDown]
        public static void TearDown()
        {
            callStack.Pop();
            if (callStack.Count != 0)
                return;
            Console.WriteLine("Cleaning up {0}", typeof(AssemblyCleaner).Assembly.FullName);
            Console.WriteLine(DateTime.Now.ToLongTimeString());
        }
    }
}
