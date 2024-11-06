using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_PatientStatus
    {
        public string location { get; set; }
        public string appointmentlocation { get; set; }
        public string patient_ehr_id { get; set; }
        public string patient_status { get; set; }
        public string webid { get; set; }
    }

    public class Push_PatientStatus_Response
    {
        public string message { get; set; }
        public string code { get; set; }
        public string status { get; set; }
        public List<GetListpatientStatus> data { get; set; }
    }

    public class GetListpatientStatus
    {
        public string _id { get; set; }
        public string patient_ehr_id { get; set; }    

    }
      
}
