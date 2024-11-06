using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class UpdateLocationEHRUPdateForVersionBO
    {
        public string appointmentlocation_id { get; set; }
        public Int32 is_auto_update { get; set; }
        public string last_updated { get; set; }
        public string server_app_version { get; set; }
        public string system_name { get; set; }
        public string operating_system { get; set; }
        public string processor_name { get; set; }
        public string service_pack { get; set; }
        public string total_ram { get; set; }
        public string available_ram { get; set; }
        public string total_hdisk { get; set; }
        public string available_hdisk { get; set; }
        public string dotnetframework { get; set; }
        public string system_type { get; set; }
    }
}
