using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Tasks;
using Exortech.NetReflector;
using CCNet.VMTool.Plugin.Core;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;

namespace CCNet.VMTool.Plugin.Tasks
{
    [ReflectorType("remote-exec")]
    public class RemoteExecutableTask : ExecutableTask
    {
        public RemoteExecutableTask() :
            base(new RemoteProcessExecutor())
        {
        }

        protected override ProcessInfo CreateProcessInfo(IIntegrationResult result)
        {
            return RemoteContext.GetRemoteContext().RunWithRemoteResult<ProcessInfo>(base.CreateProcessInfo, result);
        }

        protected override string GetProcessBaseDirectory(IIntegrationResult result)
        {
            return ConfiguredBaseDirectory ?? RemoteContext.GetRemoteContext().RemoteWorkingDirectory  ?? "";
        }
    }
}
