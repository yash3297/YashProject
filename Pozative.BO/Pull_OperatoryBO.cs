using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{

    public class Pull_OperatoryBO
    {

        public string message { get; set; }
        public List<GetListOperatory> data { get; set; }

    }

    public class GetListOperatory
    {
        public string _id { get; set; }
        public string operatory_localdb_id { get; set; }
        public string operatory_ehr_id { get; set; }
        public string name { get; set; }
    }
}
