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

namespace VMTool.Client
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var options = new Options();
                var parser = new CommandLineParser();
                if (!parser.ParseArguments(args, options, Console.Error))
                    return 1;

                Command command = CreateCommand(options);
                if (command == null)
                {
                    Console.Error.WriteLine("Must specify a command.");
                    return 1;
                }

                bool haveHost = options.Host != null;
                bool haveProfile = options.Configuration != null || options.Profile != null;
                if (haveHost && haveProfile
                    || ! haveHost && ! haveProfile
                    || (options.Configuration != null) != (options.Profile != null))
                {
                    Console.Error.WriteLine("Must specify either --host or both --configuration and --profile.");
                    return 1;
                }

                ClientController controller;
                if (options.Configuration != null && options.Profile != null)
                {
                    Configuration configuration = ConfigurationFileHelper.LoadConfiguration(options.Configuration);
                    Profile profile = configuration.GetProfileById(options.Profile);
                    if (profile == null)
                    {
                        Console.Error.WriteLine("Profile not found in configuration file.");
                        return 1;
                    }

                    controller = new ClientController(profile.Host, profile.Port, profile.VM, profile.Snapshot);
                }
                else
                {
                    controller = new ClientController(options.Host, options.Port, options.VM, options.Snapshot);
                }

                controller.Quiet = options.Quiet;

                try
                {
                    using (controller)
                    {
                        if (!command.Validate(controller, options))
                            return 1;

                        return command.Execute(controller, options);
                    }
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
            catch (Exception ex)
            {
                Console.WriteLine("Fatal exception: " + ex);
                return 1;
            }
        }

        private static Command CreateCommand(Options options)
        {
            if (options.Start)
                return new StartCommand();
            else if (options.PowerOff)
                return new PowerOffCommand();
            else if (options.Shutdown)
                return new ShutdownCommand();
            else if (options.Pause)
                return new PauseCommand();
            else if (options.Resume)
                return new ResumeCommand();
            else if (options.SaveState)
                return new SaveStateCommand();
            else if (options.TakeSnapshot)
                return new TakeSnapshotCommand();
            else if (options.GetStatus)
                return new GetStatusCommand();
            else if (options.GetIP)
                return new GetIPCommand();
            else if (options.Execute)
                return new ExecuteCommand();
            else if (options.CopyToVM)
                return new CopyToVMCommand();
            else if (options.CopyFromVM)
                return new CopyFromVMCommand();
            else
                return null;
        }

        private abstract class Command
        {
            public virtual bool Validate(ClientController controller, Options options)
            {
                return true;
            }

            public abstract int Execute(ClientController controller, Options options);

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
            public override bool Validate(ClientController controller, Options options)
            {
                return Check(controller.VM != null, "--vm or --profile required for this command.");
            }
        }

        private class StartCommand : VMCommand
        {
            public override int Execute(ClientController controller, Options options)
            {
                controller.Start();
                return 0;
            }
        }

        private class PowerOffCommand : VMCommand
        {
            public override int Execute(ClientController controller, Options options)
            {
                controller.PowerOff();
                return 0;
            }
        }

        private class ShutdownCommand : VMCommand
        {
            public override int Execute(ClientController controller, Options options)
            {
                controller.Shutdown();
                return 0;
            }
        }

        private class PauseCommand : VMCommand
        {
            public override int Execute(ClientController controller, Options options)
            {
                controller.Pause();
                return 0;
            }
        }

        private class ResumeCommand : VMCommand
        {
            public override int Execute(ClientController controller, Options options)
            {
                controller.Resume();
                return 0;
            }
        }

        private class SaveStateCommand : VMCommand
        {
            public override int Execute(ClientController controller, Options options)
            {
                controller.SaveState();
                return 0;
            }
        }

        private class TakeSnapshotCommand : VMCommand
        {
            public override bool Validate(ClientController controller, Options options)
            {
                return base.Validate(controller, options)
                    && Check(options.Values != null && options.Values.Count == 1,
                    "Missing new snapshot name.");
            }

            public override int Execute(ClientController controller, Options options)
            {
                controller.TakeSnapshot(options.Values[0]);
                return 0;
            }
        }

        private class GetStatusCommand : VMCommand
        {
            public override int Execute(ClientController controller, Options options)
            {
                Status status = controller.GetStatus();
                Console.Out.WriteLine(status);
                return 0;
            }
        }

        private class GetIPCommand : VMCommand
        {
            public override int Execute(ClientController controller, Options options)
            {
                string ip = controller.GetIP();
                Console.Out.WriteLine(ip);
                return 0;
            }
        }

        private class ExecuteCommand : VMCommand
        {
            public override bool Validate(ClientController controller, Options options)
            {
                return base.Validate(controller, options)
                    && Check(options.Values != null && options.Values.Count >= 1,
                    "Missing command to execute.");
            }

            public override int Execute(ClientController controller, Options options)
            {
                StringDictionary environmentVariables = null;
                if (options.EnvironmentVariables != null)
                {
                    environmentVariables = new StringDictionary();
                    foreach (string v in options.EnvironmentVariables)
                    {
                        int equalsPos = v.IndexOf('=');
                        if (equalsPos < 0)
                            environmentVariables[v] = "";
                        else
                            environmentVariables[v.Substring(0, equalsPos)] = v.Substring(equalsPos + 1);
                    }
                }

                string executable = options.Values[0];
                StringBuilder arguments = new StringBuilder();
                for (int i = 1; i < options.Values.Count; i++)
                {
                    if (i != 1)
                        arguments.Append(" ");
                    arguments.Append("\"").Append(options.Values[i]).Append("\"");
                }

                return controller.RemoteExecute(executable, arguments.ToString(), options.WorkingDirectory,
                    environmentVariables,
                    line => Console.Out.WriteLine(line),
                    line => Console.Error.WriteLine(line));
            }
        }

        private class CopyToVMCommand : VMCommand
        {
            public override bool Validate(ClientController controller, Options options)
            {
                return base.Validate(controller, options)
                    && Check(options.Values != null && options.Values.Count == 2,
                    "Missing local or remote files to copy.");
            }

            public override int Execute(ClientController controller, Options options)
            {
                controller.CopyToVM(options.Values[0], options.Values[1], options.Recursive);
                return 0;
            }
        }

        private class CopyFromVMCommand : VMCommand
        {
            public override bool Validate(ClientController controller, Options options)
            {
                return base.Validate(controller, options)
                    && Check(options.Values != null && options.Values.Count == 2,
                    "Missing local or remote files to copy.");
            }

            public override int Execute(ClientController controller, Options options)
            {
                controller.CopyFromVM(options.Values[0], options.Values[1], options.Recursive);
                return 0;
            }
        }
    }
}
