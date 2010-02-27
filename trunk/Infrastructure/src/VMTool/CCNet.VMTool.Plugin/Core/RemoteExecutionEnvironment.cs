using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Util;

namespace CCNet.VMTool.Plugin.Core
{
    public class RemoteExecutionEnvironment : IExecutionEnvironment
    {
        public char DirectorySeparator
        {
            get { throw new NotSupportedException(); }
        }

        public string EnsurePathIsRooted(string path)
        {
            throw new NotSupportedException();
        }

        public string GetDefaultProgramDataFolder(ApplicationType application)
        {
            throw new NotSupportedException();
        }

        public bool IsRunningOnWindows
        {
            get { return RemoteContext.GetRemoteContext().Controller.IsWindows; }
        }

        public string RuntimeDirectory
        {
            get
            {
                if (RemoteContext.GetRemoteContext().Controller.IsWindows)
                    return @"C:\Windows\Microsoft.Net\Framework\v2.0.50727";
                throw new NotSupportedException();
            }
        }
    }
}
