using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Tasks;
using Exortech.NetReflector;
using CCNet.VMTool.Plugin.Core;

namespace CCNet.VMTool.Plugin.Tasks
{
    [ReflectorType("remote-exec")]
    public class RemoteExecutableTask : ExecutableTask
    {
        public RemoteExecutableTask() :
            base(new RemoteProcessExecutor())
        {
        }
    }
}
