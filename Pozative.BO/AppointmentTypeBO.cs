using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class AppointmentTypeBO
    {
        public string organization { get; set; }
        public string appointmentlocation { get; set; }
        public string name { get; set; }
        public bool is_new { get; set; }
        public string apptype_ehr_id { get; set; }
        public string apptype_localdb_id { get; set; }
        public string ApptType_Web_ID { get; set; }
        public string created_by { get; set; }
        public bool is_deleted { get; set; }
    }
}
