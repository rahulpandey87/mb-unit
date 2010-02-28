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
    [ReflectorType("vm")]
    public class VMTask : TaskBase
    {
        public VMTask()
        {
            ConnectionTimeout = Constants.DefaultConnectionTimeoutSeconds;
        }

        [ReflectorProperty("configuration", Required = true)]
        public string Configuration { get; set; }

        [ReflectorProperty("profile", Required = true)]
        public string Profile { get; set; }

        [ReflectorProperty("tasks")]
        public ITask[] Tasks { get; set; }

        [ReflectorProperty("connectionTimeout")]
        public int ConnectionTimeout { get; set; }

        protected override bool Execute(IIntegrationResult result)
        {
            result.BuildProgressInformation.SignalStartRunTask(
                string.Format("Starting VM profile '{0}' declared in configuration file '{1}'.",
                Profile, Configuration));

            string configurationFullPath = result.BaseFromWorkingDirectory(Configuration);

            XmlConfiguration xmlConfiguration = ConfigurationFileHelper.LoadConfiguration(configurationFullPath);
            XmlProfile xmlProfile = xmlConfiguration.GetProfileById(Profile);
            if (xmlProfile == null)
                throw new BuilderException(this, string.Format("Did not find profile '{0}' in configuration file '{1}'.", Profile, configurationFullPath));

            global::VMTool.Core.Profile profile = xmlProfile.ToProfile();

            using (CCNetController controller = new CCNetController(profile))
            {
                controller.ConnectionTimeout = TimeSpan.FromSeconds(ConnectionTimeout);

                using (RemoteContext remoteContext = new RemoteContext(controller))
                {
                    Status status = remoteContext.Controller.GetStatus();

                    if (xmlProfile.Snapshot != null)
                    {
                        if (status == Status.RUNNING || status == Status.PAUSED)
                            remoteContext.Controller.PowerOff();

                        remoteContext.Controller.Start();
                    }
                    else
                    {
                        if (status == Status.PAUSED)
                            remoteContext.Controller.Resume();
                        else if (status == Status.OFF || status == Status.SAVED)
                            remoteContext.Controller.Start();
                    }

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
                        if (xmlProfile.Snapshot != null)
                            remoteContext.Controller.PowerOff();
                        else
                            remoteContext.Controller.SaveState();
                    }
                }
            }

            return true;
        }
    }
}
