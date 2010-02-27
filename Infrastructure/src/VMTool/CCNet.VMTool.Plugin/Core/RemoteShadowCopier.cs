using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Util;

namespace CCNet.VMTool.Plugin.Core
{
    public class RemoteShadowCopier : IShadowCopier
    {
        public string RetrieveFilePath(string fileName)
        {
            var localShadowCopier = new DefaultShadowCopier();
            string localFilePath = localShadowCopier.RetrieveFilePath(fileName);
            return RemoteContext.GetRemoteContext().ShadowCopy(localFilePath);
        }
    }
}
