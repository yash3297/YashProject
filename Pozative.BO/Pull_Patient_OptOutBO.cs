using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_Patient_OptOutBO
    {
        public string status { get; set; }
        public string message { get; set; }
        public string error { get; set; }
        public List<GetListPatient_OptOutBO> data { get; set; }
    }
    public class Patient_OptOutBO_StatusUpdate
    {
       public string locationId { get; set; }
        public string organizationId { get; set; }
        public List<Patientids_OptOutBO_StatusUpdate> patientIds { get; set; }       
       
    }
    public class Patientids_OptOutBO_StatusUpdate
    {
        public string patientId { get; set; }
        public string esId { get; set; }
    }
    public class GetListPatient_OptOutBO
 {
     public string patientid { get; set; }
     public string esid { get; set; }
     public string patient_ehr_id { get; set; }
     public string receive_sms { get; set; }
 }
}
