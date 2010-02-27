using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace VMTool.Client
{
    public class ClientController : Controller
    {
        public ClientController(string host, int port, string vm, string snapshot)
            : base(host, port, vm, snapshot)
        {
        }

        public bool Quiet { get; set; }

        protected override void Log(string message)
        {
            if (! Quiet)
                Console.Out.WriteLine(message);
        }
    }
}
