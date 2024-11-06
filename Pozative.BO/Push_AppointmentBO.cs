using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_AppointmentBO
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Location_ID { get; set; }
        public string MI { get; set; }
        public string Home_Contact { get; set; }
        public string Mobile_Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ST { get; set; }
        public string Zip { get; set; }
        public string[] Operatory_Name { get; set; }
        public string[] Provider_Name { get; set; }
        public string comment { get; set; }
        public string birth_date { get; set; }
        public string Appt_Type { get; set; }
        public string Appt_DateTime { get; set; }
        public string Appt_EndDateTime { get; set; }
        public string Status { get; set; }
        public string Is_Appt { get; set; }
        public bool is_ehr_updated { get; set; }
        public string Entry_DateTime { get; set; }
        public string ehr_entry_date { get; set; }
        public string Patient_Status { get; set; }
        public string appointment_status_ehr_key { get; set; }
        public string Appointment_Status { get; set; }
        public string confirmed_status_ehr_key { get; set; }
        public string confirmed_status { get; set; }
        public string unschedule_status_ehr_key { get; set; }
        public string unschedule_status { get; set; }
        public string Organization_ID { get; set; }
        public string Appt_Web_ID { get; set; }
        public string appt_localdb_id { get; set; }
        public string appt_ehr_id { get; set; }
        public string patient_ehr_id { get; set; }
        public string created_by { get; set; }
        public bool is_deleted { get; set; }
        public string ParentLocation_ID { get; set; }
        public bool is_asap { get; set; }
        public string proceduredesc { get; set; }
        public string procedurecode { get; set; }
//        public string ehr_patient_status { get; set; }
    }
    public class Push_AppointmentBOMultiLocation
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Location_ID { get; set; }
        public string MI { get; set; }
        public string Home_Contact { get; set; }
        public string Mobile_Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string ST { get; set; }
        public string Zip { get; set; }
        public string[] Operatory_Name { get; set; }
        public string[] Provider_Name { get; set; }
        public string comment { get; set; }
        public string birth_date { get; set; }
        public string Appt_Type { get; set; }
        public string Appt_DateTime { get; set; }
        public string Appt_EndDateTime { get; set; }
        public string Status { get; set; }
        public string Is_Appt { get; set; }
        public bool is_ehr_updated { get; set; }
        public string Entry_DateTime { get; set; }
        public string ehr_entry_date { get; set; }
        public string Patient_Status { get; set; }
        public string appointment_status_ehr_key { get; set; }
        public string Appointment_Status { get; set; }
        public string confirmed_status_ehr_key { get; set; }
        public string confirmed_status { get; set; }
        public string unschedule_status_ehr_key { get; set; }
        public string unschedule_status { get; set; }
        public string Organization_ID { get; set; }
        public string Appt_Web_ID { get; set; }
        public string appt_localdb_id { get; set; }
        public string appt_ehr_id { get; set; }
        public string patient_ehr_id { get; set; }
        public string created_by { get; set; }
        public bool is_deleted { get; set; }
        public string ParentLocation_ID { get; set; }
        public bool is_asap { get; set; }
        public string proceduredesc { get; set; }
        public string procedurecode { get; set; }
        public string office_id { get; set; }
        public string[] multilocation { get; set; }
//        public string ehr_patient_status { get; set; }
    }
}
