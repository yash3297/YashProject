using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class Pull_LocationEHRUPdateForVersionBO
    {
        public string message { get; set; }
        public GetLocEHRUPdateForVer data { get; set; }

    }

   public class GetLocEHRUPdateForVer
   {
       public string location_id { get; set; }
       public string org_id { get; set; }
       public string appointmentlocation_id { get; set; }
       public string location_name { get; set; }
       public string org_name { get; set; }
       public string ehr_name { get; set; }
       public string ehr_version { get; set; }
       public Int32 is_auto_update { get; set; }
       public string last_updated { get; set; }
       public string server_app_version { get; set; }
   }
}
