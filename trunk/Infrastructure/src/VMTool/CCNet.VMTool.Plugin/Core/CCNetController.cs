using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMTool;
using ThoughtWorks.CruiseControl.Core.Util;
using System.Collections.Specialized;
using System.Collections;
using VMTool.Core;
using System.Diagnostics;

namespace CCNet.VMTool.Plugin.Core
{
    public class CCNetController : Controller
    {
        public delegate void OnProcessOutput(ProcessOutputEventArgs e);

        private ProcessResult lastProcessResult;

        public CCNetController(Profile profile)
            : base(profile)
        {
        }

        protected override void Log(string message)
        {
            ThoughtWorks.CruiseControl.Core.Util.Log.Info(message);
        }

        public ProcessResult RemoteExecute(ProcessInfo processInfo, OnProcessOutput output)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

			var filteredEnvironmentVariables = new Dictionary<string, string>();
			foreach (DictionaryEntry entry in processInfo.EnvironmentVariables)
			{
                string name = (string)entry.Key;
                string value = (string)entry.Value;

                if (startInfo.EnvironmentVariables[name] == value)
                    continue; // skip locally defined variables

                filteredEnvironmentVariables.Add(name, value);
			}
		
            base.RemoteExecute(processInfo.FileName, processInfo.Arguments, processInfo.WorkingDirectory,
                filteredEnvironmentVariables,
                line => output(new ProcessOutputEventArgs(ProcessOutputType.StandardOutput, line)),
                line => output(new ProcessOutputEventArgs(ProcessOutputType.ErrorOutput, line)));

            return lastProcessResult;
        }

        protected override int InternalExecute(string executable, string arguments, string workingDirectory,
            IDictionary<string, string> environmentVariables, LineHandler stdoutHandler, LineHandler stderrHandler)
        {
            PrivateArguments privateArgs = new PrivateArguments(arguments);
            ProcessInfo processInfo = new ProcessInfo(executable, privateArgs, workingDirectory);

            if (environmentVariables != null)
            {
                foreach (KeyValuePair<string, string> entry in environmentVariables)
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
    }
}
