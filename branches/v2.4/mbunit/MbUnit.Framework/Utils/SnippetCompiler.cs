using System;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;

namespace MbUnit.Framework.Utils
{
    public class SnippetCompiler
    {
        private CodeDomProvider provider;
        private ICodeCompiler compiler;
        private StringWriter _out=new StringWriter();
        private CompilerParameters parameters = new CompilerParameters();
        private CompilerResults results = null;

        public SnippetCompiler()
        {
            this.provider = new CSharpCodeProvider();
            this.compiler = this.provider.CreateCompiler();
            this.parameters = new CompilerParameters();
            this.parameters.GenerateInMemory = false;
            this.parameters.IncludeDebugInformation = true;
            this.parameters.ReferencedAssemblies.Add("System.dll");
            this.parameters.OutputAssembly =
                String.Format("MbUnit.Tests.Snippet.{0}.dll",
                    DateTime.Now.Ticks
                    );
        }

        public ICodeCompiler Compiler
        {
            get
            {
                return this.compiler;
            }
        }

        public TextWriter Out
        {
            get
            {
                return this._out;
            }
        }

        public CompilerParameters Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public CompilerResults Results
        {
            get
            {
                return this.results;
            }
        }

        public void Clear()
        {
            this._out = new StringWriter();
        }

        public void LoadFromResource(string name, Assembly a)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (name.Length == 0)
                throw new ArgumentException("Lengh cannot be 0", "name");
            if (a == null)
                throw new ArgumentNullException("a");

            Stream stream = a.GetManifestResourceStream(name);
            if (stream == null)
                throw new ArgumentException("Could not find resource", name);
            using (StreamReader sr = new StreamReader(stream))
            {
                this.Out.Write(sr.ReadToEnd());
            }
        }

        public void Compile()
        {
            this.results = this.compiler.CompileAssemblyFromSource(this.Parameters, this.Out.ToString());
        }

        public void ShowErrors(TextWriter writer)
        {
            if (this.Results == null)
                throw new InvalidOperationException("Must compile something first");

            if (this.Results.Errors.Count > 0)
            {
                foreach (CompilerError ce in this.Results.Errors)
                {
                    writer.WriteLine("  {0}", ce.ToString());
                    writer.WriteLine();
                }
            }
            else
                writer.WriteLine("Compilation successful");
        }
    }
}
