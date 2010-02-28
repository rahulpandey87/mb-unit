using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Collections;

namespace VMTool.Core
{
    public static class ProcessUtil
    {
        public static int Execute(string executable, string arguments, string workingDirectory,
            IDictionary<string, string> environmentVariables,
            LineHandler stdoutHandler, LineHandler stderrHandler,
            TimeSpan? timeout)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(executable)
            {
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            if (workingDirectory != null)
            {
                startInfo.WorkingDirectory = workingDirectory;
            }

            if (environmentVariables != null)
            {
                foreach (KeyValuePair<string, string> entry in environmentVariables)
                    startInfo.EnvironmentVariables.Add(entry.Key, entry.Value);
            }

            Process process = Process.Start(startInfo);
            process.OutputDataReceived += (sender, e) => { if (e.Data != null) stdoutHandler(e.Data); };
            process.ErrorDataReceived += (sender, e) => { if (e.Data != null) stderrHandler(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            if (timeout.HasValue)
            {
                if (!process.WaitForExit((int)timeout.Value.TotalMilliseconds))
                {
                    process.CancelOutputRead();
                    process.CancelErrorRead();

                    try
                    {
                        process.Kill();
                    }
                    catch (Exception)
                    {
                    }

                    throw new TimeoutException(string.Format("Timed out process execution after {0} seconds.", timeout.Value.TotalSeconds));
                }
            }
            else
            {
                process.WaitForExit();
            }

            return process.ExitCode;
        }
    }
}
