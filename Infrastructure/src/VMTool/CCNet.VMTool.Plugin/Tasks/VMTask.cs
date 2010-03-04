using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Tasks;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using VMTool;
using VMTool.Schema;
using VMTool.Thrift;
using VMTool.Core;
using Thrift.Protocol;
using Thrift.Transport;
using CCNet.VMTool.Plugin.Core;
using ThoughtWorks.CruiseControl.Remote;

namespace CCNet.VMTool.Plugin.Tasks
{
    [ReflectorType("vm")]
    public class VMTask : TaskBase
    {
		public enum StartAction
		{
			None,
			Auto,
			StartOrResume,
			Restart
		}
		
		public enum StopAction
		{
			None,
			Auto,
			SaveState,
			Pause,
			Shutdown,
			PowerOff
		}
	
        public VMTask()
        {
            ConnectionTimeout = Constants.DefaultConnectionTimeoutSeconds;
			ConfiguredStartAction = StartAction.Auto;
			ConfiguredStopAction = StopAction.Auto;
        }

        [ReflectorProperty("configuration", Required = true)]
        public string Configuration { get; set; }

        [ReflectorProperty("profile", Required = true)]
        public string Profile { get; set; }

        [ReflectorProperty("tasks", Required = true)]
        public ITask[] Tasks { get; set; }

        [ReflectorProperty("connectionTimeout", Required = false)]
        public int ConnectionTimeout { get; set; }

        [ReflectorProperty("startAction", Required = false)]
        public StartAction ConfiguredStartAction { get; set; }

        [ReflectorProperty("stopAction", Required = false)]
        public StopAction ConfiguredStopAction { get; set; }

        [ReflectorProperty("remoteArtifactDirectory", Required = false)]
        public string RemoteArtifactDirectory { get; set; }

        [ReflectorProperty("remoteWorkingDirectory", Required = false)]
        public string RemoteWorkingDirectory { get; set; }

        protected override bool Execute(IIntegrationResult result)
        {
            result.BuildProgressInformation.SignalStartRunTask(
                string.Format("Running virtual machine profile '{0}' declared in configuration file '{1}'.",
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
                    remoteContext.RemoteArtifactDirectory = RemoteArtifactDirectory;
                    remoteContext.RemoteWorkingDirectory = RemoteWorkingDirectory;

                    Status status = controller.GetStatus();

					switch (ConfiguredStartAction)
					{
						case StartAction.Auto:
							if (xmlProfile.Snapshot != null)
								Restart(controller, status);
							else
								StartOrResume(controller, status);
							break;
						case StartAction.StartOrResume:
							StartOrResume(controller, status);
							break;
						case StartAction.Restart:
							Restart(controller, status);
							break;
					}

                    try
                    {
                        foreach (ITask task in Tasks)
                        {
                            IIntegrationResult subResult = result.Clone();
                            try
                            {
                                task.Run(subResult);
                            }
                            finally
                            {
                                result.Merge(subResult);
                            }
							
							if (result.Status != IntegrationStatus.Success)
								break;
                        }
                    }
                    finally
                    {
						switch (ConfiguredStopAction)
						{
							case StopAction.Auto:
								if (xmlProfile.Snapshot != null)
									controller.PowerOff();
								else
									controller.SaveState();
								break;
							case StopAction.SaveState:
								controller.SaveState();
								break;
							case StopAction.Pause:
								controller.Pause();
								break;
							case StopAction.Shutdown:
								controller.Shutdown();
								break;
							case StopAction.PowerOff:
								controller.PowerOff();
								break;
						}
                    }
                }
            }

            return true;
        }
		
		private static void Restart(Controller controller, Status status)
		{
			if (status == Status.RUNNING || status == Status.PAUSED)
				controller.PowerOff();
			controller.Start();
		}
		
		private static void StartOrResume(Controller controller, Status status)
		{
			if (status == Status.PAUSED)
				controller.Resume();
			else if (status == Status.OFF || status == Status.SAVED)
				controller.Start();
		}
    }
}
