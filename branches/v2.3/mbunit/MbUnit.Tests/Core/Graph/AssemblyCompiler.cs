using System;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;

using MbUnit.Framework;
namespace MbUnit.Tests.Core.Graph
{
    public sealed class AssemblyCompiler
    {
        private AssemblyCompiler()
        {}

        public static void CreateAssemblies()
        {
            CreateAssembly("ParentAssembly");
            CreateAssembly("ChildAssembly");
            CreateAssembly("SickParentAssembly");
        }

        private static string LoadSource(string name)
        {
            Stream stream = typeof(AssemblyCompiler).Assembly.GetManifestResourceStream("MbUnit.Tests.Core.Graph." + name + ".cs");
            Assert.IsNotNull(stream);
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static Assembly CreateAssembly(string name)
        {
            ICodeCompiler compiler = new Microsoft.CSharp.CSharpCodeProvider().CreateCompiler();

            string source = LoadSource(name);

            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = false;
            options.OutputAssembly = name+".dll";
            options.ReferencedAssemblies.Add("MbUnit.Framework.dll");

            CompilerResults results = compiler.CompileAssemblyFromSource(options, source);
            if (results.Errors.HasErrors)
                CompilerAssert.DisplayErrors(results, Console.Out);
            Assert.IsFalse(results.Errors.HasErrors);
            return results.CompiledAssembly;
        }
    }
}
