using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thrift.Protocol;
using CommandLine;
using Thrift.Transport;
using VMTool.Thrift;
using System.IO;
using VMTool.Schema;
using System.Collections.Specialized;
using VMTool.Core;

namespace VMTool.Client
{
    public class ClientProgram
    {
        public static int Main(string[] args)
        {
            try
            {
                var options = new ClientOptions();
                var parser = new CommandLineParser();
                if (!parser.ParseArguments(args, options, Console.Error))
                    return 1;

                if (options.Values == null || options.Values.Count == 0)
                {
                    PrintErrorMessageAndHelp(options,"Must specify a command.");
                    return 1;
                }

                string commandName = options.Values[0];
                Command command = CreateCommand(commandName);
                if (command == null)
                {
                    PrintErrorMessageAndHelp(options, "Unrecognized command name.");
                    return 1;
                }

                bool haveMaster = options.Master != null;
                bool haveProfile = options.Configuration != null || options.Profile != null;
                if (haveMaster && haveProfile
                    || ! haveMaster && ! haveProfile
                    || (options.Configuration != null) != (options.Profile != null))
                {
                    PrintErrorMessageAndHelp(options, "Must specify either --master or both --configuration and --profile.");
                    return 1;
                }

                Profile profile;
                if (options.Configuration != null && options.Profile != null)
                {
                    XmlConfiguration xmlConfiguration = ConfigurationFileHelper.LoadConfiguration(options.Configuration);
                    XmlProfile xmlProfile = xmlConfiguration.GetProfileById(options.Profile);
                    if (xmlProfile == null)
                    {
                        PrintErrorMessageAndHelp(options, "Profile not found in configuration file.");
                        return 1;
                    }

                    profile = xmlProfile.ToProfile();
                }
                else
                {
                    profile = new Profile()
                    {
                        Master = options.Master,
                        MasterPort = options.MasterPort,
                        Slave = options.Slave,
                        SlavePort = options.SlavePort,
                        VM = options.VM,
                        Snapshot = options.Snapshot
                    };
                }

                if (!command.Validate(profile, options))
                    return 1;

                using (var controller = new ClientController(profile))
                {
                    controller.Quiet = options.Quiet;

                    try
                    {
                        return command.Execute(controller, options);
                    }
                    catch (OperationFailedException ex)
                    {
                        Console.Error.WriteLine("Operation failed.");
                        Console.Error.WriteLine(ex.Why);

                        if (ex.__isset.details)
                        {
                            Console.Error.WriteLine("Details:");
                            Console.Error.WriteLine(ex.Details);
                        }
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal exception: " + ex);
                return 1;
            }
        }

        private static void PrintErrorMessageAndHelp(ClientOptions options, string message)
        {
            Console.Error.WriteLine(message);
            Console.Error.WriteLine();
            Console.Error.WriteLine(options.GetUsage());
        }

        private static Command CreateCommand(string commandName)
        {
            switch (commandName)
            {
                case "start":
                    return new StartCommand();
                case "poweroff":
                    return new PowerOffCommand();
                case "shutdown":
                    return new ShutdownCommand();
                case "pause":
                    return new PauseCommand();
                case "resume":
                    return new ResumeCommand();
                case "savestate":
                    return new SaveStateCommand();
                case "takesnapshot":
                    return new TakeSnapshotCommand();
                case "getstatus":
                    return new GetStatusCommand();
                case "getip":
                    return new GetIPCommand();
                case "execute":
                    return new ExecuteCommand();
                case "copytovm":
                    return new CopyToVMCommand();
                case "copyfromvm":
                    return new CopyFromVMCommand();
                default:
                    return null;
            }
        }

        private abstract class Command
        {
            public virtual bool Validate(Profile profile, ClientOptions options)
            {
                return CheckValues(options)
                    && CheckUnusedRecursive(options)
                    && CheckUnusedForce(options)
                    && CheckUnusedEnvironmentVariables(options)
                    && CheckUnusedWorkingDirectory(options);
            }

            public abstract int Execute(ClientController controller, ClientOptions options);

            protected virtual bool CheckValues(ClientOptions options)
            {
                return Check(options.Values.Count == 1,
                    "Found excess arguments.");
            }

            protected virtual bool CheckUnusedRecursive(ClientOptions options)
            {
                return Check(! options.Recursive,
                    "--recursive is not used by this command.");
            }

            protected virtual bool CheckUnusedForce(ClientOptions options)
            {
                return Check(! options.Force,
                    "--force is not used by this command.");
            }

            protected virtual bool CheckUnusedEnvironmentVariables(ClientOptions options)
            {
                return Check(options.EnvironmentVariables == null || options.EnvironmentVariables.Length == 0,
                    "--env is not used by this command.");
            }

            protected virtual bool CheckUnusedWorkingDirectory(ClientOptions options)
            {
                return Check(options.WorkingDirectory == null,
                    "--dir is not used by this command.");
            }

            protected static bool Check(bool condition, string message)
            {
                if (! condition)
                {
                    Console.Error.WriteLine(message);
                    return false;
                }

                return true;
            }
        }

        private abstract class VMCommand : Command
        {
            public override bool Validate(Profile profile, ClientOptions options)
            {
                return base.Validate(profile, options)
                    && Check(profile.VM != null, "--vm or --profile required for this command.");
            }
        }

        private class StartCommand : VMCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.Start();
                return 0;
            }
        }

        private class PowerOffCommand : VMCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.PowerOff();
                return 0;
            }
        }

        private class ShutdownCommand : VMCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.Shutdown();
                return 0;
            }
        }

        private class PauseCommand : VMCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.Pause();
                return 0;
            }
        }

        private class ResumeCommand : VMCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.Resume();
                return 0;
            }
        }

        private class SaveStateCommand : VMCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.SaveState();
                return 0;
            }
        }

        private class TakeSnapshotCommand : VMCommand
        {
            protected override bool CheckValues(ClientOptions options)
            {
                return Check(options.Values.Count >= 2, "Missing new snapshot name.")
                    && Check(options.Values.Count == 2, "Found excess arguments.");                
            }

            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.TakeSnapshot(options.Values[1]);
                return 0;
            }
        }

        private class GetStatusCommand : VMCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                Status status = controller.GetStatus();
                Console.Out.WriteLine(status);
                return 0;
            }
        }

        private class GetIPCommand : VMCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                string ip = controller.GetIP();
                Console.Out.WriteLine(ip);
                return 0;
            }
        }

        private class ExecuteCommand : VMCommand
        {
            protected override bool CheckValues(ClientOptions options)
            {
                return Check(options.Values.Count >= 2, "Missing command to execute.");
            }

            protected override bool CheckUnusedEnvironmentVariables(ClientOptions options)
            {
                return true;
            }

            protected override bool CheckUnusedWorkingDirectory(ClientOptions options)
            {
                return true;
            }

            public override int Execute(ClientController controller, ClientOptions options)
            {
                Dictionary<string, string> environmentVariables = null;
                if (options.EnvironmentVariables != null)
                {
                    environmentVariables = new Dictionary<string, string>();
                    foreach (string v in options.EnvironmentVariables)
                    {
                        int equalsPos = v.IndexOf('=');
                        if (equalsPos < 0)
                            environmentVariables[v] = "";
                        else
                            environmentVariables[v.Substring(0, equalsPos)] = v.Substring(equalsPos + 1);
                    }
                }

                string executable = options.Values[1];
                StringBuilder arguments = new StringBuilder();
                for (int i = 2; i < options.Values.Count; i++)
                {
                    if (i != 2)
                        arguments.Append(" ");

                    string argument = options.Values[i];
                    if (ShouldArgumentBeQuoted(argument))
                        arguments.Append("\"").Append(argument).Append("\"");
                    else
                        arguments.Append(argument);
                }

                return controller.RemoteExecute(executable, arguments.ToString(), options.WorkingDirectory,
                    environmentVariables,
                    line => Console.Out.WriteLine(line),
                    line => Console.Error.WriteLine(line));
            }

            private static bool ShouldArgumentBeQuoted(string arg)
            {
                return arg.Contains(" ");
            }
        }

        private abstract class CopyCommand : VMCommand
        {
            protected override bool CheckValues(ClientOptions options)
            {
                return Check(options.Values.Count >= 3, "Missing files to copy.")
                    && Check(options.Values.Count == 3, "Found excess arguments.");
            }

            protected override bool CheckUnusedRecursive(ClientOptions options)
            {
                return true;
            }

            protected override bool CheckUnusedForce(ClientOptions options)
            {
                return true;
            }
        }

        private class CopyToVMCommand : CopyCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.CopyToVM(options.Values[1], options.Values[2],
                    options.Recursive, options.Force);
                return 0;
            }
        }

        private class CopyFromVMCommand : CopyCommand
        {
            public override int Execute(ClientController controller, ClientOptions options)
            {
                controller.CopyFromVM(options.Values[1], options.Values[2],
                    options.Recursive, options.Force);
                return 0;
            }
        }
    }
}
