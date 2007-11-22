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
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using MbUnit.Core.Exceptions;
using MbUnit.Core.Reports.Serialization;

namespace MbUnit.Core.Remoting
{
    [Serializable]
    public class SourceRemoteTestEngine : RemoteTestEngine
    {
        [NonSerialized]
        private ICodeCompiler compiler = new CSharpCodeProvider().CreateCompiler();
        [NonSerialized]
        private CompilerParameters options = new CompilerParameters();
        private CompilerResults compilerResults = null;

        public SourceRemoteTestEngine()
        {
            this.options.GenerateExecutable = false;
            this.options.GenerateInMemory = true;
            this.options.IncludeDebugInformation = true;
            this.options.TreatWarningsAsErrors = false;
        }

        private StringCollection sources = new StringCollection();
        private StringCollection referencedAssemblies = new StringCollection();

        public void AddSource(string source)
        {
            this.sources.Add(source);
        }

        public void AddReferencedAssembly(string referencedAssembly)
        {
            this.referencedAssemblies.Add(referencedAssembly);
        }

        public virtual void CreateAssembly()
        {
            try
            {
                this.options.ReferencedAssemblies.Clear();
                this.options.ReferencedAssemblies.Add("MbUnit.Framework.dll");
                this.options.ReferencedAssemblies.Add("TestFu.dll");

                foreach (string referencedAssembly in this.referencedAssemblies)
                    this.options.ReferencedAssemblies.Add(referencedAssembly);

                string[] srs = new string[this.sources.Count];
                this.sources.CopyTo(srs, 0);

                this.compilerResults = this.compiler.CompileAssemblyFromSourceBatch(options, srs);
                if (this.compilerResults.Errors.HasErrors)
                {
                    Console.WriteLine("Compilation has errors");
                    foreach (CompilerError error in this.compilerResults.Errors)
                    {
                        Console.WriteLine("Error: {0}", error);
                    }
                    throw new CompilationException(
                        this.compiler,
                        this.options,
                         this.compilerResults,
                        srs
                       );
                }
            }
            catch (Exception ex)
            {
                ReportException rex = ReportException.FromException(ex);
                throw new ReportExceptionException("Compilation failed", rex);
            }
            this.SetTestAssembly(this.compilerResults.CompiledAssembly);
        }

    }
}
