using System.Reflection;

namespace MbUnit.Framework {
    /// <summary>
    /// Defines standard interface for Custom Assembly Resolver classes
    /// </summary>
    /// <remarks>
    /// <para>Resolving assemblies can be a tricky business. MbUnit has a complex support for this but it is not always sufficient. 
    /// In order to give full flexibility, one can define a assembly resolver type that will be used by MbUnit to resolve assemblies 
    /// (as well as the default behavior).</para>
    /// <para>There are 2 stops to implement this feature:</para>
    /// <list type="number">
    /// <item>Implement IAssemblyResolver (MbUnit.Core.Framework), don't forget to have a public constructor without arguments</item>
    /// <item>Add a assembly level custom attribute AssemblyResolverAttribute with the type of the IAssemblyResolver implementation</item>
    /// </list>
    /// </remarks>
    /// <example>
    /// <para>The following demonstrates the use of IAssemblyResolver</para>
    /// <code>
    ///public class MyAssemblyResolver : IAssemblyResolver
    /// {
    ///     public Assembly Resolve(string assemblyName)
    ///     { ... }
    /// }
    ///
    /// [assembly: AssemblyResolver(typeof(MyAssemblyResolver))]
    /// </code>
    /// </example>
    public interface IAssemblyResolver {
        Assembly Resolve(string assemblyName);
    }
}
