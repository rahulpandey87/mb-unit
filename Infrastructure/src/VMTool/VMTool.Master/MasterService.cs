using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using VMTool.Thrift;
using Thrift.Transport;
using Thrift.Server;
using log4net;
using System.Threading;

namespace VMTool.Master
{
    public partial class MasterService : ServiceBase
    {
        private readonly MasterServer server;

        public MasterService(MasterServer server)
        {
            if (server == null)
                throw new ArgumentNullException("server");

            this.server = server;

            ServiceName = "VMTool Master";
        }

        protected override void OnStart(string[] args)
        {
            new Thread(() => server.Run()).Start();
        }

        protected override void OnStop()
        {
            server.Stop();
        }
    }
}
