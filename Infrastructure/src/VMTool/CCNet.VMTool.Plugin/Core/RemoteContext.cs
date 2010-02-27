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

        private CCNetController controller;

        public RemoteContext(CCNetController controller)
        {
            this.controller = controller;

            CallContext.SetData(CallContextSlot, this);
        }

        public void Dispose()
        {
            if (controller != null)
            {
                controller.Dispose();
                controller = null;
            }

            CallContext.FreeNamedDataSlot(CallContextSlot);
        }

        public static RemoteContext GetRemoteContext()
        {
            var context = CallContext.GetData(CallContextSlot) as RemoteContext;
            if (context == null)
                throw new RemoteContextException("There is no remote context.  Is the task nested within a <vm> element?");
            return context;
        }

        public CCNetController Controller
        {
            get
            {
                if (controller == null)
                    throw new ObjectDisposedException("The remote context has been disposed.");
                return controller;
            }
        }
    }
}
