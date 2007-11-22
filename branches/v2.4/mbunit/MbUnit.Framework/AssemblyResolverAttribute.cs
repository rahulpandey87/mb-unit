using System;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple =true)]
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
