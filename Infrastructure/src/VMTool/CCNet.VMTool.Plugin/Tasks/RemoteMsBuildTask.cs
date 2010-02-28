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
    [ReflectorType("remote-msbuild")]
    public class RemoteMsBuildTask : MsBuildTask
    {
        public RemoteMsBuildTask() :
            base(new RemoteProcessExecutor(),
                new RemoteExecutionEnvironment(),
                new RemoteShadowCopier())
        {
        }

        protected override ProcessInfo CreateProcessInfo(IIntegrationResult result)
        {
            return RemoteContext.GetRemoteContext().RunWithRemoteResult<ProcessInfo>(base.CreateProcessInfo, result);
        }

        protected override string GetProcessBaseDirectory(IIntegrationResult result)
        {
            return WorkingDirectory ?? RemoteContext.GetRemoteContext().RemoteWorkingDirectory ?? "";
        }
    }
}
