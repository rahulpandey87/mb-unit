using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace VMTool.Slave
{
    public class SlaveOptions
    {
        [Option("d", "debug", HelpText="Run in debug mode as a console application.")]
        public bool Debug;

        [Option("p", "port", HelpText = "The port number.  Default is " + Constants.DefaultSlavePortString + ".")]
        public int Port = Constants.DefaultSlavePort;

        [HelpOption(null, "help", HelpText = "Display help text.")]
        public string GetUsage()
        {
            var helpText = new HelpText("VMTool Slave");
            helpText.AddOptions(this);
            return helpText.ToString();
        }
    }
}
