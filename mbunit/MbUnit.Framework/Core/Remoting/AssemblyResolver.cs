// MbUnit Test Framework
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		MbUnit HomePage: http://www.mbunit.org
//		Author: Jonathan de Halleux

using System;
using System.Collections;
using System.Reflection;
using System.IO;
using MbUnit.Core.Config;
using MbUnit.Framework;

namespace MbUnit.Core.Remoting
{
    [Serializable]
    public class AssemblyResolverManager : LongLivingMarshalByRefObject, IDisposable
    {
        private ArrayList hintDirectories = null;
        private ArrayList assemblyResolvers = null;

        public AssemblyResolverManager()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            this.hintDirectories = new ArrayList();
            this.assemblyResolvers = new ArrayList();
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            this.hintDirectories = null;
            this.assemblyResolvers = null;
        }

        public void AddHintDirectory(string hintDirectory)
        {
            if (hintDirectory == null)
                throw new ArgumentNullException("hintDirectory");

            string directory = Path.GetDirectoryName(hintDirectory);
            if (directory.Length == 0)
                directory = ".";
            directory = Path.GetFullPath(directory);
            if (this.hintDirectories.Contains(directory))
                return;
            this.hintDirectories.Add(directory);
        }

        public void AddAssemblyResolvers(Assembly assembly)
        {
            foreach (AssemblyResolverAttribute resolverAttribute in
                assembly.GetCustomAttributes(typeof(AssemblyResolverAttribute), false))
            {
                IAssemblyResolver resolver =
                    Activator.CreateInstance(resolverAttribute.AssemblyResolverType) as IAssemblyResolver;
                if (resolver == null)
                    throw new InvalidOperationException("Failed creating AssemblyResolverManager");
                this.assemblyResolvers.Add(resolver);
            }
        }

        public bool AssemblyInDomain(AssemblyName name)
        {
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.GetName().Name == name.Name)
                    return true;
            }
            return false;
        }

        public void AddMbUnitDirectories()
        {
            this.AddHintDirectory(typeof(AssemblyResolverManager).Assembly.Location);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.EndsWith("XmlSerializers"))
                return null;

            Assembly assembly=null;

            // try custom resolver
            foreach (IAssemblyResolver resolver in this.assemblyResolvers)
            {
                assembly = resolver.Resolve(args.Name);
                if (assembly != null)
                    return assembly;
            }

            // try builtin
            assembly = RecursiveAssemblyResolve(args);
            if (assembly != null)
                return assembly;

            return null;
        }

        private Assembly RecursiveAssemblyResolve(ResolveEventArgs args)
        {
            String[] saNames = args.Name.Split(',');
            String sFile = saNames[0];

            Assembly a = null;
            // try with current directory
            a = resolveAssembly(Directory.GetCurrentDirectory(), sFile);
            if (a != null)
                return a;

            // try with hint directories
            foreach (String directory in this.hintDirectories)
            {
                a = resolveAssembly(directory.TrimEnd('\\'), sFile);
                if (a != null)
                    return a;
            }
            return null;
        }

        private Assembly resolveAssembly(string directory, string file)
        {
            string assemblyName = Path.GetFullPath(Path.Combine(directory, file));
            if (File.Exists(assemblyName))
                return Assembly.LoadFrom(assemblyName);
            if (File.Exists(assemblyName + ".dll"))
                return Assembly.LoadFrom(assemblyName + ".dll");
            if (File.Exists(assemblyName + ".exe"))
                return Assembly.LoadFrom(assemblyName + ".exe");
            return null;
        }

    }
}
