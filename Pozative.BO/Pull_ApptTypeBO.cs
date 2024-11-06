using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_ApptTypeBO
    {
   
        public string message { get; set; }
        public List<GetListApptType> data { get; set; }

    }

    public class GetListApptType
    {
        public string _id { get; set; }
        public string Apptype_localdb_id { get; set; }
        public string Apptype_ehr_id { get; set; }
        public string name { get; set; }
    }
}

