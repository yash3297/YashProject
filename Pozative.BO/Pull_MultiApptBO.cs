using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{

    public class Pull_MultiApptBO
    {
        public string message { get; set; }
        public List<GetListMultiAppt> data { get; set; }

    }

    public class GetListMultiAppt
    {
        public string _id { get; set; }
        public string appt_ehr_id { get; set; }
        
    }
}
