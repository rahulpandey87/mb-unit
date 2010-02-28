using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMTool.Thrift;
using System.IO;
using Thrift.Transport;
using Thrift.Protocol;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Collections;

namespace VMTool
{
    public class Controller : IDisposable
    {
        private readonly string host;
        private readonly int port;
        private readonly string vm;
        private readonly string snapshot;

        private VMToolService.Client clientLazy;
        private string ipLazy;

        public delegate void LineHandler(string line);

        public Controller(string host, int port, string vm, string snapshot)
        {
            this.host = host;
            this.port = port;
            this.vm = vm;
            this.snapshot = snapshot;
        }

        public void Dispose()
        {
            if (clientLazy != null)
            {
                clientLazy.InputProtocol.Transport.Close();
                clientLazy = null;
            }
        }

        public string Host
        {
            get { return host; }
        }

        public int Port
        {
            get { return port; }
        }

        public string VM
        {
            get { return vm; }
        }

        public string Snapshot
        {
            get { return snapshot; }
        }

        public bool IsWindows
        {
            get { return true; }
        }

        public virtual int LocalExecute(string executable, string arguments, string workingDirectory,
            StringDictionary environmentVariables,
            LineHandler stdoutHandler, LineHandler stderrHandler)
        {
            LogExecute("Executing local command: ", executable, arguments, workingDirectory, environmentVariables);

            ProcessStartInfo startInfo = new ProcessStartInfo(executable)
            {
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            if (workingDirectory != null)
            {
                startInfo.WorkingDirectory = workingDirectory;
            }
            if (environmentVariables != null)
            {
                foreach (DictionaryEntry entry in environmentVariables)
                    startInfo.EnvironmentVariables.Add((string)entry.Key, (string)entry.Value);
            }

            Process process = Process.Start(startInfo);
            process.OutputDataReceived += (sender, e) => { if (e.Data != null) stdoutHandler(e.Data); };
            process.ErrorDataReceived += (sender, e) => { if (e.Data != null) stderrHandler(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            return process.ExitCode;
        }

        public virtual int RemoteExecute(string executable, string arguments, string workingDirectory,
            StringDictionary environmentVariables,
            LineHandler stdoutHandler, LineHandler stderrHandler)
        {
            LogExecute("Executing remote command: ", executable, arguments, workingDirectory, environmentVariables);

            if (!IsWindows)
                throw new NotImplementedException();

            // Write a temporary batch file to initialize the environment and execute the process.
            string remoteShadowFilePath;
            string localBatchFilePath = Path.Combine(Path.GetTempPath(), "VMTool-" + Path.GetRandomFileName() + ".bat");
            try
            {
                using (StreamWriter batchWriter = new StreamWriter(localBatchFilePath))
                {
                    batchWriter.WriteLine("@echo off");
                    batchWriter.WriteLine("setlocal");

                    if (environmentVariables != null)
                    {
                        foreach (DictionaryEntry entry in environmentVariables)
                            batchWriter.WriteLine("set {0}={1}", EscapeLiteralForDOS((string)entry.Key), EscapeLiteralForDOS((string)entry.Value));
                    }

                    if (workingDirectory != null)
                        batchWriter.WriteLine("cd \"{0}\"", workingDirectory); // don't double-escape, already in DOS format

                    batchWriter.WriteLine("cmd /c \"{0}\" {1}", executable, arguments); // don't double-escape, already in DOS format
                }

                remoteShadowFilePath = ShadowCopy(localBatchFilePath);
            }
            finally
            {
                File.Delete(localBatchFilePath);
            }

            // Run command remotely via SSH.
            StringBuilder argsBuilder = new StringBuilder();
            argsBuilder.Append("-xa ");
            argsBuilder.Append(GetIP());
            argsBuilder.Append(" cmd /c ");
            argsBuilder.Append(EscapeLiteralForUnix(remoteShadowFilePath));
            return LocalExecute("ssh", argsBuilder.ToString(), null, null,
                stdoutHandler, stderrHandler);
        }

        protected void LogExecute(string heading, string executable, string arguments,
            string workingDirectory, StringDictionary environmentVariables)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(heading);
            message.AppendFormat("  Executable  : {0}\n", executable);
            message.AppendFormat("  Arguments   : {0}\n", arguments);

            if (workingDirectory != null)
                message.AppendFormat("  Directory   : {0}\n", workingDirectory);

            if (environmentVariables != null)
            {
                message.AppendLine("  Environment :");
                foreach (DictionaryEntry entry in environmentVariables)
                    message.AppendFormat("    {0}={1}\n", entry.Key, entry.Value);
            }

            Log(message.ToString());
        }

        public string ShadowCopy(string localFilePath)
        {
            // FIXME: bad assumption about location of Cygwin /tmp
            string remoteFilePath = Path.Combine(@"C:\cygwin\tmp", Path.GetFileName(localFilePath));
            CopyToVM(localFilePath, remoteFilePath, false);
            return remoteFilePath;
        }

        public void CopyToVM(string localFilePath, string remoteFilePath, bool recursive)
        {
            Log(string.Format("Copying file from local path '{0}' to remote path '{1}'{2}.", localFilePath, remoteFilePath,
                recursive ? " recursively" : ""));

            if (!IsWindows)
                throw new NotImplementedException();

            string source = EscapeLiteralForUnix(ToCygwinPath(localFilePath));
            string dest = GetIP() + ":" + EscapeLiteralForUnix(ToCygwinPath(remoteFilePath));
            SCP(source, dest, recursive);
        }

        public void CopyFromVM(string remoteFilePath, string localFilePath, bool recursive)
        {
            Log(string.Format("Copying file from remote path '{0}' to local path '{1}'{2}.", localFilePath, remoteFilePath,
                recursive ? " recursively" : ""));

            if (!IsWindows)
                throw new NotImplementedException();

            string source = GetIP() + ":" + EscapeLiteralForUnix(ToCygwinPath(remoteFilePath));
            string dest = EscapeLiteralForUnix(ToCygwinPath(localFilePath));
            SCP(source, dest, recursive);
        }

        private void SCP(string source, string dest, bool recursive)
        {
            StringBuilder argsBuilder = new StringBuilder();
            if (recursive)
                argsBuilder.Append("-r ");
            argsBuilder.Append("\"");
            argsBuilder.Append(source);
            argsBuilder.Append("\" \"");
            argsBuilder.Append(dest);
            argsBuilder.Append("\"");
            int exitCode = LocalExecute("scp", argsBuilder.ToString(), null, null, Log, Log);
            if (exitCode != 0)
            {
                throw new OperationFailedException()
                {
                    Why = string.Format("SCP from '{0}' to '{1}'{2} failed.", source, dest,
                        recursive ? " recursively" : "")
                };
            }
        }

        public void Start()
        {
            if (snapshot != null)
            {
                Log(string.Format("Starting VM '{0}' snapshot '{1}'.", vm, snapshot));
                StartRequest request = new StartRequest() { Vm = vm, Snapshot = snapshot };
                GetClient().Start(request);
            }
            else
            {
                Log(string.Format("Starting VM '{0}'.", vm));
                StartRequest request = new StartRequest() { Vm = vm };
                GetClient().Start(request);
            }
        }

        public void PowerOff()
        {
            Log(string.Format("Powering off VM '{0}'.", vm));
            PowerOffRequest request = new PowerOffRequest() { Vm = vm };
            GetClient().PowerOff(request);
        }

        public void Shutdown()
        {
            Log(string.Format("Shutting down VM '{0}'.", vm));
            ShutdownRequest request = new ShutdownRequest() { Vm = vm };
            GetClient().Shutdown(request);
        }

        public void Pause()
        {
            Log(string.Format("Pausing VM '{0}'.", vm));
            PauseRequest request = new PauseRequest() { Vm = vm };
            GetClient().Pause(request);
        }

        public void Resume()
        {
            Log(string.Format("Resuming VM '{0}'.", vm));
            ResumeRequest request = new ResumeRequest() { Vm = vm };
            GetClient().Resume(request);
        }

        public void SaveState()
        {
            Log(string.Format("Saving state and stopping VM '{0}'.", vm));
            SaveStateRequest request = new SaveStateRequest() { Vm = vm };
            GetClient().SaveState(request);
        }

        public void TakeSnapshot(string newSnapshotName)
        {
            Log(string.Format("Taking snapshot of VM '{0}' called '{1}'.", vm, newSnapshotName));
            TakeSnapshotRequest request = new TakeSnapshotRequest() { Vm = vm, SnapshotName = newSnapshotName };
            GetClient().TakeSnapshot(request);
        }

        public string GetIP()
        {
            if (ipLazy == null)
            {
                GetIPRequest request = new GetIPRequest() { Vm = vm };
                GetIPResponse response = GetClient().GetIP(request);
                ipLazy = response.Ip;
            }

            return ipLazy;
        }

        public Status GetStatus()
        {
            GetStatusRequest request = new GetStatusRequest() { Vm = vm };
            GetStatusResponse response = GetClient().GetStatus(request);
            return response.Status;
        }

        public VMToolService.Client GetClient()
        {
            if (clientLazy == null)
            {
                TTransport transport = new TSocket(host, port);
                TProtocol protocol = new TBinaryProtocol(transport);
                transport.Open();
                clientLazy = new VMToolService.Client(protocol);
            }

            return clientLazy;
        }

        protected virtual void Log(string message)
        {
        }

        private static string EscapeLiteralForDOS(string str)
        {
            return str.Replace("^", "^^")
                .Replace("%", "^%")
                .Replace("(", "^(")
                .Replace(")", "^)")
                .Replace("&", "^&");
        }

        private static string EscapeLiteralForUnix(string str)
        {
            return str.Replace(@"\", @"\\");
        }

        private static string ToCygwinPath(string str)
        {
            if (Path.IsPathRooted(str))
                return "/cygdrive/" + str.Replace(":", "").Replace(@"\", "/");

            return str.Replace(@"\", "/");
        }
    }
}
