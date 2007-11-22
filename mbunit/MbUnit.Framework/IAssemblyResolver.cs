using System.Reflection;

namespace MbUnit.Framework
{
    public interface IAssemblyResolver
    {
        Assembly Resolve(string assemblyName);
    }
}
