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

namespace VMTool.Service
{
    public partial class Service : ServiceBase
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(Service));

        private Options options;

        private readonly object syncRoot = new object();
        private bool stopped;
        private TServer server;

        public Service(Options options)
        {
            this.options = options;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Run();
        }

        protected override void OnStop()
        {
            lock (syncRoot)
            {
                stopped = true;

                if (server != null)
                    server.Stop();
            }
        }

        public void Run()
        {
            log.InfoFormat("Starting service on port {0}.", options.Port);

            TServerTransport transport = null;
            try
            {
                lock (syncRoot)
                {
                    if (stopped)
                        return;

                    var handler = new VMToolServiceHandler();
                    var processor = new VMToolService.Processor(handler);
                    transport = new TServerSocket(options.Port);

                    // Single-threaded server is fine since VirtualBox locks its metadata during commands.
                    server = new TSimpleServer(processor, transport, message => log.Error(message));
                }

                server.Serve();
                server = null;
            }
            finally
            {
                if (server != null)
                    server.Stop();
                if (transport != null)
                    transport.Close();
            }
        }
    }
}
