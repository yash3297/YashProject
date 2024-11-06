using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
  public   class AppointmentStatusWIthTypeBO
    {
      public string ap_status_normal { get; set; }
      public string ap_status_confirm { get; set; }
      public string ap_status_unshedule { get; set; }
      

        public string organization { get; set; }
        public string appointmentlocationid { get; set; }
        public string locationId { get; set; }
        public string created_by { get; set; }
       
    }

    public class ap_status_normalBO
    {
        public ap_status_normalBO()
        {
            name = string.Empty;
            key = string.Empty;
            ehr_key = string.Empty;           
        }
        public string name { get; set; }
        public string key { get; set; }
        public string ehr_key { get; set; }
    }

    public class ap_status_confirmBO
    {
        public ap_status_confirmBO()
        {
            name = string.Empty;
            key = string.Empty;
            ehr_key = string.Empty;
        }
        public string name { get; set; }
        public string key { get; set; }
        public string ehr_key { get; set; }
    }

    public class ap_status_unsheduleBO
    {
        public ap_status_unsheduleBO()
        {
            name = string.Empty;
            key = string.Empty;
            ehr_key = string.Empty;
        }
        public string name { get; set; }
        public string key { get; set; }
        public string ehr_key { get; set; }
    }
}
