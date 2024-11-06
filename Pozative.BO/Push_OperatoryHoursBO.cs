using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
  public  class Push_OperatoryHoursBO
    {
        public string oh_localdb_id { get; set; }
        public string oh_ehr_id { get; set; }
        public string oh_web_id { get; set; }
        public string operatory_ehr_id { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string comment { get; set; }
        public bool is_deleted { get; set; }

        public string organization { get; set; }
        public string location { get; set; }
        public string created_by { get; set; }
    }

  public class Push_OperatoryOfficeHoursBO
  {
      public string Location_ID { get; set; }
      public string ParentLocation_ID { get; set; }
      public string Organization_ID { get; set; }
      public string OOH_Localdb_Id { get; set; } 
      public string OOH_EHR_ID { get; set; }
      public string OOH_Web_ID { get; set; }              
      public string Operatory_EHR_Id { get; set; }
      public string WeekDay { get; set; }
      public string StartTime1 { get; set; }
      public string EndTime1 { get; set; }
      public string StartTime2 { get; set; }
      public string EndTime2 { get; set; }
      public string StartTime3 { get; set; }
      public string EndTime3 { get; set; }       
      public bool is_deleted { get; set; }
      public string created_by { get; set; }
  }
}
