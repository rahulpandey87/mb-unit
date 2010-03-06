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

            Process process = Process.Start(startInfo);
            process.OutputDataReceived += (sender, e) => HandleLine(e.Data, stdoutHandler);
            process.ErrorDataReceived += (sender, e) => HandleLine(e.Data, stderrHandler);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            if (timeout.HasValue)
            {
                if (!process.WaitForExit((int)timeout.Value.TotalMilliseconds))
                {
                    try
                    {
                        process.Kill();
						process.WaitForExit();
                    }
                    catch (Exception)
                    {
                    }

                    throw new TimeoutException(string.Format("Timed out process execution after {0} seconds.", timeout.Value.TotalSeconds));
                }
            }
			
			// Wait for exit and for all output to be spooled.
			// Calling WaitForExit with a timeout is insufficient.
            process.WaitForExit();
            return process.ExitCode;
        }
		
		private static void HandleLine(string line, LineHandler handler)
		{
			if (line != null)
				handler(line);
		}
    }
}
