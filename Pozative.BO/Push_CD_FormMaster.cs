using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_CD_FormMaster
    {
        public string appointmentlocation { get; set; }
        public string organization { get; set; }
        public string location { get; set; }
        public string name { get; set; }
        public string cd_formmaster_localdb_id { get; set; }
        public string cd_formmaster_ehr_id { get; set; }      
        public bool is_deleted { get; set; }
       
    }
    public class Push_CD_QuestionMaster
    {
        public string appointmentlocation { get; set; }
        public string organization { get; set; }
        public string location { get; set; }
        public string question_description { get; set; }
        public int question_sequence { get; set; }
        public bool question_warnings { get; set; }
        public int question_type { get; set; }
        public string comment { get; set; }
        public string answer_value { get; set; }
        public string cd_formmaster_ehr_id { get; set; }
        public string cd_questionmaster_ehr_id { get; set; }
        public string cd_questionmaster_localdb_id { get; set; }      
        public bool is_deleted { get; set; }
        public bool is_active { get; set; }
        public string cd_form_name { get; set; }
       
    }
    
}
