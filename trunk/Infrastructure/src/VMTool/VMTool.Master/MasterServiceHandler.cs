using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VMTool.Thrift;
using System.Diagnostics;
using System.IO;
using log4net;
using System.Text.RegularExpressions;
using System.Management;
using System.Globalization;
using VMTool.Core;

namespace VMTool.Master
{
    public class MasterServiceHandler : VMToolMaster.Iface
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(MasterServiceHandler));
        
        private delegate bool RetryAction(bool lastRetry);
        private const int RetryDelayMillis = 2000;
        private const int MaxRetries = 15;

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

                // Wait a bit before starting the VM.  May help work around problems where
                // the VM will not start due to VBOX_E_INVALID_OBJECT_STATE.
                Thread.Sleep(10000);
            }

            Retry((lastRetry) =>
            {
                exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                    string.Format("startvm \"{0}\"", request.Vm),
                    TimeSpan.FromSeconds(30),
                    out output);

                if (exitCode != 0
                    || output.Contains("E_FAIL")
                    || output.Contains("VBOX_E_INVALID_OBJECT_STATE"))
                {
                    // Sometimes VirtualBox hangs with a VM in a saved state but not completely
                    // powered off.  When this happens, we need to forcibly kill the VM process.
                    if (! lastRetry)
                    {
                        Status status = GetVmStatus(request.Vm, false);
                        if (status == Status.SAVED || status == Status.OFF)
                        {
                            if (KillHungVm(request.Vm))
                                return false;
                        }
                    }
                
                    throw OperationFailed(
                        "Failed to start the virtual machine.", 
                        ErrorDetails(exitCode, output));
                }
                
                return true;
            });

            return new StartResponse();
        }
        
        private bool KillHungVm(string vm)
        {
            log.InfoFormat("Forcibly killing VM due to an apparent VirtualBox hang: {0}", vm);
            
            string commandLineArg = "--comment \"" + vm + "\"";
            
            bool found = false;
            var searcher = new ManagementObjectSearcher("SELECT ProcessId, CommandLine from Win32_Process WHERE Name='VirtualBox.exe'");
            foreach (ManagementObject result in searcher.Get())
            {
                string commandLine = (string) result["CommandLine"];
                if (commandLine.Contains(commandLineArg))
                {
                    int processId = (int) (uint) result["ProcessId"];
                    
                    log.InfoFormat("Killing VirtualBox process {0}", processId);
                    try
                    {
                        Process process = Process.GetProcessById(processId);
                        process.Kill();
                    }
                    catch (ArgumentException ex)
                    {
                        log.Warn("Could not kill VirtualBox process.", ex);
                    }
                    found = true;
                }
            }
            
            if (! found)
                log.Warn("Could not find associated VirtualBox process in order to kill it.");
            return found;
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

            // Wait a bit after powering off the VM to ensure that it has been completely
            // shut down.  If we try to manipulate the VM again too promptly then we get
            // Progress state: E_ACCESSDENIED
            // Restoring snapshot 94c516b5-ae92-4c82-9a37-b0c0a79eddd0
            // VBoxManage.exe: error: Snapshot operation failed. Error message: Assertion failed: [SUCCEEDED(rc)] at 'D:\tinderbox\win-4.1\src\VBox\Main\src-server\MachineImpl.cpp' (9143) in Machine::saveStorageControllers.
            // VBoxManage.exe: error: COM RC = E_ACCESSDENIED (0x80070005).
            // VBoxManage.exe: error: Please contact the product vendor!
            Thread.Sleep(10000);

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
            
            Status status = GetVmStatus(request.Vm, true);
            
            return new GetStatusResponse()
            {
                Status = status
            };
        }

        private Status GetVmStatus(string vm, bool throwOnError)
        {
            Status status = Status.UNKNOWN;
            Retry((lastRetry) =>
            {
                string output;
                int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                    string.Format("showvminfo \"{0}\"", vm),
                    TimeSpan.FromSeconds(30),
                    out output);

                Match match = Regex.Match(output, @"State: *([a-zA-Z ]+)");

                if (exitCode != 0 || ! match.Success)
                {
                    if (throwOnError && (exitCode != 0 || lastRetry))
                        throw OperationFailed(
                            "Failed to get the status of the virtual machine.",
                            ErrorDetails(exitCode, output));
                    else
                        return false;
                }
                
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
                        if (! lastRetry)
                            return false;
                        break;
                }
                
                return true;
            });

            return status;
        }

        public GetIPResponse GetIP(GetIPRequest request)
        {
            log.InfoFormat("GetIP:\n  VM: {0}", request.Vm);

            string ip = null;
            Retry((lastRetry) =>
            {
                string output;
                int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                    string.Format("guestproperty get \"{0}\" /VirtualBox/GuestInfo/Net/0/V4/IP", request.Vm),
                    TimeSpan.FromSeconds(30),
                    out output);

                Match match = Regex.Match(output, @"Value: ([0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)");

                if (exitCode != 0 || ! match.Success)
                {
                    if (exitCode != 0 || lastRetry)
                        throw OperationFailed(
                            "Failed to get the IP address of the virtual machine's primary network interface.",
                            ErrorDetails(exitCode, output));
                    else
                        return false;
                }
                
                ip = match.Groups[1].Value;
                return true;
            });

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
        
        private static bool Retry(RetryAction action)
        {
            for (int i = 0; ; i++)
            {
                bool lastRetry = i == MaxRetries;
                if (action(lastRetry))
                    return true;
                if (lastRetry)
                    return false;
                Thread.Sleep(RetryDelayMillis);
            }
        }
    }
}
