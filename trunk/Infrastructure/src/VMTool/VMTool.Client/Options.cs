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
        [Option("q", "quiet", HelpText = "Disables command logging.")]
        public bool Quiet;

        [Option("c", "configuration", HelpText = "The configuration file path.")]
        public string Configuration;

        [Option("p", "profile", HelpText = "The virtual machine profile to manipulate.")]
        public string Profile;

        [Option("h", "host", HelpText = "Server host name.")]
        public string Host;

        [Option("P", "port", HelpText = "Server port number.  Default is " + Constants.DefaultPortString + ".")]
        public int Port = Constants.DefaultPort;

        [Option("v", "vm", HelpText = "Specifies the VM name or UUID.")]
        public string VM;

        [Option("s", "snapshot", HelpText = "Specifies the snapshot name or UUID.")]
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

        [Option(null, "savestate", HelpText = "Saves state and stops a VM.", MutuallyExclusiveSet = "Commands")]
        public bool SaveState;

        [Option(null, "takesnapshot", HelpText = "Takes a snapshot with the specified name.", MutuallyExclusiveSet = "Commands")]
        public bool TakeSnapshot;

        [Option(null, "getstatus", HelpText = "Gets the status of the VM.", MutuallyExclusiveSet = "Commands")]
        public bool GetStatus;

        [Option(null, "getip", HelpText = "Gets the IP address of the VM's primary network interface.", MutuallyExclusiveSet = "Commands")]
        public bool GetIP;

        [Option(null, "execute", HelpText = "Executes the specified command on the VM using a remote shell.", MutuallyExclusiveSet = "Commands")]
        public bool Execute;

        [Option(null, "dir", HelpText = "Sets the working directory for remote commands.")]
        public string WorkingDirectory;

        [OptionArray(null, "env", HelpText = "Sets environment variables for remote commands.  Should have the form --env var=value var2=value2 ...")]
        public string[] EnvironmentVariables;

        [Option(null, "copytovm", HelpText = "Copies files to the VM.", MutuallyExclusiveSet = "Commands")]
        public bool CopyToVM;

        [Option(null, "copyfromvm", HelpText = "Copies files from the VM.", MutuallyExclusiveSet = "Commands")]
        public bool CopyFromVM;

        [Option("r", "recursive", HelpText = "Makes file copy operations recursive.", MutuallyExclusiveSet = "Commands")]
        public bool Recursive;

        [ValueList(typeof(List<string>))]
        public IList<string> Values;

        [HelpOption(null, "help", HelpText = "Display help text.")]
        public string GetUsage()
        {
            var helpText = new HelpText("VMTool Client");
            helpText.AddOptions(this);
            helpText.AddPostOptionsLine("Commands require a virtual machine to be specified y profile or explicitly:");
            helpText.AddPostOptionsLine("  --configuration <file> --profile <id>");
            helpText.AddPostOptionsLine("  --host <name> --port <#> --vm <name or uuid> [--snapshot <name or uuid>]");
            helpText.AddPostOptionsLine("");
            helpText.AddPostOptionsLine("Command syntax:");
            helpText.AddPostOptionsLine("  --start");
            helpText.AddPostOptionsLine("  --poweroff");
            helpText.AddPostOptionsLine("  --shutdown");
            helpText.AddPostOptionsLine("  --pause");
            helpText.AddPostOptionsLine("  --resume");
            helpText.AddPostOptionsLine("  --takesnapshot <name>");
            helpText.AddPostOptionsLine("  --getstatus");
            helpText.AddPostOptionsLine("  --getip");
            helpText.AddPostOptionsLine("  --execute <command args...> [--dir <working dir>]");
            helpText.AddPostOptionsLine("      [--env <var1>=<value1> <var2>=<value2>...]");
            helpText.AddPostOptionsLine("  --copytovm <local file> <remote file> [--recursive]");
            helpText.AddPostOptionsLine("  --copyfromvm <remote file> <local file> [--recursive]");
            return helpText.ToString();
        }
    }
}
