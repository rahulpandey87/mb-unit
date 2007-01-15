using System;
using System.Collections;
using System.Reflection;

using MbUnit.Core.Framework;
using MbUnit.Framework;

[assembly: MbUnit.Framework.AssemblyResolver(
    typeof(MbUnit.Framework.Tests.Core.Remoting.CustomAssemblyResolver)
    )]

namespace MbUnit.Framework.Tests.Core.Remoting
{
    [TestFixture]
    public class CustomAssemblyResolver : IAssemblyResolver
    {
        private static bool constructed;
        private static ArrayList resolvedAssemblies = new ArrayList();

        public CustomAssemblyResolver()
        {
            constructed = true;
        }

        [Test]
        public void CheckItHasBeenCalled()
        {
            foreach (string assemblyName in resolvedAssemblies)
                Console.WriteLine(assemblyName);
            Assert.IsTrue(constructed);
        }

        public Assembly Resolve(string assemblyName)
        {
            resolvedAssemblies.Add(assemblyName);
            return null;
        }
    }
}
