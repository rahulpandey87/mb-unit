using System;

namespace MbUnit.Framework
{
    /// <summary>
    /// Registers a custom assembly resolver.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple =true)]
    public sealed class AssemblyResolverAttribute : Attribute
    {
        private Type assemblyResolverType;

        /// <summary>
        /// Registers a custom assembly resolver.
        /// </summary>
        /// <param name="assemblyResolverType">The assembly resolver type, must
        /// implement <see cref="IAssemblyResolver" /></param>
        public AssemblyResolverAttribute(Type assemblyResolverType)
        {
            if (assemblyResolverType == null)
                throw new ArgumentNullException("assemblyResolverType");
            this.assemblyResolverType = assemblyResolverType;
        }

        /// <summary>
        /// Gets the assembly resolver type.
        /// </summary>
        public Type AssemblyResolverType
        {
            get { return this.assemblyResolverType; }
        }
    }
}
