using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class AditLocationSyncBO
    {
        public string message { get; set; }
        public string error { get; set; }
        public AditLocationSync data { get; set; }

    }
    public class AditLocationSync
    {
        public string ehr_sync_status { get; set; }
        public string webauto_sync { get; set; }
        public string last_sync { get; set; }
        public string lastSyncAppointment { get; set; }
        public string log_sync_date {get;set;}
        public string sync_practice_analytics { get; set; }
        public string pa_disabled_sync { get; set; }
        public string smscall_synclimit { get; set; }
        public string imageuploadbatch { get; set; }
        public string patient_followuplimit { get; set; }

    }

    public class AditPaymentSMSCallUpdateStatus
    {
        public string location { get; set; }
        public string created_by { get; set; }
        public string log_sync_date { get; set; }
    }
}
