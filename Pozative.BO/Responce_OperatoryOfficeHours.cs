using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative.BO
{
    public class Responce_OperatoryOfficeHours
    {
        public string message { get; set; }
        public List<GetListOperatoryrOfficeHours> data { get; set; }

    }

    public class GetListOperatoryrOfficeHours
    {
        public string Operatory_EHR_ID { get; set; }
        public string _id { get; set; }

    }
}
