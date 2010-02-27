using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMTool;
using ThoughtWorks.CruiseControl.Core.Util;
using System.Collections.Specialized;
using System.Collections;

namespace CCNet.VMTool.Plugin.Core
{
    public class CCNetController : Controller
    {
        public delegate void OnProcessOutput(ProcessOutputEventArgs e);

        private ProcessResult lastProcessResult;

        public CCNetController(string host, int port, string vm, string snapshot)
            : base(host, port, vm, snapshot)
        {
        }

        protected override void Log(string message)
        {
            ThoughtWorks.CruiseControl.Core.Util.Log.Info(message);
        }

        public override int LocalExecute(string executable, string arguments, string workingDirectory,
            StringDictionary environmentVariables, LineHandler stdoutHandler, LineHandler stderrHandler)
        {
            LogExecute("Executing local command: ", executable, arguments, workingDirectory, environmentVariables);

            PrivateArguments privateArgs = new PrivateArguments(arguments);
            ProcessInfo processInfo = new ProcessInfo(executable, privateArgs, workingDirectory);

            if (environmentVariables != null)
            {
                foreach (DictionaryEntry entry in environmentVariables)
                    processInfo.EnvironmentVariables.Add((string)entry.Key, (string)entry.Value);
            }

            ProcessExecutor localProcessExecutor = new ProcessExecutor();
            localProcessExecutor.ProcessOutput += (sender, e) =>
                {
                    if (e.OutputType == ProcessOutputType.StandardOutput)
                        stdoutHandler(e.Data);
                    else
                        stderrHandler(e.Data);
                };
            lastProcessResult = localProcessExecutor.Execute(processInfo);
            return lastProcessResult.ExitCode;
        }

        public ProcessResult RemoteExecute(ProcessInfo processInfo, OnProcessOutput output)
        {
            base.RemoteExecute(processInfo.FileName, processInfo.Arguments, processInfo.WorkingDirectory,
                processInfo.EnvironmentVariables,
                line => output(new ProcessOutputEventArgs(ProcessOutputType.StandardOutput, line)),
                line => output(new ProcessOutputEventArgs(ProcessOutputType.ErrorOutput, line)));

            return lastProcessResult;
        }
    }
}
