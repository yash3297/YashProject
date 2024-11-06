using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class LocationEHRUPdateForVersionBO
    {
        public string location_id { get; set; }
        public string org_id { get; set; }
        public string appointmentlocation_id { get; set; }
        public string location_name { get; set; }
        public string org_name { get; set; }
        public string ehr_name { get; set; }
        public string ehr_version { get; set; }
        public Int32  is_auto_update { get; set; }
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

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ClientVersion
    {
        public string appointmentlocation { get; set; }
        public string client_application_version { get; set; }
    }


}
