using System;
using System.IO;
using System.Reflection;
using System.Collections;

using MbUnit.Framework;

using System.CodeDom.Compiler;
using MbUnit.Core.Exceptions;

namespace MbUnit.Framework.Tests.Asserts
{
    [TestFixture]
    public class CompilerAssert_Test
    {
        #region GetSampleData
        protected Stream GetCSharpSample()
        {
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                            "MbUnit.Framework.Tests.Asserts.CsSample.cs");
            Assert.IsNotNull(s);
            return s;
        }

        protected Stream GetCSharpBadSample()
        {
            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                            "MbUnit.Framework.Tests.Asserts.CsBadSample.txt");
            Assert.IsNotNull(s);
            return s;
        }
        #endregion

        #region CSharpCompiler, VBCompiler
        [Test]
        public void CSharpCompilerNotNull()
        {
            Assert.IsNotNull(CompilerAssert.CSharpCompiler);
        }
        [Test]
        public void VBCompilerNotNull()
        {
            Assert.IsNotNull(CompilerAssert.VBCompiler);
        }
        #endregion

        #region Null arguments
        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void CompilerNull()
        {
            throw new AssertionException("Should Ignore");
            Stream stream = GetCSharpSample();
            CompilerAssert.Compiles(null, stream);
        }

        [Test]
        [ExpectedException(typeof(AssertionException))]
        public void StreamNull()
        {
            Stream stream = null;
            CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, stream);
        }
        #endregion

        #region Compiles

        #region String

        [Test]
        public void CSharpStringCompiles()
        {
            Stream stream = GetCSharpSample();
            using (StreamReader sr = new StreamReader(stream))
            {
                CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, sr.ReadToEnd());
            }
        }


        [Test]
        public void CSharpStringReferenceCollectionCompiles()
        {
            Stream stream = GetCSharpSample();
            using (StreamReader sr = new StreamReader(stream))
            {
                System.Collections.Specialized.StringCollection references = new System.Collections.Specialized.StringCollection();
                references.Add("System.dll");
                CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, references, sr.ReadToEnd());
            }
        }

        [Test]
        public void CSharpStringCompilerParametersCompiles()
        {
            Stream stream = GetCSharpSample();
            using (StreamReader sr = new StreamReader(stream))
            {
                CompilerParameters options;
                string[] assemblyNames = new string[] { "System.dll" };

                options = new CompilerParameters(assemblyNames, "CSharpStringCompilerParametersCompiles");
                CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, options, sr.ReadToEnd());

                options = new CompilerParameters(assemblyNames, "CSharpStringCompilerParametersCompiles", true);
                CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, options, sr.ReadToEnd());

                options = new CompilerParameters(assemblyNames, "CSharpStringCompilerParametersCompiles", false);
                CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, options, sr.ReadToEnd());
            }
        }


        #endregion

        #region Stream

        [Test]
        public void CSharpStreamCompiles()
        {
            Stream stream = GetCSharpSample();
            CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, stream);
        }

        [Test]
        public void CSharpStreamCompilerParametersCompiles()
        {
            Stream stream;

            CompilerParameters options;
            string[] assemblyNames = new string[] { "System.dll" };

            stream = GetCSharpSample();
            options = new CompilerParameters(assemblyNames, "CSharpStreamCompilerParametersCompiles");
            CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, options, stream);

            stream = GetCSharpSample();
            options = new CompilerParameters(assemblyNames, "CSharpStreamCompilerParametersCompiles", true);
            CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, options, stream);

            stream = GetCSharpSample();
            options = new CompilerParameters(assemblyNames, "CSharpStreamCompilerParametersCompiles", false);
            CompilerAssert.Compiles(CompilerAssert.CSharpCompiler, options, stream);
        }

        #endregion

        #endregion

        #region NotCompiles

        #region String

        [Test]
        public void CSharpStringNotCompiles()
        {
            Stream stream = GetCSharpBadSample();
            using (StreamReader sr = new StreamReader(stream))
            {
                CompilerAssert.NotCompiles(CompilerAssert.CSharpCompiler, sr.ReadToEnd());
            }
        }

        [Test]
        public void CSharpStringCollectionNotCompiles()
        {
            Stream stream = GetCSharpBadSample();
            using (StreamReader sr = new StreamReader(stream))
            {
                System.Collections.Specialized.StringCollection references = new System.Collections.Specialized.StringCollection();
                references.Add("System.dll");
                CompilerAssert.NotCompiles(CompilerAssert.CSharpCompiler, references, sr.ReadToEnd());
            }
        }

        [Test]
        public void CSharpStringCompilerParametersNotCompiles()
        {
            Stream stream = GetCSharpBadSample();
            using (StreamReader sr = new StreamReader(stream))
            {
                CompilerParameters options;
                string[] assemblyNames = new string[] { "System.dll" };

                options = new CompilerParameters(assemblyNames, "CSharpStringCompilerParametersCompiles");
                CompilerAssert.NotCompiles(CompilerAssert.CSharpCompiler, options, sr.ReadToEnd());
            }
        }

        #endregion

        #region Stream

        [Test]
        public void CSharpStreamNotCompilesFail()
        {
            Stream stream = GetCSharpBadSample();
            CompilerAssert.NotCompiles(CompilerAssert.CSharpCompiler, stream);
        }

        [Test]
        public void CSharpStreamCompilerParametersNotCompiles()
        {
            Stream stream = GetCSharpBadSample();
            CompilerParameters options;
            string[] assemblyNames = new string[] { "System.dll" };

            options = new CompilerParameters(assemblyNames, "CSharpStreamCompilerParametersCompiles");
            CompilerAssert.NotCompiles(CompilerAssert.CSharpCompiler, options, stream);
        }

        #endregion

        #endregion
    }
}
