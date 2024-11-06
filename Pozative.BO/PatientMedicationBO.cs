using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class PatientMedicationBO1
    {
        public string organization { get; set; }
        public string appointmentlocation { get; set; }
        public string location { get; set; }        
        public string notes { get; set; }
        public string medication_ehr_id { get; set; }
        public string patientmedication_ehr_id { get; set; }
        public string patientmedication_localdb_id { get; set; }
        public string patient_ehr_id { get; set; }
        public string providerid { get; set; }
        public string medication_name { get; set; }
        public string medicaldescription { get; set; }
        public bool is_deleted { get; set; }
        public string drug_quantity { get; set; }
        public string refills { get; set; }
        public string startdate { get; set; }
        public string stopdate { get; set; }
        public string last_synchdate { get; set; }
        public string date_entered { get; set; }
        public string expiry_date { get; set; }
        public string patientnote { get; set; }
        public bool is_active { get; set; }
    }
}
