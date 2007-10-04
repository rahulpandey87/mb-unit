// Copyright 2007 MbUnit Project - http://www.mbunit.com/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Diagnostics;
using MbUnit.Framework;
using MbUnit.Framework.Kernel.ExecutionLogs;

namespace MbUnit.Core.Harness
{
    /// <summary>
    /// A contextual log stream log trace listener writes trace messages to a stream
    /// in the execution log associated with the current test execution context at the time
    /// the message was written.
    /// </summary>
    public sealed class ContextualLogStreamTraceListener : TraceListener
    {
        private readonly string streamName;

        /// <summary>
        /// Creates a trace listener that writes to the specified execution log stream.
        /// </summary>
        /// <param name="streamName">The execution log stream name</param>
        public ContextualLogStreamTraceListener(string streamName)
        {
            this.streamName = streamName;
        }

        /// <inheritdoc />
        public override void Write(string message)
        {
            WriteIndentIfNeeded();

            CurrentLogStreamWriter.Write(message);
        }

        /// <inheritdoc />
        public override void WriteLine(string message)
        {
            WriteIndentIfNeeded();

            CurrentLogStreamWriter.WriteLine(message);
        }

        /// <inheritdoc />
        public override bool IsThreadSafe
        {
            get { return true; }
        }

        private LogStreamWriter CurrentLogStreamWriter
        {
            get { return Log.Writer[streamName]; }
        }

        private void WriteIndentIfNeeded()
        {
            if (NeedIndent)
                WriteIndent();
        }
    }
}