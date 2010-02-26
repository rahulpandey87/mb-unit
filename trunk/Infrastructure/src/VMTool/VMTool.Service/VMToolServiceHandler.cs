using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMTool.Thrift;
using System.Diagnostics;
using System.IO;
using log4net;
using System.Text.RegularExpressions;

namespace VMTool.Service
{
    public class VMToolServiceHandler : VMToolService.Iface
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(VMToolServiceHandler));

        public StartResponse Start(StartRequest request)
        {
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
                    throw new OperationFailedException()
                    {
                        Why = "Failed to restore the snapshot.",
                        Details = output
                    };
                }
            }

            exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("startvm \"{0}\"", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw new OperationFailedException()
                {
                    Why = "Failed to start the virtual machine.",
                    Details = output
                };
            }

            return new StartResponse();
        }

        public PowerOffResponse PowerOff(PowerOffRequest request)
        {
            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" poweroff", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw new OperationFailedException()
                {
                    Why = "Failed to power off the virtual machine.",
                    Details = output
                };
            }

            return new PowerOffResponse();
        }

        public ShutdownResponse Shutdown(ShutdownRequest request)
        {
            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" acpipowerbutton", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw new OperationFailedException()
                {
                    Why = "Failed to shutdown the virtual machine.",
                    Details = output
                };
            }

            return new ShutdownResponse();
        }

        public PauseResponse Pause(PauseRequest request)
        {
            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" pause", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw new OperationFailedException()
                {
                    Why = "Failed to pause the virtual machine.",
                    Details = output
                };
            }

            return new PauseResponse();
        }

        public ResumeResponse Resume(ResumeRequest request)
        {
            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("controlvm \"{0}\" resume", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw new OperationFailedException()
                {
                    Why = "Failed to resume the virtual machine.",
                    Details = output
                };
            }

            return new ResumeResponse();
        }

        public TakeSnapshotResponse TakeSnapshot(TakeSnapshotRequest request)
        {
            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("snapshot \"{0}\" take \"{1}\"", request.Vm, request.SnapshotName),
                TimeSpan.FromSeconds(30),
                out output);

            if (exitCode != 0)
            {
                throw new OperationFailedException()
                {
                    Why = "Failed to take snapshot.",
                    Details = output
                };
            }

            return new TakeSnapshotResponse();
        }

        public GetIPResponse GetIP(GetIPRequest request)
        {
            string output;
            int exitCode = ExecuteVBoxCommand("VBoxManage.exe",
                string.Format("guestproperty get \"{0}\" /VirtualBox/GuestInfo/Net/0/V4/IP", request.Vm),
                TimeSpan.FromSeconds(30),
                out output);

            Match match = Regex.Match(output, @"Value: ([0-9]+\.[0-9]+\.[0-9]+\.[0-9]+)");

            if (exitCode != 0 || ! match.Success)
            {
                throw new OperationFailedException()
                {
                    Why = "Failed to get the IP address of the virtual machine's primary network interface.",
                    Details = output
                };
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
                throw new OperationFailedException()
                {
                    Why = "Server misconfigured: VBOX_INSTALL_PATH environment variable missing."
                };
            }

            var startInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(installPath, fileName),
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            log.InfoFormat("Running command: {0} {1}", startInfo.FileName, startInfo.Arguments);

            var outputBuilder = new StringBuilder();
            int exitCode;
            try
            {
                var process = Process.Start(startInfo);
                process.OutputDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
                process.ErrorDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data);
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                if (process.WaitForExit((int) timeout.TotalMilliseconds))
                    exitCode = process.ExitCode;
                else
                    exitCode = -1;

                process.CancelOutputRead();
                process.CancelErrorRead();
            }
            catch (Exception ex)
            {
                throw new OperationFailedException()
                {
                    Why = string.Format("Error while running command on server: {0} {1}",
                        startInfo.FileName, startInfo.Arguments),
                    Details = ex.Message
                };
            }

            output = outputBuilder.ToString();

            log.Info(output);
            log.InfoFormat("Command exited with code {0}.", exitCode);

            if (exitCode == -1)
            {
                throw new OperationFailedException()
                {
                    Why = string.Format("Timed out waiting for command to complete after {0}ms: {1} {2}",
                        timeout.TotalMilliseconds, startInfo.FileName, startInfo.Arguments),
                    Details = output
                };
            }

            return exitCode;
        }
    }
}
