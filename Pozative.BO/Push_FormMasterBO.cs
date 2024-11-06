using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class Push_FormMasterBO
    {
       public string appointmentlocation { get; set; }
        public string name { get; set; }
        public string organization { get; set; }
        public string location { get; set; }
        public bool is_default { get; set; }
        public string Formmaster_Web_ID { get; set; }
        public string formmaster_localdb_id { get; set; }
        public string formmaster_ehr_id { get; set; }
        public string form_type_id { get; set; }
        public bool is_deleted { get; set; }
        public bool is_active { get; set; }        
    }
}
