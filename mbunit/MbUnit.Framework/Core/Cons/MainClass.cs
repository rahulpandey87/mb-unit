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
using System.IO;
using System.Reflection;
using MbUnit.Core;
using MbUnit.Core.Invokers;
using MbUnit.Core.Cons.CommandLine;
using MbUnit.Core.Reports.Serialization;
using MbUnit.Core.Reports;
using MbUnit.Core.Monitoring;
using MbUnit.Core.Remoting;
using MbUnit.Core.Graph;
using MbUnit.Core.Filters;
using MbUnit.Core.Framework;

namespace MbUnit.Core.Cons
{
    public sealed class MainClass : IDisposable
    {
        private ConsoleRunPipeListener listener = new ConsoleRunPipeListener();
        private TextWriter consoleOut = Console.Out;
        private TimeMonitor timer = new TimeMonitor();
        private MainArguments arguments = new MainArguments();
        private AssemblyResolverManager resolver = null;

        public MainClass()
        {
            TypeHelper.InvokeFutureStaticMethod(typeof(System.Windows.Forms.Application), "EnableVisualStyles");
            this.resolver = new AssemblyResolverManager();
            this.resolver.AddMbUnitDirectories();
        }

        public void Dispose()
        {
            if (this.resolver != null)
            {
                this.resolver.Dispose();
                this.resolver = null;
            }
        }

        public TextWriter ConsoleOut
        {
            get
            {
                return this.consoleOut;
            }
            set
            {
                if (value == null)
					throw new ArgumentNullException("value");
                this.consoleOut = value;
            }
        }

        public MainArguments Arguments
        {
            get
            {
                return this.arguments;
            }
        }

        private void LaunchFixture(Object sender, RunInvokerTreeEventArgs e)
        {
            consoleOut.WriteLine("[fixture] {0}", e.Tree.Name);
        }

        private void LaunchPipe(Object sender, RunPipeStarterEventArgs e)
        {
            e.Starter.Listeners.Add(listener);
        }

        public void Parse(String[] args)
        {
            CommandLineUtility.ParseCommandLineArguments(args, arguments);
        }

        public int Main(string[] args)
        {
            // Show help, if necessary
            if (args.Length == 0 || (args.Length == 1 && (args[0] == "/?" || args[0].ToLower() == "/help")))
            {
                ShowHelp();
                return 0;
            }
            else
            {
                // Try parsing arguments
                try
                {
                    Parse(args);
                }
                catch (Exception ex)
                {
                    consoleOut.WriteLine("Error while parsing arguments:");
                    consoleOut.WriteLine(ex.Message);
                    return -2;
                }

                consoleOut.WriteLine("Parsed arguments:");
                consoleOut.WriteLine(arguments);

                // launch main loop
                return Main();
            }
        }

        public int Main()
        {
            consoleOut.WriteLine("Start time: {0}", DateTime.Now.ToShortTimeString());
            // add path
            foreach (string assemblyPath in this.Arguments.AssemblyPath)
                this.resolver.AddHintDirectory(assemblyPath);

            // store real console
            listener.Writer = Console.Out;
            timer.Start();
            try
            {
                ReportResult result = new ReportResult();
                IFixtureFilter filter = arguments.GetFilter();

                if (this.Arguments.Files.Length == 0)
                {
                    consoleOut.WriteLine("[warning] No test assemblies to execute");
                }
                else
                {
                    consoleOut.WriteLine("[info] Loading test assemblies");
                    using (
                        TestDomainDependencyGraph graph =
                        TestDomainDependencyGraph.BuildGraph(
                            this.Arguments.Files, 
                            this.Arguments.AssemblyPath, 
                            filter, this.Arguments.Verbose))
                    {
                        //define an assembly resolver routine in case the CLR cannot find our assemblies. 
                        AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveHandler);


                        graph.Log += new ErrorReporter(graph_Log);
                        consoleOut.WriteLine("[info] Starting execution");
                        ReportResult r = graph.RunTests();
                        graph.Log -= new ErrorReporter(graph_Log);
                        result.Merge(r);
                    }
                }

                this.GenerateReport(arguments, result);
                timer.Stop();
                consoleOut.WriteLine("[info] MbUnit execution finished in {0}s.", timer.Duration);
		return (0==result.Counter.FailureCount) ? 0 : -1;
            }
            catch (Exception ex)
            {
                consoleOut.WriteLine(ex.ToString());
		return -3;
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

        private void ShowHelp()
        {
            consoleOut.WriteLine("MbUnit {0} Console Application (running on .Net {1})",
                typeof(MainClass).Assembly.GetName().Version,
                typeof(Object).Assembly.GetName().Version
                );
            consoleOut.WriteLine("Author: Jonathan de Halleux");
            consoleOut.WriteLine("Get the latest at http://www.mbunit.com");
            consoleOut.WriteLine("------------------------------------------");

            consoleOut.Write(CommandLineUtility.CommandLineArgumentsUsage(typeof(MainArguments)));
            consoleOut.WriteLine("------------------------------------------");
        }

        private void GenerateReport(MainArguments parsedArgs, ReportResult result)
        {
            result.UpdateCounts();
            if (parsedArgs.ReportTypes != null && parsedArgs.ReportTypes.Length > 0)
            {
                parsedArgs.ReportFolder = ReportBase.GetAppDataPath(parsedArgs.ReportFolder);
                consoleOut.WriteLine("[info] Creating reports in {0}", Path.GetFullPath(parsedArgs.ReportFolder));
                foreach (ReportType rt in parsedArgs.ReportTypes)
                {
                    string outputPath = null;
                    switch (rt)
                    {
                        case ReportType.Xml:
                            outputPath = XmlReport.RenderToXml(result, parsedArgs.ReportFolder, parsedArgs.Transform, parsedArgs.ReportNameFormat);
                            consoleOut.WriteLine("[info] Created xml report {0}", outputPath);
                            break;
                        case ReportType.Html:
                            outputPath = HtmlReport.RenderToHtml(result, parsedArgs.ReportFolder, parsedArgs.Transform, parsedArgs.ReportNameFormat);
                            consoleOut.WriteLine("[info] Created Html report {0}", outputPath);
                            break;
                        case ReportType.Text:
                            outputPath = TextReport.RenderToText(result, parsedArgs.ReportFolder, parsedArgs.Transform, parsedArgs.ReportNameFormat);
                            consoleOut.WriteLine("[info] Created Text report {0}", outputPath);
                            break;
                        case ReportType.Dox:
                            outputPath = DoxReport.RenderToDox(result, parsedArgs.ReportFolder, parsedArgs.ReportNameFormat);
                            consoleOut.WriteLine("[info] Created Dox report {0}", outputPath);
                            break;
                    }
                    if (parsedArgs.ShowReports && File.Exists(outputPath))
                        System.Diagnostics.Process.Start(outputPath);
                }
            }
        }

        void graph_Log(string message)
        {
            Console.WriteLine("[info] {0}", message);
        }
    }

}
