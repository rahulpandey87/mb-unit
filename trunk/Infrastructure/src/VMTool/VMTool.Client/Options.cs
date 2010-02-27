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
        [Option(null, "configuration", HelpText = "The configuration file path.")]
        public string Configuration;

        [Option(null, "profile", HelpText = "The virtual machine profile to manipulate.")]
        public string Profile;

        [Option(null, "host", HelpText = "Server host name.")]
        public string Host;

        [Option(null, "port", HelpText = "Server port number.  Default is " + Constants.DefaultPortString + ".")]
        public int Port = Constants.DefaultPort;

        [Option(null, "vm", HelpText = "Specifies the VM name or UUID.")]
        public string VM;

        [Option(null, "snapshot", HelpText = "Specifies the snapshot name or UUID.")]
        public string Snapshot;

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

        [Option(null, "getstatus", HelpText = "Gets the status of the VM.", MutuallyExclusiveSet = "Commands")]
        public bool GetStatus;

        [Option(null, "getip", HelpText = "Gets the IP address of the VM's primary network interface.", MutuallyExclusiveSet = "Commands")]
        public bool GetIP;

        [HelpOption(null, "help", HelpText = "Display help text.")]
        public string GetUsage()
        {
            var helpText = new HelpText("VMTool Client");
            helpText.AddOptions(this);
            helpText.AddPostOptionsLine("To specify a virtual machine use the following options:");
            helpText.AddPostOptionsLine("  * --configuration <file> --profile <id>");
            helpText.AddPostOptionsLine("  * --host <hostname> --port <#> --vm <name or uuid> --snapshot <name or uuid>");
            helpText.AddPostOptionsLine("");
            helpText.AddPostOptionsLine("Note: Some commands ignore the snapshot name.");
            return helpText.ToString();
        }
    }
}
