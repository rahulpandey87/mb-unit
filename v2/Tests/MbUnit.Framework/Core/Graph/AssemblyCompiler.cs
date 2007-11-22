using System;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;

using MbUnit.Framework;
namespace MbUnit.Framework.Tests.Core.Graph
{
    public sealed class AssemblyCompiler
    {
        private AssemblyCompiler()
        {}

        public static void CreateAssemblies()
        {
            foreach (string assemblyPath in AssemblyPaths)
                CreateAssembly(assemblyPath);
        }

        public static string[] AssemblyPaths
        {
            get
            { 
                string path = Path.GetDirectoryName(typeof(AssemblyCompiler).Assembly.Location);
                return new string[] 
                {
                    Path.Combine(path, "ParentAssembly.dll"), 
                    Path.Combine(path, "ChildAssembly.dll"),
                    Path.Combine(path, "SickParentAssembly.dll")
                };
            }
        }

        private static string LoadSource(string name)
        {
            Stream stream = typeof(AssemblyCompiler).Assembly.GetManifestResourceStream("MbUnit.Framework.Tests.Core.Graph." + name + ".cs");
            Assert.IsNotNull(stream);
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static string CreateAssembly(string assemblyPath)
        {
            ICodeCompiler compiler = new Microsoft.CSharp.CSharpCodeProvider().CreateCompiler();

            string source = LoadSource(Path.GetFileNameWithoutExtension(assemblyPath));

            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = false;
            options.OutputAssembly = assemblyPath;
            options.ReferencedAssemblies.Add(Path.Combine(Path.GetDirectoryName(assemblyPath) ,"MbUnit.Framework.dll"));

            CompilerResults results = compiler.CompileAssemblyFromSource(options, source);
            if (results.Errors.HasErrors)
                CompilerAssert.DisplayErrors(results, Console.Out);
            Assert.IsFalse(results.Errors.HasErrors);
            return results.PathToAssembly;
        }
    }
}
