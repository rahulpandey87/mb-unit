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

namespace VMTool.Core
{
    public class Controller : IDisposable
    {
        private readonly Profile profile;

        private VMToolMaster.Client masterClientLazy;
        private VMToolSlaveCustom.Client slaveClientLazy;
        private string ipLazy;
        private TimeSpan connectionTimeout;

        public Controller(Profile profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            this.profile = profile;
            connectionTimeout = TimeSpan.FromSeconds(Constants.DefaultConnectionTimeoutSeconds);
        }

        public void Dispose()
        {
            if (slaveClientLazy != null)
            {
                slaveClientLazy.InputProtocol.Transport.Close();
                slaveClientLazy = null;
            }

            if (masterClientLazy != null)
            {
                masterClientLazy.InputProtocol.Transport.Close();
                masterClientLazy = null;
            }
        }

        public Profile Profile
        {
            get { return profile; }
        }

        public TimeSpan ConnectionTimeout
        {
            get { return connectionTimeout; }
            set { connectionTimeout = value; }
        }

        public bool IsWindows
        {
            get { return true; }
        }

        public void Start()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            if (profile.Snapshot != null)
            {
                Log(string.Format("Starting VM '{0}' snapshot '{1}'.", profile.VM, profile.Snapshot));
                StartRequest request = new StartRequest() { Vm = profile.VM, Snapshot = profile.Snapshot };
                GetMasterClient().Start(request);
            }
            else
            {
                Log(string.Format("Starting VM '{0}'.", profile.VM));
                StartRequest request = new StartRequest() { Vm = profile.VM };
                GetMasterClient().Start(request);
            }
        }

        public void PowerOff()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            Log(string.Format("Powering off VM '{0}'.", profile.VM));
            PowerOffRequest request = new PowerOffRequest() { Vm = profile.VM };
            GetMasterClient().PowerOff(request);
        }

        public void Shutdown()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            Log(string.Format("Shutting down VM '{0}'.", profile.VM));
            ShutdownRequest request = new ShutdownRequest() { Vm = profile.VM };
            GetMasterClient().Shutdown(request);
        }

        public void Pause()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            Log(string.Format("Pausing VM '{0}'.", profile.VM));
            PauseRequest request = new PauseRequest() { Vm = profile.VM };
            GetMasterClient().Pause(request);
        }

        public void Resume()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            Log(string.Format("Resuming VM '{0}'.", profile.VM));
            ResumeRequest request = new ResumeRequest() { Vm = profile.VM };
            GetMasterClient().Resume(request);
        }

        public void SaveState()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            Log(string.Format("Saving state and stopping VM '{0}'.", profile.VM));
            SaveStateRequest request = new SaveStateRequest() { Vm = profile.VM };
            GetMasterClient().SaveState(request);
        }

        public void TakeSnapshot(string newSnapshotName)
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            Log(string.Format("Taking snapshot of VM '{0}' called '{1}'.", profile.VM, newSnapshotName));
            TakeSnapshotRequest request = new TakeSnapshotRequest() { Vm = profile.VM, SnapshotName = newSnapshotName };
            GetMasterClient().TakeSnapshot(request);
        }

        public string GetIP()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            if (ipLazy == null)
            {
                Log(string.Format("Getting IP address of VM '{0}' primary network interface.", profile.VM));

                GetIPRequest request = new GetIPRequest() { Vm = profile.VM };
                GetIPResponse response = GetMasterClient().GetIP(request);
                ipLazy = response.Ip;
            }

            return ipLazy;
        }

        public Status GetStatus()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            Log(string.Format("Getting status of VM '{0}'.", profile.VM));

            GetStatusRequest request = new GetStatusRequest() { Vm = profile.VM };
            GetStatusResponse response = GetMasterClient().GetStatus(request);
            return response.Status;
        }

        public int Execute(string executable, string arguments, string workingDirectory,
            IDictionary<string, string> environmentVariables,
            LineHandler stdoutHandler, LineHandler stderrHandler, TimeSpan? timeout)
        {
            CheckProfileCanResolveSlave();

            StringBuilder message = new StringBuilder();
            message.AppendLine("Executing remote command: ");
            message.AppendFormat("  Executable  : {0}\n", executable);
            message.AppendFormat("  Arguments   : {0}\n", arguments);

            if (workingDirectory != null)
                message.AppendFormat("  Directory   : {0}\n", workingDirectory);

            if (environmentVariables != null)
            {
                message.AppendLine("  Environment :");
                foreach (KeyValuePair<string, string> entry in environmentVariables)
                    message.AppendFormat("    {0}={1}\n", entry.Key, entry.Value);
            }

            Log(message.ToString());

            ExecuteRequest request = new ExecuteRequest();
            request.Executable = executable;
            if (arguments.Length != 0)
                request.Arguments = arguments;
            if (workingDirectory != null)
                request.WorkingDirectory = workingDirectory;
            if (environmentVariables != null)
                request.EnvironmentVariables = new Dictionary<string, string>(environmentVariables);
            if (timeout.HasValue)
                request.Timeout = (int) timeout.Value.TotalSeconds;

            ExecuteResponse response = GetSlaveClient().Execute(request,
                stream =>
                {
                    if (stream.__isset.stdoutLine)
                        stdoutHandler(stream.StdoutLine);
                    if (stream.__isset.stderrLine)
                        stderrHandler(stream.StderrLine);
                });
            return response.ExitCode;
        }

        public string ShadowCopy(string localFilePath)
        {
            CheckProfileCanResolveSlave();

            // FIXME: bad assumption about location of temporary files directory.
            string remoteFilePath = Path.Combine(@"C:\Temp", "VMToolShadow-" + Path.GetFileName(localFilePath));
            CopyToVM(localFilePath, remoteFilePath, false, true);
            return remoteFilePath;
        }

        public void CopyToVM(string localFilePathGlob, string remoteFilePath, bool recursive, bool force)
        {
            CheckProfileCanResolveSlave();

            Log(string.Format("Copying from local path '{0}' to remote path '{1}'{2}{3}.", localFilePathGlob, remoteFilePath,
                recursive ? " recursively" : "",
                force ? " force overwrite" : ""));

            var client = GetSlaveClient();

            bool foundFile = FileUtil.TraverseFiles(localFilePathGlob, recursive,
                (file, relativePath) =>
                {
                    Log(string.Format("Copying '{0}'.", relativePath));

                    byte[] contents = File.ReadAllBytes(file.FullName);

                    WriteFileRequest request = new WriteFileRequest()
                    {
                        Contents = contents,
                        Path = Path.Combine(remoteFilePath, relativePath),
                        Overwrite = force
                    };
                    client.WriteFile(request);
                },
                (directory, relativePath) =>
                {
                    Log(string.Format("Creating directory '{0}'.", relativePath));

                    CreateDirectoryRequest request = new CreateDirectoryRequest()
                    {
                        Path = Path.Combine(remoteFilePath, relativePath)                        
                    };
                    client.CreateDirectory(request);
                });

            if (!foundFile && ! FileUtil.HasWildcard(localFilePathGlob))
                throw new OperationFailedException() {
                    Why = string.Format("Local file{0} not found '{1}'.", recursive ? " or directory" : "", localFilePathGlob) };
        }

        public void CopyFromVM(string remoteFilePathGlob, string localFilePath, bool recursive, bool force)
        {
            CheckProfileCanResolveSlave();

            Log(string.Format("Copying from remote path '{0}' to local path '{1}'{2}{3}.", localFilePath, remoteFilePathGlob,
                recursive ? " recursively" : "",
                force ? " force overwrite" : ""));

            var client = GetSlaveClient();

            EnumerateRequest enumerateRequest = new EnumerateRequest()
            {
                PathGlob = remoteFilePathGlob,
                Recursive = recursive
            };
            EnumerateResponse enumerateResponse = client.Enumerate(enumerateRequest);

            if (enumerateResponse.Items.Count == 0 && !FileUtil.HasWildcard(remoteFilePathGlob))
                throw new OperationFailedException() {
                    Why = string.Format("Remote file{0} not found '{1}'.", recursive ? " or directory" : "", remoteFilePathGlob)
                };

            foreach (EnumerateItem item in enumerateResponse.Items)
            {
                string path = Path.Combine(localFilePath, item.RelativePath);

                if (item.Kind == EnumerateItemKind.DIRECTORY)
                {
                    if (!Directory.Exists(path))
                    {
                        Log(string.Format("Creating directory '{0}'.", item.RelativePath));
                        Directory.CreateDirectory(path);
                    }
                }
                else
                {
                    Log(string.Format("Copying '{0}'.", item.RelativePath));

                    ReadFileRequest request = new ReadFileRequest() { Path = item.FullPath };
                    ReadFileResponse response = client.ReadFile(request);
                    if (! force && File.Exists(path))
                        throw new OperationFailedException() {
                            Why = string.Format("Local file already exists '{0}'.", path) };
                    File.WriteAllBytes(path, response.Contents);
                }
            }
        }

        public VMToolMaster.Client GetMasterClient()
        {
            CheckProfileHasMaster();
            CheckProfileHasVM();

            if (masterClientLazy == null)
            {
                if (profile.Master == null)
                    throw new InvalidOperationException("Missing master hostname in profile.");

                Log(string.Format("Connecting to master server '{0}:{1}'.", profile.Master, profile.MasterPort));

                TTransport transport = OpenTransport(profile.Master, profile.MasterPort);
                TProtocol protocol = new TBinaryProtocol(transport);
                masterClientLazy = new VMToolMaster.Client(protocol);
            }

            return masterClientLazy;
        }

        public VMToolSlaveCustom.Client GetSlaveClient()
        {
            CheckProfileCanResolveSlave();

            if (slaveClientLazy == null)
            {
                string slave = profile.Slave != null ? profile.Slave : GetIP();
                Log(string.Format("Connecting to slave server '{0}:{1}'.", slave, profile.SlavePort));

                TTransport transport = OpenTransport(slave, profile.SlavePort);
                TProtocol protocol = new TBinaryProtocol(transport);
                slaveClientLazy = new VMToolSlaveCustom.Client(protocol);
            }

            return slaveClientLazy;
        }

        private TTransport OpenTransport(string hostname, int port)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 1; ; i++)
            {
                try
                {
                    TTransport transport = new TSocket(hostname, port);
                    transport.Open();
                    return transport;
                }
                catch (Exception ex)
                {
                    if (stopwatch.Elapsed > connectionTimeout)
                    {
                        throw new OperationFailedException()
                        {
                            Why = string.Format("Timed out attempting to connect after {0} tries in over {1} seconds.",
                                i, connectionTimeout.TotalSeconds),
                            Details = ex.Message
                        };
                    }
                }
            }
        }

        protected virtual void Log(string message)
        {
        }

        private void CheckProfileHasVM()
        {
            if (profile.VM == null)
                throw new InvalidOperationException("Profile must specify the virtual machine.");
        }

        private void CheckProfileHasMaster()
        {
            if (profile.Master == null)
                throw new InvalidOperationException("Profile must specify the master hostname.");
        }

        private void CheckProfileCanResolveSlave()
        {
            if ((profile.Master == null || profile.VM == null) && profile.Slave == null)
                throw new InvalidOperationException("Profile must specify the master hostname and virtual machine or the slave hostname.");
        }
    }
}
