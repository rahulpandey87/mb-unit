using System;
using MbUnit.Core.Framework;
using MbUnit.Core.Remoting;
using MbUnit.Framework;
using MbUnit.Core.Reports.Serialization;

using System.Reflection;
using System.Collections;
using System.Runtime.Remoting.Services;
using System.Runtime.Remoting.Lifetime;
using System.IO;

using MbUnit.Framework.Tests.Samples;

namespace MbUnit.Framework.Tests.Core.Remoting
{
    [TestFixture]
    public class SourceTestDomainTest
    {
        [Test]
        public void CompileFixture()
        {
            string source = SampleHelper.TestFixture();
            using (SourceTestDomain domain = new SourceTestDomain())
            {
                //define an assembly resolver routine in case the CLR cannot find our assemblies. 
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveHandler);

                domain.Sources.Add(source);
                domain.SandBox = false;
                domain.Load();
                domain.TestEngine.RunPipes();
            }
        }

        [Test]
        public void CompileAndCountTests()
        {
            string source = SampleHelper.TestFixture();
            using (SourceTestDomain domain = new SourceTestDomain())
            {
                //define an assembly resolver routine in case the CLR cannot find our assemblies. 
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveHandler);

                domain.Sources.Add(source);
                domain.SandBox = false;
                domain.Load();
                domain.TestEngine.RunPipes();

                ReportResult result = domain.TestEngine.Report.Result;
                Assert.AreEqual(4,result.Counter.RunCount);
                Assert.AreEqual(2, result.Counter.SuccessCount);
                Assert.AreEqual(1, result.Counter.IgnoreCount);
                Assert.AreEqual(1, result.Counter.FailureCount);
            }
        }

        /// <summary> 
        /// This method is used to provide assembly location resolver. It is called on event as needed by the CLR. 
        /// Refer to document related to AppDomain.CurrentDomain.AssemblyResolve 
        /// </summary> 
        private Assembly AssemblyResolveHandler(object sender, ResolveEventArgs e)
        {
            try
            {
                string[] assemblyDetail = e.Name.Split(',');
                string assemblyBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Assembly assembly = Assembly.LoadFrom(assemblyBasePath + @"\" + assemblyDetail[0] + ".dll");
                return assembly;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed resolving assembly", ex);
            }
        } 
    }
}
