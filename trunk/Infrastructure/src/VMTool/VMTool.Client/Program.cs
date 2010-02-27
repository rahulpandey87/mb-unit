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

                if (options.Configuration != null && options.Profile != null)
                {
                    Configuration configuration = ConfigurationFileHelper.LoadConfiguration(options.Configuration);
                    Profile profile = configuration.GetProfileById(options.Profile);
                    if (profile == null)
                    {
                        Console.Error.WriteLine("Profile not found in configuration file.");
                        return 1;
                    }

                    options.Host = profile.Host;
                    options.Port = profile.Port;
                    options.VM = profile.VM;
                    options.Snapshot = profile.Snapshot;
                }

                if (options.Host == null)
                {
                    Console.Error.WriteLine("Must specify --host or --configuration and --profile.");
                    return 1;
                }

                Command command = CreateCommand(options);
                if (command == null)
                {
                    Console.Error.WriteLine("Must specify a command.");
                    return 1;
                }

                if (! command.Validate(options))
                    return 1;

                TTransport transport = new TSocket(options.Host, options.Port);
                try
                {
                    TProtocol protocol = new TBinaryProtocol(transport);
                    VMToolService.Client client = new VMToolService.Client(protocol);

                    transport.Open();

                    command.Execute(client, options);
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
                }
                finally
                {
                    transport.Close();
                }
                return 0;
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
            else if (options.TakeSnapshot)
                return new TakeSnapshotCommand();
            else if (options.GetStatus)
                return new GetStatusCommand();
            else if (options.GetIP)
                return new GetIPCommand();
            else
                return null;
        }

        private abstract class Command
        {
            public virtual bool Validate(Options options)
            {
                return true;
            }

            public abstract void Execute(VMToolService.Client client, Options options);

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
            public override bool Validate(Options options)
            {
                return Check(options.VM != null, "--vm required for this command.");
            }
        }

        private abstract class VMAndSnapshotCommand : Command
        {
            public override bool Validate(Options options)
            {
                return Check(options.VM != null, "--vm required for this command.")
                    && Check(options.Snapshot != null, "--snapshot required for this command.");
            }
        }

        private class StartCommand : VMCommand
        {
            public override void Execute(VMToolService.Client client, Options options)
            {
                var req = new StartRequest()
                {
                    Vm = options.VM,
                    Snapshot = options.Snapshot
                };

                client.Start(req);
            }
        }

        private class PowerOffCommand : VMCommand
        {
            public override void Execute(VMToolService.Client client, Options options)
            {
                var req = new PowerOffRequest()
                {
                    Vm = options.VM
                };

                client.PowerOff(req);
            }
        }

        private class ShutdownCommand : VMCommand
        {
            public override void Execute(VMToolService.Client client, Options options)
            {
                var req = new ShutdownRequest()
                {
                    Vm = options.VM
                };

                client.Shutdown(req);
            }
        }

        private class PauseCommand : VMCommand
        {
            public override void Execute(VMToolService.Client client, Options options)
            {
                var req = new PauseRequest()
                {
                    Vm = options.VM
                };

                client.Pause(req);
            }
        }

        private class ResumeCommand : VMCommand
        {
            public override void Execute(VMToolService.Client client, Options options)
            {
                var req = new ResumeRequest()
                {
                    Vm = options.VM
                };

                client.Resume(req);
            }
        }

        private class TakeSnapshotCommand : VMAndSnapshotCommand
        {
            public override void Execute(VMToolService.Client client, Options options)
            {
                var req = new TakeSnapshotRequest()
                {
                    Vm = options.VM,
                    SnapshotName = options.Snapshot
                };

                client.TakeSnapshot(req);
            }
        }

        private class GetStatusCommand : VMCommand
        {
            public override void Execute(VMToolService.Client client, Options options)
            {
                var req = new GetStatusRequest()
                {
                    Vm = options.VM
                };

                GetStatusResponse response = client.GetStatus(req);

                Console.Out.WriteLine(response.Status);
            }
        }

        private class GetIPCommand : VMCommand
        {
            public override void Execute(VMToolService.Client client, Options options)
            {
                var req = new GetIPRequest()
                {
                    Vm = options.VM
                };

                GetIPResponse response = client.GetIP(req);

                Console.Out.WriteLine(response.Ip);
            }
        }
    }
}
