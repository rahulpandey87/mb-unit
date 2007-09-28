using System;

namespace MbUnit.Framework
{
   /// <summary>
   /// <para>Resolving assemblies can be a tricky business. In order to give full flexibility, one can define a assembly resolver type that will be used by MbUnit to resolve assemblies (as well as the default behavior).</para>
   /// <para>There are 2 stops to implement this feature:</para>
   /// <list type="number">
   /// <item>Implement <see cref="IAssemblyResolver" />, don't forget to have a public constructor without arguments</item>
   /// <item>Add a assembly level custom attribute <see cref="AssemblyResolverAttribute" /> with the type of the <see cref="IAssemblyResolver" /> implementation </item>
   /// </list>
   /// </summary>
   /// <example>
   /// <code>
   /// public class MyAssemblyResolver : IAssemblyResolver
   /// {
   ///   public Assembly Resolve(string assemblyName)
   ///   { ... }
   /// }
   /// 
   /// [assembly: AssemblyResolver(typeof(MyAssemblyResolver))]
   /// </code>
   /// </example>
   [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
   public sealed class AssemblyResolverAttribute : Attribute
   {
      private Type assemblyResolverType;
      public AssemblyResolverAttribute(Type assemblyResolverType)
      {
         if (assemblyResolverType == null)
            throw new ArgumentNullException("assemblyResolverType");
         this.assemblyResolverType = assemblyResolverType;
      }

      public Type AssemblyResolverType
      {
         get { return this.assemblyResolverType; }
      }
   }
}
