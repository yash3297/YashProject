using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Push_ProviderHoursBO
    {
        public string ph_localdb_id { get; set; }
        public string ph_ehr_id { get; set; }
        public string ph_web_id { get; set; }
        public string provider_ehr_id { get; set; }
        public string operatory_ehr_id { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string comment { get; set; }
        public bool is_deleted { get; set; }

        public string organization { get; set; }
        public string location { get; set; }
        public string created_by { get; set; }
    }
}
