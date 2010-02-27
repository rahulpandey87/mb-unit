using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Tasks;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using VMTool;
using VMTool.Schema;
using Thrift.Transport;
using VMTool.Thrift;
using Thrift.Protocol;
using CCNet.VMTool.Plugin.Core;

namespace CCNet.VMTool.Plugin.Tasks
{
    /// <summary>
    /// Starts a VM snapshot, runs a sequence of (possibly remote) tasks,
    /// then powers off the VM.
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// <!-- CCNet configuration -->
    /// <vm configuration="vm-config-file.xml" profile="Windows 2003 x86">
    ///   <tasks>
    ///     <remote-copy ... />
    ///     <remote-msbuild ... />
    ///     <remote-copy ... />
    ///   </tasks>
    /// </vm>
    /// 
    /// <!-- Config file -->
    /// <configuration>
    ///   <profiles>
    ///     <profile id="Windows 2003 x86">
    ///       <host>192.168.128.2</host>
    ///       <port>3831</port>
    ///       <vm>Gallio Build Agent - Windows 2003 x86, Everything</vm>
    ///       <snapshot>Revision 1</snapshot>
    ///     </profile>
    ///   </profiles>
    /// </configuration>
    /// ]]>
    /// </example>
    [ReflectorType("vm")]
    public class VMTask : TaskBase
    {
        /// <summary>
        /// The path of a configuration file in the workspace.
        /// </summary>
        [ReflectorProperty("configuration", Required = true)]
        public string Configuration { get; set; }

        /// <summary>
        /// The profile id of the VM to manipulate.
        /// </summary>
        [ReflectorProperty("profile", Required = true)]
        public string Profile { get; set; }

        /// <summary>
        /// Tasks to perform while the VM is running.  May include both local and remote tasks.
        /// </summary>
        [ReflectorProperty("tasks")]
        public ITask[] Tasks { get; set; }

        protected override bool Execute(IIntegrationResult result)
        {
            result.BuildProgressInformation.SignalStartRunTask(
                string.Format("Starting VM profile '{0}' declared in configuration file '{1}'.",
                Profile, Configuration));

            string configurationFullPath = result.BaseFromWorkingDirectory(Configuration);

            Configuration configuration = ConfigurationFileHelper.LoadConfiguration(configurationFullPath);
            Profile profile = configuration.GetProfileById(Profile);
            if (profile == null)
                throw new BuilderException(this, string.Format("Did not find profile '{0}' in configuration file '{1}'.", Profile, configurationFullPath));

            using (RemoteContext remoteContext = new RemoteContext(
                new CCNetController(profile.Host, profile.Port, profile.VM, profile.Snapshot)))
            {
                Status status = remoteContext.Controller.GetStatus();
                if (status != Status.OFF)
                    remoteContext.Controller.PowerOff();

                remoteContext.Controller.Start();

                try
                {
                    foreach (ITask task in Tasks)
                    {
                        IIntegrationResult subResult = result.Clone();
                        task.Run(subResult);
                        result.Merge(subResult);
                    }
                }
                finally
                {
                    remoteContext.Controller.PowerOff();
                }
            }

            return true;
        }
    }
}
