using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core.Util;
using Thrift.Transport;
using Thrift.Protocol;
using VMTool.Thrift;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;
using System.Collections;
using System.IO;
using VMTool;

namespace CCNet.VMTool.Plugin.Core
{
    public class RemoteContext : IDisposable
    {
        private const string CallContextSlot = "VMTool.RemoteContext";

        private readonly CCNetController controller;

        public RemoteContext(CCNetController controller)
        {
            if (controller == null)
                throw new ArgumentNullException("controller");

            this.controller = controller;

            CallContext.SetData(CallContextSlot, this);
        }

        public void Dispose()
        {
            CallContext.FreeNamedDataSlot(CallContextSlot);
        }

        public static RemoteContext GetRemoteContext()
        {
            RemoteContext context = GetRemoteContextOrNullIfNone();
            if (context == null)
                throw new RemoteContextException("There is no remote context.  Is the task nested within a <vm> element?");
            return context;
        }
		
		public static bool HasRemoteContext()
		{
			return GetRemoteContextOrNullIfNone() != null;
		}
		
		private static RemoteContext GetRemoteContextOrNullIfNone()
		{
			return CallContext.GetData(CallContextSlot) as RemoteContext;
		}

        public CCNetController Controller
        {
            get { return controller; }
        }
    }
}
