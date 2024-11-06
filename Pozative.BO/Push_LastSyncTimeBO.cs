
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_LastSyncDatetimeBO
    {
        public string locationId { get; set; }
        public string appointment { get; set; }
        public string appointment_type { get; set; }
        public string confirm_appointment { get; set; }       
        public string operatory { get; set; }
        public string operatory_hours { get; set; }
        public string patient { get; set; }
        public string patient_payment_log { get; set; }
        public string patient_status { get; set; }
        public string provider { get; set; }
        public string provider_hours { get; set; }
        public string pull_appointment { get; set; }
        public string pull_patient_document { get; set; }
        public string pull_treatmentplan_document { get; set; }
        public string reinstall { get; set; }
        public string sms_call_log { get; set; }       
        public string currentDate { get; set; }
        public string user { get; set; }
    }
}
