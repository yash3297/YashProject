using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
  public  class Push_OperatoryEventBO
    { 
        public string Location_ID { get; set; }
        public string[] Operatory_Name { get; set; }
        public string comment { get; set; }
        public string Appt_DateTime { get; set; }
        public string Appt_EndDateTime { get; set; }
        public string Entry_DateTime { get; set; }
        public string Organization_ID { get; set; }
        public string Appt_Web_ID { get; set; }
        public string Appointment_Status { get; set; }
        public string appt_localdb_id { get; set; }
        public string appt_ehr_id { get; set; }
        public string created_by { get; set; }
        public bool is_deleted { get; set; }
        public string ParentLocation_ID { get; set; }
        public string[] Provider_Name { get; set; }
        public bool Allow_Book_Appt { get; set; }
    }
}
