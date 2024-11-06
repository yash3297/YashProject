using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class LocationEHRBO
    {

        public string application_name { get; set; }
        public string application_version { get; set; }
        public string system_name { get; set; }
        public string system_mac_address { get; set; }
        public bool is_install_ehr { get; set; }
        public string install_date { get; set; }
        public string ehrmaster { get; set; }
        public string created_by { get; set; }
    }
}
