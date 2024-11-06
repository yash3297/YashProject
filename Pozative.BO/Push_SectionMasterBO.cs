using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class Push_SectionMasterBO
    {
        public string appointmentlocation { get; set; }
        public string name { get; set; }
        public string organization { get; set; }
        public bool show_border { get; set; }
        public string formmaster_ehr_id { get; set; }
        public string sectionmaster_ehr_id { get; set; }
        public string sectionmaster_Web_Id { get; set; }
        public string sectionmaster_localdb_id { get; set; }
        public bool show_title { get; set; }
        public string location { get; set; }
        public bool is_deleted { get; set; }
        public int section_order { get; set; } 
    }
}
