
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{

    public class Pull_AppointmentBO
    {
        public string message { get; set; }
        public List<GetListAppointment> data { get; set; }

    }

    public class GetListAppointment
    {
        public string _id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string is_active { get; set; }
        public string created_at { get; set; }
        public string shedule_time { get; set; }
        public string end_time { get; set; }
        public string patient_status { get; set; }
        public string appointment_status { get; set; }
        public string payment_mode { get; set; }
        public string comment { get; set; }
        public string birth_date { get; set; }
        public string is_appointment { get; set; }
        public string appt_ehr_id { get; set; }
        public string appt_localdb_id { get; set; }
        public string confirmed_status { get; set; }
        public string confirmed_status_ehr_key { get; set; }
        public string appt_treatmentcode { get; set; }
        public string patient_ehr_id { get; set; }
      
        public List<Pull_Appointment_Providers> provider { get; set; }
        public List<Pull_Appointment_operatory> operatory { get; set; }
        public Pull_Appointment_Type appointmenttype { get; set; }
       
    }

    public class Pull_Appointment_Providers
    {
        public Pull_Appointment_Providers()
        {
            _id = string.Empty;
            display_name = string.Empty;
            Provider_EHR_ID = string.Empty;
        }
        public string _id { get; set; }
        public string display_name { get; set; }
        public string Provider_EHR_ID { get; set; }
    }

    public class Pull_Appointment_operatory
    {
        public Pull_Appointment_operatory()
        {
            _id = string.Empty;
            name = string.Empty;
            Operatory_EHR_ID = string.Empty;
        }
        public string _id { get; set; }
        public string name { get; set; }
        public string Operatory_EHR_ID { get; set; }
    }

    public class Pull_Appointment_Type
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string apptype_ehr_id { get; set; }
    }

    public class Pull_Appointment_Reason
    {
        public Pull_Appointment_Reason()
        {
            display_name = string.Empty;
        }
        public string display_name { get; set; }
    }

}
