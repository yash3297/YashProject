using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_PatientBO
    {
        public string location { get; set; }
        public string organization { get; set; }
        public string created_by { get; set; }
        public string appointmentlocation { get; set; }

        public string patient_localdb_id { get; set; }
        public string patient_ehr_id { get; set; }
        public string Patient_Web_ID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string salutation { get; set; }
        public string preferred_name { get; set; }
        public string status { get; set; }
        public string sex { get; set; }
        public string marital_status { get; set; }
        public string birth_date { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string home_phone { get; set; }
        public string work_phone { get; set; }
        public string address_one { get; set; }
        public string address_two { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string responsibleparty_status { get; set; }
        public string current_bal { get; set; }
        public string thirty_day { get; set; }
        public string sixty_day { get; set; }
        public string ninety_day { get; set; }
        public string over_ninty { get; set; }
        public string firstvisit_date { get; set; }
        public string lastvisit_date { get; set; }
        public string primary_insurance { get; set; }
        public string primary_insurance_companyname { get; set; }
        public string primary_ins_subscriber_id { get; set; }
        public string secondary_insurance { get; set; }
        public string secondary_insurance_companyname { get; set; }
        public string secondary_ins_subscriber_id { get; set; }
        public string secondary_insurancecompanyphonenumber { get; set; }
        public string guar_id { get; set; }
        public string pri_provider_id { get; set; }
        public string sec_provider_id { get; set; }
        public string receive_sms { get; set; }
        public string receive_email { get; set; }
        public string nextvisit_date { get; set; }
        public string due_date { get; set; }
        public string[] due_dates { get; set; }
        public string[] recall_type { get; set; }
        public string[] ehr_key { get; set; }
        public string used_benefit { get; set; }
        public string remaining_benefit { get; set; }
        public string collect_payment { get; set; }
        public string last_sync_date { get; set; }
        public bool is_deleted { get; set; }
        public string ehr_status { get; set; }
        public string receive_voice_call { get; set; }
        public string patient_status { get; set; }
        public string PreferredLanguage { get; set; }
        public string patient_note { get; set; }
        
        public string ssn { get; set; }
        public string driverlicense { get; set; }
        public string groupid { get; set; }
        public string insurancecompanyphonenumber { get; set; }
        public string emergencycontactid { get; set; }
        public string emergencycontactfirstname { get; set; }
        public string emergencycontactlastname { get; set; }
        public string emergencycontactnumber { get; set; }
        public string school { get; set; }
        public string employer { get; set; }
        public string spouseid { get; set; }
        public string spousefirstname { get; set; }
        public string spouselastname { get; set; }
        public string responsiblepartyid { get; set; }
        public string responsiblepartyfirstname { get; set; }
        public string responsiblepartylastname { get; set; }
        public string responsiblepartyssn { get; set; }
        public string responsiblepartybirthdate { get; set; }     

    }

    public class Push_PatientBOMultiLocation
    {
        public string location { get; set; }
        public string organization { get; set; }
        public string created_by { get; set; }
        public string appointmentlocation { get; set; }

        public string patient_localdb_id { get; set; }
        public string patient_ehr_id { get; set; }
        public string Patient_Web_ID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string salutation { get; set; }
        public string preferred_name { get; set; }
        public string status { get; set; }
        public string sex { get; set; }
        public string marital_status { get; set; }
        public string birth_date { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string home_phone { get; set; }
        public string work_phone { get; set; }
        public string address_one { get; set; }
        public string address_two { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string responsibleparty_status { get; set; }
        public string current_bal { get; set; }
        public string thirty_day { get; set; }
        public string sixty_day { get; set; }
        public string ninety_day { get; set; }
        public string over_ninty { get; set; }
        public string firstvisit_date { get; set; }
        public string lastvisit_date { get; set; }
        public string primary_insurance { get; set; }
        public string primary_insurance_companyname { get; set; }
        public string primary_ins_subscriber_id { get; set; }
        public string secondary_insurance { get; set; }
        public string secondary_insurance_companyname { get; set; }
        public string secondary_ins_subscriber_id { get; set; }
        public string guar_id { get; set; }
        public string pri_provider_id { get; set; }
        public string sec_provider_id { get; set; }
        public string receive_sms { get; set; }
        public string receive_email { get; set; }
        public string nextvisit_date { get; set; }
        public string due_date { get; set; }
        public string[] due_dates { get; set; }
        public string[] recall_type { get; set; }
        public string[] ehr_key { get; set; }
        public string used_benefit { get; set; }
        public string remaining_benefit { get; set; }
        public string collect_payment { get; set; }
        public string last_sync_date { get; set; }
        public bool is_deleted { get; set; }
        public string ehr_status { get; set; }
        public string receive_voice_call { get; set; }
        public string patient_status { get; set; }
        public string PreferredLanguage { get; set; }
        public string patient_note { get; set; }

        public string ssn { get; set; }
        public string driverlicense { get; set; }
        public string groupid { get; set; }
        public string insurancecompanyphonenumber { get; set; }
        public string emergencycontactid { get; set; }
        public string emergencycontactfirstname { get; set; }
        public string emergencycontactlastname { get; set; }
        public string emergencycontactnumber { get; set; }
        public string school { get; set; }
        public string employer { get; set; }
        public string spouseid { get; set; }
        public string spousefirstname { get; set; }
        public string spouselastname { get; set; }
        public string responsiblepartyid { get; set; }
        public string responsiblepartyfirstname { get; set; }
        public string responsiblepartylastname { get; set; }
        public string responsiblepartyssn { get; set; }
        public string responsiblepartybirthdate { get; set; }
        public string office_id { get; set; }
        public string[] multilocation { get; set; }

    }
}
