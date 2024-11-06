using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_PatientPayment
    {
        public string message { get; set; }
        public string status { get; set; }
        public string error { get; set; }
        public string pms_ehr_log_setting { get; set; }
        public string pms_ehr_log_financing_setting { get; set; }
        public List<GetListPatientPayment> data { get; set; }
    }

    public class GetListPatientPayment
    {
        public string patient_first_name { get; set; }
        public string patient_last_name { get; set; }
        public string patient_number { get; set; }
        public string patient_email { get; set; }
        public Decimal discount { get; set; }
        public string apEhrSyncId { get; set; }
        public string amount { get; set; }
        public string created_at { get; set; }
        public string fees { get; set; }
        public string paid_date { get; set; }
        public string patient_ehr_id { get; set; }
        public string payment_method { get; set; }
        public string payment_status { get; set; }
        public string payment_type { get; set; }
        public string ready_to_import { get; set; }
        public string ready_to_import_at { get;set;}
        public string template { get; set; }
        public string updated_at { get; set; }
        public string patientId { get; set; }

        public string log_setting { get; set; }
        public string financing_log_setting { get; set; }
        //public string created_at_formatted { get; set; }



        //public string Patient_Web_ID { get; set; }
        //public string PatientPaymentWebId { get; set; }
        //public string ProviderEHRId { get; set; }


        //public string PaymentNote { get; set; }
        //public string PaymentMode { get; set; }
        //public string payment_type { get; set; }
        //public string PaymentInOut { get; set; }
        //public string BankOrBranchName { get; set; }
        //public string ChequeNumber { get; set; }
        //public string PaymentReceivedLocal { get; set; }
        //public string PaymentEntryDatetimeLocal { get; set; }
        //public string PaymentUpdatedEHR { get; set; }
        //public string PaymentUpdatedEHRDateTime { get; set; }
        //public string PaymentStatusCompletedAdit { get; set; }
        //public string PaymentStatusCompletedDateTime { get; set; }
        //public string PaymentEHRId { get; set; }


    }

    //public class patient
    //{
    //    public string $ref {GetListAppointment;AppDomainSetup;};

    //}
}
