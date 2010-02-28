using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using VMTool.Core;

namespace VMTool.Client
{
    public class ClientController : Controller
    {
        public ClientController(Profile profile)
            : base(profile)
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
