using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class AditServer_ConnectioBO
    {
        public string message { get; set; }
        public string error { get; set; }
        public ConnectioBO data { get; set; }
    }

    public class ConnectioBO
    {
        public string user { get; set; }
        public string password { get; set; }
        public string server { get; set; }
        public string database { get; set; }
    }

}
