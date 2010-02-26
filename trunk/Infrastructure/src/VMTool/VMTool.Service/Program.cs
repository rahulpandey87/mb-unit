using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using CommandLine;
using System.IO;
using log4net;

namespace VMTool.Service
{
    public static class Program
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            try
            {
                var options = new Options();
                var parser = new CommandLineParser();
                var helpWriter = new StringWriter();
                if (!parser.ParseArguments(args, options, helpWriter))
                {
                    MessageBox.Show(helpWriter.ToString(), "VMTool Service");
                    return 1;
                }

                var service = new Service(options);
                if (options.Debug)
                {
                    GlobalContext.Properties["LogToConsole"] = "true";
                    service.Run();
                }
                else
                {
                    ServiceBase.Run(service);
                }
                return 0;
            }
            catch (Exception ex)
            {
                log.Fatal("Fatal exception.", ex);
                return 1;
            }
        }
    }
}
