using System;
using System.IO;
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;
using MbUnit.Core;
using MbUnit.Core.Remoting;
using MbUnit.Core.Reports;
using MbUnit.Core.Framework;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Framework;
using MbUnit.Framework.Utils;

namespace MbUnit.Tests.Samples
{
    public class SampleCompiler
    {
        private string sampleOutputFolder = "Samples";
        private SnippetCompiler compiler = new SnippetCompiler();
        private ReportCounter counter=null;
        private ReportResult result=null;

        public SampleCompiler(string resourceName)
        {
            string rname = "MbUnit.Tests.Samples."+resourceName;
            Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(rname);
            if (resource==null)
                throw new ArgumentException("resource " + rname + " not found");

            using (StreamReader sr = new StreamReader(resource))
            {
                this.Compiler.Out.Write(sr.ReadToEnd());
            }

            //create output dir
            Directory.CreateDirectory(this.sampleOutputFolder);

            this.Compiler.Parameters.OutputAssembly =
                Path.GetFullPath(
                Path.Combine(this.sampleOutputFolder, String.Format("{0}.Assembly.dll", resourceName))
                );
        }

        public SnippetCompiler Compiler
        {
            get
            {
                return this.compiler;
            }
        }

        public ReportResult Result
        {
            get
            {
                return this.result;
            }
        }

        public void AddMbUnitReferences()
        {
            this.Compiler.Parameters.ReferencedAssemblies.Add(typeof(MbUnit.Core.RunPipe).Assembly.Location);
            this.Compiler.Parameters.ReferencedAssemblies.Add(typeof(MbUnit.Framework.TestFixtureAttribute).Assembly.Location);
        }

        public void Compiles()
        {
            this.compiler.Parameters.GenerateInMemory = true;
            this.compiler.Compile();
            this.compiler.ShowErrors(Console.Out);
            Assert.IsFalse(this.compiler.Results.Errors.HasErrors, "Compilation has errors");
            Type[] exportedTypes = this.Compiler.Results.CompiledAssembly.GetExportedTypes();
            foreach (Type t in exportedTypes)
                Console.WriteLine("Type: {0}", t.FullName);
        }

        public void CreateAssembly()
        {
            this.compiler.Parameters.GenerateInMemory = false;
            this.compiler.Compile();
            this.compiler.ShowErrors(Console.Out);
            Assert.IsFalse(this.compiler.Results.Errors.HasErrors);
        }

        public void LoadAndRunFixtures()
        {
            this.CreateAssembly();

            // load assembly
            using(TestDomain domain = new TestDomain(this.compiler.Parameters.OutputAssembly))
            {
                domain.ShadowCopyFiles = false;
                domain.InitializeEngine();
                foreach (string dir in this.Compiler.Parameters.ReferencedAssemblies)
                    domain.TestEngine.AddHintDirectory(dir);
                domain.PopulateEngine();
                Console.WriteLine("Domain loaded");
                Console.WriteLine("Tree populated, {0} tests", domain.TestEngine.GetTestCount());
                // Assert.AreEqual(1, domain.TestTree.GetTestCount());
                // domain.TestTree.Success+=new MbUnit.Core.RunSuccessEventHandler(TestTree_Success);
                // running tests
                domain.TestEngine.RunPipes();

                // display report
                TextReport report = new TextReport();
                result = domain.TestEngine.Report.Result;
                report.Render(result,Console.Out);
                counter = domain.TestEngine.GetTestCount();
            }
        }
    }
}
