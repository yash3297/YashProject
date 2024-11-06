using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_PatientSMSCallLog
    {
        public string message { get; set; }
        public string status { get; set; }
        public string error { get; set; }
        public List<GetListPatientSMSCallLog> data { get; set; }
        public Int32 totalLength { get; set; }
    }

    public class GetListPatientSMSCallLog
    {

        public string esId { get; set; }
        public string smsId { get; set; }
        public string patientId { get; set; }
        public string created_at { get; set; }
        public string created_at_date_time { get; set; }
        public string patientLastName { get; set; }
        public string organizationId { get; set; }
        public string appointmentlocationId { get; set; }
        public string patientMobile { get; set; }
        public string patientFirstName { get; set; }
        public string app_alias { get; set; }
        public string text { get; set; }
        public string message_direction { get; set; }
        public string message_type { get; set; }
        public string message_sub_type { get; set; }
        public string ehrsyncstatus { get; set; }
        public string description { get; set; }

    }
    public class Push_PatientSMSCallLog
    {
        public string locationId { get; set; }
        public List<PatientSMSCallLogStatusUpdate> statusupdate { get; set; }

    }



    public class PatientSMSCallLogStatusUpdate
    {
        public string esId { get; set; }
        public string smsId { get; set; }
        public string ehrsyncstatus { get; set; }
    }

    //public class patient
    //{
    //    public string $ref {GetListAppointment;AppDomainSetup;};

    //}
}
