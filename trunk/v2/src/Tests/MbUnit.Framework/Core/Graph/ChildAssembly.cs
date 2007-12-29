using System;
using MbUnit.Framework;

[assembly: MbUnit.Framework.AssemblyDependsOn("ParentAssembly")]
[assembly: MbUnit.Framework.AssemblyDependsOn("SickParentAssembly")]

namespace MbUnit.Framework.Tests.Core.Graph
{
    [TestFixture]
    public class ChildAssembly
    {
        [Test]
        public void Test()
        { }
    }
}
