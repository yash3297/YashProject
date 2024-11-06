using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{   
    public class Response_EaglesoftMedicalHistory
    {
        public string message { get; set; }
        public List<GetListEaglesoftMedicalHistory> data { get; set; }

    }

    public class GetListEaglesoftMedicalHistory
    {
        public string sectionmaster_ehr_id { get; set; }
        public string formmaster_ehr_id { get; set; }
        public string alertmaster_ehr_id { get; set; }
        public string sectionitemmaster_ehr_id { get; set; }
        public string sectionitemoptionmaster_ehr_id { get; set; }     
        public string _id { get; set; }
        public string dentrix_form_ehrunique_id { get; set; }
        public string dentrix_formquestion_ehrunique_id { get; set; }
        public string abeldent_form_ehrunique_id { get; set; }
        public string abeldent_formquestion_ehrunique_id { get; set; }
        public string sheetdefnum_ehr_id { get; set; }
        public string sheetfielddefnum_ehr_id { get; set; }
        public string cd_formmaster_ehr_id { get; set; }
        public string cd_questionmaster_ehr_id { get; set; }
        public string easydental_questionid { get; set; }
        public string easydental_formmasterid { get; set; }
    }
}
