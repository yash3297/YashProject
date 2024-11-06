using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_PatientFormBO
    {
        public string message { get; set; }
        public List<GetListpatientForm> data { get; set; }
    }

  

    public class Pull_TreatmentDocBO
    {
        public string message { get; set; }
        public List<GetListTreatmentDoc> data { get; set; }
    }

    public class Pull_statusTreatmentDocBO
    {
        public bool status { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public string error { get; set; }

    }


    public class GetListTreatmentDoc
    {
        public string treatmentPlanId { get; set; }
        public string patientId { get; set; }
        public string locationId { get; set; }
        public string planName { get; set; }
        public string ehrpullstatus { get; set; }
        public string patient_ehr_id { get; set; }
        public string status { get; set; }
        public string patientName { get; set; }

        public string treatment_plan_submitted_at { get; set; }

    }

    public class GetListpatientForm
    {
        public string _id { get; set; }
        public string esId { get; set; }
        public string folder_name { get; set; }
        public string folder_ehr_id { get; set; }
        public string form_name_format { get; set; }
        public List<pull_ehrmap> ehrmap { get; set; }
        
        public pull_pinfo pinfo { get; set; }
        public ehr_value ehr_value { get; set; }
        //rooja
        public string patient_name { get; set; }
        public string submit_time { get; set; }
        public string form_name { get; set; }
    }
    public class ehr_value
    {
        public string email { get; set; }
        public string patientName { get; set; }
        public string phone { get; set; }

    }
    public class pull_ehrmap
    {
        public pull_ehrmap()
        {
            ehrField = string.Empty;
            value = string.Empty;
        }
        public string ehrField { get; set; }
        public string value { get; set; }
        public List<Alg_Prb_value> Alg_Prb_value { get; set; }
        public List<Removed_Alg_Prb_value> Removed_Alg_Prb_value { get; set; }
        public List<Medication_Value> Medication_value { get; set; }
        public List<Removed_Medication_Value> Removed_Medication_value { get; set; }
    }
    public class Alg_Prb_value
    {
        public string _id { get; set; }
        public string disease_ehr_id { get; set; }
        public string disease_type { get; set; }
        public string name { get; set; }
    }
    public class Removed_Alg_Prb_value
    {
        public string disease_name { get; set; }
        public string disease_ehr_id { get; set; }
        public string patient_ehr_id { get; set; }
        public string disease_type { get; set; }
    }

    public class pull_pinfo
    {
        //public pull_pinfo()
        //{
        //    _id = string.Empty;
        //    patient_ehr_id = string.Empty;
        //}
        public string _id { get; set; }
        public string patient_ehr_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile { get; set; }
        
    }

    public class Medication_Value
    {
        public string _id { get; set; }
        public string patientmedication_ehr_id { get; set; }
        public string medication_ehr_id { get; set; }
        public string medication_type { get; set; }
        public string medication_name { get; set; }
        public string medication_note { get; set; }
    }

    public class Removed_Medication_Value
    {
        public string _id { get; set; }
        public string patientmedication_ehr_id { get; set; }
        public string medication_ehr_id { get; set; }
        public string patientnote { get; set; }
        public string medication_name { get; set; }
        public string patient_ehr_id { get; set; }
    }

    //rooja 

    public class Pull_InsuranceCarrierDocBO
    {
        public string message { get; set; }
        public List<GetListInsuranceCarrierDoc> data { get; set; }
    }

    public class GetListInsuranceCarrierDoc
    {
        public string _id { get; set; }
        public string _type { get; set; }       
        public string patientId { get; set; }
        public string pdffile { get; set; }
             public string pdfName { get; set; }
        public string foldername { get; set; }
        public string locationId { get; set; }        
        public string patientEhrId { get; set; }
        public string is_active { get; set; }
        public string patientFullName { get; set; }

        public string submitted_at { get; set; }

    }

}
