using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Pull_InsuranceBO
    {
   
        public string message { get; set; }
        public List<GetListInsurance> data { get; set; }

    }

    public class GetListInsurance
    {
        public string _id { get; set; }
        public string Insurance_localdb_id { get; set; }
        public string ehr_id { get; set; }
        public string name { get; set; }
    }
}

