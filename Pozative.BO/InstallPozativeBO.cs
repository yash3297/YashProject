using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class InstallPozativeBO
    {
        public string message { get; set; }
        public string error { get; set; }
        public installData data { get; set; }
    }

    public class installData
    {
        public string System_processorID { get; set; }
        public string system_name { get; set; }
        public bool is_install_ehr { get; set; }
    }
}
