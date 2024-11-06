using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_SectionItemMasterBO
    {
       public string  appointmentlocation { get; set; }
       public string  name { get; set; }
       public string  organization { get; set; }
       public int  sectionitemtype { get; set; }
       public int  sectionitemorder { get; set; }
       public bool  allowcomment { get; set; }
       public bool  alertonyes { get; set; }
       public bool  alertonno { get; set; }
       public string  alertmaster_ehr_id { get; set; }
       public string  question_type { get; set; }
       public string  answer_type { get; set; }
       public bool  alloweditcomment { get; set; }
       public string  numberofcolumns { get; set; }
       public string  formmaster_ehr_id { get; set; }
       public string  sectionmaster_ehr_id { get; set; }
       public string  sectionitemmaster_ehr_id { get; set; }
       public string  sectionitemmaster_localdb_id { get; set; }
       public string  location { get; set; }
       public bool is_deleted { get; set; }

    }
}
