using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Collections;
using System.Threading;

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

			var stdoutDoneEvent = new ManualResetEvent(false);
			var stderrDoneEvent = new ManualResetEvent(false);
			
            Process process = Process.Start(startInfo);
            process.OutputDataReceived += (sender, e) => HandleLine(e.Data, stdoutHandler, stdoutDoneEvent);
            process.ErrorDataReceived += (sender, e) => HandleLine(e.Data, stderrHandler, stderrDoneEvent);
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
			
			// Wait for all of the data to be read.
			stdoutDoneEvent.WaitOne(500);
			stderrDoneEvent.WaitOne(500);
            return process.ExitCode;
        }
		
		private static void HandleLine(string line, LineHandler handler, ManualResetEvent doneEvent)
		{
			if (line == null)
			{
				doneEvent.Set();
			}
			else			
			{
				handler(line);
			}
		}
    }
}
