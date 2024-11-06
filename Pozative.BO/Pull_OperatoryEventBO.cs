using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{

    public class Pull_OperatoryEventBO
    {

        public string message { get; set; }
        public List<GetListOperatoryEvent> data { get; set; }

    }

    public class GetListOperatoryEvent
    {
        public string _id { get; set; }
        public string operatoryEvent_localdb_id { get; set; }
        public string appt_ehr_id { get; set; }
        public string name { get; set; }
    }
}
