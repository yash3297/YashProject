using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_AlertMasterBO
    {
        public string appointmentlocation { get; set; }
        public string name { get; set; }
        public string organization { get; set; }        
        
        public string alertmaster_ehr_id { get; set; }
        public string alertmaster_Web_Id { get; set; }
        public string alertmaster_localdb_id { get; set; }        
        public string location { get; set; }
        public bool is_deleted { get; set; }        
    }
}
