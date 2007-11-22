using System;
using System.Reflection;
using System.Collections;
using TestFu.Operations;
using MbUnit.Core;

namespace MbUnit.Framework
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true, Inherited = true)]
    public sealed class UsingImplementationsAttribute : UsingBaseAttribute
    {
        private Type typeFromAssembly;

        public UsingImplementationsAttribute(Type typeFromAssembly)
        {
            if (typeFromAssembly == null)
                throw new ArgumentNullException("typeFromAssembly");
            this.typeFromAssembly = typeFromAssembly;
        }

        public override void GetDomains(IDomainCollection domains, ParameterInfo parameter, object fixture)
        {
            ArrayList types = new ArrayList();
            foreach (Type type in typeFromAssembly.Assembly.GetExportedTypes())
            {
                if (type.IsAbstract || type.IsInterface || !type.IsClass)
                    continue;

                if (!parameter.ParameterType.IsAssignableFrom(type))
                    continue;

                // create instance
                Object instance = TypeHelper.CreateInstance(type);
                types.Add(instance);
            }

            CollectionDomain domain = new CollectionDomain(types);
            domain.Name = typeFromAssembly.Assembly.GetName().Name;
            domains.Add(domain);
        }
    }
}
