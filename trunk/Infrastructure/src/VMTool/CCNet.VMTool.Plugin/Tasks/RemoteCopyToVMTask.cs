using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core.Tasks;
using ThoughtWorks.CruiseControl.Core;
using CCNet.VMTool.Plugin.Core;

namespace CCNet.VMTool.Plugin.Tasks
{
    [ReflectorType("remote-copy-to-vm")]
    public class RemoteCopyToVMTask : TaskBase
    {
        [ReflectorProperty("source", Required = true)]
        public string Source { get; set; }

        [ReflectorProperty("dest", Required = true)]
        public string Destination { get; set; }

        [ReflectorProperty("recursive")]
        public bool Recursive { get; set; }

        [ReflectorProperty("force")]
        public bool Force { get; set; }

        protected override bool Execute(IIntegrationResult result)
        {
            result.BuildProgressInformation.SignalStartRunTask(
                string.Format("Copying files from local path '{0}' to vm path '{1}'.",
                Source, Destination));

            RemoteContext.GetRemoteContext().Controller.CopyToVM(Source, Destination, Recursive, Force);

            return true;
        }
    }
}
