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

            StringBuilder stdoutBuilder = new StringBuilder();
            StringBuilder stderrBuilder = new StringBuilder();

            int exitCode = Execute(processInfo.FileName,
                processInfo.Arguments,
                string.IsNullOrEmpty(processInfo.WorkingDirectory) ? null : processInfo.WorkingDirectory,
                filteredEnvironmentVariables,
                line =>
                    {
                        stdoutBuilder.AppendLine(line);
                        output(new ProcessOutputEventArgs(ProcessOutputType.StandardOutput, line));
                    },
                line =>
                    {
                        stderrBuilder.AppendLine(line);
                        output(new ProcessOutputEventArgs(ProcessOutputType.ErrorOutput, line));
                    },
                TimeSpan.FromMilliseconds(processInfo.TimeOut));

            return new ProcessResult(
                stdoutBuilder.ToString(),
                stderrBuilder.ToString(),
                exitCode,
                exitCode == -1,
                ! processInfo.ProcessSuccessful(exitCode));
        }
    }
}
