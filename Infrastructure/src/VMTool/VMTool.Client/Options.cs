using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace VMTool.Client
{
    public class Options
    {
        [Option("h", "host", HelpText = "Server host name.", Required = true)]
        public string Host;

        [Option("p", "port", HelpText = "Server port number.  Default is 3831.")]
        public int Port = 3831;

        [Option(null, "start", HelpText = "Starts a VM.", MutuallyExclusiveSet = "Commands")]
        public bool Start;

        [Option(null, "poweroff", HelpText = "Powers off a VM.", MutuallyExclusiveSet = "Commands")]
        public bool PowerOff;

        [Option(null, "shutdown", HelpText = "Shuts down a VM.", MutuallyExclusiveSet = "Commands")]
        public bool Shutdown;

        [Option(null, "pause", HelpText = "Pauses a VM.", MutuallyExclusiveSet = "Commands")]
        public bool Pause;

        [Option(null, "resume", HelpText = "Resumes a VM.", MutuallyExclusiveSet = "Commands")]
        public bool Resume;

        [Option(null, "takesnapshot", HelpText = "Takes a snapshot.", MutuallyExclusiveSet = "Commands")]
        public bool TakeSnapshot;

        [Option(null, "getip", HelpText = "Gets the IP address of the VM's primary network interface.", MutuallyExclusiveSet = "Commands")]
        public bool GetIP;

        [Option("v", "vm", HelpText = "Specifies the VM name or UUID.")]
        public string VM;

        [Option("s", "snapshot", HelpText = "Specifies the snapshot name or UUID.")]
        public string Snapshot;

        [HelpOption(null, "help", HelpText = "Display help text.")]
        public string GetUsage()
        {
            var helpText = new HelpText("VMTool Client");
            helpText.AddOptions(this);
            return helpText.ToString();
        }
    }
}
