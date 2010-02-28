using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Util;

namespace CCNet.VMTool.Plugin.Core
{
    public class RemoteShadowCopier : IShadowCopier
    {
		private readonly IShadowCopier localShadowCopier = new DefaultShadowCopier();
	
        public string RetrieveFilePath(string fileName)
        {
            string localFilePath = localShadowCopier.RetrieveFilePath(fileName);
            return RemoteContext.GetRemoteContext().Controller.ShadowCopy(localFilePath);
        }
    }
}
