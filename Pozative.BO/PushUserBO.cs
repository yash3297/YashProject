using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
   public class PushUserBO
    {
        public string user_ehr_id { get; set; }
        public string password { get; set; }
        public string organization { get; set; }
        public string appointmentlocation { get; set; }
        public string location { get; set; }
        public bool is_deleted { get; set; }
   
        public string firstname { get; set; }
        public string lastname { get; set; }
        public bool is_active { get; set; }
    }
}
