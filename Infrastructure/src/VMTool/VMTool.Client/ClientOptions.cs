using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace VMTool.Client
{
    public class ClientOptions
    {
        [Option("q", "quiet", HelpText = "Disables command logging.")]
        public bool Quiet;

        [Option("c", "configuration", HelpText = "The configuration file path.")]
        public string Configuration;

        [Option("p", "profile", HelpText = "The virtual machine profile to manipulate.")]
        public string Profile;

        [Option("m", "master", HelpText = "Master host name.")]
        public string Master;

        [Option(null, "master-port", HelpText = "Master port number.  Default is " + Constants.DefaultMasterPortString + ".")]
        public int MasterPort = Constants.DefaultMasterPort;

        [Option("v", "vm", HelpText = "Specifies the VM name or UUID.")]
        public string VM;

        [Option("s", "snapshot", HelpText = "Specifies the snapshot name or UUID.")]
        public string Snapshot;

        [Option(null, "slave", HelpText = "Slave host name.")]
        public string Slave;

        [Option(null, "slave-port", HelpText = "Slave port number.  Default is " + Constants.DefaultSlavePortString + ".")]
        public int SlavePort = Constants.DefaultSlavePort;

        [Option(null, "dir", HelpText = "(execute) Sets the working directory.")]
        public string WorkingDirectory;

        [OptionArray(null, "env", HelpText = "(execute) Sets an enviroment variable.")]
        public string[] EnvironmentVariables;

        [Option(null, "recursive", HelpText = "(copytovm, copyfromvm) Makes file copy operations recursive.")]
        public bool Recursive;

        [Option(null, "force", HelpText = "(copytovm, copyfromvm) Overwrites files if they already exist.")]
        public bool Force;

        [ValueList(typeof(List<string>))]
        public IList<string> Values;

        [HelpOption(null, "help", HelpText = "Display help text.")]
        public string GetUsage()
        {
            var helpText = new HelpText("VMTool Client");
            helpText.AddPreOptionsLine("Usage: <options...> <command> [<args...>]");
            helpText.AddPreOptionsLine("");
            helpText.AddPreOptionsLine("  start");
            helpText.AddPreOptionsLine("  poweroff");
            helpText.AddPreOptionsLine("  shutdown");
            helpText.AddPreOptionsLine("  pause");
            helpText.AddPreOptionsLine("  resume");
            helpText.AddPreOptionsLine("  savestate");
            helpText.AddPreOptionsLine("  takesnapshot <name>");
            helpText.AddPreOptionsLine("  getstatus");
            helpText.AddPreOptionsLine("  getip");
            helpText.AddPreOptionsLine("  execute <command> <args...> [--dir <working dir>]");
            helpText.AddPreOptionsLine("      [--env <var>=<value>...]");
            helpText.AddPreOptionsLine("  copytovm <local file glob> <remote file> [--recursive] [--force]");
            helpText.AddPreOptionsLine("  copyfromvm <remote file glob> <local file> [--recursive] [--force]");
            helpText.AddPreOptionsLine("");
            helpText.AddPreOptionsLine("Specifying virtual machine profiles:");
            helpText.AddPreOptionsLine("");
            helpText.AddPreOptionsLine("  --configuration <file> --profile <id>");
            helpText.AddPreOptionsLine("  --master <hostname> [--master-port <port>]");
            helpText.AddPreOptionsLine("      --vm <name or uuid> --snapshot <name or uuid>");
            helpText.AddPreOptionsLine("      [--slave <hostname>] [--slave-port <port>]");

            helpText.AddOptions(this);
            return helpText.ToString();
        }
    }
}
