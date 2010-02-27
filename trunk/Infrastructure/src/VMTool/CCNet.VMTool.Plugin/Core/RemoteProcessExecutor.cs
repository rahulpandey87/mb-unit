using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Util;

namespace CCNet.VMTool.Plugin.Core
{
    public class RemoteProcessExecutor : ProcessExecutor
    {
        public override ProcessResult Execute(ProcessInfo processInfo)
        {
            return RemoteContext.GetRemoteContext().Controller.RemoteExecute(processInfo, OnProcessOutput);
        }
    }
}
