using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VMTool.Core
{
    public class Profile
    {
        public Profile()
        {
            Id = "unnamed";
            MasterPort = Constants.DefaultMasterPort;
            SlavePort = Constants.DefaultSlavePort;
        }

        public string Id { get; set; }
        public string Master { get; set; }
        public int MasterPort { get; set; }
        public string Slave { get; set; }
        public int SlavePort { get; set; }
        public string VM { get; set; }
        public string Snapshot { get; set; }
    }
}
