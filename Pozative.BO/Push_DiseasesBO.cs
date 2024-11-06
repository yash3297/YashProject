using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_DiseasesBO
    {
        public string name { get; set; }
        public string disease_type { get; set; }
        public bool is_deleted { get; set; }
        public string disease_web_id { get; set; }
        public string disease_localdb_id { get; set; }
        public string disease_ehr_id { get; set; }
        
        public string organization { get; set; }
        public string appointmentlocation { get; set; }
        public string created_by { get; set; }
    }
    public class Push_PatientDiseasesBO
    {
        public string disease_name { get; set; }
        public string disease_type { get; set; }
        public bool is_deleted { get; set; }
        public string patientdisease_web_id { get; set; }     
        public string patientdisease_localdb_id { get; set; }
        public string disease_ehr_id { get; set; }
        public string patient_ehr_id { get; set; }
        public string organization { get; set; }
        public string appointmentlocation { get; set; }
        public string location { get; set; }
        public string created_by { get; set; }
    }
}
