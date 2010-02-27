using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Util;
using Thrift.Transport;
using Thrift.Protocol;
using VMTool.Thrift;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using System.Collections;
using System.IO;

namespace CCNet.VMTool.Plugin.Core
{
    public class RemoteContext : IDisposable
    {
        private const string CallContextSlot = "VMTool.RemoteContext";

        private readonly string host;
        private readonly int port;
        private readonly string vm;
        private readonly string snapshot;

        private VMToolService.Client clientLazy;
        private string ipLazy;

        public delegate void OnProcessOutput(ProcessOutputEventArgs e);

        public RemoteContext(string host, int port, string vm, string snapshot)
        {
            this.host = host;
            this.port = port;
            this.vm = vm;
            this.snapshot = snapshot;

            CallContext.SetData(CallContextSlot, this);
        }

        public void Dispose()
        {
            if (clientLazy != null)
            {
                clientLazy.InputProtocol.Transport.Close();
                clientLazy = null;
            }

            CallContext.FreeNamedDataSlot(CallContextSlot);
        }

        public static RemoteContext GetRemoteContext()
        {
            var context = CallContext.GetData(CallContextSlot) as RemoteContext;
            if (context == null)
                throw new RemoteContextException("There is no remote context.  Is the task nested within a <vm> element?");
            return context;
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

        public ProcessResult Execute(ProcessInfo processInfo, OnProcessOutput output)
        {
            if (!IsWindows)
                throw new NotImplementedException();

            // Write a temporary batch file to initialize the environment and execute the process.
            string remoteShadowFile;
            string localBatchFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".bat");
            try
            {
                using (StreamWriter batchWriter = new StreamWriter(localBatchFile))
                {
                    batchWriter.WriteLine("@echo off");
                    batchWriter.WriteLine("setlocal");

                    foreach (DictionaryEntry entry in processInfo.EnvironmentVariables)
                        batchWriter.WriteLine("set {0}={1}", EscapeForDOS((string)entry.Key), EscapeForDOS((string)entry.Value));

                    if (processInfo.WorkingDirectory != null)
                        batchWriter.WriteLine("cd \"{0}\"", EscapeForDOS(processInfo.WorkingDirectory));

                    batchWriter.WriteLine("\"{0}\" {1}", EscapeForDOS(processInfo.FileName), EscapeForDOS(processInfo.Arguments));
                }

                remoteShadowFile = ShadowCopy(localBatchFile);
            }
            finally
            {
                File.Delete(localBatchFile);
            }

            // Run command via SSH.
            PrivateArguments privateArgs = new PrivateArguments();
            privateArgs.Add("-xa");
            privateArgs.AddQuote(GetIP());
            privateArgs.Add("cmd /c " + EscapeForUnix(remoteShadowFile));
            ProcessInfo sshProcessInfo = new ProcessInfo("ssh", privateArgs);

            ProcessExecutor localProcessExecutor = new ProcessExecutor();
            localProcessExecutor.ProcessOutput += (sender, e) => output(e);
            return localProcessExecutor.Execute(sshProcessInfo);
        }

        public string ShadowCopy(string filePath)
        {
            if (!IsWindows)
                throw new NotImplementedException();

            string source = EscapeForUnix(ToCygwinPath(filePath));
            string dest = GetIP() + ":/tmp/" + EscapeForUnix(Path.GetFileName(filePath));

            PrivateArguments privateArgs = new PrivateArguments();
            privateArgs.AddQuote(source);
            privateArgs.AddQuote(dest);
            ProcessInfo scpProcessInfo = new ProcessInfo("scp");

            ProcessExecutor localProcessExecutor = new ProcessExecutor();
            ProcessResult result = localProcessExecutor.Execute(scpProcessInfo);
            CheckProcessResult(result, string.Format("SCP from '{0}' to '{1}' failed.", source, dest));

            // FIXME: bad assumption about location of Cygwin /tmp
            return Path.Combine(@"C:\cygwin\tmp", Path.GetFileName(filePath));
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

        public void PowerOff()
        {
            Log.Info("Powering off VM '{0}'.", vm);
            PowerOffRequest request = new PowerOffRequest() { Vm = vm };
            GetClient().PowerOff(request);
        }

        public void Start()
        {
            Log.Info("Starting VM '{0}' snapshot '{1}'.", vm, snapshot);
            StartRequest request = new StartRequest() { Vm = vm, Snapshot = snapshot };
            GetClient().Start(request);
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

        private static string EscapeForDOS(string str)
        {
            return str.Replace("^", "^^")
                .Replace("%", "^%")
                .Replace("(", "^(")
                .Replace(")", "^)")
                .Replace("\"", "^\"")
                .Replace("&", "^&");
        }

        private static string EscapeForUnix(string str)
        {
            return str.Replace(@"\", @"\\")
                .Replace(" ", @"\ ");
        }

        private static string ToCygwinPath(string str)
        {
            if (Path.IsPathRooted(str))
                return "/cygdrive/" + str.Replace(":", "/").Replace(@"\", "/");

            return str.Replace(@"\", "/");
        }

        private static void CheckProcessResult(ProcessResult result, string errorMessage)
        {
            if (result.Failed)
                throw new RemoteContextException(errorMessage);
        }
    }
}
