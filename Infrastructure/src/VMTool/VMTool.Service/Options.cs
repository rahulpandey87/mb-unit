using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace VMTool.Service
{
    public class Options
    {
        [Option("d", "debug", HelpText="Run in debug mode as a console application.")]
        public bool Debug;

        [Option("p", "port", HelpText = "The port number.  Default is 3831.")]
        public int Port = 3831;

        [HelpOption(null, "help", HelpText = "Display help text.")]
        public string GetUsage()
        {
            var helpText = new HelpText("VMTool Service");
            helpText.AddOptions(this);
            return helpText.ToString();
        }
    }
}
