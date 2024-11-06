using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_RecallTypeBO
    {
        public string message { get; set; }
        public List<GetListRecallType> data { get; set; }
    }

    public class GetListRecallType
    {
        public string _id { get; set; }
        public string Apptype_localdb_id { get; set; }
        public string Apptype_ehr_id { get; set; }
        public string name { get; set; }
    }
}
