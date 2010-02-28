using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMTool.Thrift;
using System.Diagnostics;
using System.IO;
using log4net;
using System.Text.RegularExpressions;
using VMTool.Core;

namespace VMTool.Master
{
    public class MasterServiceHandler : VMToolMaster.Iface
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(MasterServiceHandler));

        public StartResponse Start(StartRequest request)
        {
            StringBuilder message = new StringBuilder();
            message.AppendFormat("Start:\n  VM: {0}", request.Vm);
            if (request.__isset.snapshot)
                message.AppendFormat("\n  Snapshot: {0}", request.Snapshot);
            log.Info(message.ToString());

            string output;
            int exitCode;

            if (request.__isset.snapshot)
            {
                exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                    string.Format("snapshot \"{0}\" restore \"{1}\"", request.Vm, request.Snapshot),
                    TimeSpan.FromSeconds(30),
                    out output);
                if (exitCode != 0)
                {
                    throw OperationFailed("Failed to restore the snapshot.", output);
                }
            }

            exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("startvm \"{0}\"", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw OperationFailed(
                    "Failed to start the virtual machine.", 
                    ErrorDetails(exitCode, output));
            }

            return new StartResponse();
        }

        public PowerOffResponse PowerOff(PowerOffRequest request)
        {
            log.InfoFormat("PowerOff:\n  VM: {0}", request.Vm);

            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" poweroff", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw OperationFailed(
                    "Failed to power off the virtual machine.", 
                    ErrorDetails(exitCode, output));
            }

            return new PowerOffResponse();
        }

        public ShutdownResponse Shutdown(ShutdownRequest request)
        {
            log.InfoFormat("Shutdown:\n  VM: {0}", request.Vm);

            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" acpipowerbutton", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw OperationFailed(
                    "Failed to shutdown the virtual machine.", 
                    ErrorDetails(exitCode, output));
            }

            return new ShutdownResponse();
        }

        public PauseResponse Pause(PauseRequest request)
        {
            log.InfoFormat("Pause:\n  VM: {0}", request.Vm);

            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" pause", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw OperationFailed(
                    "Failed to pause the virtual machine.", 
                    ErrorDetails(exitCode, output));
            }

            return new PauseResponse();
        }

        public ResumeResponse Resume(ResumeRequest request)
        {
            log.InfoFormat("Resume:\n  VM: {0}", request.Vm);

            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" resume", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw OperationFailed(
                    "Failed to resume the virtual machine.",
                    ErrorDetails(exitCode, output));
            }

            return new ResumeResponse();
        }

        public SaveStateResponse SaveState(SaveStateRequest request)
        {
            log.InfoFormat("SaveState:\n  VM: {0}", request.Vm);

            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" savestate", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw OperationFailed(
                    "Failed to save the state and stop the virtual machine.",
                    ErrorDetails(exitCode, output));
            }

            return new SaveStateResponse();
        }

        public TakeSnapshotResponse TakeSnapshot(TakeSnapshotRequest request)
        {
            log.InfoFormat("TakeSnapshot:\n  VM: {0}\n  SnapshotName: {1}", request.Vm, request.SnapshotName);

            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("snapshot \"{0}\" take \"{1}\"", request.Vm, request.SnapshotName),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw OperationFailed(
                    "Failed to take snapshot.",
                    ErrorDetails(exitCode, output));
            }

            return new TakeSnapshotResponse();
        }

        public GetStatusResponse GetStatus(GetStatusRequest request)
        {
            log.InfoFormat("GetStatus:\n  VM: {0}", request.Vm);

            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("showvminfo \"{0}\"", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            Match match = Regex.Match(output, @"State: *([a-zA-Z ]+)");

            if (exitCode != 0 || !match.Success)
            {
                throw OperationFailed(
                    "Failed to get the status of the virtual machine.",
                    ErrorDetails(exitCode, output));
            }

            Status status;
            string statusString = match.Groups[1].Value.Trim();
            switch (statusString)
            {
                case "powered off":
                    status = Status.OFF;
                    break;
                case "running":
                    status = Status.RUNNING;
                    break;
                case "paused":
                    status = Status.PAUSED;
                    break;
                case "saved":
                    status = Status.SAVED;
                    break;
                default:
                    status = Status.UNKNOWN;
                    break;
            }

            return new GetStatusResponse()
            {
                Status = status
            };
        }

        public GetIPResponse GetIP(GetIPRequest request)
        {
            log.InfoFormat("GetIP:\n  VM: {0}", request.Vm);

            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("guestproperty get \"{0}\" /VirtualBox/GuestInfo/Net/0/V4/IP", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            Match match = Regex.Match(output, @"Value: ([0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)");

            if (exitCode != 0 || ! match.Success)
            {
                throw OperationFailed(
                    "Failed to get the IP address of the virtual machine's primary network interface.",
                    ErrorDetails(exitCode, output));
            }

            string ip = match.Groups[1].Value;
            return new GetIPResponse()
            {
                Ip = ip
            };
        }

        private int ExecuteVBoxCommand(string fileName, string args, TimeSpan timeout, out string output)
        {
            string installPath = Environment.GetEnvironmentVariable("VBOX_INSTALL_PATH");
            if (installPath == null)
            {
                throw OperationFailed(
                    "Server misconfigured: VBOX_INSTALL_PATH environment variable missing.",
                    null);
            }

            string executable = Path.Combine(installPath, fileName);

            log.InfoFormat("Running command: {0} {1}", executable, args);

            var outputBuilder = new StringBuilder();
            int exitCode;
            try
            {
                exitCode = ProcessUtil.Execute(executable, args, null, null,
                    line => outputBuilder.AppendLine(line),
                    line => outputBuilder.AppendLine(line),
                    timeout);
            }
            catch (Exception ex)
            {
                throw OperationFailed(
                    string.Format("Error while running command on server: {0} {1}", executable, args),
                    ex.Message);
            }

            output = outputBuilder.ToString();

            log.Info(output);
            log.InfoFormat("Command exited with code {0}.", exitCode);

            if (exitCode == -1)
            {
                throw OperationFailed(
                    string.Format("Timed out waiting for command to complete after {0}ms: {1} {2}",
                        timeout.TotalMilliseconds, executable, args),
                    output);
            }

            return exitCode;
        }

        private static OperationFailedException OperationFailed(string why, string details)
        {
            log.ErrorFormat("Operation failed: {0}", why);

            var ex = new OperationFailedException() { Why = why };
            if (details != null)
                ex.Details = details;
            return ex;
        }

        private static string ErrorDetails(int exitCode, string output)
        {
            return output + "\nExit Code: " + exitCode;
        }
    }
}
