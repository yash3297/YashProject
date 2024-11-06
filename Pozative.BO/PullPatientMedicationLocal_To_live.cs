using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class PatientMedicationList
    {
        public string _id { get; set; }
        public string patient_ehr_id { get; set; }
        public string medication_ehr_id { get; set; }
    }

    public class PullPatientMedicationLocal_To_live1
    {
        public string message { get; set; }
        public int code { get; set; }
        public List<PatientMedicationList> data { get; set; }
        public bool status { get; set; }
    }
}
