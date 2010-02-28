using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Thrift.Server;
using Thrift.Transport;
using VMTool.Thrift;

namespace VMTool.Slave
{
    public class SlaveServer
    {
        private readonly static ILog log = LogManager.GetLogger(typeof(SlaveServer));

        private SlaveOptions options;

        private readonly object syncRoot = new object();
        private bool stopped;
        private TServer server;

        public SlaveServer(SlaveOptions options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            this.options = options;
        }

        public void Run()
        {
            log.InfoFormat("Starting slave service on port {0}.", options.Port);

            TServerTransport transport = null;
            try
            {
                lock (syncRoot)
                {
                    if (stopped)
                        return;

                    var handler = new SlaveServiceHandler();
                    var processor = new VMToolSlaveCustom.Processor(handler);
                    transport = new TServerSocket(options.Port);
                    server = new TThreadPoolServer(processor, transport, message => log.Error(message));
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

        public void Stop()
        {
            lock (syncRoot)
            {
                stopped = true;

                if (server != null)
                    server.Stop();
            }
        }
    }
}
