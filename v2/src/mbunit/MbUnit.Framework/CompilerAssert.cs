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
//		MbUnit HomePage: http://www.mbunit.com
//		Author: Jonathan de Halleux

using MbUnit.Framework;
using System;
using System.Collections;
using System.CodeDom.Compiler;
using System.IO;
using System.Collections.Specialized;
using MbUnit.Core.Exceptions;
using Microsoft.CSharp;
using Microsoft.VisualBasic;

namespace MbUnit.Framework {
    /// <summary>
    /// Assertion helper for compilation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class contains static helper methods to verify that snippets are compilable.
    /// </para>
    /// </remarks>
    public sealed class CompilerAssert {
        #region Private constructor and fields
        private static ICodeCompiler csharp = new CSharpCodeProvider().CreateCompiler();
        private static ICodeCompiler vb = new VBCodeProvider().CreateCompiler();
        private CompilerAssert() { }
        #endregion

        /// <summary>
        /// Gets the C# compiler from <see cref="CSharpCodeProvider"/>.
        /// </summary>
        /// <value>
        /// C# compiler.
        /// </value>
        public static ICodeCompiler CSharpCompiler {
            get {
                return csharp;
            }
        }

        /// <summary>
        /// Gets the VB.NET compiler from <see cref="VBCodeProvider"/>.
        /// </summary>
        /// <value>
        /// VB.NET compiler.
        /// </value>
        public static ICodeCompiler VBCompiler {
            get {
                return vb;
            }
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> compiles using the provided compiler.
        /// </summary>
        /// <param name="compiler">Compiler instance</param>
        /// <param name="source">Source code to compile</param>
        public static void Compiles(ICodeCompiler compiler, string source) {
            Assert.IsNotNull(compiler);
            Assert.IsNotNull(source);
            CompilerParameters ps = new CompilerParameters();
            Compiles(compiler, ps, source);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> compiles using the provided compiler.
        /// </summary>
        /// <param name="compiler">Compiler instance</param>
        /// <param name="source">Source code to compile</param>
        public static void Compiles(ICodeCompiler compiler, Stream source) {
            Assert.IsNotNull(compiler);
            Assert.IsNotNull(source);
            CompilerParameters ps = new CompilerParameters();
            Compiles(compiler, ps, source);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> compiles using the provided compiler.
        /// </summary>
        /// <param name="compiler">Compiler instance</param>
        /// <param name="references">Referenced assemblies</param>
        /// <param name="source">Source code to compile</param>
        public static void Compiles(ICodeCompiler compiler, StringCollection references, string source) {
            Assert.IsNotNull(compiler);
            Assert.IsNotNull(references);
            Assert.IsNotNull(source);
            CompilerParameters ps = new CompilerParameters();
            foreach (string ra in references)
                ps.ReferencedAssemblies.Add(ra);

            Compiles(compiler, ps, source);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> compiles using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="options">Compilation options</param>
        /// <param name="source">source to compile</param>
        public static void Compiles(ICodeCompiler compiler, CompilerParameters options, string source) {
            Assert.IsNotNull(compiler);
            Assert.IsNotNull(options);
            Assert.IsNotNull(source);
            Compiles(compiler, options, source, false);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> compiles using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="options">Compilation options</param>
        /// <param name="source">Source to compile</param>
        /// <param name="throwOnWarning">
        /// true if assertion should throw if any warning.
        /// </param>
        public static void Compiles(ICodeCompiler compiler, CompilerParameters options, string source, bool throwOnWarning) {
            Assert.IsNotNull(compiler);
            Assert.IsNotNull(options);
            CompilerResults results = compiler.CompileAssemblyFromSource(options, source);
            if (results.Errors.HasErrors)
                throw new CompilationException(compiler, options, results, source);
            if (throwOnWarning && results.Errors.HasWarnings)
                throw new CompilationException(compiler, options, results, source);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> compiles using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="options">Compilation options</param>
        /// <param name="source">Stream containing the source to compile</param>
        public static void Compiles(ICodeCompiler compiler, CompilerParameters options, Stream source) {
            Compiles(compiler, options, source, false);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> compiles using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="options">Compilation options</param>
        /// <param name="source">Stream containing the source to compile</param>
        /// <param name="throwOnWarning">
        /// true if assertion should throw if any warning.
        /// </param>
        public static void Compiles(ICodeCompiler compiler, CompilerParameters options, Stream source, bool throwOnWarning) {
            using (StreamReader sr = new StreamReader(source)) {
                Compiles(compiler, options, sr.ReadToEnd(), throwOnWarning);
            }
        }


        /// <summary>
        /// Verifies that <paramref name="source"/> does not compile using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="source">Source to compile</param>
        public static void NotCompiles(
            ICodeCompiler compiler,
            string source) {
            CompilerParameters options = new CompilerParameters();
            NotCompiles(compiler, options, source);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> does not compile using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="source">Source to compile</param>
        public static void NotCompiles(
            ICodeCompiler compiler,
            Stream source) {
            CompilerParameters options = new CompilerParameters();
            NotCompiles(compiler, options, source);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> does not compile using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="referencedAssemblies">Collection of referenced assemblies</param>
        /// <param name="source">Source to compile</param>
        public static void NotCompiles(
            ICodeCompiler compiler,
            StringCollection referencedAssemblies,
            string source) {
            CompilerParameters options = new CompilerParameters();
            CompilerParameters ps = new CompilerParameters();
            foreach (string ra in referencedAssemblies)
                ps.ReferencedAssemblies.Add(ra);
            NotCompiles(compiler, options, source);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> does not compile using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="options">Compilation options</param>
        /// <param name="source">Source to compile</param>
        public static void NotCompiles(
            ICodeCompiler compiler,
            CompilerParameters options,
            string source) {
            Assert.IncrementAssertCount();
            if (compiler == null)
                throw new ArgumentNullException("compiler");
            if (options == null)
                throw new ArgumentNullException("options");
            CompilerResults results = compiler.CompileAssemblyFromSource(options, source);
            if (!results.Errors.HasErrors)
                throw new CompilationException(compiler, options, results, source);
        }

        /// <summary>
        /// Verifies that <paramref name="source"/> does not compile using the provided compiler.
        /// </summary>
        /// <param name="compiler">
        /// <see cref="ICodeCompiler"/> instance.</param>
        /// <param name="options">Compilation options</param>
        /// <param name="source">Source to compile</param>
        public static void NotCompiles(
            ICodeCompiler compiler,
            CompilerParameters options,
            Stream source) {
            using (StreamReader sr = new StreamReader(source)) {
                NotCompiles(compiler, options, sr.ReadToEnd());
            }
        }

        /// <summary>
        /// Writes the errors given in the compiler <paramref name="results"/> to the named <paramref name="writer"/>
        /// </summary>
        /// <param name="results">The <see cref="CompilerResults" /> generated by the compilation</param>
        /// <param name="writer">The <see cref="TextWriter"/> to write the errors out to</param>
        public static void DisplayErrors(CompilerResults results, TextWriter writer) {
            foreach (CompilerError error in results.Errors) {
                writer.Write(error);
            }
        }
    }
}
