using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using CommandLine;
using System.IO;
using log4net;
using System.Windows.Forms;

namespace VMTool.Slave
{
    public static class SlaveProgram
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(SlaveProgram));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static int Main(string[] args)
        {
            try
            {
                var options = new SlaveOptions();
                var parser = new CommandLineParser();
                var helpWriter = new StringWriter();
                if (!parser.ParseArguments(args, options, helpWriter))
                {
                    MessageBox.Show(helpWriter.ToString(), "VMTool Slave");
                    return 1;
                }

                var server = new SlaveServer(options);
                if (options.Debug)
                {
                    GlobalContext.Properties["LogToConsole"] = "true";
                    server.Run();
                }
                else
                {
                    ServiceBase.Run(new SlaveService(server));
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
